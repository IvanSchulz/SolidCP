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
using FuseCP.EnterpriseServer;
using FuseCP.Portal.UserControls.ScheduleTaskView;

namespace FuseCP.Portal.ScheduleTaskControls
{
    public partial class HostedSolutionReport : EmptyView
    {
        private static readonly string EXCHANGE_REPORT = "EXCHANGE_REPORT";
        private static readonly string ORGANIZATION_REPORT = "ORGANIZATION_REPORT";
        private static readonly string SHAREPOINT_REPORT = "SHAREPOINT_REPORT";
        private static readonly string LYNC_REPORT = "LYNC_REPORT";
        private static readonly string SFB_REPORT = "SFB_REPORT";
        private static readonly string CRM_REPORT = "CRM_REPORT";
        private static readonly string EMAIL = "EMAIL";

        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void SetParameters(ScheduleTaskParameterInfo[] parameters)
        {
            base.SetParameters(parameters);
            SetParameter(cbExchange, EXCHANGE_REPORT);
            SetParameter(cbSharePoint, SHAREPOINT_REPORT);
            SetParameter(cbLync, LYNC_REPORT);
            SetParameter(cbSfB, SFB_REPORT);
            SetParameter(cbCRM, CRM_REPORT);
            SetParameter(cbOrganization, ORGANIZATION_REPORT);
            SetParameter(txtMail, EMAIL);         

        }

        public override ScheduleTaskParameterInfo[] GetParameters()
        {
            ScheduleTaskParameterInfo exchange = GetParameter(cbExchange, EXCHANGE_REPORT);
            ScheduleTaskParameterInfo sharepoint = GetParameter(cbSharePoint, SHAREPOINT_REPORT);
            ScheduleTaskParameterInfo lync = GetParameter(cbLync, LYNC_REPORT);
            ScheduleTaskParameterInfo sfb = GetParameter(cbSfB, SFB_REPORT);
            ScheduleTaskParameterInfo crm = GetParameter(cbCRM, CRM_REPORT);
            ScheduleTaskParameterInfo organization = GetParameter(cbOrganization, ORGANIZATION_REPORT);
            ScheduleTaskParameterInfo email = GetParameter(txtMail, EMAIL);


            return new ScheduleTaskParameterInfo[7] { exchange, sharepoint, lync, sfb, crm, organization, email};
        }
    }
}
