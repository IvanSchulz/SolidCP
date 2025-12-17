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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.Providers.Virtualization;
using FuseCP.WebPortal;
using FuseCP.EnterpriseServer;
using System.Text;

namespace FuseCP.Portal.Proxmox
{
    public partial class VdcPrivateNetwork : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                searchBox.AddCriteria("IPAddress", GetLocalizedString("SearchField.IPAddress"));
                searchBox.AddCriteria("ItemName", GetLocalizedString("SearchField.ItemName"));
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();
        }

        public string GetServerEditUrl(string itemID)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vps_general",
                    "ItemID=" + itemID);
        }

        protected void odsPrivateAddressesPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_GET_PRIVATE_NETWORK", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'PackagePrivateIPAddresses'");
            res.Append(", RedirectUrl: '" + GetServerEditUrl("{0}").Substring(2) + "'");
            res.Append(", PackageID: " + (String.IsNullOrEmpty(Request["SpaceID"]) ? "0" : Request["SpaceID"]));
            res.Append(", VPSTypeID: 'Proxmox'");
            return res.ToString();
        }
    }
}
