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
using System.Web.UI.WebControls;
using FuseCP.Providers.Common;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal.BlackBerry
{
    public partial class EditBlackBerryUser :  FuseCPModuleBase
    {
        public const string CANNOT_GET_BLACKBERRY_STATS = "CANNOT_GET_BLACKBERRY_STATS";

        public const string CANNOT_DELETE_BLACKBERRY_DATA = "CANNOT_DELETE_BLACKBERRY_DATA";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            BlackBerryUserStatsResult stats = ES.Services.BlackBerry.GetBlackBerryUserStats(PanelRequest.ItemID, PanelRequest.AccountID);
            if (stats.IsSuccess)
            {
                dvStats.Visible = true;
                dvStats.DataSource = stats.Value;
                dvStats.DataBind();
            }
            else
            {
                dvStats.Visible = false;
                messageBox.ShowWarningMessage(CANNOT_GET_BLACKBERRY_STATS);                
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {            
            ResultObject res = ES.Services.BlackBerry.DeleteBlackBerryUser(PanelRequest.ItemID, PanelRequest.AccountID);
            if (res.IsSuccess && res.ErrorCodes.Count == 0)
            {
                
                Response.Redirect(EditUrl("", "", "blackberry_users",
                    "SpaceID=" + PanelSecurity.PackageId,
                    "ItemID=" + PanelRequest.ItemID));
            }
            else
            {
                messageBox.ShowMessage(res, "DELETE_BLACKBERRY_USER", "BlackBerry");
            }
        }

        protected void rbGeneratePassword_OnCheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;            
            if (button != null)
                tblPassword.Visible = !button.Checked;
        }

        protected void rbSpecifyPassword_OnCheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button != null)
                tblPassword.Visible = button.Checked;
        }

        protected void btnSetPassword_Click(object sender, EventArgs e)
        {
            ResultObject res;

            res = rbSpecifyPassword.Checked
                      ?
                          ES.Services.BlackBerry.SetActivationPasswordWithExpirationTime(PanelRequest.ItemID,
                                                                                         PanelRequest.AccountID,
                                                                                         txtPassword.Text,
                                                                                         Utils.ParseInt(txtTime.Text))
                      : ES.Services.BlackBerry.SetEmailActivationPassword(PanelRequest.ItemID,
                                                                          PanelRequest.AccountID);

            messageBox.ShowMessage(res, "SET_BLACKBERRY_USER_PASSWORD", "BlackBerry");
        }

        protected void btnDeleteData_Click(object sender, EventArgs e)
        {
           ResultObject res = ES.Services.BlackBerry.DeleteDataFromBlackBerryDevice(PanelRequest.ItemID,
                                                                  PanelRequest.AccountID);

           if (res.IsSuccess) 
                messageBox.ShowMessage(res, "DELETE_BLACKBERRY_DEVICE_DATA", "BlackBerry");
           else
           {
               messageBox.ShowWarningMessage(CANNOT_DELETE_BLACKBERRY_DATA);
           }
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                "blackberry_users",
                "SpaceID=" + PanelSecurity.PackageId));
        }

    }
}
