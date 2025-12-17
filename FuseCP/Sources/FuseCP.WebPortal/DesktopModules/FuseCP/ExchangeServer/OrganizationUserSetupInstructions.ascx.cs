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
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class OrganizationUserSetupInstructions : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindInstructions();

                MailboxTabs.Visible = (PanelRequest.Context == "Mailbox");
                UserTabs.Visible = (PanelRequest.Context == "User");
            }

        }

        private void BindInstructions()
        {
            // load content
            litContent.Text = ES.Services.Organizations.GetOrganizationUserSummuryLetter(
                PanelRequest.ItemID, PanelRequest.AccountID,
                false, false, false);

            // bind user details
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            if (package == null)
                RedirectSpaceHomePage();

            OrganizationUser account = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID,
                                                                                        PanelRequest.AccountID);
            if (account != null)
                txtTo.Text = account.ExternalEmail;

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                int result = ES.Services.Organizations.SendOrganizationUserSummuryLetter(
                    PanelRequest.ItemID, PanelRequest.AccountID, false,
                    txtTo.Text.Trim(), txtCC.Text.Trim());

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                messageBox.ShowSuccessMessage("ORGANIZATION_LETTER_SEND");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ORGANIZATION_LETTER_SEND", ex);
                return;
            }
        }
    }
}
