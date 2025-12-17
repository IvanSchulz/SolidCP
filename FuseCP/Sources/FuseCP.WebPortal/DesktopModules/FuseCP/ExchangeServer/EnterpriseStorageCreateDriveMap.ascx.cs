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
using System.Linq;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.OS;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class EnterpriseStorageCreateDriveMap : FuseCPModuleBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindLetters();
                BindFolders();
            }

            if (ddlFolders.Items.Count < 1 || ddlLetters.Items.Count < 1)
            {
                btnCreate.Enabled = false;
            }
        }

        private void BindFolders()
        {
            ddlFolders.DataSource = ES.Services.EnterpriseStorage.GetNotMappedEnterpriseFolders(PanelRequest.ItemID).Select(x=> new {Name = x.Name, Url = x.UncPath ?? x.Url});
            ddlFolders.DataTextField = "Name";
            ddlFolders.DataValueField = "Url";
            ddlFolders.DataBind();

            if (ddlFolders.Items.Count > 0)
            {
                txtLabelAs.Text = ddlFolders.SelectedItem.Text;
                lbFolderUrl.Text = ddlFolders.SelectedItem.Value;
                txtFolderName.Value = ddlFolders.SelectedItem.Text;
            }
        }

        private void BindLetters()
        {
            //for (int i = 65; i < 91; i++) // increment from ASCII values for A-Z
            for (int i = 69; i < 91; i++) // E-Z
            {
                ddlLetters.Items.Add(new ListItem(Convert.ToChar(i).ToString() + ":", Convert.ToChar(i).ToString()));// Add uppercase letters to possible drive letters
            }
            
            //string[] usedLetters = ES.Services.EnterpriseStorage.GetUsedDriveLetters(PanelRequest.ItemID);

            //foreach (string elem in usedLetters)
            //{
            //    ListItem item = new ListItem(elem + ":", elem);
            //    if (ddlLetters.Items.Contains(item))
            //    {
            //        ddlLetters.Items.Remove(item);
            //    }
            //}
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            try
            {
                if (!ES.Services.EnterpriseStorage.CheckEnterpriseStorageInitialization(PanelSecurity.PackageId, PanelRequest.ItemID))
                {
                    ES.Services.EnterpriseStorage.CreateEnterpriseStorage(PanelSecurity.PackageId, PanelRequest.ItemID);
                }

                ResultObject result = ES.Services.EnterpriseStorage.CreateMappedDrive(
                    PanelSecurity.PackageId,
                    PanelRequest.ItemID,
                    ddlLetters.SelectedItem.Value,
                    txtLabelAs.Text,
                    txtFolderName.Value);

                if (!result.IsSuccess && result.ErrorCodes.Count > 0)
                {
                    messageBox.ShowMessage(result, "ENTERPRISE_STORAGE_CREATE_MAPPED_DRIVE", "Cloud Folders");
                    return;
                }

                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "enterprisestorage_drive_maps",
                "SpaceID=" + PanelSecurity.PackageId));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ENTERPRISE_STORAGE_CREATE_MAPPED_DRIVE", ex);
            }
        }
    }
}
