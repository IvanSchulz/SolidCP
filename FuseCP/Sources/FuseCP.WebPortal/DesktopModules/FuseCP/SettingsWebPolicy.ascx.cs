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

namespace FuseCP.Portal
{
    public partial class SettingsWebPolicy : FuseCPControlBase, IUserSettingsEditorControl
    {
        public void BindSettings(UserSettings settings)
        {
            // parking page
            chkAddParkingPage.Checked = Utils.ParseBool(settings["AddParkingPage"], false);
            txtPageName.Text = settings["ParkingPageName"];
            txtPageContent.Text = settings["ParkingPageContent"];
			PublishingProfileTextBox.Text = settings["PublishingProfile"];
            chkEnableParkingPageTokens.Checked = Utils.ParseBool(settings["EnableParkingPageTokens"], false);

            // HostName
            txtHostName.Text = settings["HostName"];

            // default documents
            if (!String.IsNullOrEmpty(settings["DefaultDocuments"]))
                txtDefaultDocs.Text = String.Join("\n", settings["DefaultDocuments"].Split(',', ';'));

            // general settings
            chkWrite.Checked = Utils.ParseBool(settings["EnableWritePermissions"], false);
            chkDirectoryBrowsing.Checked = Utils.ParseBool(settings["EnableDirectoryBrowsing"], false);
            chkParentPaths.Checked = Utils.ParseBool(settings["EnableParentPaths"], false);
            chkDedicatedPool.Checked = Utils.ParseBool(settings["EnableDedicatedPool"], false);

            chkAuthAnonymous.Checked = Utils.ParseBool(settings["EnableAnonymousAccess"], false);
            chkAuthWindows.Checked = Utils.ParseBool(settings["EnableWindowsAuthentication"], false);
            chkAuthBasic.Checked = Utils.ParseBool(settings["EnableBasicAuthentication"], false);

            // extensions
            chkAsp.Checked = Utils.ParseBool(settings["AspInstalled"], false);
            Utils.SelectListItem(ddlAspNet, settings["AspNetInstalled"]);
            Utils.SelectListItem(ddlPhp, settings["PhpInstalled"]);
            chkDynamicCompression.Checked = Utils.ParseBool(settings["EnableDynamicCompression"], false);
            chkStaticCompression.Checked = Utils.ParseBool(settings["EnableStaticCompression"], false);
            chkPerl.Checked = Utils.ParseBool(settings["PerlInstalled"], false);
            chkPython.Checked = Utils.ParseBool(settings["PythonInstalled"], false);
            chkCgiBin.Checked = Utils.ParseBool(settings["CgiBinInstalled"], false);
			chkCfExt.Checked = Utils.ParseBool(settings["ColdFusionInstalled"], false);
			chkVirtDir.Checked = Utils.ParseBool(settings["CreateCFAppVirtualDirectoriesPol"], false);

            // anonymous account policy
            anonymousUsername.Value = settings["AnonymousAccountPolicy"];

            // virtual directories
            virtDirName.Value = settings["VirtDirNamePolicy"];

            // FrontPage
            frontPageUsername.Value = settings["FrontPageAccountPolicy"];
            frontPagePassword.Value = settings["FrontPagePasswordPolicy"];

			// secured folders
			securedUserNamePolicy.Value = settings["SecuredUserNamePolicy"];
			securedUserPasswordPolicy.Value = settings["SecuredUserPasswordPolicy"];
			securedGroupNamePolicy.Value = settings["SecuredGroupNamePolicy"];

            // folders
            txtSiteRootFolder.Text = settings["WebRootFolder"];
            txtSiteLogsFolder.Text = settings["WebLogsFolder"];
            txtSiteDataFolder.Text = settings["WebDataFolder"];
            chkAddRandomDomainString.Checked = Utils.ParseBool(settings["AddRandomDomainString"], false);
        }

        public void SaveSettings(UserSettings settings)
        {
            // parking page
            settings["AddParkingPage"] = chkAddParkingPage.Checked.ToString();
            settings["ParkingPageName"] = txtPageName.Text;
            settings["ParkingPageContent"] = txtPageContent.Text;
			settings["PublishingProfile"] = PublishingProfileTextBox.Text;
            settings["EnableParkingPageTokens"] = chkEnableParkingPageTokens.Checked.ToString();

            settings["HostName"] = txtHostName.Text.Trim();

            // default documents
            settings["DefaultDocuments"] = String.Join(",", Utils.ParseDelimitedString(txtDefaultDocs.Text, '\n', '\r', ';', ',')); ;

            // general settings
            settings["EnableWritePermissions"] = chkWrite.Checked.ToString();
            settings["EnableDirectoryBrowsing"] = chkDirectoryBrowsing.Checked.ToString();
            settings["EnableParentPaths"] = chkParentPaths.Checked.ToString();
            settings["EnableDedicatedPool"] = chkDedicatedPool.Checked.ToString();

            settings["EnableAnonymousAccess"] = chkAuthAnonymous.Checked.ToString();
            settings["EnableWindowsAuthentication"] = chkAuthWindows.Checked.ToString();
            settings["EnableBasicAuthentication"] = chkAuthBasic.Checked.ToString();

            // extensions
            settings["AspInstalled"] = chkAsp.Checked.ToString();
            settings["AspNetInstalled"] = ddlAspNet.SelectedValue;
            settings["PhpInstalled"] = ddlPhp.SelectedValue;
            settings["EnableDynamicCompression"] = chkDynamicCompression.Checked.ToString();
            settings["EnableStaticCompression"] = chkStaticCompression.Checked.ToString();
            settings["PerlInstalled"] = chkPerl.Checked.ToString();
            settings["PythonInstalled"] = chkPython.Checked.ToString();
            settings["CgiBinInstalled"] = chkCgiBin.Checked.ToString();
			settings["ColdFusionInstalled"] = chkCfExt.Checked.ToString();
			settings["CreateCFAppVirtualDirectoriesPol"] = chkVirtDir.Checked.ToString();

            // anonymous account policy
            settings["AnonymousAccountPolicy"] = anonymousUsername.Value;

            // virtual directories
            settings["VirtDirNamePolicy"] = virtDirName.Value;

            // FrontPage
            settings["FrontPageAccountPolicy"] = frontPageUsername.Value;
            settings["FrontPagePasswordPolicy"] = frontPagePassword.Value;

			// secured folders
			settings["SecuredUserNamePolicy"] = securedUserNamePolicy.Value;
			settings["SecuredUserPasswordPolicy"] = securedUserPasswordPolicy.Value;
			settings["SecuredGroupNamePolicy"] = securedGroupNamePolicy.Value;

            // folders
            settings["WebRootFolder"] = txtSiteRootFolder.Text;
            settings["WebLogsFolder"] = txtSiteLogsFolder.Text;
            settings["WebDataFolder"] = txtSiteDataFolder.Text;
            settings["AddRandomDomainString"] = chkAddRandomDomainString.Checked.ToString();
        }
    }
}
