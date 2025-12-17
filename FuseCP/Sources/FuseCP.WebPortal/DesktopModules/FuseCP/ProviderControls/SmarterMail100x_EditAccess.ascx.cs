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
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.Providers.Database;
using FuseCP.Providers.Mail;
using System.Xml;
using System.Linq;

namespace FuseCP.Portal.ProviderControls
{
    public partial class SmarterMail100x_EditAccess : FuseCPControlBase, IMailEditDomainControl
    {
        private List<Country> selectedCountries
        {
            get
            {
                List<Country> countries = ViewState["SelectedCountries"] as List<Country>;
                if (countries == null)
                {
                    countries = new List<Country>();
                    ViewState["SelectedCountries"] = countries;
                }
                return countries;
            }
            set { ViewState["SelectedCountries"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PackageInfo info = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);

            if (!IsPostBack)
            {
                LoadSelectedCountries();
                BindAvailableCountriesDropdown();
            }
        }

        public void BindItem(MailDomain item)
        {
            ddlAuthType.SelectedValue = item[MailDomain.SMARTERMAIL100_BLOCKED_COUNTRIES_AT_AUTH_TYPE];

            string SelectedCountryCodes = item[MailDomain.SMARTERMAIL100_BLOCKED_COUNTRIES_AT_AUTH_COUNTRIES];
            if (!string.IsNullOrEmpty(SelectedCountryCodes))
            {
                ViewState["SelectedCountryCodes"] = SelectedCountryCodes;
                LoadSelectedCountries();
            }
        }

        public void SaveItem(MailDomain item)
        {
            item[MailDomain.SMARTERMAIL100_BLOCKED_COUNTRIES_AT_AUTH_TYPE] = ddlAuthType.SelectedValue;
                
            List<string> codesToSave = selectedCountries.Select(c => c.Code).ToList();
            item[MailDomain.SMARTERMAIL100_BLOCKED_COUNTRIES_AT_AUTH_COUNTRIES] = string.Join(",", codesToSave);

            item[MailDomain.SMARTERMAIL100_SET_BLOCKED_COUNTRIES] = "1";
        }

        private void BindAvailableCountriesDropdown()
        {
            List<Country> availableCountries = GetCountryList();

            availableCountries = availableCountries.Where(c => !selectedCountries.Any(sc => sc.Code == c.Code)).ToList();

            ddlAddCountry.DataSource = availableCountries;
            ddlAddCountry.DataTextField = "Name";
            ddlAddCountry.DataValueField = "Code";
            ddlAddCountry.DataBind();
        }

        private List<Country> GetCountryList()
        {
            string countriesPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Countries.config");
            XmlDocument xmlCountriesDoc = new XmlDocument();
            xmlCountriesDoc.Load(countriesPath);

            List<Country> countries = new List<Country>();

            XmlNodeList xmlCountries = xmlCountriesDoc.SelectNodes("/Countries/Country");

            foreach (XmlElement xmlCountry in xmlCountries)
            {
                countries.Add(new Country { Code = xmlCountry.GetAttribute("key"), Name = xmlCountry.GetAttribute("name") });
            }
            return countries;
        }

        private void LoadSelectedCountries()
        {
            selectedCountries.Clear();

            string selectedCodes = ViewState["SelectedCountryCodes"] as string;
            if (!string.IsNullOrEmpty(selectedCodes))
            {
                string[] codes = selectedCodes.Split(',');
                List<Country> allCountries = GetCountryList();

                foreach (string code in codes)
                {
                    Country country = allCountries.FirstOrDefault(c => c.Code == code);

                    if (country != null)
                    {
                        selectedCountries.Add(country);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Warning: Country code '{code}' not found in Countries.config.");
                    }
                }
            }
            BindSelectedCountriesGrid();
        }

        private void BindSelectedCountriesGrid()
        {
            gvSelectedCountries.DataSource = selectedCountries;
            gvSelectedCountries.DataBind();
        }


        protected void btnAddCountry_Click(object sender, EventArgs e)
        {
            string code = ddlAddCountry.SelectedValue;
            string name = ddlAddCountry.SelectedItem.Text;

            if (!selectedCountries.Any(c => c.Code == code))
            {
                selectedCountries.Add(new Country { Code = code, Name = name });
                BindSelectedCountriesGrid();
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            string codeToRemove = ((CPCC.StyleButton)sender).CommandArgument;
            selectedCountries.RemoveAll(c => c.Code == codeToRemove);
            BindSelectedCountriesGrid();
        }

        [Serializable]
        public class Country
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }


    }
}
