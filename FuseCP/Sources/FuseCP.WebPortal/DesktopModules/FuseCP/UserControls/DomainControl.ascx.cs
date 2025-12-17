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
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace FuseCP.Portal.UserControls
{
    public partial class DomainControl : FuseCPControlBase
    {
        public class DomainNameEventArgs : EventArgs
        {
            public string DomainName { get; set; }
        }

        public event EventHandler<DomainNameEventArgs> TextChanged;

        public virtual void OnTextChanged()
        {
            var handler = TextChanged;
            if (handler != null) handler(this, new DomainNameEventArgs {DomainName = Text});
        }

        public object DataSource
        {
            set { DomainsList.DataSource = value; }
        }

        public bool AutoPostBack
        {
            get { return txtDomainName.AutoPostBack; }
            set { txtDomainName.AutoPostBack = value; }
        }

        public Unit Width
        {
            get { return txtDomainName.Width; }
            set { txtDomainName.Width = value; }
        }

        public bool RequiredEnabled
        {
            get { return DomainRequiredValidator.Enabled; }
            set { DomainRequiredValidator.Enabled = value; }
        }

        public string Text
        {
            get
            {
                var domainName = txtDomainName.Text.Trim();
                if (!IsSubDomain) return domainName;

                if (string.IsNullOrEmpty(domainName))
                {
                    // Only return selected domain from DomainsList when no subdomain is entered yet
                    return DomainsList.SelectedValue;
                }

                return domainName + "." + DomainsList.SelectedValue;
            }
            set { txtDomainName.Text = value; }
        }

        public string ValidationGroup
        {
            get { return DomainRequiredValidator.ValidationGroup; }
            set { DomainRequiredValidator.ValidationGroup = value; DomainFormatValidator.ValidationGroup = value; }
        }

        public bool IsSubDomain {
            get { return SubDomainSeparator.Visible; }
            set
            {
                SubDomainSeparator.Visible = value;
                DomainsList.Visible = value;
                DomainRequiredValidator.Enabled = !value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected new void DataBind()
        {
            DomainsList.DataBind();
        }

        protected void DomainFormatValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var idn = new IdnMapping();
            try
            {
                var ascii = idn.GetAscii(Text);
                var regex = new Regex(@"^(?!-)(xn--)?[a-zA-Z0-9][a-zA-Z0-9-_.]{0,61}[a-zA-Z0-9]{0,1}\.(?!-)(xn--)?([a-zA-Z0-9\-.]{1,50}|[a-zA-Z0-9-.]{1,30}\.[a-zA-Z]{2,})$", RegexOptions.CultureInvariant);
                args.IsValid = regex.IsMatch(ascii);
            }
            catch (Exception)
            {
                args.IsValid = false;
            }
        }

        protected void txtDomainName_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged();
        }
    }
}
