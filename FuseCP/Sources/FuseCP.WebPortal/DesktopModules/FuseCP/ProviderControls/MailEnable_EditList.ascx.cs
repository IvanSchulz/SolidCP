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
    public partial class MailEnable_EditList : FuseCPControlBase, IMailEditListControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           HeaderFooterSection.Visible = pHeaderFooter.Visible = (PanelRequest.ItemID > 0);
        }

        public void BindItem(MailList item)
        {
            Utils.SelectListItem(ddlReplyTo, item.ReplyToMode);
            Utils.SelectListItem(ddlPostingMode, item.PostingMode);
            Utils.SelectListItem(ddlPrefixOption, item.PrefixOption);
            txtSubjectPrefix.Text = item.SubjectPrefix;
            chkModerationEnabled.Checked = item.Moderated;
            txtModeratorEmail.Text = item.ModeratorAddress;
            mailEditItems.Items = item.Members;
            if (!String.IsNullOrEmpty(item.Password))
            {
                txtPassword.Attributes["value"] = txtPassword.Text = item.Password;
            }
            else
            {
                txtPassword.Text = "";          
            }
            txtDescription.Text = item.Description;
      
            switch (item.AttachHeader)
            {
                case 1:
                    cbHeader.Checked = true;
                    break;
                case 0:
                    cbHeader.Checked = false;
                    break;
            }
            switch (item.AttachFooter)
            {
                case 1:
                    cbFooter.Checked = true;
                    break;
                case 0:
                    cbFooter.Checked = false;
                    break;
            }
            txtHeaderText.Text = item.TextHeader;
            txtFooterText.Text = item.TextFooter;
            txtHeaderHtml.Text = item.HtmlHeader;
            txtFooterHtml.Text = item.HtmlFooter;
        }



        public void SaveItem(MailList item)
        {
            item.ReplyToMode = (ReplyTo)Enum.Parse(typeof(ReplyTo), ddlReplyTo.SelectedValue, true);
            item.PostingMode = (PostingMode)Enum.Parse(typeof(PostingMode), ddlPostingMode.SelectedValue, true);
            item.PrefixOption = (PrefixOption) Enum.Parse(typeof (PrefixOption), ddlPrefixOption.SelectedValue, true);
            item.SubjectPrefix = txtSubjectPrefix.Text;
            item.Description = txtDescription.Text;
            // save password
            item.Password = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState["PWD"];
            item.Moderated = chkModerationEnabled.Checked;
            item.ModeratorAddress = txtModeratorEmail.Text;
            item.Members = mailEditItems.Items;
            item.AttachHeader = (cbHeader.Checked) ? 1 : 0;
            item.AttachFooter = (cbFooter.Checked) ? 1 : 0;
            item.TextHeader = txtHeaderText.Text;
            item.TextFooter = txtFooterText.Text;
            item.HtmlHeader = txtHeaderHtml.Text;
            item.HtmlFooter = txtFooterHtml.Text;
        }
    }
}
