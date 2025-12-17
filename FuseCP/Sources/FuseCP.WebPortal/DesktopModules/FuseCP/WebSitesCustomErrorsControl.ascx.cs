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
    public partial class WebSitesCustomErrorsControl : FuseCPControlBase
    {
        private bool IIs7
        {
            get { return (bool)ViewState["IIs7"]; }
            set { ViewState["IIs7"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public string GetSubCode(object subCode)
        {
            return subCode.ToString() == "-1" ? "" : subCode.ToString();
        }

        public void BindWebItem(WebAppVirtualDirectory item)
        {
            IIs7 = item.IIs7;

            // bind error mode
            ddlErrorMode.Items.Add(HttpErrorsMode.DetailedLocalOnly.ToString());
            ddlErrorMode.Items.Add(HttpErrorsMode.Custom.ToString());
            ddlErrorMode.Items.Add(HttpErrorsMode.Detailed.ToString());

            ddlErrorMode.SelectedValue = item.ErrorMode.ToString();
            
            // bind errors list
            gvErrorPages.DataSource = item.HttpErrors;
            gvErrorPages.DataBind();
        }

        public void SaveWebItem(WebAppVirtualDirectory item)
        {
            item.ErrorMode = GetSelectedErrorMode();
            item.HttpErrors = CollectFormData(false).ToArray();
        }

        private HttpErrorsMode GetSelectedErrorMode()
        {
            return (HttpErrorsMode)Enum.Parse(typeof (HttpErrorsMode), ddlErrorMode.SelectedValue);
        }

        public List<HttpError> CollectFormData(bool includeEmpty)
        {
            List<HttpError> errors = new List<HttpError>();
            foreach (GridViewRow row in gvErrorPages.Rows)
            {
                //CheckBox chkDelete = (CheckBox)row.FindControl("chkDelete");
                TextBox txtErrorCode = (TextBox)row.FindControl("txtErrorCode");
                TextBox txtErrorSubcode = (TextBox)row.FindControl("txtErrorSubcode");
                DropDownList ddlHandlerType = (DropDownList)row.FindControl("ddlHandlerType");
                TextBox txtErrorContent = (TextBox)row.FindControl("txtErrorContent");

                // create a new HttpError object and add it to the collection
                HttpError error = new HttpError();
                error.ErrorCode = txtErrorCode.Text.Trim();
                error.ErrorSubcode = txtErrorSubcode.Text.Trim();
                if (IIs7 && error.ErrorSubcode == "")
                    error.ErrorSubcode = "-1";

                error.HandlerType = ddlHandlerType.SelectedValue;
                error.ErrorContent = txtErrorContent.Text.Trim();

                if (includeEmpty || error.ErrorCode != "")
                    errors.Add(error);
            }

            return errors;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            List<HttpError> errors = CollectFormData(true);

            // add empty error
            HttpError error = new HttpError();
            error.ErrorCode = "";
            error.ErrorSubcode = "";
            error.HandlerType = "URL";
            error.ErrorContent = "";
            errors.Add(error);

            gvErrorPages.DataSource = errors;
            gvErrorPages.DataBind();
        }
        protected void gvErrorPages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete_item")
            {
                List<HttpError> errors = CollectFormData(true);

                // remove error
                errors.RemoveAt(Utils.ParseInt((string)e.CommandArgument, 0));

                gvErrorPages.DataSource = errors;
                gvErrorPages.DataBind();
            }
        }
        protected void gvErrorPages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // fill dropdown
            DropDownList ddlHandlerType = (DropDownList)e.Row.FindControl("ddlHandlerType");
            if (ddlHandlerType != null)
            {
                if (IIs7)
                {
                    // IIS 7
                    ddlHandlerType.Items.Add(new ListItem(GetLocalizedString("IIS7.File"), "File"));
                    ddlHandlerType.Items.Add(new ListItem(GetLocalizedString("IIS7.Redirect"), "Redirect"));
                    ddlHandlerType.Items.Add(new ListItem(GetLocalizedString("IIS7.ExecuteURL"), "ExecuteURL"));
                }
                else
                {
                    // IIS 6
                    ddlHandlerType.Items.Add(new ListItem(GetLocalizedString("IIS6.FILE"), "FILE"));
                    ddlHandlerType.Items.Add(new ListItem(GetLocalizedString("IIS7.URL"), "URL"));
                    ddlHandlerType.Items.Add(new ListItem(GetLocalizedString("IIS7.Default"), "Default"));
                }

                ddlHandlerType.SelectedValue = ((HttpError)e.Row.DataItem).HandlerType;
            }

            StyleButton cmdDelete = (StyleButton)e.Row.FindControl("cmdDelete");
            if (cmdDelete != null)
                cmdDelete.CommandArgument = e.Row.RowIndex.ToString();
        }
    }
}
