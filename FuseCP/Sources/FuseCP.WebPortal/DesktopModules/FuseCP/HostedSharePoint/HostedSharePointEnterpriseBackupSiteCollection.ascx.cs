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
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FuseCP.Providers.SharePoint;

namespace FuseCP.Portal
{
	public partial class HostedSharePointEnterpriseBackupSiteCollection : FuseCPModuleBase
	{
		private const string BACKUP_EXTENSION = ".bsh";

		private int OrganizationId
		{
			get
			{
				return PanelRequest.GetInt("ItemID");
			}
		}

		private int SiteCollectionId
		{
			get
			{
				return PanelRequest.GetInt("SiteCollectionID");
			}
		}

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
				SharePointEnterpriseSiteCollection siteCollection = ES.Services.HostedSharePointServersEnt.Enterprise_GetSiteCollection(this.SiteCollectionId);
				litSiteCollectionName.Text = siteCollection.PhysicalAddress;
				txtBackupName.Text = siteCollection.Url + BACKUP_EXTENSION;
				fileLookup.SelectedFile = "\\";
				fileLookup.PackageId = siteCollection.PackageId;

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

		private void BackupSiteCollection()
		{
			try
			{
                string bakFile = ES.Services.HostedSharePointServersEnt.Enterprise_BackupSiteCollection(this.SiteCollectionId,
					txtBackupName.Text, chkZipBackup.Checked, rbDownload.Checked, fileLookup.SelectedFile);

				if (rbDownload.Checked && !String.IsNullOrEmpty(bakFile))
				{

                    string fileName = bakFile;
                    
					//Response.Clear();
					Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fileName));
					Response.ContentType = "application/octet-stream";

					int FILE_BUFFER_LENGTH = 5000000;
					byte[] buffer = null;
					int offset = 0;
					do
					{
						// Read remote content.
						buffer = ES.Services.HostedSharePointServersEnt.Enterprise_GetBackupBinaryChunk(this.SiteCollectionId, fileName, offset, FILE_BUFFER_LENGTH);

						// Write to stream.
						//Response.BinaryWrite(buffer);
                        Response.OutputStream.Write(buffer, 0, buffer.Length);
						offset += FILE_BUFFER_LENGTH;
					}
					while (buffer.Length == FILE_BUFFER_LENGTH);

                    Response.Flush();
                    Response.End();
                    //Response.Close();
                    //HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //Response.End();
                    
                }
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SHAREPOINT_BACKUP_SITE", ex);
                return;
			}
            //Response.ClearContent();
		    Context.Response.Clear();
            if (!rbDownload.Checked)
                RedirectBack();
            
		}

		protected void btnBackup_Click(object sender, EventArgs e)
		{
            BackupSiteCollection();
		}
		protected void btnCancel_Click(object sender, EventArgs e)
		{
			RedirectBack();
		}

        private void RedirectBack()
        {
            HttpContext.Current.Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "sharepoint_enterprise_edit_sitecollection", "SiteCollectionID=" + this.SiteCollectionId, "ItemID=" + PanelRequest.ItemID.ToString()));
        }

		protected void chkZipBackup_CheckedChanged(object sender, EventArgs e)
		{
			BindBackupName();
		}

		protected void rbDownload_CheckedChanged(object sender, EventArgs e)
		{
			ToggleControls();
		}

        protected override void OnPreRender(EventArgs e)
        {
            string str = string.Format("var rb = document.getElementById('{0}'); if (!rb.checked) ShowProgressDialog('Backing up site collection...');", rbDownload.ClientID);
            
                
                
            btnBackup.Attributes.Add("onclick", str);
            base.OnPreRender(e);
        }

	}
}
