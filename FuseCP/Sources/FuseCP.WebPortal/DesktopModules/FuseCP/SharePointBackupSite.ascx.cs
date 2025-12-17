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
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.Providers.SharePoint;

namespace FuseCP.Portal
{
    public partial class SharePointBackupSite : FuseCPModuleBase
    {
        private const string BACKUP_EXTENSION = ".bsh";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSite();
            }
        }

        private void BindSite()
        {
            try
            {
                SharePointSite site = ES.Services.SharePointServers.GetSharePointSite(PanelRequest.ItemID);
                litSiteName.Text = site.Name;
                txtBackupName.Text = site.Name + BACKUP_EXTENSION;
                fileLookup.SelectedFile = "\\";
                fileLookup.PackageId = site.PackageId;

                BindBackupName();
                ToggleControls();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_GET_SITE", ex);
                return;
            }
        }

        private void BindBackupName()
        {
            string backupName = Path.GetFileNameWithoutExtension(txtBackupName.Text);
            txtBackupName.Text = backupName + (chkZipBackup.Checked ? ".zip" : BACKUP_EXTENSION);
        }

        private void ToggleControls()
        {
            fileLookup.Visible = rbCopy.Checked;
        }

        private void BackupSite()
        {
            try
            {
                string bakFile = ES.Services.SharePointServers.BackupVirtualServer(PanelRequest.ItemID,
                    txtBackupName.Text, chkZipBackup.Checked, rbDownload.Checked, fileLookup.SelectedFile);

                if (rbDownload.Checked && !String.IsNullOrEmpty(bakFile))
                {
                    string fileName = bakFile;

                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fileName));
                    Response.ContentType = "application/octet-stream";

                    int FILE_BUFFER_LENGTH = 5000000;
                    byte[] buffer = null;
                    int offset = 0;
                    do
                    {
                        // read remote content
                        buffer = ES.Services.SharePointServers.GetSharePointBackupBinaryChunk(PanelRequest.ItemID, fileName, offset, FILE_BUFFER_LENGTH);

                        // write to stream
                        Response.BinaryWrite(buffer);

                        offset += FILE_BUFFER_LENGTH;
                    }
                    while (buffer.Length == FILE_BUFFER_LENGTH);

                    Response.End();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_BACKUP_SITE", ex);
                return;
            }
            RedirectBack();
        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            BackupSite();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_item",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void chkZipBackup_CheckedChanged(object sender, EventArgs e)
        {
            BindBackupName();
        }

        protected void rbDownload_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }
    }
}
