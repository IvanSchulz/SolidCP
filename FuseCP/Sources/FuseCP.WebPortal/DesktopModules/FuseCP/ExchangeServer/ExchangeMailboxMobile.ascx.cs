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
using System.Drawing;
using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxMobile : FuseCPModuleBase
    {
        public const string PendingWipe = "PendingWipe";
        public const string WipeSuccessful = "WipeSuccessful";
        public const string OK = "OK";
        public const string Unknown = "Unknown";


        private void BindGrid()
        {
            ExchangeMobileDevice[] devices = ES.Services.ExchangeServer.GetMobileDevices(PanelRequest.ItemID, PanelRequest.AccountID);

            gvMobile.DataSource = devices;
            gvMobile.DataBind();

            // form title
            ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);
            litDisplayName.Text = account.DisplayName;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // ExchangeMailbox mailbox = ES.Services.ExchangeServer.GetMailboxGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID);

                // if (mailbox != null)
                //   litDisplayName.Text = mailbox.DisplayName;

                BindGrid();
            }

        }

        protected void gvMobile_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                ES.Services.ExchangeServer.RemoveDevice(PanelRequest.ItemID, e.CommandArgument.ToString());
                BindGrid();
            }
        }


        protected string GetEditUrl(string id)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "mailbox_mobile_details",
                        "DeviceID=" + id,
                        "ItemID=" + PanelRequest.ItemID, "AccountID=" + PanelRequest.AccountID);
        }

        protected void gvMobile_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ExchangeMobileDevice current = e.Row.DataItem as ExchangeMobileDevice;
                if (current != null)
                {
                    Label lblDate = e.Row.FindControl("lblLastSyncTime") as Label;
                    if (lblDate != null)
                    {
                        lblDate.Text = current.LastSuccessSync == DateTime.MinValue ? string.Empty : current.LastSuccessSync.ToString("g");
                    }

                    Label lblStatus = e.Row.FindControl("lblStatus") as Label;
                    if (lblStatus != null)
                    {
                        switch (current.Status)
                        {
                            case MobileDeviceStatus.PendingWipe:
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = GetLocalizedString(PendingWipe);
                                break;
                            case MobileDeviceStatus.WipeSuccessful:
                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Text = GetLocalizedString(WipeSuccessful);
                                break;
                            default:
                                lblStatus.Text = GetLocalizedString(OK);
                                lblStatus.ForeColor = Color.Black;
                                break;
                        }
                    }

                    if (string.IsNullOrEmpty(current.DeviceUserAgent))
                    {
                        HyperLink lnkDeviceUserAgent = e.Row.FindControl("lnkDeviceUserAgent") as HyperLink;
                        if (lnkDeviceUserAgent != null)
                            lnkDeviceUserAgent.Text = GetLocalizedString(Unknown);
                    }
                }

            }
        }

        protected void gvMobile_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvMobile_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}
