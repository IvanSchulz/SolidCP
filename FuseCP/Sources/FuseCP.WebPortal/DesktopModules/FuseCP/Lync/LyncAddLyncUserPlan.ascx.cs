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
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;
using FuseCP.Providers;
using FuseCP.Providers.Web;
using FuseCP.Providers.Common;
using FuseCP.Portal.Code.Helpers;


namespace FuseCP.Portal.Lync
{
    public partial class LyncAddLyncUserPlan : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (!IsPostBack)
            {
                string[] archivePolicy = ES.Services.Lync.GetPolicyList(PanelRequest.ItemID, LyncPolicyType.Archiving, null);
                if (archivePolicy != null)
                {
                    foreach (string policy in archivePolicy)
                    {
                        if (policy.ToLower()=="global") continue;
                        string txt = policy.Replace("Tag:","");
                        ddArchivingPolicy.Items.Add( new System.Web.UI.WebControls.ListItem( txt, policy) );
                    }
                }

                if (PanelRequest.GetInt("LyncUserPlanId") != 0)
                {
                    Providers.HostedSolution.LyncUserPlan plan = ES.Services.Lync.GetLyncUserPlan(PanelRequest.ItemID, PanelRequest.GetInt("LyncUserPlanId"));

                    txtPlan.Text = plan.LyncUserPlanName;
                    chkIM.Checked = plan.IM;
                    chkIM.Enabled = false;
                    chkFederation.Checked = plan.Federation;
                    chkConferencing.Checked = plan.Conferencing;
                    chkMobility.Checked = plan.Mobility;
                    chkEnterpriseVoice.Checked = plan.EnterpriseVoice;

                    /* because not used
                    switch (plan.VoicePolicy)
                    {
                        case LyncVoicePolicyType.None:
                            break;
                        case LyncVoicePolicyType.Emergency:
                            chkEmergency.Checked = true;
                            break;
                        case LyncVoicePolicyType.National:
                            chkNational.Checked = true;
                            break;
                        case LyncVoicePolicyType.Mobile:
                            chkMobile.Checked = true;
                            break;
                        case LyncVoicePolicyType.International:
                            chkInternational.Checked = true;
                            break;
                        default:
                            chkNone.Checked = true;
                            break;
                    }
                     */

	                chkRemoteUserAccess.Checked = plan.RemoteUserAccess;
                    chkAllowOrganizeMeetingsWithExternalAnonymous.Checked = plan.AllowOrganizeMeetingsWithExternalAnonymous;

                    Utils.SelectListItem(ddTelephony, plan.Telephony);

	                tbServerURI.Text = plan.ServerURI;

                    string planArchivePolicy = "";              if (plan.ArchivePolicy != null) planArchivePolicy = plan.ArchivePolicy;
                    string planTelephonyDialPlanPolicy = "";    if (plan.TelephonyDialPlanPolicy != null) planTelephonyDialPlanPolicy = plan.TelephonyDialPlanPolicy;
                    string planTelephonyVoicePolicy = "";       if (plan.TelephonyVoicePolicy != null) planTelephonyVoicePolicy = plan.TelephonyVoicePolicy;

                    ddArchivingPolicy.Items.Clear();
                    ddArchivingPolicy.Items.Add(new System.Web.UI.WebControls.ListItem(planArchivePolicy.Replace("Tag:", ""), planArchivePolicy));
                    ddTelephonyDialPlanPolicy.Items.Clear();
                    ddTelephonyDialPlanPolicy.Items.Add(new System.Web.UI.WebControls.ListItem(planTelephonyDialPlanPolicy.Replace("Tag:", ""), planTelephonyDialPlanPolicy));
                    ddTelephonyVoicePolicy.Items.Clear();
                    ddTelephonyVoicePolicy.Items.Add(new System.Web.UI.WebControls.ListItem(planTelephonyVoicePolicy.Replace("Tag:", ""), planTelephonyVoicePolicy));

                    locTitle.Text = plan.LyncUserPlanName;
                    this.DisableControls = true;
                }
                else
                {
                    chkIM.Checked = true;
                    chkIM.Enabled = false;

                    // chkNone.Checked = true; because not used

                    if (cntx != null)
                    {
                        foreach (QuotaValueInfo quota in cntx.QuotasArray)
                        {
                            switch (quota.QuotaId)
                            {
                                case 371:
                                    chkFederation.Checked = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    chkFederation.Enabled = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    break;
                                case 372:
                                    chkConferencing.Checked = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    chkConferencing.Enabled = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    break;
                            }
                        }
                    }
                    else
                        this.DisableControls = true;
                }
            }

            bool enterpriseVoiceQuota = Utils.CheckQouta(Quotas.LYNC_ENTERPRISEVOICE, cntx);

