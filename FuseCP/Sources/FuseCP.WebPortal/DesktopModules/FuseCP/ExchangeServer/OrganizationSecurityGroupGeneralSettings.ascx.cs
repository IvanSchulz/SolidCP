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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class OrganizationSecurityGroupGeneralSettings : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSettings();
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                OrganizationSecurityGroup securityGroup = ES.Services.Organizations.GetSecurityGroupGeneralSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID);

                litDisplayName.Text = PortalAntiXSS.Encode(securityGroup.DisplayName);

                // bind form
                txtDisplayName.Text = securityGroup.DisplayName;
                lblGroupName.Text = securityGroup.AccountName;

                members.SetAccounts(securityGroup.MembersAccounts);

                txtNotes.Text = securityGroup.Notes;

                if (securityGroup.IsDefault)
                {
                    txtDisplayName.ReadOnly = true;
                    txtNotes.ReadOnly = true;
                    members.Enabled = false;

                    btnSave.Visible = false;
                    tabs.IsDefault = true;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ORGANIZATION_GET_SECURITY_GROUP_SETTINGS", ex);
            }
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                int result = ES.Services.Organizations.SetSecurityGroupGeneralSettings(
                    PanelRequest.ItemID,
                    PanelRequest.AccountID,
                    txtDisplayName.Text,
                    members.GetAccounts(),
                    txtNotes.Text);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                litDisplayName.Text = PortalAntiXSS.Encode(txtDisplayName.Text);

                messageBox.ShowSuccessMessage("ORGANIZATION_UPDATE_SECURITY_GROUP_SETTINGS");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ORGANIZATION_UPDATE_SECURITY_GROUP_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
