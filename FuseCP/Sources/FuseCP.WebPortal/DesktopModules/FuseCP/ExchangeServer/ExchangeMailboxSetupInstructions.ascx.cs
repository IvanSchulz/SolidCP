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
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxSetupInstructions : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindInstructions();
            }

        }

        private void BindInstructions()
        {
            // load content
            litContent.Text = ES.Services.ExchangeServer.GetMailboxSetupInstructions(
                PanelRequest.ItemID, PanelRequest.AccountID,
                false, false, false,
                PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                "user_reset_password",
                "SpaceID=" + PanelSecurity.PackageId,
                "Context=Mailbox",
                "AccountID=" + PanelRequest.AccountID).Trim('~'));

            // bind user details
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            if (package == null)
                RedirectSpaceHomePage();

            // load user details
            UserInfo user = ES.Services.Users.GetUserById(package.UserId);
            txtTo.Text = user.Email;

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                int result = ES.Services.ExchangeServer.SendMailboxSetupInstructions(
                    PanelRequest.ItemID, PanelRequest.AccountID, false,
                    txtTo.Text.Trim(), txtCC.Text.Trim());

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                messageBox.ShowSuccessMessage("EXCHANGE_LETTER_SEND");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_LETTER_SEND", ex);
                return;
            }
        }
    }
}
