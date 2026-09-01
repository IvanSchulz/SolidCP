// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolidCP.Portal
{
    public partial class LicensingQuotaEdit : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int quotaId = PanelRequest.QuotaID;
            if (!IsPostBack)
            {
                LicensingQuota quota = ES.Services.Servers.GetLicensingQuotaById(quotaId);
                if (quota == null)
                {
                    messageBox.ShowErrorMessage("UPDATE_LICENSING_QUOTA_NOT_FOUND");
                    return;
                }
                litQuotaName.Text = quota.QuotaDescription;
                txtDescription.Text = quota.LicenseDescription;
                chkActive.Checked = !quota.IsHidden;
                BindVmTemplates(quota);
            }
        }

        private void BindVmTemplates(LicensingQuota quota)
        {
            List<string> result = new List<string>();
            if (quota.VmTemplateNames != null) result.AddRange(quota.VmTemplateNames);
            repVmTemplateNames.DataSource = result;
            repVmTemplateNames.DataBind();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            int quotaId = PanelRequest.QuotaID;
            LicensingQuota quota = ES.Services.Servers.GetLicensingQuotaById(quotaId);
            if (quota == null)
            {
                messageBox.ShowErrorMessage("UPDATE_LICENSING_QUOTA_NOT_FOUND");
                return;
            }
            quota.LicenseDescription = txtDescription.Text;
            quota.IsHidden = !chkActive.Checked;
            quota.VmTemplateNames = GetVmTemplates().ToArray();

            if (!quota.IsHidden)
            {
                if (String.IsNullOrEmpty(quota.LicenseDescription))
                {
                    messageBox.ShowErrorMessage("UPDATE_LICENSING_QUOTA_DESCRIPTION");
                    txtDescription.Focus();
                    return;
                }
                if (quota.VmTemplateNames == null || quota.VmTemplateNames.Length == 0)
                {
                    messageBox.ShowErrorMessage("UPDATE_LICENSING_QUOTA_TEMPLATES");
                    btnAddVmTemplateName.Focus();
                    return;
                }
            }

            ResultObject res = ES.Services.Servers.UpdateLicensingQuota(quota);
            if (res.IsSuccess)
            {
                Response.Redirect(NavigatePageURL("LicensingQuotas", null, null, null));
            }
            else
            {
                messageBox.ShowMessage(res, "UPDATE_LICENSING_QUOTA", "");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(NavigatePageURL("LicensingQuotas", null, null, null));
        }

        protected void btnAddVmTemplateName_Click(object sender, EventArgs e)
        {
            var templates = GetVmTemplates();
            templates.Add("");
            RebindVmTemplates(templates);
        }

        protected void btnRemoveVmTemplateName_OnCommand(object sender, CommandEventArgs e)
        {
            var templates = GetVmTemplates();
            templates.RemoveAt(Convert.ToInt32(e.CommandArgument));
            RebindVmTemplates(templates);
        }

        private void RebindVmTemplates(List<string> templates)
        {
            repVmTemplateNames.DataSource = templates;
            repVmTemplateNames.DataBind();
        }

        private List<string> GetVmTemplates()
        {
            var result = new List<string>();

            foreach (RepeaterItem item in repVmTemplateNames.Items)
            {
                result.Add(GetTextBoxText(item, "txtVmTemplateName"));
            }

            return result;
        }

        private string GetTextBoxText(RepeaterItem item, string name)
        {
            return (item.FindControl(name) as TextBox).Text;
        }
    }
}