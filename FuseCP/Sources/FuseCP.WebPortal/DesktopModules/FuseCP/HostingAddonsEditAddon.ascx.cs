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
using System.Web;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class HostingAddonsEditAddon : FuseCPModuleBase
    {
		protected bool ShouldCopyCurrentHostingAddon()
		{
			return (HttpContext.Current.Request["TargetAction"] == "Copy");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
			btnDelete.Visible = (PanelRequest.PlanID > 0) && (!ShouldCopyCurrentHostingAddon());

            if (!IsPostBack)
            {
                try
                {
                    BindPlan();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("ADDON_GET_ADDON", ex);
                    return;
                }
            }
        }

        private void BindPlan()
        {
            if (PanelRequest.PlanID == 0)
            {
                // new plan
                BindQuotas();
                return;
            }

            HostingPlanInfo plan = ES.Services.Packages.GetHostingPlan(PanelRequest.PlanID);
            if (plan == null)
                // plan not found
                RedirectBack();

			if (ShouldCopyCurrentHostingAddon())
			{
				plan.PlanId = 0;
				plan.PlanName = "Copy of " + plan.PlanName;
			}

            // bind plan
            txtPlanName.Text = PortalAntiXSS.DecodeOld(plan.PlanName);
            txtPlanDescription.Text = PortalAntiXSS.DecodeOld(plan.PlanDescription);
            //chkAvailable.Checked = plan.Available;

            //txtSetupPrice.Text = plan.SetupPrice.ToString("0.00");
            //txtRecurringPrice.Text = plan.RecurringPrice.ToString("0.00");
            //txtRecurrenceLength.Text = plan.RecurrenceLength.ToString();
            //Utils.SelectListItem(ddlRecurrenceUnit, plan.RecurrenceUnit);

            // bind quotas
            BindQuotas();
        }

        private void BindQuotas()
        {
            hostingPlansQuotas.BindPlanQuotas(-1, PanelRequest.PlanID, -1);
        }

        private void SavePlan()
        {
            if (!Page.IsValid)
                return;

            // gather form info
            HostingPlanInfo plan = new HostingPlanInfo();
            plan.UserId = PanelSecurity.SelectedUserId;
            plan.PlanId = PanelRequest.PlanID;
            plan.IsAddon = true;
            plan.PlanName = txtPlanName.Text;
            plan.PlanDescription = txtPlanDescription.Text;
            plan.Available = true; // always available

            plan.SetupPrice = 0;
            plan.RecurringPrice = 0;
            plan.RecurrenceLength = 1;
            plan.RecurrenceUnit = 2; // month

            plan.Groups = hostingPlansQuotas.Groups;
            plan.Quotas = hostingPlansQuotas.Quotas;

            int planId = PanelRequest.PlanID;
			if ((PanelRequest.PlanID == 0) || ShouldCopyCurrentHostingAddon())
            {
                // new plan
                try
                {
                    planId = ES.Services.Packages.AddHostingPlan(plan);
                    if (planId < 0)
                    {
                        ShowResultMessage(planId);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("ADDON_ADD_ADDON", ex);
                    return;
                }
            }
            else
            {
                // update plan
                try
                {
                    PackageResult result = ES.Services.Packages.UpdateHostingPlan(plan);
                    lblMessage.Text = PortalAntiXSS.Encode(GetExceedingQuotasMessage(result.ExceedingQuotas));
                    if (result.Result < 0)
                    {
                        ShowResultMessage(result.Result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("ADDON_UPDATE_ADDON", ex);
                    return;
                }
            }

            // redirect
            RedirectBack();
        }

        private void DeletePlan()
        {
            try
            {
                int result = ES.Services.Packages.DeleteHostingPlan(PanelRequest.PlanID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("ADDON_DELETE_ADDON", ex);
                return;
            }

            // redirect
            RedirectBack();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SavePlan();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeletePlan();
        }

        private void RedirectBack()
        {
            Response.Redirect(NavigateURL(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString()));
        }
    }
}
