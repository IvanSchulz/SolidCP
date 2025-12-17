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
using System.Web.UI.WebControls;
using System.Web.UI;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.HostedSolution;
using FuseCP.Portal.SkinControls;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxPlans : FuseCPModuleBase
    {
        private bool RetentionPolicy
        {
            get
            {
                return PanelRequest.Ctl.ToLower().Contains("retentionpolicy");
            }
        }

        private Control FindControlRecursive(Control rootControl, string controlID)
        {
            if (rootControl.ID == controlID) return rootControl;

            foreach (Control controlToSearch in rootControl.Controls)
            {
                Control controlToReturn =
                    FindControlRecursive(controlToSearch, controlID);
                if (controlToReturn != null) return controlToReturn;
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            locTitle.Text = RetentionPolicy ? GetLocalizedString("locTitleRetentionPolicy.Text") : GetLocalizedString("locTitle.Text");

            UserSpaceBreadcrumb bc = FindControlRecursive(Page, "breadcrumb") as UserSpaceBreadcrumb;
            if (bc != null)
            {
                Label lbOrgCurPage = bc.FindControl("lbOrgCurPage") as Label;
                if (lbOrgCurPage!=null)
                    lbOrgCurPage.Text = GetLocalizedString( RetentionPolicy ? "Text.PageRetentionPolicyName" : "Text.PageName");
            }
            gvMailboxPlans.Columns[2].Visible = !RetentionPolicy;
            btnSetDefaultMailboxPlan.Visible = !RetentionPolicy;
            secMainTools.Visible = !RetentionPolicy;

            if (!IsPostBack)
            {
                // bind mailboxplans
                BindMailboxPlans();

                txtStatus.Visible = false;

                if (PanelSecurity.LoggedUser.Role == UserRole.User)
                {
                    PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                    if (!RetentionPolicy)
                    {
                      if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_ENABLEDPLANSEDITING))
                      {
                        if (cntx.Quotas[Quotas.EXCHANGE2007_ENABLEDPLANSEDITING].QuotaAllocatedValue != 1)
                        {
                          gvMailboxPlans.Columns[3].Visible = false;
                          btnAddMailboxPlan.Enabled = btnAddMailboxPlan.Visible = false;
                        }
                      }
                    }
                    else
                    {
                      if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2013_ALLOWRETENTIONPOLICY))
                      {
                        if (cntx.Quotas[Quotas.EXCHANGE2013_ALLOWRETENTIONPOLICY].QuotaAllocatedValue != 1)
                        {
                          gvMailboxPlans.Columns[3].Visible = false;
                          btnAddMailboxPlan.Enabled = btnAddMailboxPlan.Visible = false;
                        }
                      }
                    }
                }

            }

        }

        public string GetMailboxPlanDisplayUrl(string MailboxPlanId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "add_mailboxplan",
                    "MailboxPlanId=" + MailboxPlanId,
                    "ItemID=" + PanelRequest.ItemID);
        }


        private void BindMailboxPlans()
        {
            ExchangeMailboxPlan[] list = ES.Services.ExchangeServer.GetExchangeMailboxPlans(PanelRequest.ItemID, RetentionPolicy);

            gvMailboxPlans.DataSource = list;
            gvMailboxPlans.DataBind();

            //check if organization has only one default domain
            if (gvMailboxPlans.Rows.Count == 1)
            {
                btnSetDefaultMailboxPlan.Enabled = false;
            }

            btnSave.Enabled = (gvMailboxPlans.Rows.Count >= 1);
        }

        public string IsChecked(bool val)
        {
            return val ? "checked" : "";
        }

        protected void btnAddMailboxPlan_Click(object sender, EventArgs e)
        {
            btnSetDefaultMailboxPlan.Enabled = true;
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "add_mailboxplan",
                "SpaceID=" + PanelSecurity.PackageId, "archiving="+RetentionPolicy));
        }

        protected void gvMailboxPlan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                int mailboxPlanId = Utils.ParseInt(e.CommandArgument.ToString(), 0);
                

                try
                {
                    ExchangeMailboxPlan plan = ES.Services.ExchangeServer.GetExchangeMailboxPlan(PanelRequest.ItemID, mailboxPlanId);

                    if (plan.MailboxPlanType > 0)
                    {
                        ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                        BindMailboxPlans();
                        return;
                    }

                    int result = ES.Services.ExchangeServer.DeleteExchangeMailboxPlan(PanelRequest.ItemID, mailboxPlanId);

                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return;
                    }
                    else
                        ShowSuccessMessage("REQUEST_COMPLETED_SUCCESFULLY");

                }
                catch (Exception)
                {
                    messageBox.ShowErrorMessage("EXCHANGE_DELETE_MAILBOXPLAN");
                }

                BindMailboxPlans();
            }
        }

        protected void btnSetDefaultMailboxPlan_Click(object sender, EventArgs e)
        {
            // get domain
            int mailboxPlanId = Utils.ParseInt(Request.Form["DefaultMailboxPlan"], 0);

            try
            {
                /*
                ExchangeMailboxPlan plan = ES.Services.ExchangeServer.GetExchangeMailboxPlan(PanelRequest.ItemID, mailboxPlanId);

                if (plan.MailboxPlanType > 0)
                {
                    ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                    BindMailboxPlans();
                    return;
                }
                */

                ES.Services.ExchangeServer.SetOrganizationDefaultExchangeMailboxPlan(PanelRequest.ItemID, mailboxPlanId);

                ShowSuccessMessage("REQUEST_COMPLETED_SUCCESFULLY");

                // rebind domains
                BindMailboxPlans();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("EXCHANGE_SET_DEFAULT_MAILBOXPLAN", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            txtStatus.Visible = true;

            try
            {
                ExchangeAccount[] Accounts = ES.Services.ExchangeServer.GetExchangeAccountByMailboxPlanId(PanelRequest.ItemID, Convert.ToInt32(mailboxPlanSelectorSource.MailboxPlanId));

                foreach (ExchangeAccount a in Accounts)
                {
                    txtStatus.Text = "Completed";
                    int result = ES.Services.ExchangeServer.SetExchangeMailboxPlan(PanelRequest.ItemID, a.AccountId, Convert.ToInt32(mailboxPlanSelectorTarget.MailboxPlanId), a.ArchivingMailboxPlanId, a.EnableArchiving);
                    if (result < 0)
                    {
                        BindMailboxPlans();
                        txtStatus.Text = "Error: " + a.AccountName;
                        ShowErrorMessage("EXCHANGE_FAILED_TO_STAMP");
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage("EXCHANGE_FAILED_TO_STAMP", ex);
            }

            BindMailboxPlans();
        }


        public string GetPlanType(int mailboxPlanType)
        {
            string imgName = string.Empty;

            ExchangeMailboxPlanType planType = (ExchangeMailboxPlanType)mailboxPlanType;
            switch (planType)
            {
                case ExchangeMailboxPlanType.Reseller:
                    imgName = "company24.png";
                    break;
                case ExchangeMailboxPlanType.Administrator:
                    imgName = "company24.png";
                    break;
                default:
                    imgName = "admin_16.png";
                    break;
            }

            return GetThemedImage("Exchange/" + imgName);
        }

    }
}
