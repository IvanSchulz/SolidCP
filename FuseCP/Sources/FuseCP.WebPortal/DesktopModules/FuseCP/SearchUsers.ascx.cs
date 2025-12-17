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
    public partial class SearchUsers : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Utils.SelectListItem(ddlFilterColumn, Request["Criteria"]);
                tbSearch.Text = Request["Query"];
            }
        }

        public string GetUserHomePageUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            string query = tbSearchText.Text.Trim().Replace("%", "");
            if (query.Length == 0)
                query = tbSearch.Text.Trim().Replace("%", "");

            Response.Redirect(NavigateURL(
                PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                "Criteria=" + ddlFilterColumn.SelectedValue,
                "Query=" + Server.UrlEncode(query)));
        }

        protected void odsUsersPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception.InnerException);
                e.ExceptionHandled = true;
            }
        }
    }
}
