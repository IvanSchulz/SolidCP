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
using FuseCP.EnterpriseServer;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal.SfB
{
    public partial class SfBUserPlans : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlans();

                txtStatus.Visible = false;

                if (PanelSecurity.LoggedUser.Role == UserRole.User)
                {
                    PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                    if (cntx.Quotas.ContainsKey(Quotas.SFB_ENABLEDPLANSEDITING))
                    {
                        if (cntx.Quotas[Quotas.SFB_ENABLEDPLANSEDITING].QuotaAllocatedValue != 1)
                        {
                            gvPlans.Columns[2].Visible = false;
                            btnAddPlan.Enabled = btnAddPlan.Visible = false;
                        }
                    }
                }


            }

        }

        public string GetPlanDisplayUrl(string SfBUserPlanId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "add_sfbuserplan",
                    "SfBUserPlanId=" + SfBUserPlanId,
                    "ItemID=" + PanelRequest.ItemID);
        }


        private void BindPlans()
        {
            SfBUserPlan[] list = ES.Services.SfB.GetSfBUserPlans(PanelRequest.ItemID);

            gvPlans.DataSource = list;
            gvPlans.DataBind();

            //check if organization has only one default domain
            if (gvPlans.Rows.Count == 1)
            {
                btnSetDefaultPlan.Enabled = false;
            }

            btnSave.Enabled = (gvPlans.Rows.Count >= 1);
        }

        public string IsChecked(bool val)
        {
            return val ? "checked" : "";
        }

        protected void btnAddPlan_Click(object sender, EventArgs e)
        {
            btnSetDefaultPlan.Enabled = true;
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "add_sfbuserplan",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void gvPlan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                int planId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                try
                {
                    SfBUserPlan plan = ES.Services.SfB.GetSfBUserPlan(PanelRequest.ItemID, planId);

                    if (plan.SfBUserPlanType > 0)
                    {
                        ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                        BindPlans();
                        return;
                    }


                    int result = ES.Services.SfB.DeleteSfBUserPlan(PanelRequest.ItemID, planId);

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
                    messageBox.ShowErrorMessage("SFB_DELETE_PLAN");
                }

                BindPlans();
            }
        }

        protected void btnSetDefaultPlan_Click(object sender, EventArgs e)
        {
            // get domain
            int planId = Utils.ParseInt(Request.Form["DefaultPlan"], 0);

            try
            {
                /*
                SfBUserPlan plan = ES.Services.SfB.GetSfBUserPlan(PanelRequest.ItemID, planId);

                if (plan.SfBUserPlanType > 0)
                {
                    ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                    BindPlans();
                    return;
                }
                */
                ES.Services.SfB.SetOrganizationDefaultSfBUserPlan(PanelRequest.ItemID, planId);

                ShowSuccessMessage("REQUEST_COMPLETED_SUCCESFULLY");

                // rebind domains
                BindPlans();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SFB_SET_DEFAULT_PLAN", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            txtStatus.Visible = true;

            try
            {
                SfBUser[] Accounts = ES.Services.SfB.GetSfBUsersByPlanId(PanelRequest.ItemID, Convert.ToInt32(sfbUserPlanSelectorSource.planId));

                foreach (SfBUser a in Accounts)
                {
                    txtStatus.Text = "Completed";
                    SfBUserResult result = ES.Services.SfB.SetUserSfBPlan(PanelRequest.ItemID, a.AccountID, Convert.ToInt32(sfbUserPlanSelectorTarget.planId));
                    if (result.IsSuccess)
                    {
                        BindPlans();
                        txtStatus.Text = "Error: " + a.DisplayName;
                        ShowErrorMessage("SFB_FAILED_TO_STAMP");
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage("SFB_FAILED_TO_STAMP", ex);
            }
        }



        public string GetPlanType(int planType)
        {
            string imgName = string.Empty;

            SfBUserPlanType type = (SfBUserPlanType)planType;
            switch (type)
            {
                case SfBUserPlanType.Reseller:
                    imgName = "company24.png";
                    break;
                case SfBUserPlanType.Administrator:
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
