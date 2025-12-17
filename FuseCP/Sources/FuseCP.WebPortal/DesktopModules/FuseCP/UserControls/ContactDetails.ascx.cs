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

namespace FuseCP.Portal
{
    public partial class ContactDetails : FuseCPControlBase
    {
		private string companyName;
		public string CompanyName
		{
			get { return txtCompanyName.Text; }
			set { companyName = value; }
		}

        private string address;
        public string Address
        {
            get { return txtAddress.Text; }
            set { address = value; }
        }

        private string country;
        public string Country
        {
            get
			{
				return ddlCountry.SelectedItem.Value;
			}
            set
            {
                country = value;
            }
        }

        private string city;
        public string City
        {
            get { return txtCity.Text; }
            set { city = value; }
        }

        private string zip;
        public string Zip
        {
            get { return txtZip.Text; }
            set { zip = value; }
        }

        private string primaryPhone;
        public string PrimaryPhone
        {
            get { return txtPrimaryPhone.Text; }
            set { primaryPhone = value; }
        }

        private string secondaryPhone;
        public string SecondaryPhone
        {
            get { return txtSecondaryPhone.Text; }
            set { secondaryPhone = value; }
        }

        private string state;
        public string State
        {
            get
            {
                if (ddlStates.Visible)
                    return ddlStates.SelectedItem.Text;
                else
                    return txtState.Text;
            }
            set
            {
                state = value;
            }
        }

        private string fax;

        public string Fax
        {
            get { return txtFax.Text; }
            set { fax = value; }
        }

        private string messengerId;
        public string MessengerId
        {
            get { return txtMessengerId.Text; }
            set { messengerId = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCountries();
                BindContact();
            }
        }

        private void BindContact()
        {
			txtCompanyName.Text = companyName;
            txtAddress.Text = address;
            txtCity.Text = city;
            SetCountry(country);
            BindStates();
            SetState(state);
            txtZip.Text = zip;
            txtPrimaryPhone.Text = primaryPhone;
            txtSecondaryPhone.Text = secondaryPhone;
            txtFax.Text = fax;
            txtMessengerId.Text = messengerId;
        }

        private void BindCountries()
        {
            /*DotNetNuke.Common.Lists.ListController lists = new DotNetNuke.Common.Lists.ListController();
            DotNetNuke.Common.Lists.ListEntryInfoCollection countries = lists.GetListEntryInfoCollection("Country");*/

            //ddlCountry.DataSource = countries;
			/*ddlCountry.DataSource = new object();
            ddlCountry.DataBind();*/

			PortalUtils.LoadCountriesDropDownList(ddlCountry, null);
			ddlCountry.Items.Insert(0, new ListItem("<Not specified>", ""));
        }

        private void SetCountry(string val)
        {
            SetDropdown(ddlCountry, val);
        }

        private void SetState(string val)
        {
            if (ddlStates.Visible)
                SetDropdown(ddlStates, val);
            else
                txtState.Text = val;
        }

        private void SetDropdown(DropDownList dropdown, string val)
        {
            dropdown.SelectedItem.Selected = false;

            ListItem item = dropdown.Items.FindByValue(val);
            if (item == null)
                item = dropdown.Items.FindByText(val);
            if (item != null)
                item.Selected = true;
        }

        private void BindStates()
        {
            ddlStates.Visible = false;
            txtState.Visible = true;

            if (ddlCountry.SelectedValue != "")
            {
                /*DotNetNuke.Common.Lists.ListController lists = new DotNetNuke.Common.Lists.ListController();
                DotNetNuke.Common.Lists.ListEntryInfoCollection states = lists.GetListEntryInfoCollection("Region", "", "Country." + ddlCountry.SelectedValue);

                if (states.Count > 0)
                {
                    ddlStates.DataSource = states;
                    ddlStates.DataBind();

                    ddlStates.Items.Insert(0, new ListItem("<Not specified>", ""));

                    ddlStates.Visible = true;
                    txtState.Visible = false;
                }*/

				PortalUtils.LoadStatesDropDownList(ddlStates, ddlCountry.SelectedValue);

				if (ddlStates.Items.Count > 0)
				{
					ddlStates.Items.Insert(0, new ListItem("<Not specified>", ""));
					ddlStates.Visible = true;
					txtState.Visible = false;
				}
            }
        }
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindStates();
        }
    }
}
