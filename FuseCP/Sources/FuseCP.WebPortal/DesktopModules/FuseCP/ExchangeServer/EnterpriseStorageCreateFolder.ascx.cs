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
using System.Text.RegularExpressions;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.OS;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class EnterpriseStorageCreateFolder : FuseCPModuleBase
    {
        #region Constants

        private const int OneGb = 1024;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ES.Services.EnterpriseStorage.CheckUsersDomainExists(PanelRequest.ItemID))
                {
                    Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "enterprisestorage_folders",
                        "ItemID=" + PanelRequest.ItemID));
                }

                OrganizationStatistics organizationStats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);

                if (organizationStats.AllocatedEnterpriseStorageSpace != -1)
                {
                    rangeFolderSize.MaximumValue = Math.Round((organizationStats.AllocatedEnterpriseStorageSpace - (decimal)organizationStats.UsedEnterpriseStorageSpace) / OneGb
                        + Utils.ParseDecimal(txtFolderSize.Text, 0), 2).ToString();
                    rangeFolderSize.ErrorMessage = string.Format("The quota you've entered exceeds the available quota for organization ({0}Gb)", rangeFolderSize.MaximumValue);
                }

                if (organizationStats.AllocatedGroups != -1)
                {
                    int groupsAvailable = organizationStats.AllocatedGroups - organizationStats.CreatedGroups;

                    if (groupsAvailable <= 0)
                    {
                        chkAddDefaultGroup.Enabled = false;
                    }
                }
            }
        }


        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            try
            {
                if (!EnterpriseStorageHelper.ValidateFolderName(txtFolderName.Text.Trim()))
                {
                    messageBox.ShowErrorMessage("FILES_INCORRECT_FOLDER_NAME");

                    return;
                }

                if (!ES.Services.EnterpriseStorage.CheckEnterpriseStorageInitialization(PanelSecurity.PackageId, PanelRequest.ItemID))
                {
                    ES.Services.EnterpriseStorage.CreateEnterpriseStorage(PanelSecurity.PackageId, PanelRequest.ItemID);
                }

                ResultObject result = ES.Services.EnterpriseStorage.CreateEnterpriseFolder(
                    PanelRequest.ItemID,
                    txtFolderName.Text.Trim(),
                    (int)(decimal.Parse(txtFolderSize.Text) * OneGb),
                    rbtnQuotaSoft.Checked ? QuotaType.Soft : QuotaType.Hard,
                    chkAddDefaultGroup.Checked);

                if (!result.IsSuccess && result.ErrorCodes.Count > 0)
                {
                    messageBox.ShowMessage(result, "ENTERPRISE_STORAGE_CREATE_FOLDER", "Cloud Folders");

                    return;
                }

                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "enterprisestorage_folder_settings",
                    "FolderID=" + txtFolderName.Text.Trim(),
                    "ItemID=" + PanelRequest.ItemID));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ENTERPRISE_STORAGE_CREATE_FOLDER", ex);
            }
        }
    }
}
