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
using FuseCP.EnterpriseServer;


namespace FuseCP.Portal.ExchangeServer
{
	public partial class ExchangeCreatePublicFolder : FuseCPModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
				BindParentFolders();
                MailEnablePublicFolder();
            }
		}

		private void BindParentFolders()
		{
			// get organization info
			Organization org = ES.Services.ExchangeServer.GetOrganization(PanelRequest.ItemID);

			string rootFolder = org.OrganizationId;
			ddlParentFolder.Items.Add("\\" + org.OrganizationId);

			// get public folder accounts
			ExchangeAccount[] folders = ES.Services.ExchangeServer.GetAccounts(PanelRequest.ItemID, ExchangeAccountType.PublicFolder);

			// add folders to the tree
			foreach (ExchangeAccount folder in folders)
			{
				ddlParentFolder.Items.Add(folder.DisplayName);
			}
		}

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            CreatePublicFolder();
        }

        private void CreatePublicFolder()
        {
            if (!Page.IsValid)
                return;

            try
            {

                int accountId = ES.Services.ExchangeServer.CreatePublicFolder(PanelRequest.ItemID,
                    ddlParentFolder.SelectedValue,
                    txtName.Text.Trim(),
                    chkMailEnabledFolder.Checked,
                    email.AccountName,
                    email.DomainName);

                if (accountId < 0)
                {
                    messageBox.ShowResultMessage(accountId);
                    return;
                }

                Response.Redirect(EditUrl("AccountID", accountId.ToString(), "public_folder_settings",
                    "SpaceID=" + PanelSecurity.PackageId.ToString(),
                    "ItemID=" + PanelRequest.ItemID.ToString()));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_CREATE_PUBLIC_FOLDER", ex);
            }
        }

        private void MailEnablePublicFolder()
        {
			PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
			chkMailEnabledFolder.Visible = cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_MAILENABLEDPUBLICFOLDERS)
				&& !cntx.Quotas[Quotas.EXCHANGE2007_MAILENABLEDPUBLICFOLDERS].QuotaExhausted;

            EmailRow.Visible = chkMailEnabledFolder.Checked;
        }

        protected void chkMailEnabledFolder_CheckedChanged(object sender, EventArgs e)
        {
            MailEnablePublicFolder();
        }
	}
}
