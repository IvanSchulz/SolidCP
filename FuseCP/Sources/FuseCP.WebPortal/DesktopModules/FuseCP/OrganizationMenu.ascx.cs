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
using System.Web.UI.WebControls;
using System.Data;
using FuseCP.EnterpriseServer;
using FuseCP.WebPortal;
using FuseCP.Portal.UserControls;


namespace FuseCP.Portal
{
    public partial class OrganizationMenu : OrganizationMenuControl
    {
        DataSet myPackages;
        int currentPackage;
        // int l_CurrentItem;  --- compile warning - never used

        private PackageContext cntx = null;
        private const string PID_SPACE_EXCHANGE_SERVER = "SpaceExchangeServer";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (PanelSecurity.SelectedUser.Role == UserRole.Administrator)
            {
                orgMenu.Visible = false;
                return;
            }

            if (PanelSecurity.PackageId == 0)
            {
                myPackages = new PackagesHelper().GetMyPackages();
                //For selectedUser have Packages or not then HIDE Menu
                if (myPackages.Tables[0].Rows.Count == 0)
                {
                    orgMenu.Visible = false;
                    return;
                }

                if (Session["currentPackage"] == null || ((int)Session["currentUser"]) != PanelSecurity.SelectedUserId)
                {
                    if (myPackages.Tables[0].Rows.Count > 0)
                    {
                        Session["currentPackage"] = myPackages.Tables[0].Rows[0][0].ToString();
                        Session["currentUser"] = PanelSecurity.SelectedUserId;
                    }
                }
                currentPackage = Convert.ToInt32(Session["currentPackage"]);
            }
            else
            {
                currentPackage = PanelSecurity.PackageId;
            }
            // load package context
            cntx = PackagesHelper.GetCachedPackageContext(currentPackage);

            ShortMenu = false;
            ShowImg = false;

            //if (currentPackage > 0 && PanelRequest.ItemID == 0)
            //{
            //    DataTable l_OrgTable;
            //    l_OrgTable = new OrganizationsHelper().GetOrganizations(currentPackage, false);
            //    if (l_OrgTable.Rows.Count > 0)
            //    {
            //        l_CurrentItem = Convert.ToInt32(l_OrgTable.Rows[0]["ItemID"]);
            //    }
            // }
            // else
            // {
            //     l_CurrentItem = PanelRequest.ItemID;
            // }


            // organization



            // if (l_CurrentItem > 0)
            // {

            //    if (!Request[DefaultPage.PAGE_ID_PARAM].Equals(PID_SPACE_EXCHANGE_SERVER, StringComparison.InvariantCultureIgnoreCase)) {
            //       MenuItem rootItem = new MenuItem(locMenuTitle.Text);
            //      rootItem.Selectable = false;

            //       menu.Items.Add(rootItem);
            //      //Add "Organization Home" menu item 
            //     MenuItem item = new MenuItem(
            //      GetLocalizedString("Text.OrganizationHome"),
            //       "",
            //       "",
            //      PortalUtils.EditUrl("ItemID", l_CurrentItem.ToString(), "organization_home", "SpaceID=" + currentPackage));//, "mid=135"
            //       makeSelectedMenu(item);
            //       rootItem.ChildItems.Add(item);
            //       this.ItemID = l_CurrentItem;
            //       this.PackageId = currentPackage;
            //       BindMenu(rootItem.ChildItems);
            //     }

            //   }
            if (cntx.Quotas.ContainsKey(Quotas.ORGANIZATIONS))
            {
                if ((cntx.Quotas[Quotas.ORGANIZATIONS].QuotaAllocatedValue > 0) || (cntx.Quotas[Quotas.ORGANIZATIONS].QuotaAllocatedValue == -1))
                {
                    MenuItem rootItem = new MenuItem(locMenuTitle.Text);
                    rootItem.Value = "ORGANIZATION MENU";
                    rootItem.Selectable = false;

                    menu.Items.Add(rootItem);
                    MenuItem item = new MenuItem(
                       "Hosted Organizations",
                       "",
                       "",
                      "~/Default.aspx?pid=SpaceExchangeServer&SpaceID=" + currentPackage);
                    makeSelectedMenu(item);
                    rootItem.ChildItems.Add(item);
                    this.PackageId = currentPackage;
                    BindMenu(rootItem.ChildItems);
                }
            }
        }
    }
}
