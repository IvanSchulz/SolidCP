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
using System.Text;

namespace FuseCP.Portal
{
    public partial class SpaceNestedSpaces : FuseCPModuleBase
    {
        private int columChangedDate = 0; 
        protected void Page_Load(object sender, EventArgs e)
        {
            // set display preferences
            gvPackages.PageSize = UsersHelper.GetDisplayItemsPerPage();

            if (!IsPostBack)
            {
                searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
                searchBox.AddCriteria("FullName", GetLocalizedString("SearchField.Name"));
                searchBox.AddCriteria("Email", GetLocalizedString("SearchField.EMail"));

                // set inital controls state from request
                if (Request["FilterColumn"] != null)
                    searchBox.FilterColumn = Request["FilterColumn"];
                if (Request["FilterValue"] != null)
                    searchBox.FilterValue = Request["FilterValue"];
                if (Request["StatusID"] != null)
                    Utils.SelectListItem(ddlStatus, Request["StatusID"]);
            }
            gvPackages.Columns[columChangedDate].Visible = false;
            //if (ddlStatus.SelectedValue != "1")
            //gvPackages.Columns[gvPackages.Columns.Count - 2].Visible = true;
            if (ddlStatus.SelectedItem.Value != "1")
                gvPackages.Columns[columChangedDate].Visible = true;

            searchBox.AjaxData = this.GetSearchBoxAjaxData();
        }

        public string GetUserHomePageUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        public string GetSpaceHomePageUrl(int spaceId)
        {
            return PortalUtils.GetSpaceHomePageUrl(spaceId);
        }

        public string GetNestedSpacesPageUrl(string parameterName, string parameterValue)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(),
                parameterName + "=" + parameterValue);
        }

        protected void odsNestedPackages_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception.InnerException);
                //this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'NestedPackages'");
            res.Append(", RedirectUrl: '" + NavigateURL(PortalUtils.SPACE_ID_PARAM, "{0}").Substring(2) + "'");

            res.Append(", PackageID: " + (String.IsNullOrEmpty(Request["SpaceID"]) ? "0" : Request["SpaceID"]));
            res.Append(", StatusID: $('#" + ddlStatus.ClientID + "').val()");
            res.Append(", PlanID: " + (String.IsNullOrEmpty(Request["PlanID"]) ? "0" : Request["PlanID"]));
            res.Append(", ServerID: " + (String.IsNullOrEmpty(Request["ServerID"]) ? "0" : Request["ServerID"]));
            return res.ToString();
        }

        protected void gvPackages_DataBound(object sender, EventArgs e)
        {
            if (Request["StatusID"] == "1")
            {
                gvPackages.Columns[columChangedDate].Visible = false;
            }                
        }

        protected void gvPackages_Init(object sender, EventArgs e)
        {
            for (int i = 0; i < gvPackages.Columns.Count; i++) //get Index of Column gvPackagesStatusIDchangeDate
            {
                if (gvPackages.Columns[i].HeaderText == "gvPackagesStatusIDchangeDate")
                {
                    columChangedDate = i;
                    break;
                }
            }
        }
    }
}