            PlanFeaturesTelephony.Visible = enterpriseVoiceQuota;
            secPlanFeaturesTelephony.Visible = enterpriseVoiceQuota;
            if (!enterpriseVoiceQuota) Utils.SelectListItem(ddTelephony, "0");

            bool enterpriseVoice = enterpriseVoiceQuota && (ddTelephony.SelectedValue == "2");

            chkEnterpriseVoice.Enabled = false;
            chkEnterpriseVoice.Checked = enterpriseVoice;
            pnEnterpriseVoice.Visible = enterpriseVoice;

            switch (ddTelephony.SelectedValue)
            {
                case "3":
                case "4":
                    pnServerURI.Visible = true;
                    break;
                default:
                    pnServerURI.Visible = false;
                    break;
            }


        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AddPlan();
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            string name = tbTelephoneProvider.Text;

            if (string.IsNullOrEmpty(name)) return;

            ddTelephonyDialPlanPolicy.Items.Clear();
            string[] dialPlan = ES.Services.Lync.GetPolicyList(PanelRequest.ItemID, LyncPolicyType.DialPlan, name);
            if (dialPlan != null)
            {
                foreach (string policy in dialPlan)
                {
                    if (policy.ToLower() == "global") continue;
                    string txt = policy.Replace("Tag:", "");
                    ddTelephonyDialPlanPolicy.Items.Add(new System.Web.UI.WebControls.ListItem(txt, policy));
                }
            }

            ddTelephonyVoicePolicy.Items.Clear();
            string[] voicePolicy = ES.Services.Lync.GetPolicyList(PanelRequest.ItemID, LyncPolicyType.Voice, name);
            if (voicePolicy != null)
            {
                foreach (string policy in voicePolicy)
                {
                    if (policy.ToLower() == "global") continue;
                    string txt = policy.Replace("Tag:", "");
                    ddTelephonyVoicePolicy.Items.Add(new System.Web.UI.WebControls.ListItem(txt, policy));
                }
            }
        }

        private void AddPlan()
        {
            try
            {
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                Providers.HostedSolution.LyncUserPlan plan = new Providers.HostedSolution.LyncUserPlan();
                plan.LyncUserPlanName = txtPlan.Text;
                plan.IsDefault = false;

                plan.IM = true;
                plan.Mobility = chkMobility.Checked;
                plan.Federation = chkFederation.Checked;
                plan.Conferencing = chkConferencing.Checked;

                bool enterpriseVoiceQuota = Utils.CheckQouta(Quotas.LYNC_ENTERPRISEVOICE, cntx);
                bool enterpriseVoice = enterpriseVoiceQuota && (ddTelephony.SelectedValue == "2");

                plan.EnterpriseVoice = enterpriseVoice;

                plan.VoicePolicy = LyncVoicePolicyType.None;

                /* because not used
                if (!plan.EnterpriseVoice)
                {
                    plan.VoicePolicy = LyncVoicePolicyType.None;
                }
                else
                {
                    if (chkEmergency.Checked)
                        plan.VoicePolicy = LyncVoicePolicyType.Emergency;
                    else if (chkNational.Checked)
                        plan.VoicePolicy = LyncVoicePolicyType.National;
                    else if (chkMobile.Checked)
                        plan.VoicePolicy = LyncVoicePolicyType.Mobile;
                    else if (chkInternational.Checked)
                        plan.VoicePolicy = LyncVoicePolicyType.International;
                    else
                        plan.VoicePolicy = LyncVoicePolicyType.None;

                } 
                */

	            plan.RemoteUserAccess = chkRemoteUserAccess.Checked;

	            plan.AllowOrganizeMeetingsWithExternalAnonymous = chkAllowOrganizeMeetingsWithExternalAnonymous.Checked;

                int telephonyId = -1;
                int.TryParse(ddTelephony.SelectedValue, out telephonyId);
	            plan.Telephony = telephonyId;

	            plan.ServerURI = tbServerURI.Text;

                plan.ArchivePolicy = ddArchivingPolicy.SelectedValue;
                plan.TelephonyDialPlanPolicy = ddTelephonyDialPlanPolicy.SelectedValue;
                plan.TelephonyVoicePolicy = ddTelephonyVoicePolicy.SelectedValue;

                int result = ES.Services.Lync.AddLyncUserPlan(PanelRequest.ItemID,
                                                                                plan);


                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    messageBox.ShowErrorMessage("LYNC_UNABLE_TO_ADD_PLAN");
                    return;
                }

                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "lync_userplans",
                    "SpaceID=" + PanelSecurity.PackageId));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("LYNC_ADD_PLAN", ex);
            }
        }
    }
}
