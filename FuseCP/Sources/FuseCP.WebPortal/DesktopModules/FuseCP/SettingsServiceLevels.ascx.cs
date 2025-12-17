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
using System.Xml;
using System.Xml.Serialization;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers.Common;

namespace FuseCP.Portal
{
    public partial class SettingsServiceLevels : FuseCPControlBase, IUserSettingsEditorControl
    {


        public void BindSettings(UserSettings settings)
        {
            if (PanelSecurity.SelectedUser.Role == UserRole.Administrator)
                BindServiceLevels();
            
            txtStatus.Visible = false;
        
            try
            {
                //Change container title
                ((Label)this.Parent.Parent.Parent.Parent.Parent.FindControl(FuseCP.WebPortal.DefaultPage.MODULE_TITLE_CONTROL_ID)).Text = "Service Levels";
            }
            catch { /*to do*/ }
        }


        private void BindServiceLevels()
        {
            ServiceLevel[] array = ES.Services.Organizations.GetSupportServiceLevels();

            gvServiceLevels.DataSource = array;
            gvServiceLevels.DataBind();

            btnAddServiceLevel.Enabled = (string.IsNullOrEmpty(txtServiceLevelName.Text)) ? true : false;
            btnUpdateServiceLevel.Enabled = (string.IsNullOrEmpty(txtServiceLevelName.Text)) ? false : true;
        }


        public void btnAddServiceLevel_Click(object sender, EventArgs e)
        {
            Page.Validate("CreateServiceLevel");

            if (!Page.IsValid)
                return;

            ServiceLevel serviceLevel = new ServiceLevel();

            int res = ES.Services.Organizations.AddSupportServiceLevel(txtServiceLevelName.Text, txtServiceLevelDescr.Text);

            if (res < 0)
            {
                messageBox.ShowErrorMessage("ADD_SERVICE_LEVEL");

                return;
            }

            txtServiceLevelName.Text = string.Empty;
            txtServiceLevelDescr.Text = string.Empty;

            BindServiceLevels();
        }


        protected void gvServiceLevels_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int levelID = Utils.ParseInt(e.CommandArgument.ToString(), 0);
            
            switch (e.CommandName)
            {
                case "DeleteItem":

                    ResultObject result = ES.Services.Organizations.DeleteSupportServiceLevel(levelID);

                    if (!result.IsSuccess)
                    {
                        if (result.ErrorCodes.Contains("SERVICE_LEVEL_IN_USE:Service Level is being used; ")) messageBox.ShowErrorMessage("SERVICE_LEVEL_IN_USE");
                        else messageBox.ShowMessage(result, "DELETE_SERVICE_LEVEL", null);

                        return;
                    }

                    ViewState["ServiceLevelID"] = null;

                    txtServiceLevelName.Text = string.Empty;
                    txtServiceLevelDescr.Text = string.Empty;

                    BindServiceLevels();
                    break;

                case "EditItem":
                    ServiceLevel serviceLevel;

                    ViewState["ServiceLevelID"] = levelID;

                    serviceLevel = ES.Services.Organizations.GetSupportServiceLevel(levelID);

                    txtServiceLevelName.Text = serviceLevel.LevelName;
                    txtServiceLevelDescr.Text = serviceLevel.LevelDescription;

                    btnUpdateServiceLevel.Enabled = (string.IsNullOrEmpty(txtServiceLevelName.Text)) ? false : true;
                    btnAddServiceLevel.Enabled = (string.IsNullOrEmpty(txtServiceLevelName.Text)) ? true : false;
                    break;
            }
        }


        public void SaveSettings(UserSettings settings)
        {
            settings["ServiceLevels"] = "";
        }


        protected void btnUpdateServiceLevel_Click(object sender, EventArgs e)
        {
            Page.Validate("CreateServiceLevel");

            if (!Page.IsValid)
                return;

            if (ViewState["ServiceLevelID"] == null)
                return;

            int levelID = (int)ViewState["ServiceLevelID"];

            ES.Services.Organizations.UpdateSupportServiceLevel(levelID, txtServiceLevelName.Text, txtServiceLevelDescr.Text);

            txtServiceLevelName.Text = string.Empty;
            txtServiceLevelDescr.Text = string.Empty;

            BindServiceLevels();
        }

    }
}
