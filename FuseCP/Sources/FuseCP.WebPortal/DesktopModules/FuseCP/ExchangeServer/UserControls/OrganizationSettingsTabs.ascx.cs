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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.Portal.Code.UserControls;

namespace FuseCP.Portal.ExchangeServer.UserControls
{
    public partial class OrganizationSettingsTabs : FuseCPControlBase
    {
        public string SelectedTab { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindTabs()
        {
            List<Tab> tabsList = new List<Tab>();
            tabsList.Add(CreateTab("organization_settings_general_settings", "Tab.GeneralSettigns"));
            tabsList.Add(CreateTab("organization_settings_password_settings", "Tab.PasswordSettings"));

            

            int idx = 0;

            foreach (Tab tab in tabsList)
            {
                if (String.Compare(tab.Id, SelectedTab, true) == 0)
                {
                    break;
                }

                idx++;
            }

            osTabs.SelectedIndex = idx;
            osTabs.DataSource = tabsList;
            osTabs.DataBind();
        }

        private Tab CreateTab(string id, string text)
        {
            return new Tab(id, GetLocalizedString(text),
                HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), id,
                "SpaceID=" + PanelSecurity.PackageId
                ));
        }
    }
}
