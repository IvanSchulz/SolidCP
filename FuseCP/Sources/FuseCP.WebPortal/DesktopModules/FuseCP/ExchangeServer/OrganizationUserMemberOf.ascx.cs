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
using System.Collections.Generic;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal.HostedSolution
{
    public partial class UserMemberOf : FuseCPModuleBase
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

        protected bool EnableDistributionLists
        {
            get
            {
                return Cntx.Groups.ContainsKey(ResourceGroups.Exchange) & Utils.CheckQouta(Quotas.EXCHANGE2007_DISTRIBUTIONLISTS, Cntx);
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
            groups.DistributionListsEnabled = EnableDistributionLists;
            groups.SecurityGroupsEnabled = EnableSecurityGroups;

            if (!IsPostBack)
            {
                BindSettings();

                MailboxTabsId.Visible = (PanelRequest.Context == "Mailbox");

                UserTabsId.Visible = (PanelRequest.Context == "User");
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID);

                groups.DistributionListsEnabled = EnableDistributionLists && (user.AccountType == ExchangeAccountType.Mailbox
                    || user.AccountType == ExchangeAccountType.Room
                        || user.AccountType == ExchangeAccountType.Equipment);

                litDisplayName.Text = user.DisplayName;

                List<ExchangeAccount> groupsList = new List<ExchangeAccount>();

                if (EnableDistributionLists)
                {
                    //Distribution Lists
                    ExchangeAccount[] dLists = ES.Services.ExchangeServer.GetDistributionListsByMember(PanelRequest.ItemID, PanelRequest.AccountID);

                    foreach (ExchangeAccount distList in dLists)
                    {
                        groupsList.Add(distList);
                    }
                }

                if (EnableSecurityGroups)
                {
                    //Security Groups
                    //ExchangeAccount[] securGroups = ES.Services.Organizations.GetUserGroups(PanelRequest.ItemID, PanelRequest.AccountID);
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
                messageBox.ShowErrorMessage("ORGANIZATION_GET_USER_SETTINGS", ex);
            }
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                IList<ExchangeAccount> oldGroups = new List<ExchangeAccount>();

                if (EnableDistributionLists)
                {
                    ExchangeAccount[] oldDistLists = ES.Services.ExchangeServer.GetDistributionListsByMember(PanelRequest.ItemID, PanelRequest.AccountID);

                    foreach (ExchangeAccount distList in oldDistLists)
                    {
                        oldGroups.Add(distList);
                    }
                }

                if (EnableSecurityGroups)
                {
                    ExchangeAccount[] oldSecGroups = ES.Services.Organizations.GetSecurityGroupsByMember(PanelRequest.ItemID, PanelRequest.AccountID);

                    foreach (ExchangeAccount secGroup in oldSecGroups)
                    {
                        oldGroups.Add(secGroup);
                    }
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

                messageBox.ShowSuccessMessage("ORGANIZATION_UPDATE_USER_SETTINGS");

                BindSettings();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ORGANIZATION_UPDATE_USER_SETTINGS", ex);
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
                (PanelRequest.Context == "Mailbox") ? "mailboxes" : "users",
                "SpaceID=" + PanelSecurity.PackageId));
        }

    }
}
