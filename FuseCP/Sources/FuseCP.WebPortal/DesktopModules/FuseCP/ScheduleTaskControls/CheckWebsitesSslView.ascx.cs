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
using FCP = FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ScheduleTaskControls
{
    public partial class CheckWebsitesSslView : EmptyView
    {
        private static readonly string sendMailToCustomerParameter = "SEND_MAIL_TO_CUSTOMER";
        private static readonly string sendBccParameter = "SEND_BCC";
        private static readonly string bccMailParameter = "BCC_MAIL";
        private static readonly string expirationMailSubjectParameter = "EXPIRATION_MAIL_SUBJECT";
        private static readonly string expirationMailBodyParameter = "EXPIRATION_MAIL_BODY";
        private static readonly string send30DaysBeforeExpirationParameter = "SEND_30_DAYS_BEFORE_EXPIRATION";
        private static readonly string send14DaysBeforeExpirationParameter = "SEND_14_DAYS_BEFORE_EXPIRATION";
        private static readonly string sendTodayExpiredParameter = "SEND_TODAY_EXPIRED";
        private static readonly string sendSslErrorParameter = "SEND_SSL_ERROR";
        private static readonly string errorMailSubjectParameter = "ERROR_MAIL_SUBJECT";
        private static readonly string errorMailBodyParameter = "ERROR_MAIL_BODY";

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

            FCP.SystemSettings settings = ES.Services.System.GetSystemSettingsActive(FCP.SystemSettings.SMTP_SETTINGS, false);
            if (settings != null)
            {
                txtMailFrom.Text = settings["SmtpUsername"];
            }
            if (String.IsNullOrEmpty(txtMailFrom.Text))
            {
                txtMailFrom.Text = GetLocalizedString("SMTPWarning.Text");
            }

            SetParameter(cbMailToCustomer, sendMailToCustomerParameter);
            SetParameter(cbSendBcc, sendBccParameter);
            SetParameter(txtBccMail, bccMailParameter);
            SetParameter(txtExpirationMailSubject, expirationMailSubjectParameter);
            SetParameter(txtExpirationMailBody, expirationMailBodyParameter);
            SetParameter(cbSend30DaysBeforeExpiration, send30DaysBeforeExpirationParameter);
            SetParameter(cbSend14DaysBeforeExpiration, send14DaysBeforeExpirationParameter);
            SetParameter(cbSendTodayExpired, sendTodayExpiredParameter);
            SetParameter(cbSendSslError, sendSslErrorParameter);
            SetParameter(txtErrorMailSubject, errorMailSubjectParameter);
            SetParameter(txtErrorMailBody, errorMailBodyParameter);
        }

        /// <summary>
        /// Gets scheduler task parameters from view.
        /// </summary>
        /// <returns>Parameters list filled  from view.</returns>
        public override ScheduleTaskParameterInfo[] GetParameters()
        {
            ScheduleTaskParameterInfo mailToCustomer = GetParameter(cbMailToCustomer, sendMailToCustomerParameter);
            ScheduleTaskParameterInfo sendBcc = GetParameter(cbSendBcc, sendBccParameter);
            ScheduleTaskParameterInfo bccMail = GetParameter(txtBccMail, bccMailParameter);
            ScheduleTaskParameterInfo expirationMailSubject = GetParameter(txtExpirationMailSubject, expirationMailSubjectParameter);
            ScheduleTaskParameterInfo expirationMailBody = GetParameter(txtExpirationMailBody, expirationMailBodyParameter);
            ScheduleTaskParameterInfo send30DaysBeforeExpiration = GetParameter(cbSend30DaysBeforeExpiration, send30DaysBeforeExpirationParameter);
            ScheduleTaskParameterInfo send14DaysBeforeExpiration = GetParameter(cbSend14DaysBeforeExpiration, send14DaysBeforeExpirationParameter);
            ScheduleTaskParameterInfo sendTodayExpired = GetParameter(cbSendTodayExpired, sendTodayExpiredParameter);
            ScheduleTaskParameterInfo sendSslError = GetParameter(cbSendSslError, sendSslErrorParameter);
            ScheduleTaskParameterInfo errorMailSubject = GetParameter(txtErrorMailSubject, errorMailSubjectParameter);
            ScheduleTaskParameterInfo errorMailBody = GetParameter(txtErrorMailBody, errorMailBodyParameter);

            return new ScheduleTaskParameterInfo[11] { mailToCustomer, sendBcc, bccMail, expirationMailSubject, expirationMailBody, send30DaysBeforeExpiration,
                send14DaysBeforeExpiration, sendTodayExpired, sendSslError, errorMailSubject, errorMailBody};

        }

        private string GetLocalizedString(string resourceKey)
        {
            return (string)GetLocalResourceObject(resourceKey);
        }
    }
}
