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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.Providers.Mail;

namespace FuseCP.Portal.ProviderControls
{
	public partial class hMailServer5_EditAccount : FuseCPControlBase, IMailEditAccountControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public void BindItem(MailAccount item)
		{
            chkEnabled.Checked = item.Enabled;
            lblSizeInfo.Text = item.Size.ToString() + " MB";
            lblQuotaUsedInfo.Text = item.QuotaUsed.ToString() + " %";
            lblLastLoginDateInfo.Text = item.LastLogonTime;
            chkResponderEnabled.Checked = item.ResponderEnabled;
            chkResponderExpires.Checked = item.ResponderExpires;
            txtResponderExireDate.Text = item.ResponderExpirationDate;
			txtSubject.Text = item.ResponderSubject;
			txtMessage.Text = item.ResponderMessage;
            chkForwardingEnabled.Checked = item.ForwardingEnabled;
			txtForward.Text = item.ForwardingAddresses != null ? String.Join("; ", item.ForwardingAddresses) : "";
            chkOriginalMessage.Checked = item.RetainLocalCopy;
            txtFirstName.Text = item.FirstName;
            txtLastName.Text = item.LastName;
            cbSignatureEnabled.Checked = item.SignatureEnabled;
            txtPlainSignature.Text = item.Signature;
            txtHtmlSignature.Text = item.SignatureHTML;
            secStatusInfo.IsCollapsed = false;            
		}

		public void SaveItem(MailAccount item)
		{
            item.Enabled = chkEnabled.Checked;
			item.ResponderEnabled = chkResponderEnabled.Checked;
			item.ResponderSubject = txtSubject.Text;
			item.ResponderMessage = txtMessage.Text;
            item.ResponderExpires = chkResponderExpires.Checked;
            if (txtResponderExireDate.Text.Trim().Length >= 10) {
                item.ResponderExpirationDate = txtResponderExireDate.Text.Trim().Substring(0, 10);}
            item.ForwardingEnabled = chkForwardingEnabled.Checked;
            if (txtForward.Text.Trim().Length > 0) {
                item.ForwardingAddresses = new string[] { txtForward.Text.Trim() };}
            item.RetainLocalCopy = chkOriginalMessage.Checked;
            item.FirstName = txtFirstName.Text;
            item.LastName = txtLastName.Text;
            item.SignatureEnabled = cbSignatureEnabled.Checked;
            item.Signature = txtPlainSignature.Text;
            item.SignatureHTML = txtHtmlSignature.Text;
		}
	}
}
