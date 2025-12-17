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
	public partial class HostedSharePointRestoreSiteCollection : FuseCPModuleBase
	{
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
				SharePointSiteCollection siteCollection = ES.Services.HostedSharePointServers.GetSiteCollection(this.SiteCollectionId);
				litSiteCollectionName.Text = siteCollection.PhysicalAddress;
				fileLookup.SelectedFile = String.Empty;
				fileLookup.PackageId = siteCollection.PackageId;

				ToggleControls();
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SHAREPOINT_GET_SITE", ex);
				return;
			}
		}

		private void ToggleControls()
		{
			cellFile.Visible = radioFile.Checked;
			cellUploadFile.Visible = radioUpload.Checked;
		}

		private void RestoreSiteCollection()
		{
			try
			{
				string uploadedFile = null;
				string packageFile = null;

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
							string tempPath = ES.Services.HostedSharePointServers.AppendBackupBinaryChunk(this.SiteCollectionId, fileName, path, buffer);
							if (path == null)
								path = tempPath;

							offset += FILE_BUFFER_LENGTH;
						}
						while (readBytes == FILE_BUFFER_LENGTH);

						uploadedFile = path;
					}
				}
				else
				{
					// package files
					packageFile = fileLookup.SelectedFile;
				}

				int result = ES.Services.HostedSharePointServers.RestoreSiteCollection(this.SiteCollectionId, uploadedFile, packageFile);
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SHAREPOINT_RESTORE_SITE", ex);
				return;
			}

			RedirectBack();
		}

		protected void btnRestore_Click(object sender, EventArgs e)
		{
			RestoreSiteCollection();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			RedirectBack();
		}

		private void RedirectBack()
		{
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "sharepoint_edit_sitecollection", "SiteCollectionID=" + this.SiteCollectionId, "ItemID=" + PanelRequest.ItemID.ToString()));
		}
		protected void radioUpload_CheckedChanged(object sender, EventArgs e)
		{
			ToggleControls();
		}
	}
}
