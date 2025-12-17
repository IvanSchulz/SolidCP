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
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxMailFlowSettings : FuseCPModuleBase
    {
        private StringDictionary ConvertArrayToDictionary(string[] settings)
        {
            StringDictionary r = new StringDictionary();
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
            }
            return r;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int serviceId = ES.Services.ExchangeServer.GetExchangeServiceID(PanelRequest.ItemID);
            StringDictionary settings = ConvertArrayToDictionary(ES.Services.Servers.GetServiceSettingsRDS(serviceId));
            var AllowSentItems = Utils.ParseBool(settings["ex2016cu6orhigher"], false);
            if (!AllowSentItems)
            {
                tablesavesentitems.Visible = false;
            }
            if (!IsPostBack)
            {
                BindSettings();

                if (GetLocalizedString("buttonPanel.OnSaveClientClick") != null)
                    buttonPanel.OnSaveClientClick = GetLocalizedString("buttonPanel.OnSaveClientClick");
            }

        }

        private void BindSettings()
        {
            try
            {
                // get settings
                ExchangeMailbox mailbox = ES.Services.ExchangeServer.GetMailboxMailFlowSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID);

                // title
                litDisplayName.Text = mailbox.DisplayName;

                // bind form
                chkEnabledForwarding.Checked = mailbox.EnableForwarding;
                forwardingAddress.SetAccount(mailbox.ForwardingAccount);
                chkDoNotDeleteOnForward.Checked = mailbox.DoNotDeleteOnForward;

                accessAccounts.SetAccounts(mailbox.SendOnBehalfAccounts);


                if (mailbox.SaveSentItems == 1)
                {
                    // Enabled
                    chkSaveSentItems.Checked = true;
                }
                else
                {
                    // Disabled
                    chkSaveSentItems.Checked = false;
                }


                acceptAccounts.SetAccounts(mailbox.AcceptAccounts);
                chkSendersAuthenticated.Checked = mailbox.RequireSenderAuthentication;
                rejectAccounts.SetAccounts(mailbox.RejectAccounts);

                // toggle
                ToggleControls();

                // get account meta
                ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);
                chkPmmAllowed.Checked = (account.MailboxManagerActions & MailboxManagerActions.MailFlowSettings) > 0;

            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_MAILBOX_MAILFLOW", ex);
            }
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                int SaveSentItems = 0;
                if (!tablesavesentitems.Visible)
                {
                    // Not CU6
                    SaveSentItems = 0;
                }
                else
                {
                    if (chkSaveSentItems.Checked)
                    {
                        // Enabled
                        SaveSentItems = 1;
                    }
                    else
                    {
                        // Disabled
                        SaveSentItems = 2;
                    }
                }

                int result = ES.Services.ExchangeServer.SetMailboxMailFlowSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID,

                    chkEnabledForwarding.Checked,
                    SaveSentItems,
                    forwardingAddress.GetAccount(),
                    chkDoNotDeleteOnForward.Checked,

                    accessAccounts.GetAccounts(),
                    acceptAccounts.GetAccounts(),
                    rejectAccounts.GetAccounts(),

                    chkSendersAuthenticated.Checked);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_MAILBOX_MAILFLOW");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_MAILBOX_MAILFLOW", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            SaveSettings();

            Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                "mailboxes",
                "SpaceID=" + PanelSecurity.PackageId));
        }


        private void ToggleControls()
        {
            ForwardSettingsPanel.Visible = chkEnabledForwarding.Checked;
        }

        protected void chkEnabledForwarding_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void chkPmmAllowed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int result = ES.Services.ExchangeServer.SetMailboxManagerSettings(PanelRequest.ItemID, PanelRequest.AccountID,
                chkPmmAllowed.Checked, MailboxManagerActions.MailFlowSettings);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_MAILMANAGER");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_MAILMANAGER", ex);
            }
        }
    }
}
