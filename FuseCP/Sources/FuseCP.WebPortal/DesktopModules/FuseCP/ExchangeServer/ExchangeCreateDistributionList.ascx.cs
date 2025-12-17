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

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeCreateDistributionList : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            CreateDistributionList();
        }

        private void CreateDistributionList()
        {
            if (!Page.IsValid)
                return;



            try
            {

                int accountId = ES.Services.ExchangeServer.CreateDistributionList(PanelRequest.ItemID,
                    txtDisplayName.Text.Trim(),
                    email.AccountName,
                    email.DomainName,
                    manager.GetAccountId());


                if (accountId < 0)
                {
                    messageBox.ShowResultMessage(accountId);
                    return;
                }

                Response.Redirect(EditUrl("AccountID", accountId.ToString(), "dlist_settings",
                    "SpaceID=" + PanelSecurity.PackageId.ToString(),
                    "ItemID=" + PanelRequest.ItemID.ToString()));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_CREATE_DISTRIBUTION_LIST", ex);
            }
        }

        protected void valManager_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = manager.GetAccountId() != 0;

        }


    }
}
