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
using CPCC;

using FuseCP.Providers.Web;

namespace FuseCP.Portal
{
    public partial class WebSitesCustomHeadersControl : FuseCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindWebItem(WebAppVirtualDirectory item)
        {
            gvCustomHeaders.DataSource = item.HttpHeaders;
            gvCustomHeaders.DataBind();
        }

        public void SaveWebItem(WebAppVirtualDirectory item)
        {
            item.HttpHeaders = CollectFormData(false).ToArray();
        }

        public List<HttpHeader> CollectFormData(bool includeEmpty)
        {
            List<HttpHeader> headers = new List<HttpHeader>();
            foreach (GridViewRow row in gvCustomHeaders.Rows)
            {
                TextBox txtName = (TextBox)row.FindControl("txtName");
                TextBox txtValue = (TextBox)row.FindControl("txtValue");

                // create a new HttpError object and add it to the collection
                HttpHeader header = new HttpHeader();
                header.Key = txtName.Text.Trim();
                header.Value = txtValue.Text.Trim();

                if (includeEmpty || header.Key != "")
                    headers.Add(header);
            }

            return headers;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            List<HttpHeader> headers = CollectFormData(true);

            // add empty error
            HttpHeader header = new HttpHeader();
            header.Key = "";
            header.Value = "";
            headers.Add(header);

            gvCustomHeaders.DataSource = headers;
            gvCustomHeaders.DataBind();
        }
        protected void gvCustomHeaders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete_item")
            {
                List<HttpHeader> headers = CollectFormData(true);

                // remove error
                headers.RemoveAt(Utils.ParseInt((string)e.CommandArgument, 0));

                gvCustomHeaders.DataSource = headers;
                gvCustomHeaders.DataBind();
            }
        }
        protected void gvCustomHeaders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            StyleButton cmdDelete = (StyleButton)e.Row.FindControl("cmdDelete");
            if (cmdDelete != null)
                cmdDelete.CommandArgument = e.Row.RowIndex.ToString();
        }
    }
}
