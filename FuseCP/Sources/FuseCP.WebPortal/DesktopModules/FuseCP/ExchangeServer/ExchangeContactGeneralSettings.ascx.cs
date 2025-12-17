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
using System.Web.UI.WebControls;
using FuseCP.Providers.HostedSolution;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal.ExchangeServer
{
    public partial class ExchangeContactGeneralSettings : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMapiRichTextFormat();
                BindSettings();
            }

            
        }

        private void BindMapiRichTextFormat()
        {
            ddlMAPIRichTextFormat.Items.Clear();


            ddlMAPIRichTextFormat.Items.Add(new ListItem(GetLocalizedString("MAPIRichTextFormat.Always"), "1"));
            ddlMAPIRichTextFormat.Items.Add(new ListItem(GetLocalizedString("MAPIRichTextFormat.Never"), "0"));
            ddlMAPIRichTextFormat.Items.Add(new ListItem(GetLocalizedString("MAPIRichTextFormat.Default"), "2"));
        }
        private void BindSettings()
        {
            try
            {
                // get settings
                ExchangeContact contact = ES.Services.ExchangeServer.GetContactGeneralSettings(PanelRequest.ItemID,
                    PanelRequest.AccountID);

                litDisplayName.Text = PortalAntiXSS.Encode(contact.DisplayName);

                // bind form
                txtDisplayName.Text = contact.DisplayName;
                txtEmail.Text = contact.EmailAddress;
                chkHideAddressBook.Checked = contact.HideFromAddressBook;

                txtFirstName.Text = contact.FirstName;
                txtInitials.Text = contact.Initials;
                txtLastName.Text = contact.LastName;

                txtJobTitle.Text = contact.JobTitle;
                txtCompany.Text = contact.Company;
                txtDepartment.Text = contact.Department;
                txtOffice.Text = contact.Office;
                manager.SetAccount(contact.ManagerAccount);

                txtBusinessPhone.Text = contact.BusinessPhone;
                txtFax.Text = contact.Fax;
                txtHomePhone.Text = contact.HomePhone;
                txtMobilePhone.Text = contact.MobilePhone;
                txtPager.Text = contact.Pager;
                txtWebPage.Text = contact.WebPage;

                txtAddress.Text = contact.Address;
                txtCity.Text = contact.City;
                txtState.Text = contact.State;
                txtZip.Text = contact.Zip;
                country.Country = contact.Country;
                txtNotes.Text = contact.Notes;
                ddlMAPIRichTextFormat.SelectedValue = contact.UseMapiRichTextFormat.ToString();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_CONTACT_SETTINGS", ex);
            }
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                int result = ES.Services.ExchangeServer.SetContactGeneralSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID,
                    txtDisplayName.Text,
                    txtEmail.Text,
                    chkHideAddressBook.Checked,

                    txtFirstName.Text,
                    txtInitials.Text,
                    txtLastName.Text,

                    txtAddress.Text,
                    txtCity.Text,
                    txtState.Text,
                    txtZip.Text,
                    country.Country,

                    txtJobTitle.Text,
                    txtCompany.Text,
                    txtDepartment.Text,
                    txtOffice.Text,
                    manager.GetAccount(),

                    txtBusinessPhone.Text,
                    txtFax.Text,
                    txtHomePhone.Text,
                    txtMobilePhone.Text,
                    txtPager.Text,
                    txtWebPage.Text,
                    txtNotes.Text,
                    Utils.ParseInt(ddlMAPIRichTextFormat.SelectedValue, 2/*  UseDefaultSettings */));

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                litDisplayName.Text = PortalAntiXSS.Encode(txtDisplayName.Text);

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_CONTACT_SETTINGS");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_CONTACT_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
