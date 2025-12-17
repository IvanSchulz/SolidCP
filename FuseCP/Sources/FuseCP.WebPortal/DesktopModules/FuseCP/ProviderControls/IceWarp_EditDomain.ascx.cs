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
﻿using System.Collections.Generic;
﻿using System.Collections.Specialized;
﻿using System.Linq;
﻿using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Mail;

namespace FuseCP.Portal.ProviderControls
{
    public partial class IceWarp_EditDomain : FuseCPControlBase, IMailEditDomainControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AdvancedSettingsPanel.Visible = PanelSecurity.EffectiveUser.Role == UserRole.Administrator;
            MaxDomainDiskSpaceValidator.MaximumValue = int.MaxValue.ToString();
            MaxDomainUsersValidator.MaximumValue = int.MaxValue.ToString();
            txtLimitNumberValidator.MaximumValue = int.MaxValue.ToString();
            txtLimitVolumeValidator.MaximumValue = int.MaxValue.ToString();
            txtDefaultUserMaxMessageSizeMegaByteValidator.MaximumValue = int.MaxValue.ToString();
            txtDefaultUserMegaByteSendLimitValidator.MaximumValue = int.MaxValue.ToString();
            txtDefaultUserQuotaInMBValidator.MaximumValue = int.MaxValue.ToString();
            txtDefaultUserNumberSendLimitValidator.MaximumValue = int.MaxValue.ToString();
        }

        public void BindItem(MailDomain item)
        {
            // Hide/show controls when not enabled on service level
            rowMaxDomainDiskSpace.Visible = item.UseDomainDiskQuota;
            rowDomainLimits.Visible = item.UseDomainLimits;
            rowUserLimits.Visible = item.UseUserLimits;

            txtMaxDomainDiskSpace.Text = item.MaxDomainSizeInMB.ToString();
            txtMaxDomainUsers.Text = item.MaxDomainUsers.ToString();
            txtLimitVolume.Text = item.MegaByteSendLimit.ToString();
            txtLimitNumber.Text = item.NumberSendLimit.ToString();
            txtDefaultUserQuotaInMB.Text = item.DefaultUserQuotaInMB.ToString();
            txtDefaultUserMaxMessageSizeMegaByte.Text = item.DefaultUserMaxMessageSizeMegaByte.ToString();
            txtDefaultUserMegaByteSendLimit.Text = item.DefaultUserMegaByteSendLimit.ToString();
            txtDefaultUserNumberSendLimit.Text = item.DefaultUserNumberSendLimit.ToString();

            if (!IsPostBack)
            {
                var accounts = ES.Services.MailServers.GetMailAccounts(item.PackageId, false);
                ddlCatchAllAccount.DataSource = accounts;
                ddlCatchAllAccount.DataBind();
                ddlPostMasterAccount.DataSource = accounts;
                ddlPostMasterAccount.DataBind();
            }

            Utils.SelectListItem(ddlCatchAllAccount, item.CatchAllAccount);
            Utils.SelectListItem(ddlPostMasterAccount, item.PostmasterAccount);

        }

        public void SaveItem(MailDomain item)
        {
            item.CatchAllAccount = ddlCatchAllAccount.SelectedValue;
            item.PostmasterAccount = ddlPostMasterAccount.SelectedValue;
            item.MaxDomainSizeInMB = Convert.ToInt32(txtMaxDomainDiskSpace.Text);
            item.MaxDomainUsers = Convert.ToInt32(txtMaxDomainUsers.Text);
            item.NumberSendLimit = Convert.ToInt32(txtLimitNumber.Text);
            item.MegaByteSendLimit = Convert.ToInt32(txtLimitVolume.Text);
            item.DefaultUserQuotaInMB = Convert.ToInt32(txtDefaultUserQuotaInMB.Text);
            item.DefaultUserMaxMessageSizeMegaByte = Convert.ToInt32(txtDefaultUserMaxMessageSizeMegaByte.Text);
            item.DefaultUserMegaByteSendLimit = Convert.ToInt32(txtDefaultUserMegaByteSendLimit.Text);
            item.DefaultUserNumberSendLimit = Convert.ToInt32(txtDefaultUserNumberSendLimit.Text);
        }
    }
}
