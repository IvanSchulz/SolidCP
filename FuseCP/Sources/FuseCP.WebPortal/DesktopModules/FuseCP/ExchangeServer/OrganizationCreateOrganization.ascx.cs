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
using System.Linq;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class OrganizationCreateOrganization : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DomainInfo[] domains = ES.Services.Servers.GetMyDomains(PanelSecurity.PackageId).Where(d => !Utils.IsIdnDomain(d.DomainName)).ToArray();
            Organization[] orgs = ES.Services.Organizations.GetOrganizations(PanelSecurity.PackageId, false);
            var list = new List<OrganizationDomainName>();
            SetPolicy(PanelSecurity.PackageId, UserSettings.EXCHANGE_POLICY, "OrgIdPolicy");

            foreach (Organization o in orgs)
            {
                OrganizationDomainName[] tmpList = ES.Services.Organizations.GetOrganizationDomains(o.Id);

                foreach (OrganizationDomainName name in tmpList)
                {
                    list.Add(name);
                }
            }

            if (!IsPostBack)
            {
                foreach (DomainInfo d in domains)
                {
                    if (!d.IsDomainPointer)
                    {
                        bool bAdd = true;
                        foreach (OrganizationDomainName acceptedDomain in list)
                        {
                            if (d.DomainName.ToLower() == acceptedDomain.DomainName.ToLower())
                            {
                                bAdd = false;
                                break;
                            }
                        }
                        if (bAdd)
                        {
                            ddlDomains.Items.Add(d.DomainName.ToLower());
                        }
                    }
                }
                SetDefaultOrgId();
            }

            if (ddlDomains.Items.Count == 0)
            {
                ddlDomains.Visible = btnCreate.Enabled = false;
            }
        }

        private string GetOrgId(string orgIdPolicy, string domainName, int packageId)
        {
            string[] values = orgIdPolicy.Split(';');

            if (values.Length > 1 && Convert.ToBoolean(values[0]))
            {
                try
                {
                    int maxLength = Convert.ToInt32(values[1]);

                    if (domainName.Length > maxLength)
                    {
                        domainName = domainName.Substring(0, maxLength);
                        string orgId = domainName;
                        int counter = 0;

                        while (ES.Services.Organizations.CheckOrgIdExists(orgId))
                        {
                            counter++;
                            orgId = maxLength > 3 ? string.Format("{0}{1}", orgId.Substring(0, orgId.Length - 3), counter.ToString("d3")) : counter.ToString("d3");
                        }

                        return orgId;
                    }
                }
                catch (Exception)
                {
                }
            }

            return domainName;
        }

        public void SetPolicy(int packageId, string settingsName, string key)
        {
            PackageInfo package = PackagesHelper.GetCachedPackage(packageId);

            if (package != null)
            {
                SetOrgIdPolicy(package.UserId, settingsName, key);
            }
        }

        public void SetOrgIdPolicy(int userId, string settingsName, string key)
        {
            UserInfo user = UsersHelper.GetCachedUser(userId);

            if (user != null)
            {
                UserSettings settings = ES.Services.Users.GetUserSettings(userId, settingsName);

                if (settings != null && settings["OrgIdPolicy"] != null)
                {
                    SetOrgIdPolicy(settings);
                }
            }
        }

        private void SetOrgIdPolicy(UserSettings settings)
        {
            string policyValue = settings["OrgIdPolicy"];
            string[] values = policyValue.Split(';');

            if (values.Length > 1 && Convert.ToBoolean(values[0]))
            {
                try
                {
                    int maxLength = Convert.ToInt32(values[1]);
                    txtOrganizationID.MaxLength = maxLength;
                    valRequireCorrectOrgID.ValidationExpression = string.Format("[a-zA-Z0-9.-]{{1,{0}}}", maxLength);
                }
                catch (Exception)
                {
                }
            }
        }

        private void SetDefaultOrgId()
        {
            UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);

            if (user != null)
            {
                string domainName = ddlDomains.SelectedValue;
                if (!string.IsNullOrEmpty(domainName))
                {
                    UserSettings settings = ES.Services.Users.GetUserSettings(user.UserId, UserSettings.EXCHANGE_POLICY);
                    string orgId = domainName.ToLower();

                    if (settings != null && settings["OrgIdPolicy"] != null)
                    {
                        orgId = GetOrgId(settings["OrgIdPolicy"], domainName, PanelSecurity.PackageId);
                    }
                    else
                    {
                        int num = 2;
                        while (ES.Services.Organizations.CheckOrgIdExists(orgId))
                        {
                            orgId = domainName.ToLower() + num.ToString();
                            num++;
                        }
                    }
                    txtOrganizationName.Text = orgId;
                    txtOrganizationID.Text = orgId;
                }
            }
        }

        protected void ddlDomains_Changed(object sender, EventArgs e)
        {
            SetDefaultOrgId();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            CreateOrganization();
        }

        private void CreateOrganization()
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                int itemId = ES.Services.Organizations.CreateOrganization(PanelSecurity.PackageId, txtOrganizationID.Text.Trim().ToLower(), txtOrganizationName.Text.Trim().ToLower(), ddlDomains.SelectedValue.Trim().ToLower());

                if (itemId < 0)
                {
                    messageBox.ShowResultMessage(itemId);
                    return;
                }

                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "organization_home", "ItemID=" + itemId));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ORGANIZATION_CREATE_ORG", ex);
            }
        }
    }
}
