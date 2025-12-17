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

using FuseCP.Providers.Database;

namespace FuseCP.Portal.ProviderControls
{
    public partial class MSSQL_EditUser : FuseCPControlBase, IDatabaseEditUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void InitControl(string groupName)
        {
            if (!IsPostBack)
            {
                BindDatabases(PanelSecurity.PackageId, groupName);
            }
        }

        public void BindItem(SqlUser item)
        {
            // bind again
            BindDatabases(item.PackageId, item.GroupName);

            Utils.SelectListItem(ddlDefaultDatabase, item.DefaultDatabase);
        }

        public void SaveItem(SqlUser item)
        {
            item.DefaultDatabase = ddlDefaultDatabase.SelectedValue;
        }

        private void BindDatabases(int packageId, string groupName)
        {
            try
            {
                SqlDatabase[] databases = ES.Services.DatabaseServers.GetSqlDatabases(packageId,
                    groupName, false);

                ddlDefaultDatabase.DataSource = databases;
                ddlDefaultDatabase.DataBind();
                ddlDefaultDatabase.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectDatabase"), ""));
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("SQL_GET_DATABASE", ex);
                return;
            }
        }
    }
}
