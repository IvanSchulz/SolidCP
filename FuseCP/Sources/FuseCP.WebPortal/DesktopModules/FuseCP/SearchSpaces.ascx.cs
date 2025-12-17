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

using FuseCP.WebPortal;
using FuseCP.Portal.UserControls;

namespace FuseCP.Portal
{
    public partial class SearchSpaces : FuseCPModuleBase
    {

        string ItemTypeName;

        const string type_WebSite = "WebSite";
        const string type_Domain = "Domain";
        const string type_Organization = "Organization";

        List<string> linkTypes = new List<string>(new string[] {type_WebSite, type_Domain, type_Organization});

        const string PID_SPACE_WEBSITES = "SpaceWebSites";
        const string PID_SPACE_DIMAINS = "SpaceDomains";
        const string PID_SPACE_EXCHANGESERVER = "SpaceExchangeServer";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // bind item types
                DataTable dtItemTypes = ES.Services.Packages.GetSearchableServiceItemTypes().Tables[0];
                foreach (DataRow dr in dtItemTypes.Rows)
                {
                    string displayName = dr["DisplayName"].ToString();
                    ddlItemType.Items.Add(new ListItem(
                        GetSharedLocalizedString("ServiceItemType." + displayName),
                        dr["ItemTypeID"].ToString()));

                    if (Request["ItemTypeID"] == dr["ItemTypeID"].ToString())
                        ItemTypeName = displayName;
                }

                // bind filter
                Utils.SelectListItem(ddlItemType, Request["ItemTypeID"]);
                tbSearch.Text = Request["Query"];
            }
        }

        public string GetUserHomePageUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        public string GetSpaceHomePageUrl(int spaceId)
        {
            return PortalUtils.GetSpaceHomePageUrl(spaceId);
        }

        public string GetItemPageUrl(int itemId, int spaceId)
        {
            string res = "";

            switch(ItemTypeName)
            {
                case type_WebSite:
                    res = PortalUtils.NavigatePageURL(PID_SPACE_WEBSITES, "ItemID", itemId.ToString(),
                        PortalUtils.SPACE_ID_PARAM + "=" + spaceId, DefaultPage.CONTROL_ID_PARAM + "=" + "edit_item",
                        "moduleDefId=websites");
                    break;
                case type_Domain:
                    res = PortalUtils.NavigatePageURL(PID_SPACE_DIMAINS, "DomainID", itemId.ToString(),
                        PortalUtils.SPACE_ID_PARAM + "=" + spaceId, DefaultPage.CONTROL_ID_PARAM + "=" + "edit_item",
                        "moduleDefId=domains");
                    break;
                case type_Organization:
                    res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                        PortalUtils.SPACE_ID_PARAM + "=" + spaceId, DefaultPage.CONTROL_ID_PARAM + "=" + "organization_home",
                        "moduleDefId=ExchangeServer");
                    break;
            }

            return res;
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            string query = tbSearchText.Text.Trim().Replace("%", "");
            if (query.Length == 0)
                query = tbSearch.Text.Trim().Replace("%", "");

            Response.Redirect(NavigateURL(
                PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                "ItemTypeID=" + ddlItemType.SelectedValue,
                "Query=" + Server.UrlEncode(query)));
        }

        protected void odsPackagesPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception.InnerException);
                e.ExceptionHandled = true;
            }
        }

        public bool AllowItemLink()
        {
            bool res = linkTypes.Exists(x => x == ItemTypeName);

            return res;
        }
    }
}
