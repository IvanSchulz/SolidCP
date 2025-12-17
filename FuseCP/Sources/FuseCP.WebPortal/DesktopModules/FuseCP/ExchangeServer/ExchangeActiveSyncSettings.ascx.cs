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
using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeActiveSyncSettings : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPolicy();
            }

            

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SavePolicy();
        }

        private void BindPolicy()
        {
            try
            {
                // read limits
                ExchangeActiveSyncPolicy policy = ES.Services.ExchangeServer.GetActiveSyncPolicy(PanelRequest.ItemID);

                // bind data
                chkAllowNonProvisionable.Checked = policy.AllowNonProvisionableDevices;

                chkAllowAttachments.Checked = policy.AttachmentsEnabled;
                sizeMaxAttachmentSize.ValueKB = policy.MaxAttachmentSizeKB;

                chkWindowsFileShares.Checked = policy.UNCAccessEnabled;
                chkWindowsSharePoint.Checked = policy.WSSAccessEnabled;

                chkRequirePasword.Checked = policy.DevicePasswordEnabled;
                chkRequireAlphaNumeric.Checked = policy.AlphanumericPasswordRequired;
                chkEnablePasswordRecovery.Checked = policy.PasswordRecoveryEnabled;
                chkRequireEncryption.Checked = policy.DeviceEncryptionEnabled;
                chkAllowSimplePassword.Checked = policy.AllowSimplePassword;

                sizeNumberAttempts.ValueKB = policy.MaxPasswordFailedAttempts;
                sizeMinimumPasswordLength.ValueKB = policy.MinPasswordLength;
                sizeTimeReenter.ValueKB = policy.InactivityLockMin;
                sizePasswordExpiration.ValueKB = policy.PasswordExpirationDays;
                sizePasswordHistory.ValueKB = policy.PasswordHistory;
                hoursRefreshInterval.ValueHours = policy.RefreshInterval;
                ToggleControls();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_ACTIVESYNC_POLICY", ex);
            }
        }

        private void SavePolicy()
        {
            if (!Page.IsValid)
                return;

            try
            {
                // set limits
                int result = ES.Services.ExchangeServer.SetActiveSyncPolicy(PanelRequest.ItemID,
                    chkAllowNonProvisionable.Checked,
                    chkAllowAttachments.Checked,
                    sizeMaxAttachmentSize.ValueKB,

                    chkWindowsFileShares.Checked,
                    chkWindowsSharePoint.Checked,

                    chkRequirePasword.Checked,
                    chkRequireAlphaNumeric.Checked,
                    chkEnablePasswordRecovery.Checked,
                    chkRequireEncryption.Checked,
                    chkAllowSimplePassword.Checked,

                    sizeNumberAttempts.ValueKB,
                    sizeMinimumPasswordLength.ValueKB,
                    sizeTimeReenter.ValueKB,
                    sizePasswordExpiration.ValueKB,
                    sizePasswordHistory.ValueKB,
                    hoursRefreshInterval.ValueHours);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                messageBox.ShowSuccessMessage("EXCHANGE_SET_ACTIVESYNC_POLICY");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_SET_ACTIVESYNC_POLICY", ex);
            }
        }

        private void ToggleControls()
        {
            PasswordSettingsRow.Visible = chkRequirePasword.Checked;
        }

        protected void chkRequirePasword_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }
    }
}
