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

using FuseCP.Providers.Mail;

namespace FuseCP.Portal
{
    public partial class MailForwardingsEditForwarding : FuseCPModuleBase
    {
        MailAlias item = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Visible = (PanelRequest.ItemID > 0);

            // bind item
            BindItem();
        }

        private void BindItem()
        {
            try
            {
                if (!IsPostBack)
                {
                    // load item if required
                    if (PanelRequest.ItemID > 0)
                    {
                        // existing item
                        try
                        {
                            item = ES.Services.MailServers.GetMailForwarding(PanelRequest.ItemID);
                        }
                        catch (Exception ex)
                        {
                            ShowErrorMessage("MAIL_GET_FORWARDING", ex);
                            return;
                        }

                        if (item != null)
                        {
                            // save package info
                            ViewState["PackageId"] = item.PackageId;
                            emailAddress.PackageId = item.PackageId;
                        }
                        else
                            RedirectToBrowsePage();
                    }
                    else
                    {
                        // new item
                        ViewState["PackageId"] = PanelSecurity.PackageId;
                        emailAddress.PackageId = PanelSecurity.PackageId;
                    }
                }

                // load provider control
                LoadProviderControl((int)ViewState["PackageId"], "Mail", providerControl, "EditForwarding.ascx");

                if (!IsPostBack)
                {
                    // bind item to controls
                    if (item != null)
                    {
                        // bind item to controls
                        emailAddress.Email = item.Name;
                        emailAddress.EditMode = true;

                        txtForwardTo.Text = item.ForwardTo;

                        // other controls
                        IMailEditForwardingControl ctrl = (IMailEditForwardingControl)providerControl.Controls[0];
                        ctrl.BindItem(item);
                    }
                }
            }
            catch
            {
                ShowWarningMessage("INIT_SERVICE_ITEM_FORM");
                DisableFormControls(this, btnCancel);
                return;
            }
        }

        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            MailAlias item = new MailAlias();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;
            item.Name = emailAddress.Email;
            item.ForwardTo = txtForwardTo.Text.Trim();

            //checking if forwarding name is different from existing e-mail accounts
            MailAccount[] accounts = ES.Services.MailServers.GetMailAccounts(PanelSecurity.PackageId, true);
            foreach (MailAccount account in accounts)
            {
                if (item.Name == account.Name)
                {
                    ShowWarningMessage("MAIL_FORW_NAME");
                    return;
                }
            }

            //checking if forwarding name is different from existing e-mail lists
            MailList[] lists = ES.Services.MailServers.GetMailLists(PanelSecurity.PackageId, true);
            foreach (MailList list in lists)
            {
                if (item.Name == list.Name)
                {
                    ShowWarningMessage("MAIL_FORW_NAME");
                    return;
                }
            }

            //checking if forwarding name is different from existing e-mail groups
            MailGroup[] mailgroups = ES.Services.MailServers.GetMailGroups(PanelSecurity.PackageId, true);
            foreach (MailGroup group in mailgroups)
            {
                if (item.Name == group.Name)
                {
                    ShowWarningMessage("MAIL_FORW_NAME");
                    return;
                }
            }

            // get other props
            IMailEditForwardingControl ctrl = (IMailEditForwardingControl)providerControl.Controls[0];
            ctrl.SaveItem(item);

            if (PanelRequest.ItemID == 0)
            {
                // new item
                try
                {
                    int result = ES.Services.MailServers.AddMailForwarding(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    ShowErrorMessage("MAIL_ADD_FORWARDING", ex);
                    return;
                }
            }
            else
            {
                // existing item
                try
                {
                    int result = ES.Services.MailServers.UpdateMailForwarding(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("MAIL_UPDATE_FORWARDING", ex);
                    return;
                }
            }

            // return
            RedirectSpaceHomePage();
        }

        private void DeleteItem()
        {
            // delete
            try
            {
                int result = ES.Services.MailServers.DeleteMailForwarding(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("MAIL_DELETE_FORWARDING", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveItem();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // return
            RedirectSpaceHomePage();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
    }
}
