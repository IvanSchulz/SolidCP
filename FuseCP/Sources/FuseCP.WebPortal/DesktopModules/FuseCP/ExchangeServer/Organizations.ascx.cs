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
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class Organizations : FuseCPModuleBase
    {
        private int CurrentDefaultOrgId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // set display preferences
            gvOrgs.PageSize = UsersHelper.GetDisplayItemsPerPage();

            // visibility
            chkRecursive.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User);
            gvOrgs.Columns[2].Visible = gvOrgs.Columns[3].Visible = (PanelSecurity.SelectedUser.Role != UserRole.User) && chkRecursive.Checked;

            btnSetDefaultOrganization.Enabled = !(gvOrgs.Rows.Count < 2);

            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            if (cntx.Quotas.ContainsKey(Quotas.ORGANIZATIONS))
            {
                btnCreate.Enabled = (!(cntx.Quotas[Quotas.ORGANIZATIONS].QuotaAllocatedValue <= gvOrgs.Rows.Count) || (cntx.Quotas[Quotas.ORGANIZATIONS].QuotaAllocatedValue == -1));
            }

            /*
            if (PanelSecurity.LoggedUser.Role == UserRole.User)
            {
                gvOrgs.Columns[2].Visible = gvOrgs.Columns[3].Visible = gvOrgs.Columns[5].Visible = false;
                btnCreate.Enabled = false;
                btnSetDefaultOrganization.Enabled = false;
            }
             */

            if (!Page.IsPostBack)
            {
                RedirectToRequiredOrg();
            }
        }

        private List<string> GetPossibleUrlRefferers()
        {
            List<string> urlReferrers = new List<string>();
            var queryBuilder = new StringBuilder();

            queryBuilder.AppendFormat("?pid=Home&UserID={0}", PanelSecurity.SelectedUserId);

            urlReferrers.Add(queryBuilder.ToString());
            urlReferrers.Add("?pid=Home");
            urlReferrers.Add("?");
            urlReferrers.Add(string.Empty);

            queryBuilder.Clear();

            return urlReferrers;
        }

        private void RedirectToRequiredOrg()
        {
            if (Request.UrlReferrer != null && gvOrgs.Rows.Count > 0)
            {
                List<string> referrers = GetPossibleUrlRefferers();

                if (PanelSecurity.SelectedUser.Role == UserRole.User)
                {
                    if (Request.UrlReferrer.Query.Equals(referrers[0]))
                    {
                        RedirectToOrgHomePage();
                    }
                }

                if (PanelSecurity.LoggedUser.Role == UserRole.User)
                {
                    if (referrers.Contains(Request.UrlReferrer.Query))
                    {
                        RedirectToOrgHomePage();
                    }
                }
            }
        }

        private void RedirectToOrgHomePage()
        {
            if (CurrentDefaultOrgId > 0) Response.Redirect(GetOrganizationEditUrl(CurrentDefaultOrgId.ToString()));

            Response.Redirect(((HyperLink)gvOrgs.Rows[0].Cells[1].Controls[1]).NavigateUrl);
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "create_organization"));
        }

        protected void odsOrgsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("GET_ORGS", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        public string GetOrganizationEditUrl(string itemId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "organization_home",
                    "ItemID=" + itemId);
        }

        public string GetUserHomePageUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        public string GetSpaceHomePageUrl(int spaceId)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM, spaceId.ToString());
        }

        protected void gvOrgs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // delete organization
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                try
                {
                    int result = ES.Services.Organizations.DeleteOrganization(itemId);
                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return;
                    }

                    // rebind grid
                    gvOrgs.DataBind();

                    orgsQuota.BindQuota();

                    PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                    if (cntx.Quotas.ContainsKey(Quotas.ORGANIZATIONS))
                    {
                        btnCreate.Enabled = !(cntx.Quotas[Quotas.ORGANIZATIONS].QuotaAllocatedValue <= gvOrgs.Rows.Count);
                    }

                }
                catch (Exception ex)
                {
                    messageBox.ShowErrorMessage("DELETE_ORG", ex);
                }
            }
        }

        protected void btnSetDefaultOrganization_Click(object sender, EventArgs e)
        {
            // get org
            int newDefaultOrgId = Utils.ParseInt(Request.Form["DefaultOrganization"], CurrentDefaultOrgId);

            try
            {
                ES.Services.Organizations.SetDefaultOrganization(newDefaultOrgId, CurrentDefaultOrgId);

                ShowSuccessMessage("REQUEST_COMPLETED_SUCCESFULLY");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("ORGANIZATION_SET_DEFAULT_ORG", ex);
            }
        }

        public string IsChecked(string val, string itemId)
        {
            if (!string.IsNullOrEmpty(val) && val.ToLowerInvariant() == "true")
            {
                CurrentDefaultOrgId = Utils.ParseInt(itemId, 0);
                return "checked";
            }

            return "";
        }
    }
}
