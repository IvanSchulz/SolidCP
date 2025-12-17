// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FuseCP.EnterpriseServer;
using FuseCP.Providers;
using FuseCP.Providers.Web;

namespace FuseCP.EnterpriseServer
{
    public class ImportController: ControllerBase
    {
        public ImportController(ControllerBase provider): base(provider) { }

        public List<ServiceProviderItemType> GetImportableItemTypes(int packageId)
        {
            // load all service item types
            List<ServiceProviderItemType> itemTypes = PackageController.GetServiceItemTypes();

            // load package context
            PackageContext cntx = PackageController.GetPackageContext(packageId);

            // build importable items list
            List<ServiceProviderItemType> importableTypes = new List<ServiceProviderItemType>();
            foreach (ServiceProviderItemType itemType in itemTypes)
            {
                if (!itemType.Importable)
                    continue;

                // load group
                ResourceGroupInfo group = ServerController.GetResourceGroup(itemType.GroupId);
                if (cntx.Groups.ContainsKey(group.GroupName))
                    importableTypes.Add(itemType);
            }
            return importableTypes;
        }

        public List<string> GetImportableItems(int packageId, int itemTypeId)
        {
            List<string> items = new List<string>();

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin | DemandAccount.NotDemo);
            if (accountCheck < 0) return items;

            // load item type
            if (itemTypeId > 0)
            {
                ServiceProviderItemType itemType = PackageController.GetServiceItemType(itemTypeId);

                // load group
                ResourceGroupInfo group = ServerController.GetResourceGroup(itemType.GroupId);

                // Is it DNS Zones? Then create a IDN Mapping object
                var isDnsZones = group.GroupName == "DNS";
                var idn = new IdnMapping();

                // get service id
                int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
                if (serviceId == 0)
                    return items;

                // Read existing packages and serviceitems
                DataTable dtServiceItems = PackageController.GetServiceItemsDataSet(serviceId).Tables[0];
                DataTable dtPackageItems = PackageController.GetPackageItemsDataSet(packageId).Tables[0];

                // Add already existing packages and serviceitems to lowercase ignorelist
                List<string> ignorelist = new List<string>();
                foreach (DataRow dr in dtServiceItems.Rows)
                {
                    string serviceItemName = (string)dr["ItemName"];
                    int serviceItemTypeId = (int)dr["ItemTypeId"];

                    if (serviceItemTypeId == itemTypeId)
                    {
                        if (!ignorelist.Contains(serviceItemName))
                            ignorelist.Add(serviceItemName.ToLower());
                    }
                }
                foreach (DataRow dr in dtPackageItems.Rows)
                {
                    string packageItemName = (string)dr["ItemName"];
                    int packageItemTypeId = (int)dr["ItemTypeId"];

                    if (packageItemTypeId == itemTypeId)
                    {
                        if (!ignorelist.Contains(packageItemName))
                            ignorelist.Add(packageItemName.ToLower());
                    }
                }

                // instantiate controller
                IImportController ctrl = null;
                try
                {
                    List<string> importableItems = null;
                    ctrl = Activator.CreateInstance(Type.GetType(group.GroupController)) as IImportController;
                    if (ctrl != null)
                    {
                        importableItems = ctrl.GetImportableItems(packageId, itemTypeId, Type.GetType(itemType.TypeName), group);
                    }

                    foreach (string importableItem in importableItems)
                    {
                        if (!ignorelist.Contains(importableItem.ToLower()))
                        {
                            var itemToImport = importableItem;

                            // For DNS zones the compare has been made using ascii, convert to unicode if necessary to make the list of items easier to read
                            if (isDnsZones && itemToImport.StartsWith("xn--"))
                            {
                                itemToImport = idn.GetUnicode(importableItem);
                            }

                            items.Add(itemToImport);
                        }
                    }

                }
                catch { /* do nothing */ }
            }
            else
                return GetImportableCustomItems(packageId, itemTypeId);

            return items;
        }

