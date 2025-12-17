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
using System.Text;

namespace FuseCP.Portal
{
    public partial class Schedules : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //BindServerTime();

            // set display preferences
            gvSchedules.PageSize = UsersHelper.GetDisplayItemsPerPage();

            if (!IsPostBack)
            {
                
                chkRecursive.Visible = (PanelSecurity.EffectiveUser.Role != UserRole.User);
                // toggle controls
                //btnAddItem.Enabled = PackagesHelper.CheckGroupQuotaEnabled(
                 //   PanelSecurity.PackageId, ResourceGroups.Statistics, Quotas.STATS_SITES);

                searchBox.AddCriteria("ScheduleName", GetLocalizedString("Text.ScheduleName"));
                searchBox.AddCriteria("Username", GetLocalizedString("Text.Username"));
                searchBox.AddCriteria("FullName", GetLocalizedString("Text.FullName"));
                searchBox.AddCriteria("Email", GetLocalizedString("Text.Email"));

                bool isUser = PanelSecurity.SelectedUser.Role == UserRole.User;
                gvSchedules.Columns[gvSchedules.Columns.Count - 1].Visible = !isUser;
                gvSchedules.Columns[gvSchedules.Columns.Count - 2].Visible = !isUser;
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();
        }

        protected void odsSchedules_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception);
                e.ExceptionHandled = true;
            }
        }

        /*
        private void BindServerTime()
        {
            try
            {
                litServerTime.Text = ES.Scheduler.GetSchedulerTime().ToString();
            }
            catch
            {
                // skip
            }
        }
         * */

        public string GetScheduleStatus(int statusId)
        {
			return GetSharedLocalizedString(Utils.ModuleName, "ScheduleStatus." + ((ScheduleStatus)statusId).ToString());
        }

        public bool IsScheduleActive(int statusId)
        {
            ScheduleStatus status = (ScheduleStatus)statusId;
            return (status == ScheduleStatus.Running);
        }

        public string GetUserHomePageUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        public string GetSpaceHomePageUrl(int spaceId)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM, spaceId.ToString());
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "edit"));
        }
        protected void gvSchedules_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int scheduleId = Utils.ParseInt(e.CommandArgument.ToString(), 0);
            if (e.CommandName == "start")
            {
                try
                {
                    int result = ES.Services.Scheduler.StartSchedule(scheduleId);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SCHEDULE_START_TASK", ex);
                    return;
                }
            }
            else if (e.CommandName == "stop")
            {
                try
                {
                    int result = ES.Services.Scheduler.StopSchedule(scheduleId);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SCHEDULE_STOP_TASK", ex);
                    return;
                }
            }

            // rebind grid
            gvSchedules.DataBind();
        }

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'Schedules'");
            res.Append(", RedirectUrl: '" + EditUrl("ScheduleID", "{0}", "edit", "SpaceID=" + PanelSecurity.PackageId).Substring(2) + "'");
            res.Append(", PackageID: " + PanelSecurity.PackageId.ToString());
            res.Append(", Recursive: ($('#" + chkRecursive.ClientID + "').val() == 'on')");
            return res.ToString();
        }

        protected void tasksTimer_Tick(object sender, EventArgs e)
        {
            gvSchedules.DataBind();
        }
    }
}
