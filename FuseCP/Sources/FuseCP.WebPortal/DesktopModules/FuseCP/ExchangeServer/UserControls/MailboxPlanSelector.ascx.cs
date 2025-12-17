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

namespace FuseCP.Portal.ExchangeServer.UserControls
{
    public partial class MailboxPlanSelector : FuseCPControlBase
    {
        private void UpdateMailboxPlanSelected()
        {
            foreach (ListItem li in ddlMailboxPlan.Items)
            {
                if (li.Value == mailboxPlanToSelect)
                {
                    ddlMailboxPlan.ClearSelection();
                    li.Selected = true;
                    break;
                }
            }

        }

        private string mailboxPlanToSelect = null;

        public string MailboxPlanId
        {
            get {
                if (ddlMailboxPlan.SelectedItem != null)
                    return ddlMailboxPlan.SelectedItem.Value;
                return mailboxPlanToSelect;
            }
            set
            {
                mailboxPlanToSelect = value;
                UpdateMailboxPlanSelected();
            }
        }

        public bool AddNone
        {
            get { return ViewState["AddNone"] != null ? (bool)ViewState["AddNone"] : false; }
            set { ViewState["AddNone"] = value; }
        }


        public int MailboxPlansCount
        {
            get
            {
                return this.ddlMailboxPlan.Items.Count;
            }
        }

        private bool archiving = false;
        public bool Archiving
        {
            get { return archiving; }
            set { archiving = value; }
        }

        private bool isForJournaling = false;
        public bool IsForJournaling
        {
            get { return isForJournaling; }
            set { isForJournaling = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMailboxPlans();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlMailboxPlan.AutoPostBack = (Changed!=null);
        }

        private void BindMailboxPlans()
        {

            FuseCP.Providers.HostedSolution.ExchangeMailboxPlan[] plans = ES.Services.ExchangeServer.GetExchangeMailboxPlans(PanelRequest.ItemID, Archiving);

            if (AddNone)
            {
                ListItem li = new ListItem();
                li.Text =  "None";
                li.Value = "-1";
                li.Selected = false;
                ddlMailboxPlan.Items.Add(li);
            }

            foreach (FuseCP.Providers.HostedSolution.ExchangeMailboxPlan plan in plans)
            {
                if (!archiving && plan.IsForJournaling != isForJournaling) continue;
                ListItem li = new ListItem();
                li.Text = plan.MailboxPlan;
                li.Value = plan.MailboxPlanId.ToString();
                li.Selected = plan.IsDefault;
                ddlMailboxPlan.Items.Add(li);
            }

            UpdateMailboxPlanSelected();

        }

        public event EventHandler Changed = null;
        protected void ddlMailboxPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }
    }
}
