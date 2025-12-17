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
    public partial class MailDomainsEditDomain : FuseCPModuleBase
    {
        MailDomain item = null;

        protected void Page_Load(object sender, EventArgs e)
        {
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
                            item = ES.Services.MailServers.GetMailDomain(PanelRequest.ItemID);
                        }
                        catch (Exception ex)
                        {
                            ShowErrorMessage("MAIL_GET_DOMAIN", ex);
                            return;
                        }

                        if (item != null)
                        {
                            // save package info
                            ViewState["PackageId"] = item.PackageId;
                        }
                        else
                            RedirectToBrowsePage();
                    }
                }

                // load provider control
                LoadProviderControl((int)ViewState["PackageId"], "Mail", providerControl, "EditDomain.ascx");

                if (!IsPostBack)
                {
                    // bind item to controls
                    if (item != null)
                    {
                        // bind item to controls
                        litDomainName.Text = item.Name;

                        // other controls
                        IMailEditDomainControl ctrl = (IMailEditDomainControl)providerControl.Controls[0];
                        ctrl.BindItem(item);

                        BindPointers();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("MAIL_INIT_DOMAIN_FORM", ex);
                return;
            }
        }

        private void BindPointers()
        {
            gvPointers.DataSource = ES.Services.MailServers.GetMailDomainPointers(PanelRequest.ItemID);
            gvPointers.DataBind();
        }

        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            MailDomain item = new MailDomain();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;

            // get other props
            IMailEditDomainControl ctrl = (IMailEditDomainControl)providerControl.Controls[0];
            ctrl.SaveItem(item);

            // existing item
            try
            {
                int result = ES.Services.MailServers.UpdateMailDomain(item);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("MAIL_UPDATE_DOMAIN", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        private void DeleteItem()
        {
            // delete
            try
            {
                int result = ES.Services.MailServers.DeleteMailDomain(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("MAIL_DELETE_DOMAIN", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
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


        protected void btnAddPointer_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "add_pointer",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void gvPointers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int domainId = (int)gvPointers.DataKeys[e.RowIndex][0];
            ES.Services.MailServers.DeleteMailDomainPointer(PanelRequest.ItemID, domainId);

            // rebind pointers
            BindPointers();
        }
    }
}
