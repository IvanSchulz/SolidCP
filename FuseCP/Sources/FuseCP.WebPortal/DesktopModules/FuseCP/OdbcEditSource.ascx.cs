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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;
using FuseCP.Providers.Database;
using FuseCP.Providers.OS;

namespace FuseCP.Portal
{
    public partial class OdbcEditSource : FuseCPModuleBase
    {
        SystemDSN item = null;

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
                    string[] supportedDrivers = null;
                    ArrayList drivers = null;
                    
                    
                    // load item if required
                    if (PanelRequest.ItemID > 0)
                    {
                        // existing item
                        try
                        {
                            item = ES.Services.OperatingSystems.GetOdbcSource(PanelRequest.ItemID);
                        }
                        catch (Exception ex)
                        {
                            ShowErrorMessage("DSN_GET", ex);
                            return;
                        }

                        if (item != null)
                        {
                            // save package info
                            ViewState["PackageId"] = item.PackageId;
                            fileLookup.PackageId = item.PackageId;
                            dsnName.SetPackagePolicy(item.PackageId, UserSettings.OS_POLICY, "DsnNamePolicy");
                            supportedDrivers = ES.Services.OperatingSystems.GetInstalledOdbcDrivers(item.PackageId);
                        }
                        else
                            RedirectToBrowsePage();
                    }
                    else
                    {
                        // new item
                        bool isMsSQLavailable = false;
                        ViewState["PackageId"] = PanelSecurity.PackageId;
                        dsnName.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.OS_POLICY, "DsnNamePolicy");
                        fileLookup.PackageId = PanelSecurity.PackageId;
                        fileLookup.SelectedFile = "\\";
                        supportedDrivers = ES.Services.OperatingSystems.GetInstalledOdbcDrivers(PanelSecurity.PackageId);
                        PackageInfo pack = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                        HostingPlanContext cont = ES.Services.Packages.GetHostingPlanContext(pack.PlanId);
                        HostingPlanGroupInfo[] groups = cont.GroupsArray;
                        foreach (HostingPlanGroupInfo info in (groups))
                        {
                            if (info.GroupName.Contains("MsSQL"))
                                isMsSQLavailable = true;
                        }
                        drivers = new ArrayList(supportedDrivers);
                        if (!isMsSQLavailable)
                        {
                            //remove unnecessary drivers from list if MS SQL Server is not available in Hosting Plan
                            drivers.Remove("MsSql");
                            drivers.Remove("MsSqlNative");
                        }
                        
                    }

                    // bind drivers
                    if (supportedDrivers != null)
                        foreach (string driver in supportedDrivers)
                            ddlDriver.Items.Add(new ListItem(driver, driver));

