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
    public partial class SmarterMail100x_EditGroup : FuseCPControlBase, IMailEditGroupControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindItem(MailGroup item)
        {
            mailEditItems.Items = item.Members;
            chkEnableGAL.Checked = !item.HideFromGAL;
            chkEnableChat.Checked = item.EnableChat;
            chkInternalOnly.Checked = item.InternalOnly;
            chkIncludeAllDomainUsers.Checked = item.IncludeAllDomainUsers;
            txtDisplayName.Text = item.DisplayName;
            chkAllowSending.Checked = item.AllowSending;
        }

        public void SaveItem(MailGroup item)
        {
            item.Members = mailEditItems.Items;
            item.HideFromGAL = !chkEnableGAL.Checked;
            item.EnableChat = chkEnableChat.Checked;
            item.InternalOnly = chkInternalOnly.Checked;
            item.IncludeAllDomainUsers = chkIncludeAllDomainUsers.Checked;
            item.DisplayName = txtDisplayName.Text;
            item.AllowSending = chkAllowSending.Checked;
        }
    }

}
