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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using EntServer = FuseCP.EnterpriseServer;
using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxEmailAddresses : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindEmails();
            }

            
        }

        private void BindEmails()
        {
            EntServer.ExchangeEmailAddress[] emails = ES.Services.ExchangeServer.GetMailboxEmailAddresses(
                PanelRequest.ItemID, PanelRequest.AccountID);

            gvEmails.DataSource = emails;
            gvEmails.DataBind();

            lblTotal.Text = emails.Length.ToString();

            // form title
            ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);
            chkPmmAllowed.Checked = (account.MailboxManagerActions & MailboxManagerActions.EmailAddresses) > 0;

            litDisplayName.Text = account.DisplayName;

            //disable buttons if only one e-mail available, it is primary and cannot be deleted
            if (gvEmails.Rows.Count == 1)
            {
                btnDeleteAddresses.Enabled = false;
                btnSetAsPrimary.Enabled = false;
            }
        }

        protected void btnAddEmail_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            btnDeleteAddresses.Enabled = true;
            btnSetAsPrimary.Enabled = true;

            try
            {
                int result = ES.Services.ExchangeServer.AddMailboxEmailAddress(
                    PanelRequest.ItemID, PanelRequest.AccountID, email.Email.ToLower());

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                // rebind
                BindEmails();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_MAILBOX_ADD_EMAIL", ex);
            }

            // clear field
            email.AccountName = "";
        }

        protected void btnSetAsPrimary_Click(object sender, EventArgs e)
        {
            try
            {
                string email = null;
                bool Checked = false;

                for (int i = 0; i < gvEmails.Rows.Count; i++)
                {
                    GridViewRow row = gvEmails.Rows[i];
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        Checked = true;
                        email = gvEmails.DataKeys[i].Value.ToString();
                        break;
                    }
                }

                //check if any e-mail is selected to be primary
                if (!Checked)
                {
                    messageBox.ShowWarningMessage("PRIMARY_EMAIL_IS_NOT_CHECKED");
                }

                if (email == null)
                    return;

                int result = ES.Services.ExchangeServer.SetMailboxPrimaryEmailAddress(
                    PanelRequest.ItemID, PanelRequest.AccountID, email);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                // rebind
                BindEmails();

                messageBox.ShowSuccessMessage("EXCHANGE_MAILBOX_SET_DEFAULT_EMAIL");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_MAILBOX_SET_DEFAULT_EMAIL", ex);
            }
        }

        protected void btnDeleteAddresses_Click(object sender, EventArgs e)
        {
            // get selected e-mail addresses
            List<string> emails = new List<string>();
            bool containsUPN = false;
            EntServer.ExchangeEmailAddress[] tmpEmails = ES.Services.ExchangeServer.GetMailboxEmailAddresses( PanelRequest.ItemID, PanelRequest.AccountID);


            for (int i = 0; i < gvEmails.Rows.Count; i++)
            {
                GridViewRow row = gvEmails.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect.Checked)
                {
                    emails.Add(gvEmails.DataKeys[i].Value.ToString());
                    foreach (EntServer.ExchangeEmailAddress tmpEmail in tmpEmails)
                    {
                        if (gvEmails.DataKeys[i].Value.ToString() == tmpEmail.EmailAddress)
                        {
                            if (tmpEmail.IsUserPrincipalName)
                            {
                                containsUPN = true;
                                break;
                            }

                        }
                    }
                }
            }

            if (emails.Count == 0)
            {
                messageBox.ShowWarningMessage("DIST_LIST_SELECT_EMAILS_TO_DELETE");
            }

            try
            {
                int result = ES.Services.ExchangeServer.DeleteMailboxEmailAddresses(
                    PanelRequest.ItemID, PanelRequest.AccountID, emails.ToArray());

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }
                else
                {
                    if (containsUPN)
                        messageBox.ShowWarningMessage("NOT_ALL_EMAIL_ADDRESSES_DELETED");
                }

                // rebind
                BindEmails();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_MAILBOX_DELETE_EMAILS", ex);
            }
        }

        protected void chkPmmAllowed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int result = ES.Services.ExchangeServer.SetMailboxManagerSettings(PanelRequest.ItemID, PanelRequest.AccountID,
                chkPmmAllowed.Checked, MailboxManagerActions.EmailAddresses);

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
