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
    public partial class SmarterMail100x_EditDomain_Features : FuseCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SaveItem(MailDomain item)
        {
            item.ShowDomainAliasMenu = cbShowdomainaliasmenu.Checked;
            item.ShowListMenu = cbShowlistmenu.Checked;
            item.ShowSpamMenu = cbShowspammenu.Checked;
            item[MailDomain.SMARTERMAIL5_CATCHALLS_ENABLED] = cbEnableCatchAlls.Checked.ToString();
        }

        public void BindItem(MailDomain item)
        {
            cbShowdomainaliasmenu.Checked = item.ShowDomainAliasMenu;
            cbShowlistmenu.Checked = item.ShowListMenu;
            cbShowspammenu.Checked = item.ShowSpamMenu;
            cbEnableCatchAlls.Checked = Convert.ToBoolean(item[MailDomain.SMARTERMAIL5_CATCHALLS_ENABLED]);
        }
    }
}
