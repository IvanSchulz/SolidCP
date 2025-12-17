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
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.OCS
{
    public partial class EditOCSUser : FuseCPModuleBase
    {
        public const string UPDATE_OCS_USER = "UPDATE_OCS_USER";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindItems();

        }

        private void BindItems()
        {
            OCSUser user =
                       ES.Services.OCS.GetUserGeneralSettings(PanelRequest.ItemID, PanelRequest.InstanceID);

            if (user != null)
            {
                lblDisplayName.Text = user.DisplayName;                
                lblSIP.Text = user.PrimaryUri;
                litDisplayName.Text = user.DisplayName;                

                cbEnableFederation.Checked = user.EnabledForFederation;
                cbEnablePublicConnectivity.Checked = user.EnabledForPublicIMConectivity;
                cbArchiveInternal.Checked = user.ArchiveInternalCommunications;
                cbArchiveFederation.Checked = user.ArchiveFederatedCommunications;
                cbEnablePresence.Checked = user.EnabledForEnhancedPresence;
                cbEnablePresence.Enabled = !user.EnabledForEnhancedPresence;

            }

            Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
            if (org != null)
            {

                cbEnableFederation.Visible = PackagesHelper.CheckGroupQuotaEnabled(org.PackageId,
                                                                                   ResourceGroups.OCS,
                                                                                   Quotas.OCS_Federation);

                cbEnablePublicConnectivity.Visible = PackagesHelper.CheckGroupQuotaEnabled(org.PackageId,
                                                                                           ResourceGroups.OCS,
                                                                                           Quotas.
                                                                                               OCS_PublicIMConnectivity);
                cbArchiveInternal.Visible = PackagesHelper.CheckGroupQuotaEnabled(org.PackageId,
                                                                                  ResourceGroups.OCS,
                                                                                  Quotas.OCS_ArchiveIMConversation);

                cbArchiveFederation.Visible = PackagesHelper.CheckGroupQuotaEnabled(org.PackageId,
                                                                                    ResourceGroups.OCS,
                                                                                    Quotas.
                                                                                        OCS_ArchiveFederatedIMConversation);

                cbEnablePresence.Visible = PackagesHelper.CheckGroupQuotaEnabled(org.PackageId,
                                                                                 ResourceGroups.OCS,
                                                                                 Quotas.OCS_PresenceAllowed);
                locNote.Visible = cbEnablePresence.Visible;

                secFedaration.Visible = cbEnableFederation.Visible || cbEnablePublicConnectivity.Visible;
                secArchiving.Visible = cbArchiveInternal.Visible || cbArchiveFederation.Visible;
                secPresence.Visible = cbEnablePresence.Visible;                               
            }            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ES.Services.OCS.SetUserGeneralSettings(PanelRequest.ItemID, PanelRequest.InstanceID, cbEnableFederation.Checked, cbEnablePublicConnectivity.Checked, cbArchiveInternal.Checked, cbArchiveFederation.Checked, 
                    cbEnablePresence.Enabled && cbEnablePresence.Checked);
                
                messageBox.ShowSuccessMessage(UPDATE_OCS_USER);
            }
            catch(Exception ex)
            {
                messageBox.ShowErrorMessage(UPDATE_OCS_USER, ex);
            }
        }
    }
}
