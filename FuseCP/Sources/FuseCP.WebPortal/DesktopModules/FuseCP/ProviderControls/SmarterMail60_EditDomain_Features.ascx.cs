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
    public partial class SmarterMail60_EditDomain_Features : FuseCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SaveItem(MailDomain item)
        {
            item.ShowContentFilteringMenu = cbShowcontentfilteringmenu.Checked;
            item.ShowDomainAliasMenu = cbShowdomainaliasmenu.Checked;
            item.ShowListMenu = cbShowlistmenu.Checked;
            item.ShowSpamMenu = cbShowspammenu.Checked;
            item[MailDomain.SMARTERMAIL5_SHOW_DOMAIN_REPORTS] = cbShowDomainReports.Checked.ToString();
            item[MailDomain.SMARTERMAIL5_POP_RETREIVAL_ENABLED] = cbEnablePopRetreival.Checked.ToString();
            item[MailDomain.SMARTERMAIL5_CATCHALLS_ENABLED] = cbEnableCatchAlls.Checked.ToString();
            item[MailDomain.SMARTERMAIL6_IMAP_RETREIVAL_ENABLED] = cbEnableIMAPRetreival.ToString();
            item[MailDomain.SMARTERMAIL6_MAIL_SIGNING_ENABLED] = cbEnableEmailSigning.ToString();
            item[MailDomain.SMARTERMAIL6_EMAIL_REPORTS_ENABLED] = cbEnableEmailReports.ToString();
            item[MailDomain.SMARTERMAIL6_SYNCML_ENABLED] = cbEnableSyncML.ToString();
        }

        public void BindItem(MailDomain item)
        {
            cbShowcontentfilteringmenu.Checked = item.ShowContentFilteringMenu;
            cbShowdomainaliasmenu.Checked = item.ShowDomainAliasMenu;
            cbShowlistmenu.Checked = item.ShowListMenu;
            cbShowspammenu.Checked = item.ShowSpamMenu;
            cbShowDomainReports.Checked = Convert.ToBoolean(item[MailDomain.SMARTERMAIL5_SHOW_DOMAIN_REPORTS]);
            cbEnablePopRetreival.Checked = Convert.ToBoolean(item[MailDomain.SMARTERMAIL5_POP_RETREIVAL_ENABLED]);
            cbEnableCatchAlls.Checked = Convert.ToBoolean(item[MailDomain.SMARTERMAIL5_CATCHALLS_ENABLED]);
            cbEnableIMAPRetreival.Checked = Convert.ToBoolean(item[MailDomain.SMARTERMAIL6_IMAP_RETREIVAL_ENABLED]);
            cbEnableEmailSigning.Checked = Convert.ToBoolean(item[MailDomain.SMARTERMAIL6_MAIL_SIGNING_ENABLED]);
            cbEnableEmailReports.Checked = Convert.ToBoolean((item[MailDomain.SMARTERMAIL6_EMAIL_REPORTS_ENABLED]));
            cbEnableSyncML.Checked = Convert.ToBoolean(item[MailDomain.SMARTERMAIL6_SYNCML_ENABLED]);
        }
    }
}
