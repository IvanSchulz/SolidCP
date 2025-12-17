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
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal.OCS
{
    public partial class CreateOCSUser : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            int accountId = userSelector.GetAccountId();
            OCSUserResult res = ES.Services.OCS.CreateOCSUser(PanelRequest.ItemID, accountId);
            if (res.IsSuccess && res.ErrorCodes.Count == 0)
            {
                Response.Redirect(EditUrl("AccountID", accountId.ToString(), "edit_ocs_user",
                    "SpaceID=" + PanelSecurity.PackageId,
                    "ItemID=" + PanelRequest.ItemID,
                    "InstanceID=" + res.Value.InstanceId));
            }
            else
            {
                messageBox.ShowMessage(res, "CREATE_OCS_USER", "OCS");
            }
        }
    }
}
