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
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FuseCP.Portal.ProviderControls;
using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Providers.OS;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class EnterpriseStorageFolderGeneralSettings : FuseCPModuleBase
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

                BindSettings();

                OrganizationStatistics organizationStats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);

                if (organizationStats.AllocatedEnterpriseStorageSpace != -1)
                {
                    rangeFolderSize.MaximumValue = Math.Round((organizationStats.AllocatedEnterpriseStorageSpace - (decimal)organizationStats.UsedEnterpriseStorageSpace) / OneGb
                        + Utils.ParseDecimal(txtFolderSize.Text, 0), 2).ToString();
                    rangeFolderSize.ErrorMessage = string.Format("The quota you've entered exceeds the available quota for organization ({0}Gb)", rangeFolderSize.MaximumValue);
                }
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                SystemFile folder = ES.Services.EnterpriseStorage.GetEnterpriseFolder(PanelRequest.ItemID, PanelRequest.FolderID);

                litFolderName.Text = string.Format("{0}", folder.Name);

                // bind form
                txtFolderName.Text = folder.Name;
                lblFolderUrl.Text = folder.Url;
                
                if (folder.FRSMQuotaMB != -1)
                {
                    txtFolderSize.Text = (Math.Round((decimal)folder.FRSMQuotaMB / OneGb, 2)).ToString();
                }

                switch (folder.FsrmQuotaType)
                {
                    case QuotaType.Hard:
                        rbtnQuotaHard.Checked = true;
                        break;
                    case QuotaType.Soft:
                        rbtnQuotaSoft.Checked = true;
                        break;
                }

                var serviceId = ES.Services.EnterpriseStorage.GetEnterpriseStorageServiceId(PanelRequest.ItemID);

                StringDictionary settings = ConvertArrayToDictionary(ES.Services.Servers.GetServiceSettingsRDS(serviceId));

                btnMigrate.Visible = folder.StorageSpaceFolderId == null
                    && Utils.ParseBool(settings[EnterpriseStorage_Settings.UseStorageSpaces], false);

                if (folder.StorageSpaceFolderId != null)
                {
                    uncPathRow.Visible = edaRow.Visible = abeRow.Visible = true;

                    lblUncPath.Text = folder.UncPath;


                    chkAbe.Checked = ES.Services.StorageSpaces.GetStorageSpaceFolderAbeStatus(folder.StorageSpaceFolderId.Value);
                    chkEda.Checked = ES.Services.StorageSpaces.GetStorageSpaceFolderEncryptDataAccessStatus(folder.StorageSpaceFolderId.Value);
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ENETERPRISE_STORAGE_GET_FOLDER_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                litFolderName.Text = txtFolderName.Text;

                SystemFile folder = new SystemFile { Name = PanelRequest.FolderID, Url = lblFolderUrl.Text };

                if (!ES.Services.EnterpriseStorage.CheckEnterpriseStorageInitialization(PanelSecurity.PackageId, PanelRequest.ItemID))
                {
                    ES.Services.EnterpriseStorage.CreateEnterpriseStorage(PanelSecurity.PackageId, PanelRequest.ItemID);
                }

                //File is renaming
                if (PanelRequest.FolderID != txtFolderName.Text)
                {
                    //check if filename is correct
                    if (!EnterpriseStorageHelper.ValidateFolderName(txtFolderName.Text))
                    {
                        messageBox.ShowErrorMessage("FILES_INCORRECT_FOLDER_NAME");

                        return;
                    }

                    folder = ES.Services.EnterpriseStorage.RenameEnterpriseFolder(PanelRequest.ItemID, PanelRequest.FolderID, txtFolderName.Text);

                    if (folder == null)
                    {
                        messageBox.ShowErrorMessage("FOLDER_ALREADY_EXIST");

                        return;
                    }
                }

                ES.Services.EnterpriseStorage.SetEnterpriseFolderGeneralSettings(
                    PanelRequest.ItemID,
                    folder,
                    false,
                    (int)(decimal.Parse(txtFolderSize.Text) * OneGb),
                    rbtnQuotaSoft.Checked ? QuotaType.Soft : QuotaType.Hard);

                if (edaRow.Visible && abeRow.Visible)
                {
                   ES.Services.EnterpriseStorage.SetEsFolderShareSettings(PanelRequest.ItemID, PanelRequest.FolderID, chkAbe.Checked, chkEda.Checked);
                }

                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "enterprisestorage_folders",
                        "ItemID=" + PanelRequest.ItemID));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ENTERPRISE_STORAGE_UPDATE_FOLDER_SETTINGS", ex);
            }
        }

        protected void btnMigrate_Click(object sender, EventArgs e)
        {
            try
            {
                var result = ES.Services.EnterpriseStorage.MoveToStorageSpace(
                PanelRequest.ItemID,
                PanelRequest.FolderID);

                messageBox.ShowMessage(result, "ES_MOVE_TO_STORAGE_SPACE", null);

                if (result.IsSuccess)
                {
                    Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "enterprisestorage_folders",
                        "ItemID=" + PanelRequest.ItemID));
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ENTERPRISE_STORAGE_MIGRATE_TO_STORAGE_SPACES", ex);
            }
        }

        private StringDictionary ConvertArrayToDictionary(string[] settings)
        {
            StringDictionary r = new StringDictionary();
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
            }
            return r;
        }
    }
}
