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
    public partial class CRMStorageSettings : FuseCPModuleBase
    {
        public string SizeValueToString(long val)
        {
            return (val == -1) ? GetSharedLocalizedString("Text.Unlimited") : val.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            warningValue.UnlimitedText = GetLocalizedString("WarningUnlimitedValue");
            if (!IsPostBack)
            {
                Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
                if (org.CrmOrganizationId == Guid.Empty)
                {
                    messageBox.ShowErrorMessage("NOT_CRM_ORGANIZATION");
                    StorageLimits.Enabled = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    BindValues();
                }
            }
        }

        private void BindValues()
        {
            Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            string quotaName = "";
            if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013)) quotaName = Quotas.CRM2013_MAXDATABASESIZE;
            else if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM)) quotaName = Quotas.CRM_MAXDATABASESIZE;

            int limitDBSize = cntx.Quotas[quotaName].QuotaAllocatedValue;
            //maxStorageSettingsValue.ParentQuotaValue = limitDBSize;
            maxStorageSettingsValue.ParentQuotaValue = -1;

            long maxDBSize = ES.Services.CRM.GetMaxDBSize(PanelRequest.ItemID, PanelSecurity.PackageId);
            long DBSize = ES.Services.CRM.GetDBSize(PanelRequest.ItemID, PanelSecurity.PackageId);

            DBSize = DBSize > 0 ? DBSize = DBSize / (1024 * 1024) : DBSize;
            maxDBSize = maxDBSize > 0 ? maxDBSize = maxDBSize / (1024 * 1024) : maxDBSize;

            maxStorageSettingsValue.QuotaValue = Convert.ToInt32(maxDBSize);

            lblDBSize.Text = SizeValueToString(DBSize);
            lblMAXDBSize.Text = SizeValueToString(maxDBSize);

            lblLimitDBSize.Text = SizeValueToString(limitDBSize);
        }

        private void Save()
        {            
            try
            {
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                string quotaName = "";
                if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013)) quotaName = Quotas.CRM2013_MAXDATABASESIZE;
                else if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM)) quotaName = Quotas.CRM_MAXDATABASESIZE;

                long limitSize = cntx.Quotas[quotaName].QuotaAllocatedValue;

                long maxSize = maxStorageSettingsValue.QuotaValue;

                if (limitSize != -1)
                {
                    if (maxSize == -1) maxSize = limitSize;
                    if (maxSize > limitSize) maxSize = limitSize;
                }

                if (maxSize > 0)
                {
                    maxSize = maxSize * 1024 * 1024;
                }

                ES.Services.CRM.SetMaxDBSize(PanelRequest.ItemID, PanelSecurity.PackageId, maxSize);

                messageBox.ShowSuccessMessage("HOSTED_SHAREPOINT_UPDATE_QUOTAS");

                BindValues();
            }
            catch (Exception)
            {
                messageBox.ShowErrorMessage("HOSTED_SHAREPOINT_UPDATE_QUOTAS");
            }

        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();            
        }

    }
}
