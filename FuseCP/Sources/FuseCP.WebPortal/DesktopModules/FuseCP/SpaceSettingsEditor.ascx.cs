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
    public partial class SpaceSettingsEditor : FuseCPModuleBase
    {
        private string SettingsName
        {
            get { return Request["SettingsName"]; }
        }

        IPackageSettingsEditorControl ctlSettings;

        protected void Page_Load(object sender, EventArgs e)
        {
            // load settings control
            LoadSettingsControl();

            // entry point
            try
            {
                if (!IsPostBack)
                {
                    BindSettings();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("PACKAGE_SETTINGS_GET", ex);
                return;
            }
        }

        private void BindSettings()
        {
            // load user settings
            PackageSettings settings = ES.Services.Packages.GetPackageSettings(PanelSecurity.PackageId, SettingsName);

            ddlOverride.SelectedIndex = (settings.PackageId == PanelSecurity.PackageId) ? 1 : 0;
            ToggleControls();

            // bind settings
            ctlSettings.BindSettings(settings);
        }

        protected void ddlOverride_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOverride.SelectedIndex == 0) // use host settings
            {
                // delete current settings
                PackageSettings settings = new PackageSettings();
                settings.PackageId = PanelSecurity.PackageId;
                settings.SettingsName = SettingsName;
                ES.Services.Packages.UpdatePackageSettings(settings);

                // rebind settings
                BindSettings();
            }
            else
            {
                ToggleControls();
            }
        }

        private void ToggleControls()
        {
            // check if we should enable controls
            bool enabled = (ddlOverride.SelectedIndex == 1);

            // enable/disable controls
            EnableControlRecursively((Control)ctlSettings, enabled);
        }

        private void EnableControlRecursively(Control ctrl, bool enabled)
        {
            WebControl wc = ctrl as WebControl;
            if (wc != null && !(wc is Label))
                wc.Enabled = enabled;

            // process children
            foreach (Control childCtrl in ctrl.Controls)
                EnableControlRecursively(childCtrl, enabled);
        }

        private void SaveSettings()
        {
            try
            {
                PackageSettings settings = new PackageSettings();
                settings.PackageId = PanelSecurity.PackageId;
                settings.SettingsName = SettingsName;

                // set properties
                if (ddlOverride.SelectedIndex == 1)
                {
                    // gather settings from the control
                    // if overriden
                    ctlSettings.SaveSettings(settings);
                }

                int result = ES.Services.Packages.UpdatePackageSettings(settings);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                ReturnBack();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("PACKAGE_SETTINGS_UPDATE", ex);
                return;
            }
        }

        private void LoadSettingsControl()
        {
            string controlName = Request["SettingsControl"];
            if (!String.IsNullOrEmpty(controlName))
            {
                string currPath = this.AppRelativeVirtualPath;
                currPath = currPath.Substring(0, currPath.LastIndexOf("/"));
                string ctrlPath = currPath + "/" + controlName + ".ascx";

                Control ctrl = Page.LoadControl(ctrlPath);
                ctlSettings = (IPackageSettingsEditorControl)ctrl;
                settingsPlace.Controls.Add(ctrl);
            }
            else
            {
                ReturnBack();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReturnBack();
        }

        private void ReturnBack()
        {
            RedirectSpaceHomePage();
        }
    }
}
