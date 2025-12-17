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
using System.Web.UI;
using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxAutoReply : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSettings();
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                ExchangeMailbox mailbox = ES.Services.ExchangeServer.GetMailboxGeneralSettings(PanelRequest.ItemID,
                    PanelRequest.AccountID);

                ExchangeMailboxAutoReplySettings autoReply = ES.Services.ExchangeServer.GetMailboxAutoReplySettings(PanelRequest.ItemID, PanelRequest.AccountID);

                // title
                litDisplayName.Text = mailbox.DisplayName;

                // auto reply settings
                rblSetAutoreply.SelectedIndex = (autoReply.AutoReplyState != OofState.Disabled) ? 1 : 0;
                chkAutoReplyTime.Checked = autoReply.AutoReplyState == OofState.Scheduled;
                chkOutsideOrganization.Checked = autoReply.ExternalAudience != ExternalAudience.None;
                txtIntReply.Text = autoReply.InternalMessage;
                txtExtReply.Text = autoReply.ExternalMessage;
                txtStartTime.Text = autoReply.StartTime.ToString("HH:mm");
                txtStartDate.Text = autoReply.StartTime.ToString("yyyy-MM-dd");
                txtEndTime.Text = autoReply.EndTime.ToString("HH:mm");
                txtEndDate.Text = autoReply.EndTime.ToString("yyyy-MM-dd");
                rblExternalAudience.SelectedIndex = (autoReply.ExternalAudience == ExternalAudience.Known) ? 0 : 1;
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_AUTOREPLY_SETTINGS", ex);
            }
            ToggleControls();
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                ExchangeMailboxAutoReplySettings autoReply = new ExchangeMailboxAutoReplySettings();
                autoReply.AutoReplyState = (rblSetAutoreply.SelectedIndex == 0) ? OofState.Disabled : (chkAutoReplyTime.Checked) ? OofState.Scheduled : OofState.Enabled;
                autoReply.ExternalAudience = (!chkOutsideOrganization.Checked) ? ExternalAudience.None : (rblExternalAudience.SelectedIndex == 0) ? ExternalAudience.Known : ExternalAudience.All;
                autoReply.InternalMessage = txtIntReply.Text;
                autoReply.ExternalMessage = txtExtReply.Text;
                autoReply.StartTime = DateTime.Parse(txtStartDate.Text + " " + txtStartTime.Text);
                autoReply.EndTime = DateTime.Parse(txtEndDate.Text + " " + txtEndTime.Text);
                int result = ES.Services.ExchangeServer.SetMailboxAutoReplySettings(PanelRequest.ItemID, PanelRequest.AccountID, autoReply);
                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }
                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_AUTOREPLY_SETTINGS");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_AUTOREPLY_SETTINGS", ex);
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
            bool showAll = rblSetAutoreply.SelectedIndex == 1;
            bool showTime = chkAutoReplyTime.Checked;
            bool showExternal = chkOutsideOrganization.Checked;

            txtIntReply.Visible = showAll;
            locIntReply.Visible = showAll;
            chkAutoReplyTime.Visible = showAll;
            locStartTime.Visible = showAll && showTime;
            locEndTime.Visible = showAll && showTime;
            txtStartTime.Visible = showAll && showTime;
            txtEndTime.Visible = showAll && showTime;
            txtStartDate.Visible = showAll && showTime;
            txtEndDate.Visible = showAll && showTime;
            chkOutsideOrganization.Visible = showAll;
            rblExternalAudience.Visible = showAll && showExternal;
            txtExtReply.Visible = showAll && showExternal;
            locExtReply.Visible = showAll && showExternal;
        }

        protected void rblSetAutoreply_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void chkAutoReplyTime_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void chkOutsideOrganization_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }
    }
}
