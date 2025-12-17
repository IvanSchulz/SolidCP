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

using FuseCP.EnterpriseServer;
using FuseCP.WebPortal;
using System.Text;

namespace FuseCP.Portal
{
	public partial class UserCustomers : FuseCPModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			// set display preferences
			gvUsers.PageSize = UsersHelper.GetDisplayItemsPerPage();

			if (!IsPostBack)
			{
				searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
				searchBox.AddCriteria("FullName", GetLocalizedString("SearchField.Name"));
				searchBox.AddCriteria("Email", GetLocalizedString("SearchField.EMail"));
				searchBox.AddCriteria("CompanyName", GetLocalizedString("SearchField.CompanyName"));

				// set inital controls state from request
				if (Request["FilterColumn"] != null)
					searchBox.FilterColumn = Request["FilterColumn"];
				if (Request["FilterValue"] != null)
					searchBox.FilterValue = Request["FilterValue"];
				if (Request["StatusID"] != null)
					Utils.SelectListItem(ddlStatus, Request["StatusID"]);
				if (Request["RoleID"] != null)
					Utils.SelectListItem(ddlRole, Request["RoleID"]);


                gvUsers.Sort("Username", System.Web.UI.WebControls.SortDirection.Ascending);

            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();

            searchBox.Focus();
        }

		public string GetUserHomePageUrl(int userId)
		{
			return PortalUtils.GetUserHomePageUrl(userId);
		}

		protected void odsUsersPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
		{
			if (e.Exception != null)
			{
				ProcessException(e.Exception.InnerException);
				this.DisableControls = true;
				e.ExceptionHandled = true;
			}
		}

		protected void btnAddUser_Click(object sender, EventArgs e)
		{
			Response.Redirect(EditUrl(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(), "create_user",
				"frm=customers"));
		}

        protected string GetStateImage(object status)
        {
            string imgName = "enabled.png";

            if (status != null)
            {
                try
                {
                    switch ((int)status)
                    {
                        case (int)UserLoginStatus.Disabled:
                            imgName = "disabled.png";
                            break;
                        case (int)UserLoginStatus.LockedOut:
                            imgName = "locked.png";
                            break;
                        default:
                            imgName = "enabled.png";
                            break;
                    }
                }
                catch (Exception)
                {

                }

            }

            return GetThemedImage("Exchange/" + imgName);
        }

        public string GetSearchBoxAjaxData()
        {
            string userHomePageId = PortalConfiguration.SiteSettings["UserHomePage"];
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'Users'");
            res.Append(", RedirectUrl: '" + NavigatePageURL(userHomePageId, PortalUtils.USER_ID_PARAM, "{0}").Substring(2) + "'");
            res.Append(", UserID: " + (String.IsNullOrEmpty(Request["UserID"]) ? "0" : Request["UserID"]));
            res.Append(", StatusID: $('#" + ddlStatus.ClientID + "').val()");
            res.Append(", RoleID: $('#" + ddlRole.ClientID + "').val()");
            res.Append(", Recursive: false");
            return res.ToString();
        }

	}
}
