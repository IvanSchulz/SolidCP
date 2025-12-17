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
using System.Collections.Generic;
using FuseCP.Portal.Code.UserControls;
using FuseCP.WebPortal;
using FuseCP.EnterpriseServer;


namespace FuseCP.Portal.ExchangeServer.UserControls
{
    public partial class DistributionListTabs : FuseCPControlBase
    {
        private string selectedTab;
        public string SelectedTab
        {
            get { return selectedTab; }
            set { selectedTab = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindTabs()
        {
            List<Tab> tabsList = new List<Tab>();
            tabsList.Add(CreateTab("dlist_settings", "Tab.Settings"));
            tabsList.Add(CreateTab("dlist_addresses", "Tab.Addresses"));
            tabsList.Add(CreateTab("dlist_mailflow", "Tab.Mailflow"));
            tabsList.Add(CreateTab("dlist_permissions", "Tab.Permissions"));

            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (Utils.CheckQouta(Quotas.EXCHANGE2007_DISTRIBUTIONLISTS, cntx))
                tabsList.Add(CreateTab("dlist_memberof", "Tab.MemberOf"));

            // find selected menu item
            int idx = 0;
            foreach (Tab tab in tabsList)
            {
                if (String.Compare(tab.Id, SelectedTab, true) == 0)
                    break;
                idx++;
            }
            dlTabs.SelectedIndex = idx;

            dlTabs.DataSource = tabsList;
            dlTabs.DataBind();
        }

        private Tab CreateTab(string id, string text)
        {
            return new Tab(id, GetLocalizedString(text),
                HostModule.EditUrl("AccountID", PanelRequest.AccountID.ToString(), id,
                "SpaceID=" + PanelSecurity.PackageId.ToString(),
                "ItemID=" + PanelRequest.ItemID.ToString()));
        }
    }
}
