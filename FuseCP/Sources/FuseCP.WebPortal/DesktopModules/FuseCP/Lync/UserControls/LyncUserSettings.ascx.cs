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
using FuseCP.EnterpriseServer;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal.Lync.UserControls
{
    public partial class LyncUserSettings : FuseCPControlBase
    {

        private string sipAddressToSelect;

        public string sipAddress
        {
                        
            get 
            {
                if (ddlSipAddresses.Visible)
                {
                    if ((ddlSipAddresses != null) && (ddlSipAddresses.SelectedItem != null))
                        return ddlSipAddresses.SelectedItem.Value;
                    else
                        return string.Empty;
                }
                else
                {
                    return email.Email;
                }
            }
            set
            {
                sipAddressToSelect = value;

                if (ddlSipAddresses.Visible)
                {
                    if ((ddlSipAddresses != null) && (ddlSipAddresses.Items != null))
                    {
                        foreach (ListItem li in ddlSipAddresses.Items)
                        {
                            if (li.Value == value)
                            {
                                ddlSipAddresses.ClearSelection();
                                li.Selected = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        string[] Tmp = value.Split('@');
                        email.AccountName = Tmp[0];

                        if (Tmp.Length > 1)
                        {
                            email.DomainName = Tmp[1];
                        }
                    }
                }
            }
        }

        public int plansCount
		{
			get
			{
                return this.ddlSipAddresses.Items.Count;
			}
		}


        protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
			{
                BindAddresses();
			}
        }

        private void BindAddresses()
		{

            OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID);

            if (user == null)
                return;

            if (user.AccountType == ExchangeAccountType.Mailbox)
            {
                email.Visible = false;
                ddlSipAddresses.Visible = true;

                FuseCP.EnterpriseServer.ExchangeEmailAddress[] emails = ES.Services.ExchangeServer.GetMailboxEmailAddresses(PanelRequest.ItemID, PanelRequest.AccountID);

                foreach (FuseCP.EnterpriseServer.ExchangeEmailAddress mail in emails)
                {
                    ListItem li = new ListItem();
                    li.Text = mail.EmailAddress;
                    li.Value = mail.EmailAddress;
                    li.Selected = mail.IsPrimary;
                    ddlSipAddresses.Items.Add(li);
                }

                foreach (ListItem li in ddlSipAddresses.Items)
                {
                    if (li.Value == sipAddressToSelect)
                    {
                        ddlSipAddresses.ClearSelection();
                        li.Selected = true;
                        break;
                    }
                }
            }
            else
            {
                email.Visible = true;
                ddlSipAddresses.Visible = false;

                if (!string.IsNullOrEmpty(sipAddressToSelect))
                {
                    string[] Tmp = sipAddressToSelect.Split('@');
                    email.AccountName = Tmp[0];

                    if (Tmp.Length > 1)
                    {
                        email.DomainName = Tmp[1];
                    }
                }


            }

		}
    }
}
