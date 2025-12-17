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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FuseCP.Portal
{
	public partial class SetupControlPanelAccounts : FuseCPModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				EnsureFCPAEnabled();
			}
		}

		protected void CompleteSetupButton_Click(object sender, EventArgs e)
		{
			if (Page.IsValid == false)
			{
				return;
			}
			//
			CompleteSetupControlPanelAccounts();
		}

		private void EnsureFCPAEnabled()
		{
			var enabledScpa = ES.Services.Authentication.GetSystemSetupMode();
			//
			if (enabledScpa == false)
			{
				Response.Redirect(EditUrl(""));
			}
		}

		private void CompleteSetupControlPanelAccounts()
		{
			var resultCode = ES.Services.Authentication.SetupControlPanelAccounts(PasswordControlA.Password, PasswordControlB.Password, Request.UserHostAddress);
			//
			if (resultCode < 0)
			{
				ShowResultMessage(resultCode);
				//
				return;
			}
			//
			Response.Redirect(EditUrl("u", "admin", String.Empty, String.Format("p={0}", PasswordControlB.Password)));
		}
	}
}
