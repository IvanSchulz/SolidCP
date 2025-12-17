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
using FuseCP.Providers.Common;
using FuseCP.Providers.Mail;

namespace FuseCP.Portal.ProviderControls
{
    public partial class SmarterMail100_EditAccount : FuseCPControlBase, IMailEditAccountControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            passwordRow.Visible = (PanelRequest.ItemID > 0);
		}

        public void BindItem(MailAccount item)
        {
            txtFirstName.Text = item.FirstName;
            txtLastName.Text = item.LastName;
            txtSignature.Text = item.Signature;
            cbEnableAccount.Checked = item.Enabled;
            chkResponderEnabled.Checked = item.ResponderEnabled;
            txtReplyTo.Text = item.ReplyTo;
            txtSubject.Text = item.ResponderSubject;
            txtMessage.Text = item.ResponderMessage;
            txtForward.Text = item.ForwardingAddresses != null ? String.Join("; ", item.ForwardingAddresses) : "";
            chkDeleteOnForward.Checked = item.DeleteOnForward;
			if (item.IsDomainAdminEnabled)
			{
				domainAdminRow.Visible = item.IsDomainAdminEnabled;
				cbDomainAdmin.Checked = item.IsDomainAdmin;
			}
			else
			{
				domainAdminRow.Visible = item.IsDomainAdminEnabled;
				cbDomainAdmin.Checked = false;
			}
        }

        public void SaveItem(MailAccount item)
        {
            item.FirstName = txtFirstName.Text;
            item.LastName = txtLastName.Text;
            item.Signature = txtSignature.Text;
            item.ResponderEnabled = chkResponderEnabled.Checked;
            item.Enabled = cbEnableAccount.Checked;
            item.ReplyTo = txtReplyTo.Text;
            item.ResponderSubject = txtSubject.Text;
            item.ResponderMessage = txtMessage.Text;
            item.ForwardingAddresses = Utils.ParseDelimitedString(txtForward.Text, ';', ' ', ',');
            item.DeleteOnForward = chkDeleteOnForward.Checked;
            item.ChangePassword = cbChangePassword.Checked;
            item.ChangePassword = cbChangePassword.Checked;
            item.IsDomainAdmin = cbDomainAdmin.Checked;
        }
    }
}
