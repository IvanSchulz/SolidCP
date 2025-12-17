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

namespace FuseCP.Portal
{
	public partial class EnableAsyncTasksSupport : FuseCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes["onsubmit"] += "return ShowProgressDialogInternal();";

            // get task ID from request and place it to context
            Context.Items["FuseCPAtlasTaskID"] = taskID.Value;
        }

		public string GetAjaxUtilsUrl()
		{
			return Page.ClientScript.GetWebResourceUrl(
				typeof(EnableAsyncTasksSupport), "FuseCP.Portal.Scripts.AjaxUtils.js");
		}

        protected override void OnPreRender(EventArgs e)
        {
            // call base handler
            base.OnPreRender(e);

            // check if async task was runned
            string asyncScript = "";
            string asyncTaskID = (string)Context.Items["FuseCPAtlasAsyncTaskID"];
            if (!String.IsNullOrEmpty(asyncTaskID))
            {
				string taskTitle = (string)Context.Items["FuseCPAtlasAsyncTaskTitle"];
				if (String.IsNullOrEmpty(taskTitle))
					taskTitle = GetLocalizedString("Text.GenericTitle");

				asyncScript = "ShowProgressDialogAsync('" + asyncTaskID + "', '" + taskTitle + "');";
            }
            else
            {
                string url = (string)Context.Items["RedirectUrl"];

                if (!String.IsNullOrWhiteSpace(url))
                {
                    Response.Redirect(url);
                }
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "Atlas", @"<script>
        function pageLoad()
        {
            " + asyncScript + @"
            ShowProgressDialogInternal();
        }
    </script>
");

            // generate new task ID
            taskID.Value = Guid.NewGuid().ToString("N");
        }
    }
}
