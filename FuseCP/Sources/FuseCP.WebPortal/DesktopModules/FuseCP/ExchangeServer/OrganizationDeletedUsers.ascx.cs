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
using System.IO;

namespace FuseCP.Portal.HostedSolution
{
    public partial class OrganizationDeletedUsers : FuseCPModuleBase
    {
        private ServiceLevel[] ServiceLevels;
        private PackageContext cntx;

        protected void Page_Load(object sender, EventArgs e)
        {
            string downloadFile = Request["DownloadFile"];
            if (downloadFile != null)
            {
                // download file
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(downloadFile));
                Response.ContentType = "application/octet-stream";

                int FILE_BUFFER_LENGTH = 5000000;
                byte[] buffer = null;
                int offset = 0;
                do
                {
                    try
                    {
                        // read remote content
                        buffer = ES.Services.Organizations.GetArchiveFileBinaryChunk(PanelSecurity.PackageId, PanelRequest.ItemID, PanelRequest.AccountID, offset, FILE_BUFFER_LENGTH);
                    }
                    catch (Exception ex)
                    {
                        messageBox.ShowErrorMessage("ARCHIVE_FILE_READ_FILE", ex);
                        break;
                    }

                    // write to stream
                    Response.BinaryWrite(buffer);

                    offset += FILE_BUFFER_LENGTH;
                }
                while (buffer.Length == FILE_BUFFER_LENGTH);
                Response.End();
            }

            cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (!IsPostBack)
            {    
                BindStats();
            }

            BindServiceLevels();

            gvDeletedUsers.Columns[3].Visible = cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels);
        }

        private void BindServiceLevels()
        {
            ServiceLevels = ES.Services.Organizations.GetSupportServiceLevels();
        }

        private void BindStats()
        {
            // quota values
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            deletedUsersQuota.QuotaUsedValue = stats.DeletedUsers;
            deletedUsersQuota.QuotaValue = stats.AllocatedDeletedUsers;
            if (stats.AllocatedUsers != -1) deletedUsersQuota.QuotaAvailable = stats.AllocatedDeletedUsers - stats.DeletedUsers;
        }

        public string GetUserEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "view_deleted_user",
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

        protected void gvDeletedUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // delete user
                int accountId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                try
                {
                    int result = ES.Services.Organizations.DeleteUser(PanelRequest.ItemID, accountId);
                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return;
                    }

                    // rebind grid
                    gvDeletedUsers.DataBind();

                    // bind stats
                    BindStats();
                }
                catch (Exception ex)
                {
                    messageBox.ShowErrorMessage("ORGANIZATIONS_DELETE_USERS", ex);
                }
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
                default:
                    imgName = "admin_16.png";
                    break;
            }
            if (vip && cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels)) imgName = "vip_user_16.png";

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
            gvDeletedUsers.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);   
                 
            // rebind grid   
            gvDeletedUsers.DataBind();   
       
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

        public string GetDownloadLink(int accountId, string fileName)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM,
                PanelSecurity.PackageId.ToString(),
                "ctl=" + PanelRequest.Ctl,
                "ItemID=" + PanelRequest.ItemID,
                "mid=" + this.ModuleID,
                "AccountID=" + accountId,
                "DownloadFile=" + Server.UrlEncode(fileName));
        }
    }
}
