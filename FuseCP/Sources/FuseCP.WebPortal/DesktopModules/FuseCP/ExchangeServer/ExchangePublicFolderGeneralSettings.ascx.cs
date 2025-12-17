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
using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace FuseCP.Portal.ExchangeServer
{
	public partial class ExchangePublicFolderGeneralSettings : FuseCPModuleBase
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
				PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
				bool mailFoldersAllowed = cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_MAILENABLEDPUBLICFOLDERS)
					&& !cntx.Quotas[Quotas.EXCHANGE2007_MAILENABLEDPUBLICFOLDERS].QuotaExhausted;

                // get settings
                ExchangePublicFolder folder = ES.Services.ExchangeServer.GetPublicFolderGeneralSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID);

				litDisplayName.Text = folder.DisplayName;

				btnMailEnable.Visible = !folder.MailEnabled && mailFoldersAllowed;
				btnMailDisable.Visible = folder.MailEnabled && mailFoldersAllowed;

				tabs.MailEnabledFolder = folder.MailEnabled;

                // bind form
                txtName.Text = folder.Name;
                chkHideAddressBook.Checked = folder.HideFromAddressBook;
				List<ExchangeAccount> list = new List<ExchangeAccount>();
				
					foreach (ExchangeAccount ex in folder.Accounts)
					{ 
                      try
                        {
                            if (ex != null) 
                            { 
                            list.Add(ex);
                           
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
					}
				
                    ExchangeAccount[] accounts = list.ToArray();
                    allAccounts.SetAccounts(accounts);                   	
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("EXCHANGE_GET_PFOLDER_SETTINGS", ex);              
            }
        }
       
        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
               int result = ES.Services.ExchangeServer.SetPublicFolderGeneralSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID,
                    txtName.Text,
                    chkHideAddressBook.Checked,
					allAccounts.GetAccounts());
               
                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_PFOLDER_SETTINGS");

                // folder name
                string origName = litDisplayName.Text;
                origName = origName.Substring(0, origName.LastIndexOf("\\"));

                litDisplayName.Text = PortalAntiXSS.Encode(origName + txtName.Text);

				BindSettings();
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("EXCHANGE_UPDATE_PFOLDER_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

		protected void btnMailEnable_Click(object sender, EventArgs e)
		{
			Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "public_folder_mailenable",
				"SpaceID=" + PanelSecurity.PackageId.ToString(),
				"AccountID=" + PanelRequest.AccountID.ToString()));
		}

		protected void btnMailDisable_Click(object sender, EventArgs e)
		{
			// disable mail on folder
			try
			{

				int result = ES.Services.ExchangeServer.DisableMailPublicFolder(PanelRequest.ItemID,
					PanelRequest.AccountID);

				if (result < 0)
				{
					messageBox.ShowResultMessage(result);
					return;
				}

				// re-bind settings
				BindSettings();
			}
			catch (Exception ex)
			{
				messageBox.ShowErrorMessage("EXCHANGE_MAIL_DISABLE_PUBLIC_FOLDER", ex);
			}
		}
	}
}
