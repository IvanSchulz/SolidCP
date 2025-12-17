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
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FuseCP.EnterpriseServer;
using System.Text;

namespace FuseCP.Portal.ProviderControls
{
    public partial class IIS60_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
		private string FilteredAppIds;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindSettings(StringDictionary settings)
        {
            ipAddress.AddressId = (settings["SharedIP"] != null) ? Utils.ParseInt(settings["SharedIP"], 0) : 0;
            ipAddress.SelectValueText = GetLocalizedString("ipAddress.SelectValueText");
            txtPublicSharedIP.Text = settings["PublicSharedIP"];

            txtWebGroupName.Text = settings["WebGroupName"];
            chkAssignIPAutomatically.Checked = Utils.ParseBool(settings["AutoAssignDedicatedIP"], true);

            txtAspNet11Pool.Text = settings["AspNet11Pool"];
            txtAspNet20Pool.Text = settings["AspNet20Pool"];
			txtAspNet40Pool.Text = settings["AspNet40Pool"];
            txtAspPath.Text = settings["AspPath"];
            txtAspNet11Path.Text = settings["AspNet11Path"];
            txtAspNet20Path.Text = settings["AspNet20Path"];
			txtAspNet40Path.Text = settings["AspNet40Path"];
            txtPhp4Path.Text = settings["Php4Path"];
            txtPhp5Path.Text = settings["Php5Path"];
            txtPerlPath.Text = settings["PerlPath"];
            txtPythonPath.Text = settings["PythonPath"];
            txtColdFusionPath.Text = settings["ColdFusionPath"];
            txtScriptsDirectory.Text = settings["CFScriptsDirectory"];
            txtFlashRemotingDir.Text = settings["CFFlashRemotingDirectory"];

            txtPasswordFilterPath.Text = settings["SecuredFoldersFilterPath"];
			txtProtectedAccessFile.Text = settings["ProtectedAccessFile"];
			txtProtectedUsersFile.Text = settings["ProtectedUsersFile"];
			txtProtectedGroupsFile.Text = settings["ProtectedGroupsFile"];
			txtProtectedFoldersFile.Text = settings["ProtectedFoldersFile"];

            sharedSslSites.Value = settings[PackageSettings.SHARED_SSL_SITES];
            ActiveDirectoryIntegration.BindSettings(settings);
			
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings["SharedIP"] = ipAddress.AddressId.ToString();
            settings["PublicSharedIP"] = txtPublicSharedIP.Text.Trim();
            settings["WebGroupName"] = txtWebGroupName.Text.Trim();
            settings["AutoAssignDedicatedIP"] = chkAssignIPAutomatically.Checked.ToString();

            settings["AspPath"] = txtAspPath.Text.Trim();
            settings["AspNet11Pool"] = txtAspNet11Pool.Text.Trim();
            settings["AspNet20Pool"] = txtAspNet20Pool.Text.Trim();
			settings["AspNet40Pool"] = txtAspNet40Pool.Text.Trim();
            settings["AspNet11Path"] = txtAspNet11Path.Text.Trim();
            settings["AspNet20Path"] = txtAspNet20Path.Text.Trim();
			settings["AspNet40Path"] = txtAspNet40Path.Text.Trim();
            settings["Php4Path"] = txtPhp4Path.Text.Trim();
            settings["Php5Path"] = txtPhp5Path.Text.Trim();
            settings["PerlPath"] = txtPerlPath.Text.Trim();
            settings["PythonPath"] = txtPythonPath.Text.Trim();
            settings["ColdFusionPath"] = txtColdFusionPath.Text.Trim();
            settings["CFScriptsDirectory"] = txtScriptsDirectory.Text.Trim();
            settings["CFFlashRemotingDirectory"] = txtFlashRemotingDir.Text.Trim();
			settings[PackageSettings.SHARED_SSL_SITES] = sharedSslSites.Value;

            settings["SecuredFoldersFilterPath"] = txtPasswordFilterPath.Text.Trim();
			settings["ProtectedAccessFile"] = txtProtectedAccessFile.Text.Trim();
			settings["ProtectedUsersFile"] = txtProtectedUsersFile.Text.Trim();
			settings["ProtectedGroupsFile"] = txtProtectedGroupsFile.Text.Trim();
			settings["ProtectedFoldersFile"] = txtProtectedFoldersFile.Text.Trim();
            
            ActiveDirectoryIntegration.SaveSettings(settings);

        }
    }
}
