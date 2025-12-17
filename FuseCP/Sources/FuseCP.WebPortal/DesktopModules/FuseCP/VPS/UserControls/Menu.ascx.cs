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

﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.VPS.UserControls
{
    public partial class Menu : FuseCPControlBase
    {
        public class MenuItem
        {
            private string url;
            private string text;
            private string key;

            public string Url
            {
                get { return url; }
                set { url = value; }
            }

            public string Text
            {
                get { return text; }
                set { text = value; }
            }

            public string Key
            {
                get { return key; }
                set { key = value; }
            }
        }

        private string selectedItem;
        public string SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindMenu();
        }

        private void BindMenu()
        {
            bool isAdmin = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);

            // build the list of menu items
            List<MenuItem> items = new List<MenuItem>();

            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            
            // add items
            items.Add(CreateMenuItem("Vps", ""));

            if(cntx.Quotas.ContainsKey(Quotas.VPS_EXTERNAL_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.VPS_EXTERNAL_NETWORK_ENABLED].QuotaExhausted
                || (PanelSecurity.PackageId == 1 && isAdmin))
                items.Add(CreateMenuItem("ExternalNetwork", "vdc_external_network"));

            if (isAdmin)
                items.Add(CreateMenuItem("ManagementNetwork", "vdc_management_network"));

            if (cntx.Quotas.ContainsKey(Quotas.VPS_PRIVATE_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.VPS_PRIVATE_NETWORK_ENABLED].QuotaExhausted)
                items.Add(CreateMenuItem("PrivateNetwork", "vdc_private_network"));

            //items.Add(CreateMenuItem("UserPermissions", "vdc_permissions"));
            items.Add(CreateMenuItem("AuditLog", "vdc_audit_log"));

            // selected menu item
            for (int i = 0; i < items.Count; i++)
            {
                if (String.Compare(items[i].Key, SelectedItem, true) == 0)
                {
                    MenuItems.SelectedIndex = i;
                    break;
                }
            }

            // bind items
            MenuItems.DataSource = items;
            MenuItems.DataBind();
        }

        private MenuItem CreateMenuItem(string text, string key)
        {
            MenuItem item = new MenuItem();
            item.Key = key;
            item.Text = GetLocalizedString("Text." + text);
            item.Url = HostModule.EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), key);
            return item;
        }
    }
}
