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

﻿using System;
﻿using System.Web;
﻿using FuseCP.Providers.WebAppGallery;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal
{
    public partial class WebApplicationGalleryInstall : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindApplicationDetails();
            }
        }

        private void BindApplicationDetails()
        {
            try
            {
                GalleryApplicationResult appResult = 
                    ES.Services.WebApplicationGallery.GetGalleryApplicationDetails(PanelSecurity.PackageId,
                                                                                    PanelRequest.ApplicationID);
                // check for errors
                if (!appResult.IsSuccess)
                {
                    messageBox.ShowMessage(appResult, "WAG_NOT_AVAILABLE", "WebAppGallery");
                    return;
                }

                // bind details
                if (appResult.Value != null)
                    appHeader.BindApplicationDetails(appResult.Value);

                // check for warnings
                if (appResult.ErrorCodes.Count > 0)
                {
                    // app does not meet requirements
                    if (appResult.ErrorCodes.Count > 1)
                    {
                        // remove "- Your hosting package does not meet..." message
                        appResult.ErrorCodes.RemoveAt(0);
                    }
                    messageBox.ShowMessage(appResult, "WAG_CANNOT_INSTALL_APPLICATION", "WebAppGallery");
                    chIgnoreDependencies.Visible = false;
                    btnInstall.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("GET_GALLERY_APPLIACTION_DETAILS", ex);
            }
        }

        protected void btnInstall_Click(object sender, EventArgs e)
        {
			// Ensure server-side validation has taken its part in the play
			if (!Page.IsValid)
				return;

            bool isSuccess = true;
            try
            {
                GalleryWebAppStatus status;
                do
                {
                    status =
                        ES.Services.WebApplicationGallery.GetGalleryApplicationStatus(PanelSecurity.PackageId,
                                                                                      PanelRequest.ApplicationID);
                }
				while (status == GalleryWebAppStatus.Downloading);
				//
				switch(status)
				{
					case GalleryWebAppStatus.Failed:
					case GalleryWebAppStatus.NotDownloaded:
						ShowErrorMessage("GALLERY_APP_DOWNLOAD_FAILED");
						isSuccess = false;
						break;

                    case GalleryWebAppStatus.UnauthorizedAccessException:
                        ShowErrorMessage("GALLERY_APP_UNAUTHORIZEDACCESSEXCEPTION");
                        isSuccess = false;
                        break;

				}
            }
            catch(Exception ex)
            {
                isSuccess = false;
                ShowErrorMessage("GET_GALLERY_APPLICATION_STATUS", ex);                
            }

            if (isSuccess)
            {
                // web app downloaded successfully
                string url = EditUrl("ApplicationID", PanelRequest.ApplicationID, "editParams",
                                     "SpaceID=" + PanelSecurity.PackageId);
                
                string targetSite = HttpContext.Current.Request["SiteId"];
                if (!string.IsNullOrEmpty(targetSite))
                {
                    url += "&SiteId=" + targetSite;
                }
                string returnUrl = HttpContext.Current.Request["ReturnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    url += "&ReturnUrl=" + Server.UrlEncode(returnUrl);
                }

                Response.Redirect(url);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string returnUrl = HttpContext.Current.Request["ReturnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                string redirectUrl = HttpUtility.UrlDecode(returnUrl);
                Response.Redirect(redirectUrl);

            }
            else
            {
                RedirectSpaceHomePage();
            }
        }

        protected void chIgnoreDependencies_CheckedChanged(object sender, EventArgs e)
        {
            btnInstall.Enabled = chIgnoreDependencies.Checked;
        }

    }
}
