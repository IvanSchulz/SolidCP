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
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using FuseCP.EnterpriseServer.Code.SharePoint;
using FuseCP.Providers;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.SharePoint;
using FuseCP.Server.Client;

namespace FuseCP.EnterpriseServer
{
    public class CalculatePackagesDiskspaceTask : SchedulerTask
    {
        private readonly bool suspendOverused = false;

        public override void DoWork()
        {
            // Input parameters:
            //  - SUSPEND_OVERUSED_PACKAGES

            CalculateDiskspace();
        }

        public void CalculateDiskspace()
        {
            // get all owned packages
            List<PackageInfo> packages = PackageController.GetPackagePackages(TaskManager.TopTask.PackageId, true);
            TaskManager.Write("Packages to calculate: " + packages.Count.ToString());

            foreach (PackageInfo package in packages)
            {
                // calculating package diskspace
                CalculatePackage(package.PackageId);
            }
        }

        public void CalculatePackage(int packageId)
        {
            try
            {
                // get all package items
                List<ServiceProviderItem> items = PackageController.GetServiceItemsForStatistics(
                    0, packageId, true, false, false, false);

                //TaskManager.Write("Items: " + items.Count);

                // order items by service
                Dictionary<int, List<ServiceProviderItem>> orderedItems =
                    PackageController.OrderServiceItemsByServices(items);

                // calculate statistics for each service set
                List<ServiceProviderItemDiskSpace> itemsDiskspace = new List<ServiceProviderItemDiskSpace>();
                foreach (int serviceId in orderedItems.Keys)
                {
                    ServiceProviderItemDiskSpace[] serviceDiskspace = CalculateItems(serviceId, orderedItems[serviceId]);
                    if (serviceDiskspace != null)
                        itemsDiskspace.AddRange(serviceDiskspace);
                }

                // update info in the database
                string xml = BuildDiskSpaceStatisticsXml(itemsDiskspace.ToArray());
                PackageController.UpdatePackageDiskSpace(packageId, xml);
                //TaskManager.Write("XML: " + xml);

                // suspend package if requested
                if (suspendOverused)
                {
                    // disk space
                    QuotaValueInfo dsQuota = PackageController.GetPackageQuota(packageId, Quotas.OS_DISKSPACE);

                    if (dsQuota.QuotaExhausted)
                        PackageController.ChangePackageStatus(null, packageId, PackageStatus.Suspended, false);
                }
            }
            catch (Exception ex)
            {
                // load package details
                PackageInfo package = PackageController.GetPackage(packageId);

                // load user details
                UserInfo user = PackageController.GetPackageOwner(package.PackageId);

                // log error
                TaskManager.WriteError(String.Format("Error calculating diskspace for '{0}' space of user '{1}': {2}",
                    package.PackageName, user.Username, ex.ToString()));
            }
        }

        private int GetExchangeServiceID(int packageId)
        {
            return PackageController.GetPackageServiceId(packageId, ResourceGroups.Exchange);
        }


        public ServiceProviderItemDiskSpace[] CalculateItems(int serviceId, List<ServiceProviderItem> items)
        {
            // convert items to SoapObjects
            List<SoapServiceProviderItem> objItems = new List<SoapServiceProviderItem>();
            
            //hack for organization... Refactoring!!!

           
            List<ServiceProviderItemDiskSpace> organizationDiskSpaces = new List<ServiceProviderItemDiskSpace>();
            foreach (ServiceProviderItem item in items)
            {
                long size = 0;
                if (item is Organization)
                {
                    Organization org = (Organization) item;

                    //Exchange DiskSpace
                    if (!string.IsNullOrEmpty(org.GlobalAddressList))
                    {
                        int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                        if (exchangeServiceId > 0)
                        {
                            ServiceProvider exchangeProvider = ExchangeServerController.GetExchangeServiceProvider(exchangeServiceId, item.ServiceId);

                            SoapServiceProviderItem soapOrg = SoapServiceProviderItem.Wrap(org);
                            ServiceProviderItemDiskSpace[] itemsDiskspace =
                                exchangeProvider.GetServiceItemsDiskSpace(new SoapServiceProviderItem[] { soapOrg });

                            if (itemsDiskspace != null && itemsDiskspace.Length > 0)
                            {
                                size += itemsDiskspace[0].DiskSpace;
                            }
                        }
                    }

                    // Crm DiskSpace
                    if (org.CrmOrganizationId != Guid.Empty)
                    {
                        //CalculateCrm DiskSpace
                    }

                    //SharePoint DiskSpace

                    int res;

                    PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                    if (cntx.Groups.ContainsKey(ResourceGroups.SharepointFoundationServer))
                    {
                        SharePointSiteDiskSpace[] sharePointSiteDiskSpaces =
                            HostedSharePointServerController.CalculateSharePointSitesDiskSpace(org.Id, out res);
                        if (res == 0)
                        {
                            foreach (SharePointSiteDiskSpace currecnt in sharePointSiteDiskSpaces)
                            {
                                size += currecnt.DiskSpace;
                            }
                        }
                    }

                    if (cntx.Groups.ContainsKey(ResourceGroups.SharepointEnterpriseServer))
                    {
                        SharePointSiteDiskSpace[] sharePointSiteDiskSpaces =
                            HostedSharePointServerEntController.CalculateSharePointSitesDiskSpace(org.Id, out res);
                        if (res == 0)
                        {
                            foreach (SharePointSiteDiskSpace currecnt in sharePointSiteDiskSpaces)
                            {
                                size += currecnt.DiskSpace;
                            }
                        }
                    }

                    ServiceProviderItemDiskSpace tmp = new ServiceProviderItemDiskSpace();
                    tmp.ItemId = item.Id;
                    tmp.DiskSpace = size;
                    organizationDiskSpaces.Add(tmp);
                }
                else
                    objItems.Add(SoapServiceProviderItem.Wrap(item));
            }
            
            
            int attempt = 0;
            int ATTEMPTS = 3;
            while (attempt < ATTEMPTS)
            {
                // increment attempt
                attempt++;

                try
                {
                    // send packet for calculation
                    // invoke service provider
                    //TaskManager.Write(String.Format("{0} - Invoke GetServiceItemsDiskSpace method ('{1}' items) - {2} attempt",
                    //    DateTime.Now, objItems.Count, attempt));

                    if (objItems.Count > 0)
                    {
                        ServiceProvider prov = new ServiceProvider();
                        ServiceProviderProxy.Init(prov, serviceId);
                        ServiceProviderItemDiskSpace[] itemsDiskSpace = prov.GetServiceItemsDiskSpace(objItems.ToArray());
                        if (itemsDiskSpace != null && itemsDiskSpace.Length > 0)
                            organizationDiskSpaces.AddRange(itemsDiskSpace);
                    }
                    
                    return organizationDiskSpaces.ToArray();
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex.ToString());
                }
            }

            throw new Exception("The number of attemtps has been reached. The package calculation has been aborted.");
        }

        private string BuildDiskSpaceStatisticsXml(ServiceProviderItemDiskSpace[] itemsDiskspace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<items>");

			if (itemsDiskspace != null)
			{
				foreach (ServiceProviderItemDiskSpace item in itemsDiskspace)
				{
					sb.Append("<item id=\"").Append(item.ItemId).Append("\"")
						.Append(" bytes=\"").Append(item.DiskSpace).Append("\"></item>\n");
				}
			}

            sb.Append("</items>");
            return sb.ToString();
        }
    }
}
