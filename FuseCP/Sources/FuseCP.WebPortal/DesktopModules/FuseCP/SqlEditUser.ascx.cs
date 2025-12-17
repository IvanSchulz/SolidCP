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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;
using FuseCP.Providers.Database;

namespace FuseCP.Portal
{
    public partial class SqlEditUser : FuseCPModuleBase
    {
        SqlUser item = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Visible = (PanelRequest.ItemID > 0);

            BindItem();
        }

        private void BindDatabases(int packageId)
        {
            try
            {
                SqlDatabase[] databases = ES.Services.DatabaseServers.GetSqlDatabases(packageId,
                    SqlDatabases.GetDatabasesGroupName(Settings), false);

                dlDatabases.DataSource = databases;
                dlDatabases.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SQL_GET_DATABASE", ex);
                return;
            }
        }

        private void BindItem()
        {
            var policyName = "";
            if (SqlDatabases.GetDatabasesGroupName(Settings).ToLower().StartsWith("mssql")) { policyName = "MsSqlPolicy"; }
            else if (SqlDatabases.GetDatabasesGroupName(Settings).ToLower().StartsWith("mysql")) { policyName = "MySqlPolicy"; }
            else { policyName = "MariaDBPolicy"; }

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
                            item = ES.Services.DatabaseServers.GetSqlUser(PanelRequest.ItemID);
                        }
                        catch (Exception ex)
                        {
                            ShowErrorMessage("SQL_GET_USER", ex);
                            return;
                        }

                        if (item != null)
                        {
                            // save package info
                            ViewState["PackageId"] = item.PackageId;
                            usernameControl.SetPackagePolicy(item.PackageId, policyName, "UserNamePolicy");
                            passwordControl.SetPackagePolicy(item.PackageId, policyName, "UserPasswordPolicy");
                            BindDatabases(item.PackageId);
                        }
                        else
                            RedirectToBrowsePage();
                    }
                    else
                    {
                        // new item
                        ViewState["PackageId"] = PanelSecurity.PackageId;
                        usernameControl.SetPackagePolicy(PanelSecurity.PackageId, policyName, "UserNamePolicy");
                        passwordControl.SetPackagePolicy(PanelSecurity.PackageId, policyName, "UserPasswordPolicy");
                        BindDatabases(PanelSecurity.PackageId);
                    }
                }

                // load provider control
                LoadProviderControl((int)ViewState["PackageId"], SqlDatabases.GetDatabasesGroupName(Settings),
                    providerControl, "EditUser.ascx");

                IDatabaseEditUserControl ctrl = (IDatabaseEditUserControl)providerControl.Controls[0];
                ctrl.InitControl(SqlDatabases.GetDatabasesGroupName(Settings));

                if (!IsPostBack)
                {
                    // bind item to controls
                    if (item != null)
                    {
                        // bind item to controls
                        usernameControl.Text = item.Name;
                        usernameControl.EditMode = true;
                        passwordControl.EditMode = true;

                        foreach (string database in item.Databases)
                        {
							foreach (ListItem li in dlDatabases.Items)
							{
								if (String.Compare(database, li.Value, true) == 0)
								{
									li.Selected = true;
									break;
								}
							}
                        }

                        // other controls
                        ctrl.BindItem(item);
                    }
                }
            }
            catch
            {
                ShowWarningMessage("INIT_SERVICE_ITEM_FORM");
                DisableFormControls(this, btnCancel);
                return;
            }
        }

        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            SqlUser item = new SqlUser();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;
            item.Name = usernameControl.Text;
            item.Password = passwordControl.Password;

            List<string> databases = new List<string>();
            foreach (ListItem li in dlDatabases.Items)
            {
                if (li.Selected)
                    databases.Add(li.Value);
            }
            item.Databases = databases.ToArray();

            // get other props
            IDatabaseEditUserControl ctrl = (IDatabaseEditUserControl)providerControl.Controls[0];
            ctrl.SaveItem(item);

            if (PanelRequest.ItemID == 0)
            {
                // new item
                try
                {
                    int result = ES.Services.DatabaseServers.AddSqlUser(item, SqlDatabases.GetDatabasesGroupName(Settings));
					// Show an error message if the operation has failed to complete
                    if (result < 0)
                    {
                        ShowResultMessageWithContactForm(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SQL_ADD_USER", ex);
                    return;
                }
            }
            else
            {
                // existing item
                try
                {
                    int result = ES.Services.DatabaseServers.UpdateSqlUser(item);
					// Show an error message if the operation has failed to complete
                    if (result < 0)
                    {
						ShowResultMessageWithContactForm(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SQL_UPDATE_USER", ex);
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
                int result = ES.Services.DatabaseServers.DeleteSqlUser(PanelRequest.ItemID);
				// Show an error message if the operation has failed to complete
                if (result < 0)
                {
					ShowResultMessageWithContactForm(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SQL_DELETE_USER", ex);
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
            RedirectSpaceHomePage();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
    }
}
