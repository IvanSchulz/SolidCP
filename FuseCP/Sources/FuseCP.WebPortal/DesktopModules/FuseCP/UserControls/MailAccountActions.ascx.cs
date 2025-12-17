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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Portal.UserControls;
using FuseCP.Providers;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal
{
    public enum MailAccountActionTypes
    {
        None = 0,
        Disable = 1,
        Enable = 2,
    }

    public partial class MailAccountActions : ActionListControlBase<MailAccountActionTypes>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override DropDownList ActionsList
        {
            get { return ddlMailAccountActions; }
        }

        protected override int DoAction(List<int> ids)
        {
            switch (SelectedAction)
            {
                case MailAccountActionTypes.Disable:
                    return ChangeMailAccountState(false, ids);
                case MailAccountActionTypes.Enable:
                    return ChangeMailAccountState(true, ids);
            }

            return 0;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            switch (SelectedAction)
            {
                case MailAccountActionTypes.Disable:
                case MailAccountActionTypes.Enable:
                    FireExecuteAction();
                    break;
            }
        }

        private int ChangeMailAccountState(bool enable, List<int> ids)
        {
            foreach (var id in ids)
            {
                var mailAccount = ES.Services.MailServers.GetMailAccount(id);
                mailAccount.Enabled = enable;
                int result = ES.Services.MailServers.UpdateMailAccount(mailAccount);

                if (result < 0)
                    return result;
            }

            return 0;
        }
    }
}
