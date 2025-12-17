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
using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxMemberOf : FuseCPModuleBase
    {
        protected PackageContext cntx;

        protected PackageContext Cntx
        {
            get
            {
                if (cntx == null)
                {
                    cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                }

                return cntx;
            }
        }

        protected bool EnableSecurityGroups
        {
            get
            {
                return Utils.CheckQouta(Quotas.ORGANIZATION_SECURITYGROUPS, Cntx);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            groups.SecurityGroupsEnabled = EnableSecurityGroups;

            if (!IsPostBack)
            {
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                BindSettings();

                UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                ExchangeMailbox mailbox = ES.Services.ExchangeServer.GetMailboxGeneralSettings(PanelRequest.ItemID,
                    PanelRequest.AccountID);

                // title
                litDisplayName.Text = mailbox.DisplayName;

                List<ExchangeAccount> groupsList = new List<ExchangeAccount>();

                //Distribution Lists
                ExchangeAccount[] dLists = ES.Services.ExchangeServer.GetDistributionListsByMember(PanelRequest.ItemID, PanelRequest.AccountID);
                foreach (ExchangeAccount distList in dLists)
                {
                    groupsList.Add(distList);
                }

                if (EnableSecurityGroups)
                {
                    //Security Groups
                    ExchangeAccount[] securGroups = ES.Services.Organizations.GetSecurityGroupsByMember(PanelRequest.ItemID, PanelRequest.AccountID);

                    foreach (ExchangeAccount secGroup in securGroups)
                    {
                        groupsList.Add(secGroup);
                    }
                }

                groups.SetAccounts(groupsList.ToArray());

            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_MAILBOX_SETTINGS", ex);
            }
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                IList<ExchangeAccount> oldGroups = new List<ExchangeAccount>();

                if (EnableSecurityGroups)
                {
                    ExchangeAccount[] oldSecGroups = ES.Services.Organizations.GetSecurityGroupsByMember(PanelRequest.ItemID, PanelRequest.AccountID);

                    foreach (ExchangeAccount secGroup in oldSecGroups)
                    {
                        oldGroups.Add(secGroup);
                    }
                }

                ExchangeAccount[] oldDistLists = ES.Services.ExchangeServer.GetDistributionListsByMember(PanelRequest.ItemID, PanelRequest.AccountID);

                foreach (ExchangeAccount distList in oldDistLists)
                {
                    oldGroups.Add(distList);
                }

                IDictionary<string, ExchangeAccountType> newGroups = groups.GetFullAccounts();
                foreach (ExchangeAccount oldGroup in oldGroups)
                {
                    if (newGroups.ContainsKey(oldGroup.AccountName))
                    {
                        newGroups.Remove(oldGroup.AccountName);
                    }
                    else
                    {
                        switch (oldGroup.AccountType)
                        {
                            case ExchangeAccountType.DistributionList:
                                ES.Services.ExchangeServer.DeleteDistributionListMember(PanelRequest.ItemID, oldGroup.AccountName, PanelRequest.AccountID);
                                break;
                            case ExchangeAccountType.SecurityGroup:
                                ES.Services.Organizations.DeleteObjectFromSecurityGroup(PanelRequest.ItemID, PanelRequest.AccountID, oldGroup.AccountName);
                                break;
                        }
                    }
                }

                foreach (KeyValuePair<string, ExchangeAccountType> newGroup in newGroups)
                {
                    switch (newGroup.Value)
                    {
                        case ExchangeAccountType.DistributionList:
                            ES.Services.ExchangeServer.AddDistributionListMember(PanelRequest.ItemID, newGroup.Key, PanelRequest.AccountID);
                            break;
                        case ExchangeAccountType.SecurityGroup:
                            ES.Services.Organizations.AddObjectToSecurityGroup(PanelRequest.ItemID, PanelRequest.AccountID, newGroup.Key);
                            break;
                    }
                }

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_MAILBOX_SETTINGS");
                BindSettings();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_MAILBOX_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            SaveSettings();

            Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                (PanelRequest.Context == "JournalingMailbox" ? "journaling_mailboxes" : "mailboxes"),
                "SpaceID=" + PanelSecurity.PackageId));
        }

    }
}
