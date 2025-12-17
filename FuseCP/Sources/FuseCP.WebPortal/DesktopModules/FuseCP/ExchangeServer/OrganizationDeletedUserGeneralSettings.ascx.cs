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
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal.HostedSolution
{
    public partial class DeletedUserGeneralSettings : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindServiceLevels();

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
                OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID,
                    PanelRequest.AccountID);

                litDisplayName.Text = PortalAntiXSS.Encode(user.DisplayName);

                lblUserDomainName.Text = user.DomainUserName;

                // bind form
                lblDisplayName.Text = user.DisplayName;

                chkDisable.Checked = user.Disabled;

                lblFirstName.Text = user.FirstName;
                lblInitials.Text = user.Initials;
                lblLastName.Text = user.LastName;



                lblJobTitle.Text = user.JobTitle;
                lblCompany.Text = user.Company;
                lblDepartment.Text = user.Department;
                lblOffice.Text = user.Office;

                if (user.Manager != null)
                {
                    lblManager.Text = user.Manager.DisplayName;
                }

                lblBusinessPhone.Text = user.BusinessPhone;
                lblFax.Text = user.Fax;
                lblHomePhone.Text = user.HomePhone;
                lblMobilePhone.Text = user.MobilePhone;
                lblPager.Text = user.Pager;
                lblWebPage.Text = user.WebPage;

                lblAddress.Text = user.Address;
                lblCity.Text = user.City;
                lblState.Text = user.State;
                lblZip.Text = user.Zip;
                lblCountry.Text = user.Country;

                lblNotes.Text = user.Notes;
                lblExternalEmailAddress.Text = user.ExternalEmail;

                lblExternalEmailAddress.Enabled = user.AccountType == ExchangeAccountType.User;
                lblUserDomainName.Text = user.DomainUserName;

                lblSubscriberNumber.Text = user.SubscriberNumber;
                lblUserPrincipalName.Text = user.UserPrincipalName;

                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_ISCONSUMER))
                {
                    if (cntx.Quotas[Quotas.EXCHANGE2007_ISCONSUMER].QuotaAllocatedValue != 1)
                    {
                        locSubscriberNumber.Visible = false;
                        lblSubscriberNumber.Visible = false;
                    }
                }

                if (user.LevelId > 0 && secServiceLevels.Visible)
                {
                    secServiceLevels.IsCollapsed = false;

                    ServiceLevel serviceLevel = ES.Services.Organizations.GetSupportServiceLevel(user.LevelId);

                    litServiceLevel.Visible = true;
                    litServiceLevel.Text = serviceLevel.LevelName;
                    litServiceLevel.ToolTip = serviceLevel.LevelDescription;

                    lblServiceLevel.Text = serviceLevel.LevelName;
                }

                chkVIP.Checked = user.IsVIP && secServiceLevels.Visible;
                imgVipUser.Visible = user.IsVIP && secServiceLevels.Visible;

                if (cntx.Quotas.ContainsKey(Quotas.ORGANIZATION_ALLOWCHANGEUPN))
                {
                    if (cntx.Quotas[Quotas.ORGANIZATION_ALLOWCHANGEUPN].QuotaAllocatedValue != 1)
                    {
                        chkInherit.Visible = false;
                    }
                    else
                    {
                        chkInherit.Visible = true;
                    }
                }

                if (user.Locked)
                    chkLocked.Enabled = true;
                else
                    chkLocked.Enabled = false;

                chkLocked.Checked = user.Locked;
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ORGANIZATION_GET_USER_SETTINGS", ex);
            }
        }

        private void BindServiceLevels()
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels))
            {
                secServiceLevels.Visible = true;
            }
            else
            {
                secServiceLevels.Visible = false;
            }
        }

        private bool CheckServiceLevelQuota(QuotaValueInfo quota)
        {

            if (quota.QuotaAllocatedValue != -1)
            {
                return quota.QuotaAllocatedValue > quota.QuotaUsedValue;
            }

            return true;
        }
    }
}
