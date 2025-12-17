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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class SpaceDetails : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack) { 
                BindSpace();

                UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

                if (user != null)
                {
                    PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                    if ((user.Role == UserRole.User) & (Utils.CheckQouta(Quotas.EXCHANGE2007_ISCONSUMER, cntx)))
                    {
                        lnkSummaryLetter.Visible = false;
                    }

                }

            }
        }

        private void BindSpace()
        {
            // load space
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            if (package != null)
            {
                litSpaceName.Text = PortalAntiXSS.EncodeOld(package.PackageName);
                chkDefault.Checked = package.DefaultTopPackage;

                // bind space status
                PackageStatus status = (PackageStatus)package.StatusId;
                litStatus.Text = PanelFormatter.GetPackageStatusName(package.StatusId);

                cmdActive.Visible = (status != PackageStatus.Active);
                cmdSuspend.Visible = (status == PackageStatus.Active);
                cmdCancel.Visible = (status != PackageStatus.Cancelled);

                StatusBlock.Visible = (PanelSecurity.SelectedUserId != PanelSecurity.EffectiveUserId);

                // bind account details
                litCreated.Text = package.PurchaseDate.ToString();
                bool isNotUser = ((PanelSecurity.LoggedUser.Role != UserRole.User));
                lblSuspendedDate.Visible = litSuspendedDate.Visible = false;
                if (status != PackageStatus.Active)
                {
                    lblSuspendedDate.Visible = litSuspendedDate.Visible = isNotUser;
                    litSuspendedDate.Text = package.StatusIDchangeDate.ToString();
                }
                serverDetails.ServerId = package.ServerId;

                // load plan
                HostingPlanInfo plan = ES.Services.Packages.GetHostingPlan(package.PlanId);
                if (plan != null)
                    litHostingPlan.Text = plan.PlanName;

                // links
                lnkSummaryLetter.NavigateUrl = EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "summary_letter");
                lnkSummaryLetter.Visible = (PanelSecurity.PackageId > 1);

				lnkOverusageReport.NavigateUrl = NavigatePageURL("OverusageReport", PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString());
				OverusageReport.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User);

                lnkEditSpaceDetails.NavigateUrl = EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "edit_details");

				bool ownSpace = (package.UserId == PanelSecurity.EffectiveUserId);
                lnkEditSpaceDetails.Visible = (PanelSecurity.PackageId > 1 && !ownSpace);

                lnkDelete.NavigateUrl = EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "delete");
                if (!((PanelSecurity.LoggedUser.Role == UserRole.Reseller) | (PanelSecurity.LoggedUser.Role == UserRole.Administrator))) 
                    lnkDelete.Visible = false;
                else
                    lnkDelete.Visible = ((PanelSecurity.SelectedUserId != PanelSecurity.EffectiveUserId) && (PanelSecurity.PackageId > 1));
            }
        }

        protected void statusButton_Click(object sender, ImageClickEventArgs e)
        {
            string sStatus = ((ImageButton)sender).CommandName;
            PackageStatus status = (PackageStatus)Enum.Parse(typeof(PackageStatus), sStatus, true);

            // chanhe user status
            try
            {
                int result = ES.Services.Packages.ChangePackageStatus(PanelSecurity.PackageId, status);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                BindSpace();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("PACKAGE_CHANGE_STATUS", ex);
                return;
            }
        }

        protected void chkDefault_CheckedChanged(object sender, EventArgs e) {
            ES.Services.Packages.SetDefaultTopPackage(PanelSecurity.SelectedUserId, PanelSecurity.PackageId);
            return;
        }
    }
}
