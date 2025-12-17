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
    public partial class SmaterMail_EditDomain_Sharing : FuseCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SaveItem(MailDomain item)
        {
            item.IsGlobalAddressList = cbGlobalAddressList.Checked;
            item.SharedCalendars = cbSharedCalendars.Checked;
            item.SharedContacts = cbSharedContacts.Checked;
            item.SharedFolders = cbSharedFolders.Checked;
            item.SharedNotes = cbSharedNotes.Checked;
            item.SharedTasks = cbSharedTasks.Checked;
            
        }

        public void BindItem(MailDomain item)
        {
            cbGlobalAddressList.Checked = item.IsGlobalAddressList;
            cbSharedCalendars.Checked = item.SharedCalendars;
            cbSharedContacts.Checked = item.SharedContacts;
            cbSharedFolders.Checked = item.SharedFolders;
            cbSharedNotes.Checked = item.SharedNotes;
            cbSharedTasks.Checked = item.SharedTasks;
        }
    }
}
