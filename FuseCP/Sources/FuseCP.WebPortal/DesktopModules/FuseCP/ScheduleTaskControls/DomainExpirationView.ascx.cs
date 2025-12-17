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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Portal.UserControls.ScheduleTaskView;

namespace FuseCP.Portal.ScheduleTaskControls
{
    public partial class DomainExpirationView : EmptyView
    {
        private static readonly string DaysBeforeParameter = "DAYS_BEFORE";
        private static readonly string MailToParameter = "MAIL_TO";
        private static readonly string EnableNotificationParameter = "ENABLE_NOTIFICATION";
        private static readonly string IncludeNonExistentDomainsParameter = "INCLUDE_NONEXISTEN_DOMAINS";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Sets scheduler task parameters on view.
        /// </summary>
        /// <param name="parameters">Parameters list to be set on view.</param>
        public override void SetParameters(ScheduleTaskParameterInfo[] parameters)
        {
            base.SetParameters(parameters);

            this.SetParameter(this.txtDaysBeforeNotify, DaysBeforeParameter);
            this.SetParameter(this.txtMailTo, MailToParameter);
            this.SetParameter(this.cbEnableNotify, EnableNotificationParameter);
            this.SetParameter(this.cbIncludeNonExistentDomains, IncludeNonExistentDomainsParameter);
        }

        /// <summary>
        /// Gets scheduler task parameters from view.
        /// </summary>
        /// <returns>Parameters list filled  from view.</returns>
        public override ScheduleTaskParameterInfo[] GetParameters()
        {
            ScheduleTaskParameterInfo daysBefore = this.GetParameter(this.txtDaysBeforeNotify, DaysBeforeParameter);
            ScheduleTaskParameterInfo mailTo = this.GetParameter(this.txtMailTo, MailToParameter);
            ScheduleTaskParameterInfo enableNotification = this.GetParameter(this.cbEnableNotify, EnableNotificationParameter);
            ScheduleTaskParameterInfo includeNonExistenDomains = this.GetParameter(this.cbIncludeNonExistentDomains, IncludeNonExistentDomainsParameter);

            return new ScheduleTaskParameterInfo[4] { daysBefore, mailTo, enableNotification, includeNonExistenDomains };
        }
    }
}
