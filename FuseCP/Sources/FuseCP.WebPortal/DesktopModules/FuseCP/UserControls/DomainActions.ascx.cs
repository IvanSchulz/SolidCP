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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Portal.UserControls;
using FuseCP.Providers;
using FuseCP.Providers.HostedSolution;

namespace FuseCP.Portal
{
    public enum DomainActionTypes
    {
        None = 0,
        EnableDns = 1,
        DisableDns = 2,
        CreatePreviewDomain = 3,
        DeletePreviewDomain = 4,
    }

    public partial class DomainActions : ActionListControlBase<DomainActionTypes>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Remove DNS items if current Hosting plan does not allow it
            if (!PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId).Groups.ContainsKey(ResourceGroups.Dns))
            {
                RemoveActionItem(DomainActionTypes.EnableDns);
                RemoveActionItem(DomainActionTypes.DisableDns);
            }

            // Remove Preview Domain items if current Hosting plan does not allow it
            PackageSettings packageSettings = ES.Services.Packages.GetPackageSettings(PanelSecurity.PackageId, PackageSettings.INSTANT_ALIAS);
            if (packageSettings == null || String.IsNullOrEmpty(packageSettings["PreviewDomain"]))
            {
                RemoveActionItem(DomainActionTypes.CreatePreviewDomain);
                RemoveActionItem(DomainActionTypes.DeletePreviewDomain);
            }

            // hide control if no actions allowed
            if (ActionsList.Items.Count <= 1)
            {
                Visible = false;
            }
        }

        protected override DropDownList ActionsList
        {
            get { return ddlDomainActions; }
        }

        protected override int DoAction(List<int> ids)
        {
            switch (SelectedAction)
            {
                case DomainActionTypes.EnableDns:
                    return EnableDns(true, ids);
                case DomainActionTypes.DisableDns:
                    return EnableDns(false, ids);
                case DomainActionTypes.CreatePreviewDomain:
                    return CreatePreviewDomain(true, ids);
                case DomainActionTypes.DeletePreviewDomain:
                    return CreatePreviewDomain(false, ids);
            }

            return 0;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            switch (SelectedAction)
            {
                case DomainActionTypes.EnableDns:
                case DomainActionTypes.DisableDns:
                case DomainActionTypes.CreatePreviewDomain:
                case DomainActionTypes.DeletePreviewDomain:
                    FireExecuteAction();
                    break;
            }
        }

        private int EnableDns(bool enable, List<int> ids)
        {
            foreach (var id in ids)
            {
                // load domain
                DomainInfo domain = ES.Services.Servers.GetDomain(id);
                if (domain == null)
                    continue;

                // load package context
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(domain.PackageId);
                bool dnsEnabled = cntx.Groups.ContainsKey(ResourceGroups.Dns);
                if (!dnsEnabled)
                    continue;

                int result;
                
                if (enable)
                    result = ES.Services.Servers.EnableDomainDns(id);
                else 
                    result = ES.Services.Servers.DisableDomainDns(id);


                if (result < 0)
                    return result;
            }

            return 0;
        }

        private int CreatePreviewDomain(bool enable, List<int> ids)
        {
            foreach (var id in ids)
            {
                 // load domain
                DomainInfo domain = ES.Services.Servers.GetDomain(id);
                if (domain == null)
                    continue;

                // Preview Domain
                bool instantAliasAllowed = !String.IsNullOrEmpty(domain.PreviewDomainName);
                if (!instantAliasAllowed || domain.IsDomainPointer || domain.IsPreviewDomain)
                    continue;

                int result;

                if (enable)
                    result = ES.Services.Servers.CreateDomainPreviewDomain("", id);
                else
                    result = ES.Services.Servers.DeleteDomainPreviewDomain(id);

                if (result < 0)
                    return result;
            }

            return 0;
        }
    }
}
