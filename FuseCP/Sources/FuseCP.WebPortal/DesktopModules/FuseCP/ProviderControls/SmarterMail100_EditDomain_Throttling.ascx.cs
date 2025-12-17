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
    public partial class SmarterMail100_EditDomain_Throttling : FuseCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitValidators();
        }

        public void SaveItem(MailDomain item)
        {
            item[MailDomain.SMARTERMAIL5_MESSAGES_PER_HOUR] = txtMessagesPerHour.Text;
            item[MailDomain.SMARTERMAIL5_MESSAGES_PER_HOUR_ENABLED] = cbMessagesPerHour.Checked.ToString();
            item[MailDomain.SMARTERMAIL5_BANDWIDTH_PER_HOUR] = txtBandwidthPerHour.Text;
            item[MailDomain.SMARTERMAIL5_BANDWIDTH_PER_HOUR_ENABLED] = cbBandwidthPerHour.Checked.ToString();
            item[MailDomain.SMARTERMAIL5_BOUNCES_PER_HOUR] = txtBouncesPerHour.Text;
            item[MailDomain.SMARTERMAIL5_BOUNCES_PER_HOUR_ENABLED] = cbBouncesPerHour.Checked.ToString();
        }

        public void BindItem(MailDomain item)
        {
            txtMessagesPerHour.Text = item[MailDomain.SMARTERMAIL5_MESSAGES_PER_HOUR];
            cbMessagesPerHour.Checked = Convert.ToBoolean(item[MailDomain.SMARTERMAIL5_MESSAGES_PER_HOUR_ENABLED]);
            txtBandwidthPerHour.Text = item[MailDomain.SMARTERMAIL5_BANDWIDTH_PER_HOUR];
            cbBandwidthPerHour.Checked = Convert.ToBoolean(item[MailDomain.SMARTERMAIL5_BANDWIDTH_PER_HOUR_ENABLED]);
            txtBouncesPerHour.Text = item[MailDomain.SMARTERMAIL5_BOUNCES_PER_HOUR];
            cbBouncesPerHour.Checked = Convert.ToBoolean(item[MailDomain.SMARTERMAIL5_BOUNCES_PER_HOUR_ENABLED]);
        }

        public void InitValidators()
        {
            string message = "*";

            reqValMessagesPerHour.ErrorMessage = message;
            valMessagesPerHour.ErrorMessage = message;
            valMessagesPerHour.MaximumValue = int.MaxValue.ToString();

            reqValBandwidth.ErrorMessage = message;
            valBandwidthPerHour.ErrorMessage = message;
            valBandwidthPerHour.MaximumValue = int.MaxValue.ToString();

            reqValBouncesPerHour.ErrorMessage = message;
            valBouncesPerHour.ErrorMessage = message;
            valBouncesPerHour.MaximumValue = int.MaxValue.ToString();
        }
    }

}
