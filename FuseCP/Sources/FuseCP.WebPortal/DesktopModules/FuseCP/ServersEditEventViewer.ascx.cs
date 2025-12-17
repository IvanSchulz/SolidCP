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

namespace FuseCP.Portal
{
    public partial class ServersEditEventViewer : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // set display preferences
                gvEntries.PageSize = UsersHelper.GetDisplayItemsPerPage();

                btnClearLog.Enabled = false;
                ddlLogNames.Attributes.Add("onchange", "ShowProgressDialog('Loading...');");
                BindLogNames();
            }
        }

        private void BindLogNames()
        {
            ddlLogNames.DataSource = ES.Services.Servers.GetLogNames(PanelRequest.ServerId);
            ddlLogNames.DataBind();

            // add empty
            ddlLogNames.Items.Insert(0, new ListItem(GetLocalizedString("SelectLog.Text"), ""));
        }

        protected void odsLogEntries_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.Visible = true;
                messageBox.ShowErrorMessage("SERVERSEDITEVENTVIEWER_SELECTED", e.Exception);
                btnClearLog.Enabled = false;
                e.ExceptionHandled = true;
            }
            else
            {
                messageBox.Visible = false;
            }
        }

        protected void btnClearLog_Click(object sender, EventArgs e)
        {
            int result = ES.Services.Servers.ClearLog(PanelRequest.ServerId, ddlLogNames.SelectedValue);
            if (result < 0)
            {
                ShowResultMessage(result);
                return;
            }

            // rebind items
            gvEntries.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_server"));
        }

        protected void gvEntries_DataBound(object sender, EventArgs e)
        {
            if (gvEntries.Rows.Count > 0)
            {
                btnClearLog.Enabled = true;
            }
            else
            {
                btnClearLog.Enabled = false;
            }
        }

        protected void LogNameSelected(object sender, EventArgs e)
        {
            if (gvEntries.PageIndex > 0)
            {
                gvEntries.PageIndex = 0;
                gvEntries.DataBind();
            }
        }
    }
}
