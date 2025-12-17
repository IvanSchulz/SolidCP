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
using FuseCP.Providers.ResultObjects;
using FuseCP.EnterpriseServer;

using FuseCP.Providers.HostedSolution;

using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace FuseCP.Portal.SfB
{
    public partial class CreateSfBUser : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FuseCP.Providers.HostedSolution.SfBUserPlan[] plans = ES.Services.SfB.GetSfBUserPlans(PanelRequest.ItemID);

                BindPhoneNumbers();

                if (plans.Length == 0)
                    btnCreate.Enabled = false;
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
                    ddlPhoneNumber.Items.Add(new ListItem(phone, ip.PackageAddressID.ToString()));
                }
            }

        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            bool enterpriseVoiceQuota = Utils.CheckQouta(Quotas.SFB_ENTERPRISEVOICE, cntx);

            bool enterpriseVoice = false;

            FuseCP.Providers.HostedSolution.SfBUserPlan plan = planSelector.plan;
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
                string[] pinPolicy = ES.Services.SfB.GetPolicyList(PanelRequest.ItemID, SfBPolicyType.Pin, "MinPasswordLength");
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

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            int accountId = userSelector.GetAccountId();
            SfBUserResult res = ES.Services.SfB.CreateSfBUser(PanelRequest.ItemID, accountId, Convert.ToInt32(planSelector.planId));
            if (res.IsSuccess && res.ErrorCodes.Count == 0)
            {

                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                bool enterpriseVoiceQuota = Utils.CheckQouta(Quotas.SFB_ENTERPRISEVOICE, cntx);

                string lineUri = "";
                if ((enterpriseVoiceQuota) & (ddlPhoneNumber.Items.Count != 0)) lineUri = ddlPhoneNumber.SelectedItem.Text + ":" + tbPin.Text;

                //#1
                SfBUser sfbUser = ES.Services.SfB.GetSfBUserGeneralSettings(PanelRequest.ItemID, accountId);
                ES.Services.SfB.SetSfBUserGeneralSettings(PanelRequest.ItemID, accountId, sfbUser.SipAddress, lineUri);

                Response.Redirect(EditUrl("AccountID", accountId.ToString(), "edit_sfb_user",
                    "SpaceID=" + PanelSecurity.PackageId,
                    "ItemID=" + PanelRequest.ItemID));
            }
            else
            {
                messageBox.ShowMessage(res, "CREATE_SFB_USER", "SFB");
            }
        }
    }
}
