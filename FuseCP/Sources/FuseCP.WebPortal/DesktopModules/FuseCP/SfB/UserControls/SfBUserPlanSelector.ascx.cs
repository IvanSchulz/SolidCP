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

namespace FuseCP.Portal.SfB.UserControls
{
    public partial class SfBUserPlanSelector : FuseCPControlBase
    {

        private string planToSelect;

        public string planId
        {
                        
            get {
                if (ddlPlan.Items.Count == 0) return "";
                return ddlPlan.SelectedItem.Value; 
            }
            set
            {
                planToSelect = value;
                foreach(ListItem li in ddlPlan.Items)
                {
                    if (li.Value == value)
                    {
                        ddlPlan.ClearSelection();
                        li.Selected = true;
                        break;
                    }
                }
            }
        }

        public int plansCount
		{
			get
			{
                return this.ddlPlan.Items.Count;
			}
		}


        protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
			{
                BindPlans();
			}
        }

        public FuseCP.Providers.HostedSolution.SfBUserPlan plan
        {
            get
            {
                FuseCP.Providers.HostedSolution.SfBUserPlan[] plans = ES.Services.SfB.GetSfBUserPlans(PanelRequest.ItemID);
                foreach (FuseCP.Providers.HostedSolution.SfBUserPlan planitem in plans)
                {
                    if (planitem.SfBUserPlanId.ToString() == planId) return planitem;
                }
                return null;
            }
        }

        private void BindPlans()
		{
            FuseCP.Providers.HostedSolution.SfBUserPlan[] plans = ES.Services.SfB.GetSfBUserPlans(PanelRequest.ItemID);

            foreach (FuseCP.Providers.HostedSolution.SfBUserPlan plan in plans)
			{
				ListItem li = new ListItem();
                li.Text = plan.SfBUserPlanName;
                li.Value = plan.SfBUserPlanId.ToString();
                li.Selected = plan.IsDefault;
                ddlPlan.Items.Add(li);
			}

            foreach (ListItem li in ddlPlan.Items)
            {
                if (li.Value == planToSelect)
                {
                    ddlPlan.ClearSelection();
                    li.Selected = true;
                    break;
                }
            }

		}
    }
}
