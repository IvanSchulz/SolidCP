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
using FuseCP.Portal.Code.UserControls;
using FuseCP.WebPortal;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer.UserControls
{
    public partial class MailboxTabs : FuseCPControlBase
    {
        private string selectedTab;
        public string SelectedTab
        {
            get { return selectedTab; }
            set { selectedTab = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindTabs()
        {
            bool hideItems = false;

            UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            Providers.HostedSolution.ExchangeMailboxPlan plan = ES.Services.ExchangeServer.GetExchangeMailboxPlan(PanelRequest.ItemID, ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID).MailboxPlanId);

            if (user != null)
            {
                if ((user.Role == UserRole.User) & (Utils.CheckQouta(Quotas.EXCHANGE2007_ISCONSUMER, cntx)))
                    hideItems = true;
            }

            List<Tab> tabsList = new List<Tab>();
            tabsList.Add(CreateTab("edit_user", "Tab.General"));
            if (PanelRequest.Context == "JournalingMailbox")
            {
                tabsList.Add(CreateTab("journaling_mailbox_settings", "Tab.Settings"));
            }
            else
            {
                tabsList.Add(CreateTab("mailbox_settings", "Tab.Settings"));
            }
            if (!hideItems && PanelRequest.Context == "Mailbox") tabsList.Add(CreateTab("mailbox_addresses", "Tab.Addresses"));
            if (!hideItems && PanelRequest.Context == "Mailbox") tabsList.Add(CreateTab("mailbox_mailflow", "Tab.Mailflow"));
            if (!hideItems && PanelRequest.Context == "Mailbox") tabsList.Add(CreateTab("mailbox_permissions", "Tab.Permissions"));
            if (plan != null && plan.EnableAutoReply && PanelRequest.Context == "Mailbox") tabsList.Add(CreateTab("mailbox_autoreply", "Tab.AutoReply"));

            string instructions = ES.Services.ExchangeServer.GetMailboxSetupInstructions(PanelRequest.ItemID, PanelRequest.AccountID, false, false, false, " ");
            if (!string.IsNullOrEmpty(instructions) && PanelRequest.Context == "Mailbox")
                tabsList.Add(CreateTab("mailbox_setup", "Tab.Setup"));

            if (!hideItems && PanelRequest.Context == "Mailbox") tabsList.Add(CreateTab("mailbox_mobile", "Tab.Mobile"));
            //tabsList.Add(CreateTab("mailbddox_spam", "Tab.Spam"));

            if (Utils.CheckQouta(Quotas.EXCHANGE2007_DISTRIBUTIONLISTS, cntx))
                tabsList.Add(CreateTab("mailbox_memberof", "Tab.MemberOf"));

            // find selected menu item
            int idx = 0;
            foreach (Tab tab in tabsList)
            {
                if (String.Compare(tab.Id, SelectedTab, true) == 0)
                    break;
                idx++;
            }
            dlTabs.SelectedIndex = idx;

            dlTabs.DataSource = tabsList;
            dlTabs.DataBind();
        }

        private Tab CreateTab(string id, string text)
        {
            return new Tab(id, GetLocalizedString(text),
                HostModule.EditUrl("AccountID", PanelRequest.AccountID.ToString(), id,
                "SpaceID=" + PanelSecurity.PackageId.ToString(),
                "ItemID=" + PanelRequest.ItemID.ToString(),
                "Context=" + PanelRequest.Context));
        }


    }
}
