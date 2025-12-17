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
using FuseCP.Providers.Mail;

namespace FuseCP.Portal.ProviderControls
{
    public partial class IceWarp_EditAccount : FuseCPControlBase, IMailEditAccountControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Hide some form items when creating a new account
            AutoresponderPanel.Visible = (PanelRequest.ItemID > 0);
            secAutoresponder.Visible = (PanelRequest.ItemID > 0);
            ForwardingPanel.Visible = (PanelRequest.ItemID > 0);
            secForwarding.Visible = (PanelRequest.ItemID > 0);
            OlderMailsPanel.Visible = (PanelRequest.ItemID > 0);
            secOlderMails.Visible = (PanelRequest.ItemID > 0);
            Utils.SelectListItem(ddlAccountType, "1");  // Set default account type to POP3 & IMAP
        }

        public void BindItem(MailAccount item)
        {
            txtFullName.Text = item.FullName;
            Utils.SelectListItem(ddlAccountType, item.IceWarpAccountType);
            Utils.SelectListItem(ddlAccountState, item.IceWarpAccountState);
            Utils.SelectListItem(ddlRespondType, item.IceWarpRespondType);
            chkRespondOnlyBetweenDates.Checked = item.RespondOnlyBetweenDates;

            // Set respond dates to something useful if they are null in IceWarp
            if (item.RespondFrom == DateTime.MinValue)
            {
                item.RespondFrom = DateTime.Today;
            }
            if (item.RespondTo == DateTime.MinValue)
            {
                item.RespondTo = DateTime.Today.AddDays(21);
            }
            calRespondFrom.SelectedDate = item.RespondFrom;
            calRespondTo.SelectedDate = item.RespondTo;

            chkRespondOnlyBetweenDates_CheckedChanged(this, null);

            txtRespondPeriodInDays.Text = item.RespondPeriodInDays.ToString();
            txtRespondWithReplyFrom.Text = item.RespondWithReplyFrom;
            txtSubject.Text = item.ResponderSubject;
            txtMessage.Text = item.ResponderMessage;
            txtForward.Text = item.ForwardingAddresses != null ? String.Join("; ", item.ForwardingAddresses) : "";
            cbDeleteOnForward.Checked = item.DeleteOnForward;
            cbDomainAdmin.Visible = item.IsDomainAdminEnabled;
            cbDomainAdmin.Checked = item.IsDomainAdmin;

            ddlRespondType_SelectedIndexChanged(this, null);

            cbForwardOlder.Checked = item.ForwardOlder;
            txtForwardOlderDays.Text = item.ForwardOlderDays.ToString();
            txtForwardOlderTo.Text = item.ForwardOlderTo;
            cbForwardOlder_CheckedChanged(this, null);

            cbDeleteOlder.Checked = item.DeleteOlder;
            txtDeleteOlderDays.Text = item.DeleteOlderDays.ToString();
            cbDeleteOlder_CheckedChanged(this, null);
        }

        public void SaveItem(MailAccount item)
        {
            item.FullName = txtFullName.Text;
            item.IceWarpAccountType = Convert.ToInt32(ddlAccountType.SelectedValue);
            item.IceWarpAccountState = Convert.ToInt32(ddlAccountState.SelectedValue);
            item.IceWarpRespondType = Convert.ToInt32(ddlRespondType.SelectedValue);
            if (!string.IsNullOrWhiteSpace(txtRespondPeriodInDays.Text))
            {
                item.RespondPeriodInDays = Convert.ToInt32(txtRespondPeriodInDays.Text);
            }
            item.RespondOnlyBetweenDates = chkRespondOnlyBetweenDates.Checked;
            item.RespondFrom = calRespondFrom.SelectedDate;
            item.RespondTo = calRespondTo.SelectedDate;
            item.RespondWithReplyFrom = txtRespondWithReplyFrom.Text;
            item.ResponderSubject = txtSubject.Text;
            item.ResponderMessage = txtMessage.Text;
            item.ForwardingEnabled = !string.IsNullOrWhiteSpace(txtForward.Text);
            item.ForwardingAddresses = Utils.ParseDelimitedString(txtForward.Text, ';', ' ', ',');
            item.DeleteOnForward = cbDeleteOnForward.Checked;
            item.IsDomainAdmin = cbDomainAdmin.Checked;

            item.DeleteOlder = cbDeleteOlder.Checked;
            item.DeleteOlderDays = string.IsNullOrWhiteSpace(txtDeleteOlderDays.Text) ? 0 : Convert.ToInt32(txtDeleteOlderDays.Text);

            item.ForwardOlder = cbForwardOlder.Checked;
            item.ForwardOlderDays = string.IsNullOrWhiteSpace(txtForwardOlderDays.Text) ? 0 : Convert.ToInt32(txtForwardOlderDays.Text);
            item.ForwardOlderTo = txtForwardOlderTo.Text;
        }

        protected void ddlRespondType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RespondPeriod.Visible = ddlRespondType.SelectedValue == "3";
            RespondEnabled.Visible = Convert.ToInt32(ddlRespondType.SelectedValue) > 0;
        }

        protected void cbForwardOlder_CheckedChanged(object sender, EventArgs e)
        {
            ForwardOlderEnabled.Visible = cbForwardOlder.Checked;
        }

        protected void cbDeleteOlder_CheckedChanged(object sender, EventArgs e)
        {
            DeleteOlderEnabled.Visible = cbDeleteOlder.Checked;
        }

        protected void chkRespondOnlyBetweenDates_CheckedChanged(object sender, EventArgs e)
        {
            RespondFrom.Visible = chkRespondOnlyBetweenDates.Checked;
            RespondTo.Visible = chkRespondOnlyBetweenDates.Checked;
        }
    }
}
