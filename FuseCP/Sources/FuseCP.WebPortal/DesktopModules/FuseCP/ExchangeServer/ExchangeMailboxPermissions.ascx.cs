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
using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxPermissions : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPermissions();

                if (GetLocalizedString("buttonPanel.OnSaveClientClick") != null)
                    buttonPanel.OnSaveClientClick = GetLocalizedString("buttonPanel.OnSaveClientClick");
            }

        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            SavePermissions();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            SavePermissions();

            Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                "mailboxes",
                "SpaceID=" + PanelSecurity.PackageId));
        }


        private void BindPermissions()
        {
            try
            {
                ExchangeMailbox mailbox =
                    ES.Services.ExchangeServer.GetMailboxPermissions(PanelRequest.ItemID, PanelRequest.AccountID);

                litDisplayName.Text = mailbox.DisplayName;
                sendAsPermission.SetAccounts(mailbox.SendAsAccounts);
                fullAccessPermission.SetAccounts(mailbox.FullAccessAccounts);
                onBehalfOfPermissions.SetAccounts(mailbox.OnBehalfOfAccounts);
                calendarPermissions.SetAccounts(mailbox.CalendarAccounts);
                contactsPermissions.SetAccounts(mailbox.ContactAccounts);

                // get account meta
                ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);

                if (account.AccountType == ExchangeAccountType.SharedMailbox)
                    litDisplayName.Text += GetSharedLocalizedString("SharedMailbox.Text");

                if (account.AccountType == ExchangeAccountType.Room)
                    litDisplayName.Text += GetSharedLocalizedString("RoomMailbox.Text");

                if (account.AccountType == ExchangeAccountType.Equipment)
                    litDisplayName.Text += GetSharedLocalizedString("EquipmentMailbox.Text");

            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_MAILBOX_PERMISSIONS", ex);
            }
        }


        private void SavePermissions()
        {
            try
            {
                string[] fullAccess = fullAccessPermission.GetAccounts();
                string[] sendAs = sendAsPermission.GetAccounts();
                string[] onBehalf = onBehalfOfPermissions.GetAccounts();
                var calendar = calendarPermissions.GetAccounts();
                var contacts = contactsPermissions.GetAccounts();
                
                int result =
                    ES.Services.ExchangeServer.SetMailboxPermissions(PanelRequest.ItemID, PanelRequest.AccountID, sendAs, fullAccess, onBehalf, calendar, contacts);


                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }



                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_MAILBOX_PERMISSIONS");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_MAILBOX_PERMISSIONS", ex);
            }
        }
    }
}
