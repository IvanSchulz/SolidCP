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

namespace FuseCP.Portal.ExchangeServer
{
	public partial class ExchangePublicFolderMailFlowSettings : FuseCPModuleBase
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSettings();
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                ExchangePublicFolder folder = ES.Services.ExchangeServer.GetPublicFolderMailFlowSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID);

				litDisplayName.Text = folder.DisplayName;

                // bind form
                acceptAccounts.SetAccounts(folder.AcceptAccounts);
                chkSendersAuthenticated.Checked = folder.RequireSenderAuthentication;
                rejectAccounts.SetAccounts(folder.RejectAccounts);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_PFOLDER_MAILFLOW", ex);
            }
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                int result = ES.Services.ExchangeServer.SetPublicFolderMailFlowSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID,
                    acceptAccounts.GetAccounts(),
                    rejectAccounts.GetAccounts(),

                    chkSendersAuthenticated.Checked);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

				messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_PFOLDER_MAILFLOW");
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("EXCHANGE_UPDATE_PFOLDER_MAILFLOW", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
	}
}
