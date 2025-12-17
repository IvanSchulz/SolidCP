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
    public partial class WebSitesMimeTypesControl : FuseCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindWebItem(WebAppVirtualDirectory item)
        {
            gvMimeTypes.DataSource = item.MimeMaps;
            gvMimeTypes.DataBind();
        }

        public void SaveWebItem(WebAppVirtualDirectory item)
        {
            item.MimeMaps = CollectFormData(false).ToArray();
        }

        public List<MimeMap> CollectFormData(bool includeEmpty)
        {
            List<MimeMap> types = new List<MimeMap>();
            foreach (GridViewRow row in gvMimeTypes.Rows)
            {
                TextBox txtExtension = (TextBox)row.FindControl("txtExtension");
                TextBox txtMimeType = (TextBox)row.FindControl("txtMimeType");

                // create a new HttpError object and add it to the collection
                MimeMap type = new MimeMap();
                type.Extension = txtExtension.Text.Trim();
                type.MimeType = txtMimeType.Text.Trim();

                if (includeEmpty || type.Extension != "")
                    types.Add(type);
            }

            return types;
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            List<MimeMap> types = CollectFormData(true);

            // add empty error
            MimeMap type = new MimeMap();
            type.Extension = "";
            type.MimeType = "";
            types.Add(type);

            gvMimeTypes.DataSource = types;
            gvMimeTypes.DataBind();
        }
        protected void gvMimeTypes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete_item")
            {
                List<MimeMap> types = CollectFormData(true);

                // remove error
                types.RemoveAt(Utils.ParseInt((string)e.CommandArgument, 0));

                gvMimeTypes.DataSource = types;
                gvMimeTypes.DataBind();
            }
        }
        protected void gvMimeTypes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            StyleButton cmdDelete = (StyleButton)e.Row.FindControl("cmdDeleteMime");
            if (cmdDelete != null)
                cmdDelete.CommandArgument = e.Row.RowIndex.ToString();
        }
    }
}
