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
    public partial class IceWarp_EditList : FuseCPControlBase, IMailEditListControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var moderators = ES.Services.MailServers.GetMailAccounts(PanelSecurity.PackageId, true);
                ddlListModerators.DataSource = moderators;
                ddlListModerators.DataBind();
            }

            txtMaxMembersValidator.MaximumValue = int.MaxValue.ToString();
            txtMaxMessageSizeValidator.MaximumValue = int.MaxValue.ToString();
            txtMaxMessagesPerMinuteValidator.MaximumValue = int.MaxValue.ToString();
        }

        public void BindItem(MailList item)
        {
            txtDescription.Text = item.Description;
            Utils.SelectListItem(ddlListModerators, item.ModeratorAddress);
            Utils.SelectListItem(ddlMembersSource, item.MembersSource.ToString());
            mailEditItems.Items = item.Members;
            Utils.SelectListItem(ddlFromHeaderAction, item.FromHeader.ToString());
            Utils.SelectListItem(ddlReplyToHeaderAction, item.ReplyToHeader.ToString());
            txtFromHeaderValue.Text = item.ListFromAddress;
            txtReplyToHeaderValue.Text = item.ListReplyToAddress;
            txtSubjectPrefix.Text = item.SubjectPrefix;
            Utils.SelectListItem(ddllblOriginator, item.Originator.ToString());
            Utils.SelectListItem(ddlPostingMode, item.PostingMode.ToString());
            Utils.SelectListItem(ddlPasswordProtection, item.PasswordProtection.ToString());
            txtPassword.Text = item.Password;
            Utils.SelectListItem(ddlDefaultRights, ((int) item.DefaultRights).ToString());
            txtMaxMessageSize.Text = item.MaxMessageSize.ToString();
            txtMaxMembers.Text = item.MaxMembers.ToString();
            chkSendToSender.Checked = item.SendToSender;
            chkSetRecipientToToHeader.Checked = item.SetReceipientsToToHeader;
            chkDigestMailingList.Checked = item.DigestMode;
            txtMaxMessagesPerMinute.Text = item.MaxMessagesPerMinute.ToString();
            chkSendSubscribe.Checked = item.SendSubscribe;
            chkSendUnSubscribe.Checked = item.SendUnsubscribe;
            Utils.SelectListItem(ddlConfirmSubscription, item.ConfirmSubscription.ToString());
            chkCommandInSubject.Checked = item.CommandsInSubject;
            chkEnableSubscribe.Checked = !item.DisableSubscribecommand;
            chkEnableUnsubscribe.Checked = item.AllowUnsubscribe;
            chkEnableLists.Checked = !item.DisableListcommand;
            chkEnableWhich.Checked = !item.DisableWhichCommand;
            chkEnableReview.Checked = !item.DisableReviewCommand;
            chkEnableVacation.Checked = !item.DisableVacationCommand;
            chkModerated.Checked = item.Moderated;
            txtCommandPassword.Text = item.CommandPassword;
            chkSuppressCommandResponses.Checked = item.SuppressCommandResponses;

            ddlMembersSource_SelectedIndexChanged(this, null);
            ddlFromHeaderAction_SelectedIndexChanged(this, null);
            ddlReplyToHeaderAction_SelectedIndexChanged(this, null);
            chkModerated_CheckedChanged(this, null);
            ddlPasswordProtection_SelectedIndexChanged(this, null);
        }

        public void SaveItem(MailList item)
        {
            item.Description = txtDescription.Text;
            item.ModeratorAddress = ddlListModerators.SelectedValue;
            item.MembersSource = (IceWarpListMembersSource)Enum.Parse(typeof (IceWarpListMembersSource), ddlMembersSource.SelectedValue);
            item.Members = mailEditItems.Items;
            item.FromHeader = (IceWarpListFromAndReplyToHeader)Enum.Parse(typeof (IceWarpListFromAndReplyToHeader), ddlFromHeaderAction.SelectedValue);
            item.ReplyToHeader = (IceWarpListFromAndReplyToHeader)Enum.Parse(typeof (IceWarpListFromAndReplyToHeader), ddlReplyToHeaderAction.SelectedValue);
            item.ListFromAddress = txtFromHeaderValue.Text;
            item.ListReplyToAddress = txtReplyToHeaderValue.Text;
            item.SubjectPrefix = txtSubjectPrefix.Text;
            item.Originator = (IceWarpListOriginator)Enum.Parse(typeof (IceWarpListOriginator), ddllblOriginator.SelectedValue);
            item.PostingMode = (PostingMode)Enum.Parse(typeof (PostingMode), ddlPostingMode.SelectedValue);
            item.PasswordProtection = (PasswordProtection)Enum.Parse(typeof (PasswordProtection), ddlPasswordProtection.SelectedValue);
            item.Password = txtPassword.Text;
            item.DefaultRights = (IceWarpListDefaultRights)Enum.Parse(typeof (IceWarpListDefaultRights), ddlDefaultRights.SelectedValue);
            item.MaxMessageSize = Convert.ToInt32(txtMaxMessageSize.Text);
            item.MaxMembers = Convert.ToInt32(txtMaxMembers.Text);
            item.SetReceipientsToToHeader = chkSetRecipientToToHeader.Checked;
            item.SendToSender = chkSendToSender.Checked;
            item.DigestMode = chkDigestMailingList.Checked;
            item.MaxMessagesPerMinute = Convert.ToInt32(txtMaxMessagesPerMinute.Text);
            item.SendSubscribe = chkSendSubscribe.Checked;
            item.SendUnsubscribe = chkSendUnSubscribe.Checked;
            item.ConfirmSubscription = (IceWarpListConfirmSubscription)Enum.Parse(typeof (IceWarpListConfirmSubscription), ddlConfirmSubscription.SelectedValue);
            item.CommandsInSubject = chkCommandInSubject.Checked;
            item.DisableSubscribecommand = !chkEnableSubscribe.Checked;
            item.AllowUnsubscribe = chkEnableUnsubscribe.Checked;
            item.DisableListcommand = !chkEnableLists.Checked;
            item.DisableWhichCommand = !chkEnableWhich.Checked;
            item.DisableReviewCommand = !chkEnableReview.Checked;
            item.DisableVacationCommand = !chkEnableVacation.Checked;
            item.Moderated = chkModerated.Checked;
            item.CommandPassword = txtCommandPassword.Text;
            item.SuppressCommandResponses = chkSuppressCommandResponses.Checked;
        }


        protected void ddlMembersSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            MembersRow.Visible = (IceWarpListMembersSource) Enum.Parse(typeof (IceWarpListMembersSource), ddlMembersSource.SelectedValue) == IceWarpListMembersSource.MembersInFile;
        }

        protected void ddlFromHeaderAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            var setToValueChoosen = (IceWarpListFromAndReplyToHeader) Enum.Parse(typeof (IceWarpListFromAndReplyToHeader), ddlFromHeaderAction.SelectedValue) == IceWarpListFromAndReplyToHeader.SetToValue;
            rowFromHeaderValue.Visible = setToValueChoosen;
            //reqValFromHeaderValue.Enabled = setToValueChoosen;
        }

        protected void ddlReplyToHeaderAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            var setToValueChoosen = (IceWarpListFromAndReplyToHeader) Enum.Parse(typeof (IceWarpListFromAndReplyToHeader), ddlReplyToHeaderAction.SelectedValue) == IceWarpListFromAndReplyToHeader.SetToValue;
            rowReplyToHeaderValue.Visible = setToValueChoosen;
            //reqValReplyToHeaderValue.Enabled = setToValueChoosen;
        }

        protected void chkModerated_CheckedChanged(object sender, EventArgs e)
        {
            rowCommandPassword.Visible = chkModerated.Checked;
        }

        protected void ddlPasswordProtection_SelectedIndexChanged(object sender, EventArgs e)
        {
            rowPostingPassword.Visible = ddlPasswordProtection.SelectedValue == "NoProtection";
        }
    }
}

