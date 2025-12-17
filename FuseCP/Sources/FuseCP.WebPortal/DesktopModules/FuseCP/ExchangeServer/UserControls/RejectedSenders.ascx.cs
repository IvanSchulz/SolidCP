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
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal.ExchangeServer.UserControls
{
	public partial class RejectedSenders : FuseCPControlBase
    {
		public void SetAccounts(ExchangeAccount[] accounts)
		{
			rejectAccounts.SetAccounts(accounts);

			rblRejectMessages.SelectedIndex = 1;
			if (accounts == null || accounts.Length == 0)
				rblRejectMessages.SelectedIndex = 0;

			ToggleControls();
		}

		public string[] GetAccounts()
		{
			return (rblRejectMessages.SelectedIndex == 0)
				? new string[0] : rejectAccounts.GetAccounts();
		}

        protected void Page_Load(object sender, EventArgs e)
        {

        }

		private void ToggleControls()
		{
			rejectAccounts.Visible = (rblRejectMessages.SelectedIndex == 1);
		}

		protected void rblRejectMessages_SelectedIndexChanged(object sender, EventArgs e)
		{
			ToggleControls();
		}
    }
}
