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
using FuseCP.Providers.ResultObjects;
using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    public partial class LyncAddFederationDomain : FuseCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ProxyFQDN.Visible = ProxyFQDN.Enabled = false;
            locProxyFQDN.Visible = false;
        }

        private void AddDomain()
        {
            if (!Page.IsValid)
                return;


            // get domain name
            string domainName = DomainName.Text.Trim();
            string proxyFQDN = ProxyFQDN.Text.Trim();


            LyncUserResult res = ES.Services.Lync.AddFederationDomain(PanelRequest.ItemID, domainName, proxyFQDN);
            if (!(res.IsSuccess && res.ErrorCodes.Count == 0))
            {
                messageBox.ShowMessage(res, "ADD_LYNC_FEDERATIONDOMAIN", "LYNC");
            }


            // return
            RedirectBack();
        }

        private void RedirectBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "lync_federationdomains",
                                "SpaceID=" + PanelSecurity.PackageId));

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // return
            RedirectBack();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AddDomain();
        }
    }
}
