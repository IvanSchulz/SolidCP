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
    public partial class Tasks : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void odsTasks_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception.InnerException);
                e.ExceptionHandled = true;
            }
        }

        protected void gvTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // get data item
            BackgroundTask task = (BackgroundTask)e.Row.DataItem;
            if (task == null)
                return;

            // find controls
            HyperLink lnkTaskName = (HyperLink)e.Row.FindControl("lnkTaskName");
            Literal litTaskDuration = (Literal)e.Row.FindControl("litTaskDuration");
            Panel pnlProgressIndicator = (Panel)e.Row.FindControl("pnlProgressIndicator");
            CPCC.StyleButton cmdStop = (CPCC.StyleButton)e.Row.FindControl("cmdStop");

            // bind controls
            lnkTaskName.Text = GetAuditLogTaskName(task.Source, task.TaskName);
            lnkTaskName.NavigateUrl = EditUrl("TaskID", task.TaskId, "view_details");

            // duration
            TimeSpan duration = (TimeSpan)(DateTime.Now - task.StartDate);
            litTaskDuration.Text = String.Format("{0}:{1}:{2}",
                duration.Hours.ToString().PadLeft(2, '0'),
                duration.Minutes.ToString().PadLeft(2, '0'),
                duration.Seconds.ToString().PadLeft(2, '0'));

            // progress
            int percent = 0;
            if (task.IndicatorMaximum > 0)
                percent = task.IndicatorCurrent * 100 / task.IndicatorMaximum;
            pnlProgressIndicator.Width = Unit.Percentage(percent);

            // stop button
            cmdStop.CommandArgument = task.TaskId;
        }

        protected void gvTasks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "stop")
            {
                // stop task
                ES.Services.Tasks.StopTask(e.CommandArgument.ToString());

                // rebind grid
                gvTasks.DataBind();
            }
        }
    }
}
