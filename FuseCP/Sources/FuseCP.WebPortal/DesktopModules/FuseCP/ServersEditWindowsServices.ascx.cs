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
using System.Linq;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using FuseCP.Providers.OS;

namespace FuseCP.Portal
{
	public partial class ServersEditWindowsServices : FuseCPModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			BindServices();
		}

		public string WithMaxLength(string str, int len)
		{
			if (str.Length <= len) return str;

			return str.Substring(0, len);
		}
		private void BindServices()
		{
			try
			{
				var services = ES.Services.Servers.GetOSServices(PanelRequest.ServerId)
					.Select(srvc =>
					{
						srvc.Name = WithMaxLength(srvc.Name ?? "", 80);
						return srvc;
					});
				gvServices.DataSource = services;
				gvServices.DataBind();
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SERVER_GET_WIN_SERVICES", ex);
				return;
			}
		}

		protected void gvServices_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			OSServiceStatus status = (OSServiceStatus)Enum.Parse(typeof(OSServiceStatus), e.CommandName, true);
			string id = (string)e.CommandArgument;

			try
			{
				int result = ES.Services.Servers.ChangeOSServiceStatus(PanelRequest.ServerId, id, status);
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}

				// rebind
				BindServices();
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SERVER_CHANGE_WIN_SERVICE_STATE", ex);
				return;
			}
		}

		protected void gvServices_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			OSService serv = (OSService)e.Row.DataItem;
			ImageButton cmdStart = (ImageButton)e.Row.FindControl("cmdStart");
			ImageButton cmdPause = (ImageButton)e.Row.FindControl("cmdPause");
			ImageButton cmdContinue = (ImageButton)e.Row.FindControl("cmdContinue");
			ImageButton cmdStop = (ImageButton)e.Row.FindControl("cmdStop");

			if (cmdStart == null)
				return;

			cmdStart.Visible = (serv.Status == OSServiceStatus.Stopped);
			cmdPause.Visible = (serv.Status == OSServiceStatus.Running && serv.CanPauseAndContinue);
			cmdContinue.Visible = (serv.Status == OSServiceStatus.Paused && serv.CanPauseAndContinue);
			cmdStop.Visible = (serv.Status == OSServiceStatus.Running
				 || serv.Status == OSServiceStatus.Paused
				 && serv.CanStop);
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_server"));
		}
	}
}
