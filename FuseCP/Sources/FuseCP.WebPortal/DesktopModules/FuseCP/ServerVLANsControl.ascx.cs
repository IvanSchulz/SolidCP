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
using FuseCP.Portal.Code.Helpers;

namespace FuseCP.Portal
{
    public partial class ServerVLANsControl : FuseCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvVLANs.PageIndex = PageIndex;
            }
        }

        public int PageIndex
        {
            get
            {
                return PanelRequest.GetInt("VLANsPage", 0);
            }
        }

        public string EditModuleUrl(string key, string keyVal, string ctrlKey)
        {
            return HostModule.EditUrl(key, keyVal, ctrlKey);
        }

        public string EditModuleUrl(string key, string keyVal, string ctrlKey, string key2, string keyVal2)
        {
            return HostModule.EditUrl(key, keyVal, ctrlKey, key2 + "=" + keyVal2);
        }

        public string GetReturnUrl()
        {
            var returnUrl = Request.Url
                .AddParameter("VLANsCollapsed", "False")
                .AddParameter("VLANsPage", gvVLANs.PageIndex.ToString());

            return Uri.EscapeDataString("~" + returnUrl.PathAndQuery);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditModuleUrl("ServerID", PanelRequest.ServerId.ToString(), "add_vlan", "ReturnUrl", GetReturnUrl()), true);
        }
    }
}
