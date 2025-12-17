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
using FuseCP.Providers.SharePoint;

namespace FuseCP.Portal
{
    public partial class HostedSharePointEnterpriseStorageUsage : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindGrid();
        }

        protected void btnRecalculateDiscSpace_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            int errorCode;
            try
            {
                var result = ES.Services.HostedSharePointServersEnt.Enterprise_CalculateSharePointSitesDiskSpace(PanelRequest.ItemID);
                SharePointSiteDiskSpace[] sharePointEnterpriseSiteDiskSpace = result.Result;
                errorCode = result.ErrorCode;

                if (errorCode < 0)
                {
                    messageBox.ShowResultMessage(errorCode);
                    return;
                }
                
                if (sharePointEnterpriseSiteDiskSpace != null && sharePointEnterpriseSiteDiskSpace.Length == 1 && string.IsNullOrEmpty(sharePointEnterpriseSiteDiskSpace[0].Url))
                {
                    gvStorageUsage.DataSource = null;
                    gvStorageUsage.DataBind();
                    lblTotalItems.Text = "0";
                    lblTotalSize.Text = "0";
                    return;
                }
                
                gvStorageUsage.DataSource = sharePointEnterpriseSiteDiskSpace;
                gvStorageUsage.DataBind();

                if (sharePointEnterpriseSiteDiskSpace != null)
                {
                    lblTotalItems.Text = sharePointEnterpriseSiteDiskSpace.Length.ToString();

                    long total = 0;
                    foreach (SharePointSiteDiskSpace current in sharePointEnterpriseSiteDiskSpace)
                    {
                        total += current.DiskSpace;
                    }

                    lblTotalSize.Text = total.ToString();
                }
            }
            catch(Exception ex)
            {
                messageBox.ShowErrorMessage("HOSTED_SHAREPOINT_RECALCULATE_SIZE", ex);   
            }
        }
    }
}
