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

namespace FuseCP.Portal
{
    public partial class HostedSharePointEnterpriseStorageSettings : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            warningValue.UnlimitedText = GetLocalizedString("WarningUnlimitedValue");
            
            
            if (!IsPostBack)
            {
                Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);

                PackageContext cntx = ES.Services.Packages.GetPackageContext(PanelSecurity.PackageId);
                foreach(QuotaValueInfo quota in cntx.QuotasArray)
                {
                    if (quota.QuotaId == 551 /*Max storage quota*/)
                    {
                        maxStorageSettingsValue.ParentQuotaValue = quota.QuotaAllocatedValue;
                        warningValue.ParentQuotaValue = quota.QuotaAllocatedValue;
                    }
                }
                
                maxStorageSettingsValue.QuotaValue = org.MaxSharePointEnterpriseStorage;
                warningValue.QuotaValue = org.WarningSharePointEnterpriseStorage;
                
            }
        }

        private void Save(bool apply)
        {            
            try
            {                
                int res = ES.Services.HostedSharePointServersEnt.Enterprise_SetStorageSettings(PanelRequest.ItemID, maxStorageSettingsValue.QuotaValue,
                                                                   warningValue.QuotaValue,
                                                                       apply);
                if (res < 0)
                {
                    messageBox.ShowResultMessage(res);
                    return;
                }            
                messageBox.ShowSuccessMessage("HOSTED_SHAREPOINT_UPDATE_QUOTAS");
            }
            catch (Exception)
            {
                messageBox.ShowErrorMessage("HOSTED_SHAREPOINT_UPDATE_QUOTAS");
            }

        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save(false);            
        }

        protected void btnSaveApply_Click(object sender, EventArgs e)
        {
            Save(true);
        }
    }
}
