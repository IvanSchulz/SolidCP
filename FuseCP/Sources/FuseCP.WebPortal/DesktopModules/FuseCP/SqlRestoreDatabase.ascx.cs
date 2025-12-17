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

using FuseCP.Providers.Database;

namespace FuseCP.Portal
{
    public partial class SqlRestoreDatabase : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDatabase();
            }
        }

        private void BindDatabase()
        {
            try
            {
                SqlDatabase database = ES.Services.DatabaseServers.GetSqlDatabase(PanelRequest.ItemID);
                litDatabaseName.Text = database.Name;
                fileLookup.SelectedFile = "";
                fileLookup.PackageId = database.PackageId;

                ToggleControls();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SQL_GET_DATABASE", ex);
                return;
            }
        }

        private void ToggleControls()
        {
            cellFile.Visible = radioFile.Checked;
            cellUploadFile.Visible = radioUpload.Checked;
        }

        private void RestoreDatabase()
        {
            try
            {
                string[] uploadedFiles = null;
                string[] packageFiles = null;

                if (radioUpload.Checked)
                {
                    if (uploadFile.PostedFile.FileName != "")
                    {
                        Stream stream = uploadFile.PostedFile.InputStream;

                        // save uploaded file
                        int FILE_BUFFER_LENGTH = 5000000;
                        string path = null;
                        int readBytes = 0;
                        string fileName = Path.GetFileName(uploadFile.PostedFile.FileName);

                        int offset = 0;
                        do
                        {
                            // read input stream
                            byte[] buffer = new byte[FILE_BUFFER_LENGTH];
                            readBytes = stream.Read(buffer, 0, FILE_BUFFER_LENGTH);

                            if (readBytes < FILE_BUFFER_LENGTH)
                                Array.Resize<byte>(ref buffer, readBytes);

                            // write remote backup file
                            string tempPath = ES.Services.DatabaseServers.AppendSqlBackupBinaryChunk(PanelRequest.ItemID, fileName, path, buffer);
                            if (path == null)
                                path = tempPath;

                            offset += FILE_BUFFER_LENGTH;
                        }
                        while (readBytes == FILE_BUFFER_LENGTH);

                        uploadedFiles = new string[] { path };
                    }
                }
                else
                {
                    // package files
                    packageFiles = new string[] { fileLookup.SelectedFile };
                }

                int result = ES.Services.DatabaseServers.RestoreSqlDatabase(PanelRequest.ItemID, uploadedFiles, packageFiles);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SQL_RESTORE_DATABASE", ex);
                return;
            }

            RedirectBack();
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            RestoreDatabase();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_item",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
        }
        protected void radioUpload_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }
    }
}
