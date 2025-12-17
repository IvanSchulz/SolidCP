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
using System.Text;

namespace FuseCP.Portal.UserControls
{
    public partial class SpaceServiceItems : FuseCPControlBase
    {
        public string GroupName
        {
            get { return litGroupName.Text; }
            set { litGroupName.Text = value; }
        }

        public string TypeName
        {
            get { return litTypeName.Text; }
            set { litTypeName.Text = value; }
        }

        public string QuotaName
        {
            get { return (string)ViewState["QuotaName"]; }
            set { ViewState["QuotaName"] = value; }
        }

        public string CreateControlID
        {
            get { return ViewState["CreateControlID"] == null ? "edit" : (string)ViewState["CreateControlID"]; }
            set { ViewState["CreateControlID"] = value; }
        }

        public string ViewLinkText
        {
            get { return ViewState["ViewLinkText"] == null ? null : (string)ViewState["ViewLinkText"]; }
            set { ViewState["ViewLinkText"] = value; }
        }

        public string CreateButtonText
        {
            get { return btnAddItem.Text; }
            set { btnAddItem.Text = value; }
        }

        public bool ShowCreateButton
        {
            get { EnsureChildControls(); return btnAddItembtn.Visible; }
            set { EnsureChildControls(); btnAddItembtn.Visible = value; }
        }

        public bool ShowQuota
        {
            get { EnsureChildControls(); return QuotasPanel.Visible; }
            set { EnsureChildControls(); QuotasPanel.Visible = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));

            //HideServiceColumns(gvWebSites);

            // set display preferences
            gvItems.PageSize = UsersHelper.GetDisplayItemsPerPage();

            itemsQuota.QuotaName = QuotaName;
            lblQuotaName.Text = GetSharedLocalizedString("Quota." + QuotaName) + ":";

            // edit button
            string localizedButtonText = HostModule.GetLocalizedString(btnAddItem.Text + ".Text");
            if (localizedButtonText != null)
                btnAddItem.Text = localizedButtonText;

            // visibility
            chkRecursive.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User);
            gvItems.Columns[2].Visible = !String.IsNullOrEmpty(ViewLinkText);
            gvItems.Columns[3].Visible = gvItems.Columns[4].Visible =
                                         (PanelSecurity.SelectedUser.Role != UserRole.User) && chkRecursive.Checked;
            gvItems.Columns[5].Visible = (PanelSecurity.SelectedUser.Role == UserRole.Administrator);
            gvItems.Columns[6].Visible = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);

            ShowActionList();

            if (!IsPostBack)
            {
                // toggle controls
                btnAddItembtn.Enabled = PackagesHelper.CheckGroupQuotaEnabled(
                    PanelSecurity.PackageId, GroupName, QuotaName);

                searchBox.AddCriteria("ItemName", GetLocalizedString("SearchField.ItemName"));
                searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
                searchBox.AddCriteria("FullName", GetLocalizedString("SearchField.FullName"));
                searchBox.AddCriteria("Email", GetLocalizedString("SearchField.EMail"));
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();
        }

        public string GetUrl(object param1, object param2)
        {
            string url = GetItemEditUrl(param1, param2) + "&Mode=View";
            url = "http://localhost:8080/Portal" + url.Remove(0, 1);
            string encodedUrl = System.Web.HttpUtility.UrlPathEncode(url);
            return string.Format("javascript:void(window.open('{0}','{1}','{2}'))", encodedUrl, "window", "width=400,height=300,scrollbars,resizable");
        }

        public string GetUrlNormalized(object param1, object param2)
        {
            string url = GetItemEditUrl(param1, param2) + "&Mode=View";
            return System.Web.HttpUtility.UrlPathEncode(url);
        }

        public string GetItemEditUrl(object packageId, object itemId)
        {
            return HostModule.EditUrl("ItemID", itemId.ToString(), "edit_item",
                 PortalUtils.SPACE_ID_PARAM + "=" + packageId.ToString());
        }

        public string GetUserHomePageUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        public string GetSpaceHomePageUrl(int spaceId)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM, spaceId.ToString());
        }

        public string GetItemsPageUrl(string parameterName, string parameterValue)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(),
                parameterName + "=" + parameterValue);
        }

        protected void odsItemsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                HostModule.ProcessException(e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            Response.Redirect(HostModule.EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(),
                CreateControlID));
        }

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detach")
            {
                // remove item from meta base
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                int result = ES.Services.Packages.DetachPackageItem(itemId);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return;
                }

                // refresh the list
                gvItems.DataBind();
            }
        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HyperLink lnkView = (HyperLink)e.Row.FindControl("lnkView");
            if (lnkView == null)
                return;

            string localizedLinkText = HostModule.GetLocalizedString(ViewLinkText + ".Text");
            lnkView.Text = localizedLinkText != null ? localizedLinkText : ViewLinkText;
        }

        private void ShowActionList()
        {
            var checkboxColumn = gvItems.Columns[0];
            websiteActions.Visible = false;
            mailActions.Visible = false;
            checkboxColumn.Visible = false;

            switch (QuotaName)
            {
                case "Web.Sites":
                    websiteActions.Visible = true;
                    checkboxColumn.Visible = true;
                    break;
                case "Mail.Accounts":
                    ProviderInfo provider = ES.Services.Servers.GetPackageServiceProvider(PanelSecurity.PackageId, "Mail");
                    if (provider.EditorControl == "SmarterMail100")
                    {
                        mailActions.Visible = true;
                        checkboxColumn.Visible = true;
                    }
                    break;
                case "Mail.AllowAccessControls":
                    gvItems.Columns[6].Visible = false;
                    break;

            }
        }

        public string GetSearchBoxAjaxData()
        {
            String spaceId = (String.IsNullOrEmpty(Request["SpaceID"]) ? "-1" : Request["SpaceID"]);
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'ServiceItems'");
            res.Append(", RedirectUrl: '" + GetItemEditUrl(spaceId, "{0}").Substring(2) + "'");
            res.Append(", PackageID: " + spaceId);
            res.Append(", ItemTypeName: $('#" + litTypeName.ClientID + "').val()");
            res.Append(", GroupName: $('#" + litGroupName.ClientID + "').val()");
            res.Append(", ServerID: " + (String.IsNullOrEmpty(Request["ServerID"]) ? "0" : Request["ServerID"]));
            res.Append(", Recursive: ($('#" + chkRecursive.ClientID + "').val() == 'on')");
            return res.ToString();
        }

    }
}
