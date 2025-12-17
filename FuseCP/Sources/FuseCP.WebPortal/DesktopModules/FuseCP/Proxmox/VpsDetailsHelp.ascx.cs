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
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;

namespace FuseCP.Portal.Proxmox
{
    public partial class VpsDetailsHelp : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSummaryInfo();
            }
        }

        private void BindSummaryInfo()
        {
            // bind user details
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            if (package != null)
            {
                UserInfo user = ES.Services.Users.GetUserById(package.UserId);
                if (user != null)
                {
                    txtTo.Text = user.Email;
                }
            }

            // load template
            string content = ES.Services.Proxmox.GetVirtualMachineSummaryText(PanelRequest.ItemID);
            if (content != null)
                litContent.Text = content;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.Proxmox.SendVirtualMachineSummaryLetter(
                    PanelRequest.ItemID, txtTo.Text.Trim(), txtBCC.Text.Trim());

                if (res.IsSuccess)
                {
                    // bind tree
                    messageBox.ShowSuccessMessage("VPS_ERROR_SEND_SUMMARY_LETTER");
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "ERROR_SEND_SUMMARY_LETTER", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_SEND_SUMMARY_LETTER", ex);
            }
        }
    }
}
