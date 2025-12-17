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

using FuseCP.WebPortal;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class UserAccountTools : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkBackup.NavigateUrl = EditUrl(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(), "backup");
            lnkRestore.NavigateUrl = EditUrl(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(), "restore");

            lnkBackup.Visible = lnkRestore.Visible = PortalUtils.PageExists("Backup");

            if (PanelSecurity.SelectedUser.RoleId.Equals(1))
            {
                lnkBackup.Visible = lnkRestore.Visible = ToolsHeader.Visible = false;
            }

            UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

            if (user != null)
            {
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                if ((user.Role == UserRole.User) & (Utils.CheckQouta(Quotas.EXCHANGE2007_ISCONSUMER, cntx)))
                {
                    lnkBackup.Visible = lnkRestore.Visible = ToolsHeader.Visible = false;
                }
                    
            }

        }

    }
}
