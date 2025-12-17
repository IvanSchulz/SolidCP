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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;
using FuseCP.Providers.FTP;

namespace FuseCP.Portal
{
    public partial class FtpAccountEditAccount : FuseCPModuleBase
    {
        FtpAccount item = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Visible = (PanelRequest.ItemID > 0);

            // bind item
            BindItem();
        }

        private void BindItem()
        {
            try
            {
                if (!IsPostBack)
                {
                    // load item if required
                    if (PanelRequest.ItemID > 0)
                    {
                        // existing item
                        try
                        {
                            item = ES.Services.FtpServers.GetFtpAccount(PanelRequest.ItemID);
                        }
                        catch (Exception ex)
                        {
                            ShowErrorMessage("FTP_GET_ACCOUNT", ex);
                            return;
                        }

                        if (item != null)
                        {
                            // save package info
                            ViewState["PackageId"] = item.PackageId;
                            fileLookup.PackageId = item.PackageId;
                            passwordControl.SetPackagePolicy(item.PackageId, UserSettings.FTP_POLICY, "UserPasswordPolicy");
                        }
                        else
                            RedirectToBrowsePage();
                    }
                    else
                    {
                        // new item
                        ViewState["PackageId"] = PanelSecurity.PackageId;
                        fileLookup.PackageId = PanelSecurity.PackageId;
                        fileLookup.SelectedFile = "\\";
                        usernameControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.FTP_POLICY, "UserNamePolicy");
                        passwordControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.FTP_POLICY, "UserPasswordPolicy");
                    }
                }

                // load provider control
                LoadProviderControl((int)ViewState["PackageId"], "Ftp", providerControl, "EditAccount.ascx");

                if (!IsPostBack)
                {
                    // bind item to controls
                    if (item != null)
                    {
                        // bind item to controls
                        usernameControl.Text = item.Name;
                        usernameControl.EditMode = true;
                        passwordControl.EditMode = true;
                        fileLookup.SelectedFile = item.Folder;

                        // other controls
                        IFtpAccountEditControl ctrl = (IFtpAccountEditControl)providerControl.Controls[0];
                        ctrl.BindItem(item);
                    }
                }
            }
            catch
            {
                ShowWarningMessage("INIT_SERVICE_ITEM_FORM");
                DisableFormControls(this, btnCancel);
            }
        }

        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            FtpAccount item = new FtpAccount();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;
            item.Name = usernameControl.Text;
            item.Password = passwordControl.Password;
            item.Folder = fileLookup.SelectedFile;

            // get other props
            IFtpAccountEditControl ctrl = (IFtpAccountEditControl)providerControl.Controls[0];
            ctrl.SaveItem(item);

            if (PanelRequest.ItemID == 0)
            {
                try
                {
                    // new item
                    int result = ES.Services.FtpServers.AddFtpAccount(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    
                    ShowErrorMessage("FTP_ADD_ACCOUNT", ex);
                    return;
                    
                }
            }
            else
            {
                try
                {
                    // existing item
                    int result = ES.Services.FtpServers.UpdateFtpAccount(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("FTP_UPDATE_ACCOUNT", ex);
                    return;
                }
            }

            // return
            RedirectSpaceHomePage();
        }

        private void DeleteItem()
        {
            // delete
            try
            {
                int result = ES.Services.FtpServers.DeleteFtpAccount(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("FTP_DELETE_ACCOUNT", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveItem();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // return
            RedirectSpaceHomePage();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
    }
}