                    ToggleDriverControls(ddlDriver.SelectedValue);
                }

                if (!IsPostBack)
                {
                    // bind item to controls
                    if (item != null)
                    {
                        // bind item to controls
                        passwordControl.EditMode = true;
                        fileLookup.PackageId = item.PackageId;
                        dsnName.Text = item.Name;
                        dsnName.EditMode = true;
                        Utils.SelectListItem(ddlDriver, item.Driver);
                        ddlDriver.Enabled = false;
                        ddlDatabaseName.Enabled = false;
                        ddlDatabaseUser.Enabled = false;

                        ToggleDriverControls(ddlDriver.SelectedValue);

                        // database
                        string driverName = item.Driver;

                        if (driverName == "MsSql" || driverName == "MsSqlNative" || driverName == "MySql" || driverName == "MariaDB")
                        {
                            // unselect currently selected item
                            if (ddlDatabaseName.SelectedIndex != -1)
                                ddlDatabaseName.SelectedItem.Selected = false;

                            foreach (ListItem li in ddlDatabaseName.Items)
                            {
                                if (li.Value.StartsWith(item.DatabaseName + "|"))
                                {
                                    li.Selected = true;
                                    break;
                                }
                            }
                        }
                        else
                            fileLookup.SelectedFile = item.DatabaseName;

                        // user
                        if (driverName == "MsAccess")
                            txtUser.Text = item.DatabaseUser;
                        if (driverName == "MsAccess2010")
                            txtUser.Text = item.DatabaseUser;
                        else
                            Utils.SelectListItem(ddlDatabaseUser, item.DatabaseUser);
                    }
                }
            }
            catch
            {
                ShowWarningMessage("INIT_SERVICE_ITEM_FORM");
                DisableFormControls(this, btnCancel);
            }
        }

        private void ToggleDriverControls(string driverName)
        {
            rowDatabaseName.Visible = rowDatabaseUser.Visible = (driverName == "MsSql"
                || driverName == "MsSqlNative"
                || driverName == "MySql"
                || driverName == "MariaDB");

            rowFile.Visible = (driverName == "MsAccess" || driverName == "MsAccess2010"
                || driverName == "Excel" || driverName == "Excel2010" || driverName == "Text");
            rowUser.Visible = (driverName == "MsAccess" || driverName == "MsAccess2010");
            rowPassword.Visible = (driverName == "MySql" || driverName == "MariaDB" || driverName == "MsAccess" || driverName == "MsAccess2010");

            // bind databases and users
            BindDatabasesAndUsers(driverName);
        }

        private void BindDatabasesAndUsers(string driverName)
        {
            List<SqlDatabase> sqlDatabases = new List<SqlDatabase>();
            List<SqlUser> sqlUsers = new List<SqlUser>();

            int packageId = (int)ViewState["PackageId"];

            if (driverName == "MsSql" || driverName == "MsSqlNative")
            {
                sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MsSql2000, false));
                sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MsSql2005, false));
                sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MsSql2008, false));
                sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MsSql2012, false));
                sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MsSql2014, false));
                sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MsSql2016, false));
                sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MsSql2017, false));
                sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MsSql2019, false));
				sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MsSql2022, false));
				sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MsSql2025, false));
				sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MsSql2000, false));
                sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MsSql2005, false));
                sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MsSql2008, false));
                sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MsSql2012, false));
                sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MsSql2014, false));
                sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MsSql2016, false));
                sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MsSql2017, false));
                sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MsSql2019, false));
				sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MsSql2022, false));
				sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MsSql2025, false));
			}
			else if (driverName == "MySql")
            {
                sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MySql4, false));
                sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MySql5, false));
                sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MySql8, false));
                sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MySql9, false));
                sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MySql4, false));
                sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MySql5, false));
                sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MySql8, false));
                sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MySql9, false));
            }
            else if (driverName == "MariaDB")
            {
                sqlDatabases.AddRange(ES.Services.DatabaseServers.GetSqlDatabases(packageId, ResourceGroups.MariaDB, false));
                sqlUsers.AddRange(ES.Services.DatabaseServers.GetSqlUsers(packageId, ResourceGroups.MariaDB, false));
            }

            // databases
            ddlDatabaseName.Items.Clear();
            foreach (SqlDatabase db in sqlDatabases)
                ddlDatabaseName.Items.Add(new ListItem(db.Name, db.Name + "|" + db.GroupName));
            ddlDatabaseName.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectDatabase"), ""));

            // users
            ddlDatabaseUser.DataSource = sqlUsers;
            ddlDatabaseUser.DataBind();
            ddlDatabaseUser.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectUser"), ""));
        }

        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            SystemDSN item = new SystemDSN();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;
            item.Name = dsnName.Text.Trim();
            item.DatabasePassword = passwordControl.Password;

            // database
            string driverName = ddlDriver.SelectedValue;
            item.Driver = driverName;

            if (driverName == "MsSql" || driverName == "MsSqlNative" || driverName == "MySql" || driverName == "MariaDB")
                item.DatabaseName = ddlDatabaseName.SelectedValue;
            else
                item.DatabaseName = fileLookup.SelectedFile;

            // user
            if (driverName == "MsAccess")
                item.DatabaseUser = txtUser.Text.Trim();
            if (driverName == "MsAccess2010")
                item.DatabaseUser = txtUser.Text.Trim();
            else
                item.DatabaseUser = ddlDatabaseUser.SelectedValue;

            if (PanelRequest.ItemID == 0)
            {
                try
                {
                    // new item
                    int result = ES.Services.OperatingSystems.AddOdbcSource(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("DSN_ADD", ex);
                    return;
                }
            }
            else
            {
                try
                {
                    // existing item
                    int result = ES.Services.OperatingSystems.UpdateOdbcSource(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("DSN_UPDATE", ex);
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
                int result = ES.Services.OperatingSystems.DeleteOdbcSource(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("DSN_DELETE", ex);
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

        protected void ddlDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleDriverControls(ddlDriver.SelectedValue);
        }
    }
}
