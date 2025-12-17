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

namespace FuseCP.Portal
{
    public partial class QuotaEditor : FuseCPControlBase
    {
        public int QuotaId
        {
            get { return (int)ViewState["QuotaId"]; }
            set { ViewState["QuotaId"] = value; }
        }

        public int QuotaTypeId
        {
            get { return (int)ViewState["QuotaTypeId"]; }
            set
            {
                ViewState["QuotaTypeId"] = value;

                // toggle controls
                txtQuotaValue.Visible = (value > 1);
                chkQuotaUnlimited.Visible = (value > 1);
                chkQuotaEnabled.Visible = (value == 1);
            }
        }

        public string UnlimitedText
        {
            get { return chkQuotaUnlimited.Text; }
            set { chkQuotaUnlimited.Text = value; }

        }

        public int QuotaValue
        {
            get
            {
                if (QuotaTypeId == 1)
                    // bool quota
                    return chkQuotaEnabled.Checked ? 1 : 0;
                else
                {
                    if (ParentQuotaValue == -1)
                    {
                        if ((QuotaMinValue > 0) | (QuotaMaxValue > 0))
                        {
                            int quotaValue = 0;
                            // numeric quota
                            if (chkQuotaUnlimited.Checked)
                                quotaValue = -1;
                            else
                            {

                                if (QuotaMinValue > 0)
                                    quotaValue = Math.Max(Utils.ParseInt(txtQuotaValue.Text, 0), QuotaMinValue);
                                else
                                    quotaValue = Utils.ParseInt(txtQuotaValue.Text, 0);

                                if (QuotaMaxValue > 0)
                                {
                                    if (Utils.ParseInt(txtQuotaValue.Text, 0) > QuotaMaxValue)
                                        quotaValue = QuotaMaxValue;
                                }
                            }
                            return quotaValue;
                        }
                        else
                            return chkQuotaUnlimited.Checked ? -1 : Utils.ParseInt(txtQuotaValue.Text, 0);
                        
                    }
                    else
                    {

                        if ((QuotaMinValue > 0) | (QuotaMaxValue > 0))
                        {

                            int quotaValue = 0;
                            // numeric quota
                            if (chkQuotaUnlimited.Checked)
                                quotaValue = ParentQuotaValue;
                            else
                            {
                                quotaValue = Utils.ParseInt(txtQuotaValue.Text, 0);


                                if (QuotaMinValue > 0)
                                    quotaValue = Math.Max(quotaValue, QuotaMinValue);

                                if (QuotaMaxValue > 0)
                                {
                                    if (quotaValue > QuotaMaxValue)
                                        quotaValue = QuotaMaxValue;
                                }

                                quotaValue = Math.Min(quotaValue, ParentQuotaValue);
                            }
                            return quotaValue;
                        }
                        else
                        {
                            return
                                chkQuotaUnlimited.Checked
                                    ? ParentQuotaValue
                                    : Math.Min(Utils.ParseInt(txtQuotaValue.Text, 0), ParentQuotaValue);
                        }


                        
                    }
                }
            }
            set
            {
                if (QuotaMinValue > 0)
                    txtQuotaValue.Text = Math.Max(value, QuotaMinValue).ToString();
                else
                    txtQuotaValue.Text = value.ToString();

                chkQuotaEnabled.Checked = (value > 0);
                chkQuotaUnlimited.Checked = (value == -1);
            }
        }

        public int QuotaMinValue
        {
            get { return ViewState["QuotaMinValue"] != null ? (int)ViewState["QuotaMinValue"] : 0; }
            set
            {
                ViewState["QuotaMinValue"] = value;

                if (QuotaMinValue > 0)
                {
                    if (QuotaValue < QuotaMinValue) QuotaValue = QuotaMinValue;
                }
            }

        }

        public int QuotaMaxValue
        {
            get { return ViewState["QuotaMaxValue"] != null ? (int)ViewState["QuotaMaxValue"] : 0; }
            set { ViewState["QuotaMaxValue"] = value; }
        }

        public int ParentQuotaValue
        {
            set
            {
                ViewState["ParentQuotaValue"] = value;
                if (value == 0)
                {
                    txtQuotaValue.Enabled = false;
                    chkQuotaEnabled.Enabled = false;
                    chkQuotaUnlimited.Visible = false;
                    chkQuotaEnabled.Checked = false;
                }

                if (value != -1)
                    chkQuotaUnlimited.Visible = false;
            }
            get
            {
                return ViewState["ParentQuotaValue"] != null ? (int)ViewState["ParentQuotaValue"] : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WriteScriptBlock();
        }

        protected override void OnPreRender(EventArgs e)
        {
            // set textbox attributes
            txtQuotaValue.Style["display"] = (txtQuotaValue.Text == "-1") ? "none" : "inline";

            chkQuotaUnlimited.Attributes["onclick"] = String.Format("ToggleQuota('{0}', '{1}', {2});",
                txtQuotaValue.ClientID, chkQuotaUnlimited.ClientID, QuotaMinValue);

            // call base handler
            base.OnPreRender(e);
        }

        private void WriteScriptBlock()
        {
            string scriptKey = "QuotaScript";
            if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptKey))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), scriptKey, @"<script language='javascript' type='text/javascript'>
                    function ToggleQuota(txtId, chkId, minValue)
                    {   
                        var unlimited = document.getElementById(chkId).checked;
                        document.getElementById(txtId).style.display = unlimited ? 'none' : 'inline';
                        document.getElementById(txtId).value = unlimited ? '-1' : '0';
                        if (minValue > 0) 
                        {
                            if (document.getElementById(txtId).value < minValue) document.getElementById(txtId).value = minValue;
                        }
                    }
                    </script>");
            }
        }
    }
}
