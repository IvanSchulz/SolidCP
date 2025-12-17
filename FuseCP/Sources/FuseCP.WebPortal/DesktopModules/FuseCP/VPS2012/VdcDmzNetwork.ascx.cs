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
using System.Text;

namespace FuseCP.Portal.VPS2012
{
    public partial class VdcDmzNetwork : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                searchBox.AddCriteria("IPAddress", GetLocalizedString("SearchField.IPAddress"));
                searchBox.AddCriteria("ItemName", GetLocalizedString("SearchField.ItemName"));
                packageVLANs.ManageAllowed = VirtualMachines2012Helper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);
                packageVLANs.IsDmz = true;
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();
        }

        public string GetServerEditUrl(string itemID)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "vps_general",
                    "ItemID=" + itemID);
        }

        protected void odsDmzAddressesPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_GET_DMZ_NETWORK", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'PackageDmzIPAddresses'");
            res.Append(", RedirectUrl: '" + GetServerEditUrl("{0}").Substring(2) + "'");
            res.Append(", PackageID: " + (String.IsNullOrEmpty(Request["SpaceID"]) ? "0" : Request["SpaceID"]));
            res.Append(", VPSTypeID: 'VPS2012'");
            return res.ToString();
        }
    }
}
