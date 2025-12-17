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
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public class FuseCPModuleBase : FuseCPControlBase
    {
        private IMessageBoxControl messageBox;

        public FuseCPModuleBase()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            // add message box control
            messageBox = (IMessageBoxControl)this.LoadControl(
                PanelGlobals.FuseCPRootPath + "UserControls/MessageBox.ascx");
            this.Controls.AddAt(0, (Control)messageBox);
            ((Control)messageBox).Visible = false;

            // call base handler
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            //Page.MaintainScrollPositionOnPostBack = true;

            // call base handler
            base.OnLoad(e);
        }

        public void SwitchUser(object arg)
        {
            //PanelSecurity.SelectedUserId = Utils.ParseInt(arg.ToString(), PanelSecurity.EffectiveUserId);
            RedirectToBrowsePage();
        }

        public void SwitchPackage(object arg)
        {
            string[] args = arg.ToString().Split(',');

            //PanelSecurity.SelectedUserId = Utils.ParseInt(args[0], PanelSecurity.EffectiveUserId);
            //PanelSecurity.PackageId = Utils.ParseInt(args[1], 0);
            RedirectToBrowsePage();
        }

        public void LoadProviderControl(int packageId, string groupName, PlaceHolder container, string controlName)
        {
            string ctrlPath = null;
            //
            ProviderInfo provider = ES.Services.Servers.GetPackageServiceProvider(packageId, groupName);

            // try to locate suitable control
            string currPath = this.AppRelativeVirtualPath;
            currPath = currPath.Substring(0, currPath.LastIndexOf("/"));

            ctrlPath = currPath + "/ProviderControls/" + provider.EditorControl + "_" + controlName;

            Control ctrl = Page.LoadControl(ctrlPath);

            // add control to the placeholder
            container.Controls.Add(ctrl);
        }

        public void HideServiceColumns(GridView gv)
        {
            try
            {
                gv.Columns[gv.Columns.Count - 1].Visible =
                    (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);
            }
            catch
            {
            }
        }

        #region Error Messages Processing
        public void ProcessException(Exception ex)
        {
            string authError = "The security token could not be authenticated or authorized";
            if (ex.Message.Contains(authError) ||
                (ex.InnerException != null &&
                ex.InnerException.Message.Contains(authError)))
            {
                ShowWarningMessage("ES_CONNECT");
            }
            else
            {
                ShowErrorMessage("MODULE_LOAD", ex);
            }

        }

        public virtual void ShowResultMessage(int resultCode)
        {
            ShowResultMessage(Utils.ModuleName, resultCode, false);
        }

        public virtual void ShowResultMessageWithContactForm(int resultCode)
        {
            ShowResultMessage(Utils.ModuleName, resultCode, true);
        }

        public void ShowResultMessage(string moduleName, int resultCode, params object[] formatArgs)
        {
            ShowResultMessage(moduleName, resultCode, false, formatArgs);
        }

        public void ShowResultMessage(string moduleName, int resultCode, bool showcf, params object[] formatArgs)
        {
            lock (this)
            {
                MessageBoxType messageType = MessageBoxType.Warning;

                // try to get warning
                string sCode = Convert.ToString(resultCode * -1);
                string localizedMessage = GetSharedLocalizedString(moduleName, "Warning." + sCode);
                string localizedDescription = GetSharedLocalizedString(moduleName, "WarningDescription." + sCode);

                if (localizedMessage == null)
                {
                    messageType = MessageBoxType.Error;

                    // try to get error
                    localizedMessage = GetSharedLocalizedString(moduleName, "Error." + sCode);
                    localizedDescription = GetSharedLocalizedString(moduleName, "ErrorDescription." + sCode);

                    if (localizedMessage == null)
                    {
                        localizedMessage = GetSharedLocalizedString(moduleName, "Message.Generic") + " " + resultCode.ToString();
                    }
                    else
                    {
                        if (formatArgs != null && formatArgs.Length > 0)
                            localizedMessage = String.Format(localizedMessage, formatArgs);
                    }
                }

                // check if this is a "demo" message and it is overriden
                if (resultCode == BusinessErrorCodes.ERROR_USER_ACCOUNT_DEMO)
                {
                    UserSettings fcpSettings = UsersHelper.GetCachedUserSettings(
                        PanelSecurity.EffectiveUserId, UserSettings.FuseCP_POLICY);
                    if (!String.IsNullOrEmpty(fcpSettings["DemoMessage"]))
                    {
                        localizedDescription = fcpSettings["DemoMessage"];
                    }
                }

                // render message
                Exception fake_ex = null;
                // Contact form is requested to be shown
                if (showcf)
                    fake_ex = new Exception();
                //
                messageBox.RenderMessage(messageType, localizedMessage, localizedDescription, fake_ex);
            }
        }

        public virtual void ShowSuccessMessage(string messageKey)
        {
            ShowSuccessMessage(Utils.ModuleName, messageKey, null);
        }

        public void ShowSuccessMessage(string moduleName, string messageKey)
        {
            ShowSuccessMessage(moduleName, messageKey, null);
        }

        public virtual void ShowSuccessMessage(string moduleName, string messageKey, params string[] formatArgs)
        {
            lock (this)
            {
                string localizedMessage = GetSharedLocalizedString(moduleName, "Success." + messageKey);
                string localizedDescription = GetSharedLocalizedString(moduleName, "SuccessDescription." + messageKey);
                if (localizedMessage == null)
                {
                    localizedMessage = messageKey;
                }
                else
                {
                    //Format message string with args
                    if (formatArgs != null && formatArgs.Length > 0)
                    {
                        localizedMessage = String.Format(localizedMessage, formatArgs);
                    }
                }
                // render message
                messageBox.RenderMessage(MessageBoxType.Information, localizedMessage, localizedDescription, null);
            }
        }

        public virtual void ShowWarningMessage(string messageKey)
        {
            ShowWarningMessage(Utils.ModuleName, messageKey);
        }

        public void ShowWarningMessage(string moduleName, string messageKey)
        {
            lock (this)
            {
                string localizedMessage = GetSharedLocalizedString(moduleName, "Warning." + messageKey);
                string localizedDescription = GetSharedLocalizedString(moduleName, "WarningDescription." + messageKey);
                if (localizedMessage == null)
                    localizedMessage = messageKey;

                // render message
                messageBox.RenderMessage(MessageBoxType.Warning, localizedMessage, localizedDescription, null);
            }
        }

        public void ShowErrorMessage(string messageKey, params string[] additionalParameters)
        {
            ShowErrorMessage(messageKey, null, additionalParameters);
        }

        public virtual void ShowErrorMessage(string messageKey, Exception ex, params string[] additionalParameters)
        {
            ShowErrorMessage(Utils.ModuleName, messageKey, ex, additionalParameters);
        }

        public void ShowErrorMessage(string moduleName, string messageKey, Exception ex, params string[] additionalParameters)
        {
            lock (this)
            {
                string exceptionKey = null;
                //
                if (ex != null)
                {
                    if (!String.IsNullOrEmpty(ex.Message) && ex.Message.Contains("FuseCP_ERROR"))
                    {
                        string[] messageParts = ex.Message.Split(new char[] { '@' });
                        if (messageParts.Length > 1)
                        {
                            exceptionKey = messageParts[1].TrimStart(new char[] { ' ' });
                        }
                    }
                }
                string localizedMessage = GetSharedLocalizedString(moduleName, "Error." + exceptionKey);
                string localizedDescription = GetSharedLocalizedString(moduleName, "ErrorDescription." + exceptionKey);

                if (localizedMessage == null)
                {
                    localizedMessage = GetSharedLocalizedString(moduleName, "Error." + messageKey);
                    localizedDescription = GetSharedLocalizedString(moduleName, messageKey);
                    if (localizedMessage == null)
                        localizedMessage = messageKey;
                }
                else
                {
                    //render localized exception message without stack trace
                    messageBox.RenderMessage(MessageBoxType.Error, localizedMessage, localizedDescription, null);
                    return;
                }

                // render message
                messageBox.RenderMessage(MessageBoxType.Error, localizedMessage, localizedDescription, ex);
            }
        }

        #endregion
    }
}
