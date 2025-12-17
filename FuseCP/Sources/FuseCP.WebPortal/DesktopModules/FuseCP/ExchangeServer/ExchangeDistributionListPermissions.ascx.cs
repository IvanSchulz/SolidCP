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
using FuseCP.Providers.Common;
using FuseCP.Providers.ResultObjects;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeDistributionListPermissions : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ExchangeDistributionListResult res = ES.Services.ExchangeServer.GetDistributionListPermissions(PanelRequest.ItemID, PanelRequest.AccountID);
                if (res.IsSuccess)
                {
                    litDisplayName.Text = res.Value.DisplayName;
                    sendBehalfList.SetAccounts(res.Value.SendOnBehalfAccounts);
                    sendAsList.SetAccounts(res.Value.SendAsAccounts);
                }
                else
                {
                    messageBox.ShowMessage(res, "SET_DISTRIBUTION_LIST_PERMISSIONS", "HostedOrganization");
                }


            }

            

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string[] sendBehalfAccouts = sendBehalfList.GetAccounts();
            string[] sendAsAccounts = sendAsList.GetAccounts();

            ResultObject res = ES.Services.ExchangeServer.SetDistributionListPermissions(PanelRequest.ItemID, PanelRequest.AccountID, sendAsAccounts, sendBehalfAccouts);
            messageBox.ShowMessage(res, "SET_DISTRIBUTION_LIST_PERMISSIONS", "");
        }
    }
}
