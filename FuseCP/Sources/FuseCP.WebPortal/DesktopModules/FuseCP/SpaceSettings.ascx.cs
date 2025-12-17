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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class SpaceSettings : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // module visibility
            this.ContainerControl.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User && 
                PanelSecurity.PackageId > 1);

            lnkNameServers.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsNameServers",
                "edit_settings", "SettingsName=NameServers", "SpaceID=" + PanelSecurity.PackageId.ToString());
            lnkPreviewDomain.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsPreviewDomain",
                "edit_settings", "SettingsName=PreviewDomain", "SpaceID=" + PanelSecurity.PackageId.ToString());
            lnkSharedSSL.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsSharedSslSites",
                "edit_settings", "SettingsName=SharedSslSites", "SpaceID=" + PanelSecurity.PackageId.ToString());
            lnkPackagesFolder.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsSpacesFolder",
                "edit_settings", "SettingsName=ChildSpacesFolder", "SpaceID=" + PanelSecurity.PackageId.ToString());
			lnkExchangeServer.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsExchangeServer",
				"edit_settings", "SettingsName=ExchangeServer", "SpaceID=" + PanelSecurity.PackageId.ToString());
            lnkVps.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsVPS",
                "edit_settings", "SettingsName=VirtualPrivateServers", "SpaceID=" + PanelSecurity.PackageId.ToString());
            lnkVps2012.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsVPS2012",
                "edit_settings", "SettingsName=VirtualPrivateServers2012", "SpaceID=" + PanelSecurity.PackageId.ToString());
            lnkVpsForPC.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsVPSForPC",
                "edit_settings", "SettingsName=VirtualPrivateServersForPrivateCloud", "SpaceID=" + PanelSecurity.PackageId.ToString());

            lnkDnsRecords.NavigateUrl = EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_globaldns");

        }
    }
}
