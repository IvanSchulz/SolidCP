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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;
using FuseCP.Providers.Web;

namespace FuseCP.Portal
{
    public partial class VirtualDirectoryHomeFolderControl : FuseCPControlBase
    {
        private bool isVirtualDirectory;
        public bool IsVirtualDirectory
        {
            get { return isVirtualDirectory; }
            set { isVirtualDirectory = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private void LoadDirectoryContentPath(int packageId, WebVirtualDirectory item)
        {
            item = ES.Services.WebServers.GetVirtualDirectory(PanelRequest.ItemID, PanelRequest.VirtDir);
            fileLookup.SelectedFile = item.ContentPath;
        }

        public void BindWebItem(int packageId, WebVirtualDirectory item)
        {
            fileLookup.PackageId = item.PackageId;
            fileLookup.SelectedFile = item.ContentPath;

            string resSuffix = item.IIs7 ? "IIS7" : "";

            chkAuthAnonymous.Checked = item.EnableAnonymousAccess;
            chkAuthWindows.Checked = item.EnableWindowsAuthentication;
            chkAuthBasic.Checked = item.EnableBasicAuthentication;

            // toggle controls by quotas
            fileLookup.Enabled = PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_HOMEFOLDERS);

            UserSettings settings = ES.Services.Users.GetUserSettings(PanelSecurity.SelectedUserId, "WebPolicy");
        }

        public void SaveWebItem(WebVirtualDirectory item)
        {
            item.ContentPath = fileLookup.SelectedFile;

            item.EnableAnonymousAccess = chkAuthAnonymous.Checked;
            item.EnableWindowsAuthentication = chkAuthWindows.Checked;
            item.EnableBasicAuthentication = chkAuthBasic.Checked;

        }

 
    }
}
