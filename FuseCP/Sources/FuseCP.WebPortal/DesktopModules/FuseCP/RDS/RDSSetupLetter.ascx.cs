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
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.RDS
{
    public partial class RDSSeupLetter : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindInstructions();
            }
        }

        private void BindInstructions()
        {            
            int accountId = PanelRequest.AccountID > 0 ? PanelRequest.AccountID : 0;   
            litContent.Text = ES.Services.RDS.GetRdsSetupLetter(PanelRequest.ItemID, accountId);
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);

            if (package == null)
            {
                RedirectSpaceHomePage();
            }

            OrganizationUser account = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID, accountId);

            if (account != null)
            {
                txtTo.Text = account.ExternalEmail;
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                int accountId = PanelRequest.AccountID > 0 ? PanelRequest.AccountID : 0;   
                int result = ES.Services.RDS.SendRdsSetupLetter(PanelRequest.ItemID, accountId, txtTo.Text.Trim(), txtCC.Text.Trim());

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                messageBox.ShowSuccessMessage("RDS_SETUP_LETTER_SEND");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("RDS_SETUP_LETTER_SEND", ex);
                return;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "rds_collection_edit_users", "CollectionId=" + PanelRequest.CollectionID, "ItemID=" + PanelRequest.ItemID));
        }
    }
}
