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
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

using FuseCP.EnterpriseServer;
using FuseCP.WebPortal;
using FuseCP.Portal.UserControls;


namespace FuseCP.Portal
{
    public partial class UserOrganization : OrganizationMenuControl
    {

        int packageId = 0;
        override public int PackageId 
        {
            get 
            {
                // test
                //return 1; 
                return packageId; 
            }
            set
            {
                packageId = value;
            }
        }

        int itemID = 0;
        override public int ItemID
        {
            get 
            {
                // test
                //return 1;
                if (itemID != 0) return itemID;
                if (PackageId == 0) return 0;

                DataTable orgs = new OrganizationsHelper().GetOrganizations(PackageId, false);

                for (int j = 0; j < orgs.Rows.Count; j++)
                {
                    DataRow org = orgs.Rows[j];
                    int iId = (int)org["ItemID"];

                    if (itemID == 0)
                        itemID = iId;

                    object isDefault = org["IsDefault"];
                    if (isDefault is bool)
                    {
                        if ((bool)isDefault)
                        {
                            itemID = iId;
                            break;
                        }
                    }
                }

                return itemID; 
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ShortMenu = false;
            ShowImg = true;
            PutBlackBerryInExchange = true;


            if ((PackageId > 0) && (Cntx.Groups.ContainsKey(ResourceGroups.HostedOrganizations)))
            {
                MenuItemCollection items = new MenuItemCollection();

                OrganizationMenuRoot = new MenuItem(GetLocalizedString("Text.OrganizationGroup"), "", "", null);
                items.Add(OrganizationMenuRoot);

                if (ItemID > 0)
                {
                    OrganizationMenuRoot.ChildItems.Add(CreateMenuItem("OrganizationHome", "organization_home", @"Icons/organization_home_48.png"));
                    BindMenu(items);
                }
                else
                {
                    OrganizationMenuRoot.ChildItems.Add(CreateMenuItem("CreateOrganization", "create_organization", @"Icons/create_organization_48.png"));
                }


                UserOrgPanel.Visible = true;

                OrgList.DataSource = items;
                OrgList.DataBind();
            }
            else
                UserOrgPanel.Visible = false;

        }

        protected override MenuItem CreateMenuItem(string text, string key, string img)
        {
            string PID_SPACE_EXCHANGE_SERVER = "SpaceExchangeServer";

            MenuItem item = new MenuItem();

            item.Text = GetLocalizedString("Text." + text);
            item.NavigateUrl = PortalUtils.NavigatePageURL( PID_SPACE_EXCHANGE_SERVER, "ItemID", ItemID.ToString(),
                PortalUtils.SPACE_ID_PARAM + "=" + PackageId, DefaultPage.CONTROL_ID_PARAM + "=" + key,
                "moduleDefId=exchangeserver");

            if (img == null)
                item.ImageUrl = PortalUtils.GetThemedIcon("Icons/tool_48.png");
            else
                item.ImageUrl = PortalUtils.GetThemedIcon(img);

            return item;
        }

        public MenuItemCollection GetIconMenuItems(object menuItems)
        {
            return (MenuItemCollection)menuItems;
        }

        public bool IsIconMenuVisible(object menuItems)
        {
            return ((MenuItemCollection)menuItems).Count > 0;
        }

    }
}