		public int ImportItems(bool async, string taskId, int packageId, string[] items)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin);
			if (accountCheck < 0) return accountCheck;


			if (async)
			{
				ImportAsyncWorker worker = new ImportAsyncWorker();
				worker.threadUserId = SecurityContext.User.UserId;
				worker.taskId = taskId;
				worker.packageId = packageId;
				worker.items = items;

				// import
				worker.ImportAsync();

				return 0;
			}
			else
			{
				return ImportItemsInternal(taskId, packageId, items);
			}
		}

        public int ImportItemsInternal(string taskId, int packageId, string[] items)
        {
			PackageInfo package = PackageController.GetPackage(packageId);

			TaskManager.StartTask(taskId, "IMPORT", "IMPORT", package.PackageName, packageId);

			TaskManager.IndicatorMaximum = items.Length;
			TaskManager.IndicatorCurrent = 0;

            Dictionary<int, List<string>> groupedItems = new Dictionary<int, List<string>>();
            List<string> customItems = new List<string>();

            // sort by groups
            foreach (string item in items)
            {

                string[] itemParts = item.Split('|');
                if (!item.StartsWith("+"))
                {
                    int itemTypeId = Utils.ParseInt(itemParts[0], 0);
                    string itemName = itemParts[1];

                    // add to group
                    if (!groupedItems.ContainsKey(itemTypeId))
                        groupedItems[itemTypeId] = new List<string>();

                    groupedItems[itemTypeId].Add(itemName);
                }
                else
                {
                    switch (itemParts[0])
                    {
                        case ("+100"):
                            if (itemParts.Length > 2)
                                customItems.Add(item);
                            break;
                    }

                }
            }

            // import each group
            foreach (int itemTypeId in groupedItems.Keys)
            {
                // load item type
                ServiceProviderItemType itemType = PackageController.GetServiceItemType(itemTypeId);

                // load group
                ResourceGroupInfo group = ServerController.GetResourceGroup(itemType.GroupId);

                // instantiate controller
                IImportController ctrl = null;
                try
                {
                    ctrl = Activator.CreateInstance(Type.GetType(group.GroupController)) as IImportController;
                    if (ctrl != null)
                    {
						foreach (string itemName in groupedItems[itemTypeId])
						{
							TaskManager.Write(String.Format("Import {0} '{1}'",
								itemType.DisplayName, itemName));

							try
							{
								// perform import
								ctrl.ImportItem(packageId, itemTypeId,
									Type.GetType(itemType.TypeName), group, itemName);
							}
							catch (Exception ex)
							{
								TaskManager.WriteError(ex, "Can't import item");
							}

							TaskManager.IndicatorCurrent++;
						}
                    }
                }
                catch { /* do nothing */ }
            }

            foreach (string s in customItems)
            {
                try
                {
                    string[] sParts = s.Split('|');
                    switch (sParts[0])
                    {
                        case "+100":
                            TaskManager.Write(String.Format("Import {0}", sParts[4]));

                            int result = WebServerController.ImporHostHeader(int.Parse(sParts[2],0), int.Parse(sParts[3],0), int.Parse(sParts[5],0));

                            if (result < 0)
                                TaskManager.WriteError(String.Format("Failed to Import {0} ,error: {1}: ", sParts[4], result.ToString()));
                            
                        break;
                    }
                }
                catch { /* do nothing */ }

                TaskManager.IndicatorCurrent++;
            }

            TaskManager.IndicatorCurrent = items.Length;

			TaskManager.CompleteTask();

            return 0;
        }

        private List<string> GetImportableCustomItems(int packageId, int itemTypeId)
        {

            List<string> items = new List<string>();
            PackageInfo packageInfo = PackageController.GetPackage(packageId);
            if (packageInfo == null) return items;

            switch (itemTypeId)
            {
                case -100:
                    List<UserInfo> users = UserController.GetUsers(packageInfo.UserId, true);
                    foreach (UserInfo user in users)
                    {
                        List<PackageInfo> packages = PackageController.GetPackages(user.UserId);
                        foreach (PackageInfo package in packages)
                        {
                            List<WebSite> webSites = WebServerController.GetWebSites(package.PackageId, false);
                            foreach (WebSite webSite in webSites)
                            {
                                items.Add(user.Username+"|"+user.UserId.ToString()+"|"+package.PackageId.ToString()+"|"+webSite.SiteId+"|"+webSite.Id);
                            }
                        }
                    }

                    break;
            }
            
            return items;
        }
    }
}
