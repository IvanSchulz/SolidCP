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
using System.Text;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class UsernameControl : FuseCPControlBase
    {
        public Unit Width
        {
            get { return txtName.Width; }
            set { txtName.Width = value; }
        }

        public string ValidationGroup
        {
            get
            {
                return valRequireUsername.ValidationGroup;
            }
            set
            {
                valRequireUsername.ValidationGroup = value;
                valCorrectUsername.ValidationGroup = value;
                valCorrectMinLength.ValidationGroup = value;
            }
        }

        public bool EditMode
        {
            get { return (ViewState["EditMode"] != null) ? (bool)ViewState["EditMode"] : false; }
            set { ViewState["EditMode"] = value; ToggleControls(); }
        }

        public bool RequiredField
        {
            get { return (ViewState["RequiredField"] != null) ? (bool)ViewState["RequiredField"] : true; }
            set { ViewState["RequiredField"] = value; ToggleControls(); }
        }

        public string Text
        {
            get { return EditMode ? txtName.Text.Trim() : litPrefix.Text + txtName.Text.Trim() + litSuffix.Text; }
            set { txtName.Text = value; lblName.Text = PortalAntiXSS.Encode(value); }
        }

        private UserInfo PolicyUser
        {
            get { return (ViewState["PolicyUser"] != null) ? (UserInfo)ViewState["PolicyUser"] : null; }
            set { ViewState["PolicyUser"] = value; }
        }

        private string PolicyValue
        {
            get { return (ViewState["PolicyValue"] != null) ? (string)ViewState["PolicyValue"] : null; }
            set { ViewState["PolicyValue"] = value; }
        }

        public void SetPackagePolicy(int packageId, string settingsName, string key)
        {
            // load package
            PackageInfo package = PackagesHelper.GetCachedPackage(packageId);
            if (package != null)
            {
                // init by user
                SetUserPolicy(package.UserId, settingsName, key);
            }
        }

        public void SetUserPolicy(int userId, string settingsName, string key)
        {
            // load user profile
            UserInfo user = UsersHelper.GetCachedUser(userId);

            if (user != null)
            {
                PolicyUser = user;

                // load settings
                UserSettings settings = UsersHelper.GetCachedUserSettings(userId, settingsName);
                if (settings != null)
                {
                    string policyValue = settings[key];
                    if (policyValue != null)
                        PolicyValue = policyValue;
                }
            }

            // toggle controls
            ToggleControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleControls()
        {
            // hide/show controls
            litPrefix.Visible = ((!EditMode) && !String.IsNullOrEmpty(litPrefix.Text));
            txtName.Visible = !EditMode;
            lblName.Visible = EditMode;
            litSuffix.Visible = ((!EditMode) && !String.IsNullOrEmpty(litSuffix.Text));
            valRequireUsername.Enabled = RequiredField && !EditMode;
            valCorrectUsername.Enabled = !EditMode;
            valCorrectMinLength.Enabled = !EditMode;

            if (EditMode)
                return;

            // require validator
            valRequireUsername.ErrorMessage = GetLocalizedString("CantBeBlank.Text");

            // disable min length validator
            valCorrectMinLength.Enabled = false;

            // username validator
            string defAllowedRegexp = PanelGlobals.UsernameDefaultAllowedRegExp;
            string defAllowedText = "a-z&nbsp;&nbsp;A-Z&nbsp;&nbsp;0-9&nbsp;&nbsp;.&nbsp;&nbsp;_";

            // parse and enforce policy
            if (PolicyValue != null)
            {
                bool enabled = false;
                string allowedSymbols = null;
                int minLength = -1;
                int maxLength = -1;
                string prefix = null;
                string suffix = null;

                try
                {
                    // parse settings
                    string[] parts = PolicyValue.Split(';');
                    enabled = Utils.ParseBool(parts[0], false);
                    allowedSymbols = parts[1];
                    minLength = Utils.ParseInt(parts[2], -1);
                    maxLength = Utils.ParseInt(parts[3], -1);
                    prefix = parts[4];
                    suffix = parts[5];
                }
                catch { /* skip */ }

                // apply policy
                if (enabled)
                {
                    // prefix
                    if (!String.IsNullOrEmpty(prefix))
                    {
                        // substitute vars
                        prefix = Utils.ReplaceStringVariable(prefix, "user_id", PolicyUser.UserId.ToString());
                        prefix = Utils.ReplaceStringVariable(prefix, "user_name", PolicyUser.Username);

                        // display
                        litPrefix.Text = prefix;

                        // adjust max length
                        maxLength -= prefix.Length;
                    }

                    // suffix
                    if (!String.IsNullOrEmpty(suffix))
                    {
                        // substitute vars
                        suffix = Utils.ReplaceStringVariable(suffix, "user_id", PolicyUser.UserId.ToString());
                        suffix = Utils.ReplaceStringVariable(suffix, "user_name", PolicyUser.Username);

                        // display
                        litSuffix.Text = suffix;

                        // adjust max length
                        maxLength -= suffix.Length;
                    }

                    // min length
                    if (minLength > 0)
                    {
                        valCorrectMinLength.Enabled = true;
                        valCorrectMinLength.ValidationExpression = "^.{" + minLength.ToString() + ",}$";
                        valCorrectMinLength.ErrorMessage = String.Format(
                            GetLocalizedString("MinLength.Text"), minLength);
                    }

                    // max length
                    if (maxLength > 0)
                        txtName.MaxLength = maxLength;

                    // process allowed symbols
                    if (!String.IsNullOrEmpty(allowedSymbols))
                    {
                        StringBuilder sb = new StringBuilder(defAllowedRegexp);
                        for (int i = 0; i < allowedSymbols.Length; i++)
                        {
							// Escape characters only if required
							if (PanelGlobals.MetaCharacters2Escape.IndexOf(allowedSymbols[i]) > -1)
								sb.Append(@"\").Append(allowedSymbols[i]);
							else
								sb.Append(allowedSymbols[i]);
                            //
                            defAllowedText += "&nbsp;&nbsp;" + allowedSymbols[i];
                        }
                        defAllowedRegexp = sb.ToString();
                    }

                } // if(enabled)
            } // if (PolicyValue != null)

            valCorrectUsername.ValidationExpression = @"^[" + defAllowedRegexp + @"]*$";
            valCorrectUsername.ErrorMessage = String.Format(GetLocalizedString("AllowedSymbols.Text"),
                defAllowedText);

        } // ToggleControls()
    }
}
