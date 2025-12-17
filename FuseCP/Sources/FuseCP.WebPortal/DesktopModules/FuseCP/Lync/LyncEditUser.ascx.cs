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
using FuseCP.EnterpriseServer;
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers.HostedSolution;

using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace FuseCP.Portal.Lync
{
    public partial class EditLyncUser : FuseCPModuleBase
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPhoneNumbers();
                BindItems();
            }
        }

        private void BindPhoneNumbers()
        {
            PackageIPAddress[] ips = ES.Services.Servers.GetPackageUnassignedIPAddresses(PanelSecurity.PackageId, PanelRequest.ItemID, IPAddressPool.PhoneNumbers);

            if (ips.Length > 0)
            {
                ddlPhoneNumber.Items.Add(new ListItem("<Select Phone>", ""));

                foreach (PackageIPAddress ip in ips)
                {
                    string phone = ip.ExternalIP;
                    ddlPhoneNumber.Items.Add(new ListItem(phone, phone));
                }
            }

        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            bool enterpriseVoiceQuota = Utils.CheckQouta(Quotas.LYNC_ENTERPRISEVOICE, cntx);

            bool enterpriseVoice = false;

            FuseCP.Providers.HostedSolution.LyncUserPlan plan = planSelector.plan;
            if (plan != null)
                enterpriseVoice = plan.EnterpriseVoice && enterpriseVoiceQuota && (ddlPhoneNumber.Items.Count > 0);

            pnEnterpriseVoice.Visible = enterpriseVoice;

            if (!enterpriseVoice)
            {
                ddlPhoneNumber.Text = "";
                tbPin.Text = "";
            }

            if (enterpriseVoice)
            {
                string[] pinPolicy = ES.Services.Lync.GetPolicyList(PanelRequest.ItemID, LyncPolicyType.Pin, "MinPasswordLength");
                if (pinPolicy != null)
                {
                    if (pinPolicy.Length > 0)
                    {
                        int MinPasswordLength = -1;
                        if (int.TryParse(pinPolicy[0], out MinPasswordLength))
                        {
                            PinRegularExpressionValidator.ValidationExpression = "^([0-9]){" + MinPasswordLength.ToString() + ",}$";
                            PinRegularExpressionValidator.ErrorMessage = "Must contain only numbers. Min. length " + MinPasswordLength.ToString();
                        }
                    }
                }
            }

        }

        private void BindItems()
        {
            // get settings
            LyncUser lyncUser = ES.Services.Lync.GetLyncUserGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID);

            // title
            litDisplayName.Text = lyncUser.DisplayName;

            planSelector.planId = lyncUser.LyncUserPlanId.ToString();
            lyncUserSettings.sipAddress = lyncUser.SipAddress;

            Utils.SelectListItem(ddlPhoneNumber, lyncUser.LineUri);

            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID,
                    PanelRequest.AccountID);

            if (user.LevelId > 0 && cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels))
            {
                FuseCP.EnterpriseServer.Base.HostedSolution.ServiceLevel serviceLevel = ES.Services.Organizations.GetSupportServiceLevel(user.LevelId);

                litServiceLevel.Visible = true;
                litServiceLevel.Text = serviceLevel.LevelName;
                litServiceLevel.ToolTip = serviceLevel.LevelDescription;

            }
            imgVipUser.Visible = user.IsVIP && cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels);
        }

        protected bool SaveSettings()
        {
            if (!Page.IsValid)
                return false;
            try
            {
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                bool enterpriseVoiceQuota = Utils.CheckQouta(Quotas.LYNC_ENTERPRISEVOICE, cntx);

                string lineUri = "";
                if ((enterpriseVoiceQuota) & (ddlPhoneNumber.Items.Count != 0)) lineUri = ddlPhoneNumber.SelectedItem.Text + ":" + tbPin.Text;

                LyncUserResult res = ES.Services.Lync.SetUserLyncPlan(PanelRequest.ItemID, PanelRequest.AccountID, Convert.ToInt32(planSelector.planId));
                if (res.IsSuccess && res.ErrorCodes.Count == 0)
                {
                    res = ES.Services.Lync.SetLyncUserGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID, lyncUserSettings.sipAddress, lineUri);
                }

                if (res.IsSuccess && res.ErrorCodes.Count == 0)
                {
                    messageBox.ShowSuccessMessage("UPDATE_LYNC_USER");
                    return true;
                }
                else
                {
                    messageBox.ShowMessage(res, "UPDATE_LYNC_USER", "LYNC");
                    return false;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("UPDATE_LYNC_USER", ex);
                return false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (SaveSettings())
            {
                Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                    "lync_users",
                    "SpaceID=" + PanelSecurity.PackageId));
            }
        }


    }
}
