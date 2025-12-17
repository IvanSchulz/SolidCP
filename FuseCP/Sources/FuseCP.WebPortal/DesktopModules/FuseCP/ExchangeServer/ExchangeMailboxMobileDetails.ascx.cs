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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.Providers.HostedSolution;
using System.Drawing;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxMobileDetails : FuseCPModuleBase
    {
        public const string Unknown = "Unknown";

        private void BindData()
        {
            ExchangeMobileDevice device = ES.Services.ExchangeServer.GetMobileDevice(PanelRequest.ItemID,
                PanelRequest.DeviceId);

            if (device != null)
            {
                lblStatus.Text = GetLocalizedString(device.Status.ToString());
                switch (device.Status)
                {
                    case MobileDeviceStatus.PendingWipe:
                        lblStatus.ForeColor = Color.Red;
                        break;
                    case MobileDeviceStatus.WipeSuccessful:
                        lblStatus.ForeColor = Color.Green;
                        break;
                    default:
                        lblStatus.ForeColor = Color.Black;
                        break;
                }
                lblDeviceModel.Text = device.DeviceModel;
                lblDeviceType.Text = device.DeviceType;
                lblFirstSyncTime.Text = DateTimeToString(device.FirstSyncTime);
                lblDeviceWipeRequestTime.Text = DateTimeToString(device.DeviceWipeRequestTime);
                lblDeviceAcnowledgeTime.Text = DateTimeToString(device.DeviceWipeAckTime);
                lblLastSync.Text = DateTimeToString(device.LastSyncAttemptTime);
                lblLastUpdate.Text = DateTimeToString(device.LastPolicyUpdateTime);
                lblLastPing.Text = device.LastPingHeartbeat == 0 ? string.Empty : device.LastPingHeartbeat.ToString();
                lblDeviceFriendlyName.Text = device.DeviceFriendlyName;
                lblDeviceId.Text = device.DeviceID;
                lblDeviceUserAgent.Text = device.DeviceUserAgent;
                lblDeviceOS.Text = device.DeviceOS;
                lblDeviceOSLanguage.Text = device.DeviceOSLanguage;
                lblDeviceIMEA.Text = device.DeviceIMEI;
                lblDevicePassword.Text = device.RecoveryPassword;

                UpdateButtons(device.Status);

                // form title
                ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);
                litDisplayName.Text = account.DisplayName;
            }
        }

        private string DateTimeToString(DateTime dateTime)
        {
            return dateTime == DateTime.MinValue ? string.Empty : dateTime.ToString("g");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();

            
        }

        private void UpdateButtons(MobileDeviceStatus status)
        {
            if (status == MobileDeviceStatus.OK)
            {
                btnWipeAllData.Visible = true;
                btnCancel.Visible = false;
            }
            else
                if (status == MobileDeviceStatus.PendingWipe)
                {
                    btnWipeAllData.Visible = false;
                    btnCancel.Visible = true;
                }
                else
                {
                    btnWipeAllData.Visible = false;
                    btnCancel.Visible = false;
                }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            string str = EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "mailbox_mobile",
                        "ItemID=" + PanelRequest.ItemID, "AccountID=" + PanelRequest.AccountID);
            Response.Redirect(str);

        }

        protected void btnWipeAllData_Click(object sender, EventArgs e)
        {
            ES.Services.ExchangeServer.WipeDataFromDevice(PanelRequest.ItemID, PanelRequest.DeviceId);
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ES.Services.ExchangeServer.CancelRemoteWipeRequest(PanelRequest.ItemID, PanelRequest.DeviceId);
            BindData();
        }
    }
}
