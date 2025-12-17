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
using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using System.Web.UI;

namespace FuseCP.Portal.HostedSolution
{
    public partial class OrganizationUsers : FuseCPModuleBase
    {
        private ServiceLevel[] ServiceLevels;
        private PackageContext cntx;

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));

            cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            BindServiceLevels();

            if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_ISCONSUMER))
            {
                if (cntx.Quotas[Quotas.EXCHANGE2007_ISCONSUMER].QuotaAllocatedValue != 1)
                {
                    gvUsers.Columns[6].Visible = false;
                }
            }
            gvUsers.Columns[4].Visible = cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            BindStats();
        }

        private void BindServiceLevels()
        {
            ServiceLevels = ES.Services.Organizations.GetSupportServiceLevels();
        }

        private void BindStats()
        {
            // quota values
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            usersQuota.QuotaUsedValue = stats.CreatedUsers;
            usersQuota.QuotaValue = stats.AllocatedUsers;
            if (stats.AllocatedUsers != -1) usersQuota.QuotaAvailable = stats.AllocatedUsers - stats.CreatedUsers;

            if(cntx != null && cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels)) BindServiceLevelsStats();
        }

        private void BindServiceLevelsStats()
        {
            ServiceLevels = ES.Services.Organizations.GetSupportServiceLevels();
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);

            List<ServiceLevelQuotaValueInfo> serviceLevelQuotas = new List<ServiceLevelQuotaValueInfo>();
            foreach (var quota in stats.ServiceLevels)
            {
                serviceLevelQuotas.Add(new ServiceLevelQuotaValueInfo
                {
                    QuotaName = quota.QuotaName,
                    QuotaDescription = quota.QuotaDescription + " in this Organization:",
                    QuotaTypeId = quota.QuotaTypeId,
                    QuotaValue = quota.QuotaAllocatedValue,
                    QuotaUsedValue = quota.QuotaUsedValue,
                    QuotaAvailable = quota.QuotaAllocatedValue - quota.QuotaUsedValue
                });
            }
            dlServiceLevelQuotas.DataSource = serviceLevelQuotas;
            dlServiceLevelQuotas.DataBind();
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_user",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        public string GetUserEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_user",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID,
                    "Context=User");
        }

        protected void odsAccountsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("ORGANZATION_GET_USERS", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                DeleteUserModal.Show();
                int rowIndex = Utils.ParseInt(e.CommandArgument.ToString(), 0);
                var accountId = Utils.ParseInt(gvUsers.DataKeys[rowIndex][0], 0);
                var accountType = (ExchangeAccountType)gvUsers.DataKeys[rowIndex][1];
                chkEnableForceArchiveMailbox.Visible = false;
                Session["delAccId"] = accountId;
            }

            if (e.CommandName == "OpenMailProperties")
            {
                int accountId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "mailbox_settings",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID));
            }

            if (e.CommandName == "OpenBlackBerryProperties")
            {
                int accountId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_blackberry_user",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID));
            }

            if (e.CommandName == "OpenCRMProperties")
            {
                int accountId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "mailbox_settings",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID));
            }

            if (e.CommandName == "OpenUCProperties")
            {
                string[] Tmp = e.CommandArgument.ToString().Split('|');

                int accountId = Utils.ParseInt(Tmp[0], 0);
                if (Tmp[1] == "True")
                    Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_ocs_user",
                        "AccountID=" + accountId,
                        "ItemID=" + PanelRequest.ItemID));
                if (Tmp[2] == "True")
                        Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_lync_user",
                            "AccountID=" + accountId,
                            "ItemID=" + PanelRequest.ItemID));
                else
                    if (Tmp[3] == "True")
                    Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_sfb_user",
                        "AccountID=" + accountId,
                        "ItemID=" + PanelRequest.ItemID));
            }
        }

        public string GetAccountImage(int accountTypeId, bool vip)
        {
            string imgName = string.Empty;

            ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;
            switch (accountType)
            {
                case ExchangeAccountType.Room:
                    imgName = "room_16.gif";
                    break;
                case ExchangeAccountType.Equipment:
                    imgName = "equipment_16.gif";
                    break;
                case ExchangeAccountType.JournalingMailbox:
                    imgName = "journaling_mailbox_16.png";
                    break;
                default:
                    imgName = "admin_16.png";
                    break;
            }
            if (vip && cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels)) imgName = "vip_user_16.png";

            return GetThemedImage("Exchange/" + imgName);
        }

        public string GetStateImage(bool locked, bool disabled)
        {
            string imgName = "enabled.png";

            if (locked)
                imgName = "locked.png";
            else
                if (disabled)
                    imgName = "disabled.png";

            return GetThemedImage("Exchange/" + imgName);
        }


        public string GetMailImage(int accountTypeId)
        {
            string imgName = "exchange24.png";

            ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;

            if (accountType == ExchangeAccountType.User)
                imgName = "blank16.gif";

            return GetThemedImage("Exchange/" + imgName);
        }

        public string GetOCSImage(bool IsOCSUser, bool IsLyncUser, bool IsSfBUser)
        {
            string imgName = "blank16.gif";

            if (IsLyncUser)
                imgName = "lync16.png";
            if (IsSfBUser)
                imgName = "SfB16.png";
            else
                if ((IsOCSUser))
                    imgName = "ocs16.png";

            return GetThemedImage("Exchange/" + imgName);
        }

        public string GetBlackBerryImage(bool IsBlackBerryUser)
        {
            string imgName = "blank16.gif";

            if (IsBlackBerryUser)
                imgName = "blackberry16.png";

            return GetThemedImage("Exchange/" + imgName);
        }

        public string GetCRMImage(Guid CrmUserId)
        {
            string imgName = "blank16.gif";

            if (CrmUserId != Guid.Empty)
                imgName = "crm_16.png";

            return GetThemedImage("Exchange/" + imgName);
        }


        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)   
        {   
            gvUsers.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);   
                 
            // rebind grid   
            gvUsers.DataBind();   
       
            // bind stats   
            BindStats();   
        }


        public bool EnableMailImageButton(int accountTypeId)
        {
            bool imgName = true;

            ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;

            if (accountType == ExchangeAccountType.User)
                imgName = false;

            return imgName;
        }

        public bool EnableOCSImageButton(bool IsOCSUser, bool IsLyncUser, bool IsSfBUser)
        {
            bool imgName = false;

            if (IsLyncUser)
                imgName = true;
            if (IsSfBUser)
                imgName = true;
            else
                if ((IsOCSUser))
                    imgName = true;

            return imgName;
        }

        public bool EnableBlackBerryImageButton(bool IsBlackBerryUser)
        {
            bool imgName = false;

            if (IsBlackBerryUser)
                imgName = true;

            return imgName;
        }


        public string GetOCSArgument(int accountID, bool IsOCS, bool IsLync, bool IsSfB)
        {
            return accountID.ToString() + "|" + IsOCS.ToString() + "|" + IsLync.ToString() + "|" + IsSfB.ToString();
        }

        public ServiceLevel GetServiceLevel(int levelId)
        {
            ServiceLevel serviceLevel = ServiceLevels.Where(x => x.LevelId == levelId).DefaultIfEmpty(new ServiceLevel { LevelName = "", LevelDescription = "" }).FirstOrDefault();

            bool enable = !string.IsNullOrEmpty(serviceLevel.LevelName);

            enable = enable ? cntx.Quotas.ContainsKey(Quotas.SERVICE_LEVELS + serviceLevel.LevelName) : false;
            enable = enable ? cntx.Quotas[Quotas.SERVICE_LEVELS + serviceLevel.LevelName].QuotaAllocatedValue != 0 : false;

            if (!enable)
            {
                serviceLevel.LevelName = "";
                serviceLevel.LevelDescription = "";
            }

            return serviceLevel;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteUserModal.Hide();

            // delete user
            try
            {
                int result = 0;
                    result = ES.Services.Organizations.DeleteUser(PanelRequest.ItemID, Convert.ToInt32(Session["delAccId"]));


                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                // rebind grid
                gvUsers.DataBind();

                // bind stats
                BindStats();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ORGANIZATIONS_DELETE_USERS", ex);
            }
        }

    }
}
