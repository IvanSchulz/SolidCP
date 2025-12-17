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
    public partial class ParameterEditor : System.Web.UI.UserControl
    {
        public string DataType
        {
            get { return (string)ViewState["DataType"]; }
            set { ViewState["DataType"] = value; }
        }

        public string Value
        {
            get { return GetValue(); }
            set
            {
                ViewState["Value"] = value;
                SetValue();
            }
        }

        public string DefaultValue
        {
            get { return (string)ViewState["DefaultValue"]; }
            set { ViewState["DefaultValue"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                SetValue();
        }

        private void SetValue()
        {
            string val = (string)ViewState["Value"];
            if (String.Compare(DataType, "string", true) == 0)
            {
                txtValue.Text = (val != null) ? val : DefaultValue;
                txtValue.Visible = true;
            }
            else if (String.Compare(DataType, "multistring", true) == 0)
            {
                txtMultiValue.Text = (val != null) ? val : DefaultValue;
                txtMultiValue.Visible = true;
            }
            else if (String.Compare(DataType, "list", true) == 0)
            {
                try
                {
                    ddlValue.Items.Clear();
                    string[] vals = DefaultValue.Split(';');
                    foreach (string v in vals)
                    {
                        string itemValue = v;
                        string itemText = v;

                        int eqIdx = v.IndexOf("=");
                        if (eqIdx != -1)
                        {
                            itemValue = v.Substring(0, eqIdx);
                            itemText = v.Substring(eqIdx + 1);
                        }

                        ddlValue.Items.Add(new ListItem(itemText, itemValue));
                    }
                }
                catch { /* skip */ }

                Utils.SelectListItem(ddlValue, val);
                ddlValue.Visible = true;
            }
        }


        private string GetValue()
        {
            if (String.Compare(DataType, "string", true) == 0)
            {
                return txtValue.Text.Trim();
            }
            else if (String.Compare(DataType, "multistring", true) == 0)
            {
                return txtMultiValue.Text.Trim();
            }
            else
            {
                return ddlValue.SelectedValue;
            }
        }
    }
}
