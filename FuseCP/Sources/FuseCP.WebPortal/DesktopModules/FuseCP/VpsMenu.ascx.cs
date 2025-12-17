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
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using FuseCP.EnterpriseServer;
using FuseCP.WebPortal;
namespace FuseCP.Portal
{
    public partial class VpsMenu : FuseCPModuleBase
    {
        private const string PID_SPACE_VPS = "SpaceVPS2012";
        private const string PID_SPACE_PROXMOX = "SpaceProxmox";
        protected void Page_Load(object sender, EventArgs e)
        {
            // organization
            bool vpsVisible = (Request[DefaultPage.PAGE_ID_PARAM].Equals(PID_SPACE_VPS, StringComparison.InvariantCultureIgnoreCase) ||
                                Request[DefaultPage.PAGE_ID_PARAM].Equals(PID_SPACE_PROXMOX, StringComparison.InvariantCultureIgnoreCase));

            vpsMenu.Visible = vpsVisible;
            if (vpsVisible)
            {
                MenuItem rootItem = new MenuItem(locMenuTitle.Text);
                rootItem.Selectable = false;
                menu.Items.Add(rootItem);
                BindMenu(rootItem.ChildItems);
            }
        }
        virtual public int PackageId
        {
            get { return PanelSecurity.PackageId; }
            set { }
        }
        virtual public int ItemID
        {
            get { return PanelRequest.ItemID; }
            set { }
        }
        private PackageContext cntx = null;
        virtual public PackageContext Cntx
        {
            get
            {
                if (cntx == null) cntx = PackagesHelper.GetCachedPackageContext(PackageId);
                return cntx;
            }
        }
        public void BindMenu(MenuItemCollection items)
        {
            if (PackageId <= 0)
                return;
            // VPS Menu
            if (Cntx.Groups.ContainsKey(ResourceGroups.VPS2012) || Cntx.Groups.ContainsKey(ResourceGroups.Proxmox))
                PrepareVPS2012Menu(items);
        }
        private void PrepareVPS2012Menu(MenuItemCollection vpsItems)
        {
            bool isAdmin = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);
            // add items
            vpsItems.Add(CreateMenuItem("VPSHome", ""));
            if (((cntx.Quotas.ContainsKey(Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED].QuotaExhausted)
                || (cntx.Quotas.ContainsKey(Quotas.PROXMOX_EXTERNAL_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.PROXMOX_EXTERNAL_NETWORK_ENABLED].QuotaExhausted))
                || (PanelSecurity.PackageId == 1 && isAdmin))
                vpsItems.Add(CreateMenuItem("ExternalNetwork", "vdc_external_network"));
            if (isAdmin)
                vpsItems.Add(CreateMenuItem("ManagementNetwork", "vdc_management_network"));
            if ((cntx.Quotas.ContainsKey(Quotas.VPS2012_PRIVATE_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.VPS2012_PRIVATE_NETWORK_ENABLED].QuotaExhausted)
                || (cntx.Quotas.ContainsKey(Quotas.PROXMOX_PRIVATE_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.PROXMOX_PRIVATE_NETWORK_ENABLED].QuotaExhausted))
                vpsItems.Add(CreateMenuItem("PrivateNetwork", "vdc_private_network"));
            if ((cntx.Quotas.ContainsKey(Quotas.VPS2012_DMZ_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.VPS2012_DMZ_NETWORK_ENABLED].QuotaExhausted))
                vpsItems.Add(CreateMenuItem("DmzNetwork", "vdc_dmz_network"));
            vpsItems.Add(CreateMenuItem("AuditLog", "vdc_audit_log"));
        }
        private MenuItem CreateMenuItem(string text, string key)
        {
            return CreateMenuItem(text, key, null);
        }
        protected virtual MenuItem CreateMenuItem(string text, string key, string img)
        {
            MenuItem item = new MenuItem();
            item.Text = GetLocalizedString("Text." + text);
            var hostModule = GetAllControlsOfType<FuseCPModuleBase>(this.Page);
            if (hostModule.Count > 0)
            {
                item.NavigateUrl = hostModule.LastOrDefault().EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), key); // PortalUtils.EditUrl("ItemID", ItemID.ToString(), key, "SpaceID=" + PackageId);
            }
            //if (img == null)
            //    item.ImageUrl = PortalUtils.GetThemedIcon("Icons/tool_48.png");
            //else
            //    item.ImageUrl = PortalUtils.GetThemedIcon(img);
            return item;
        }
        public static List<T> GetAllControlsOfType<T>(Control parent) where T : Control
        {
            var result = new List<T>();
            foreach (Control control in parent.Controls)
            {
                if (control is T)
                {
                    result.Add((T)control);
                }
                if (control.HasControls())
                {
                    result.AddRange(GetAllControlsOfType<T>(control));
                }
            }
            return result;
        }
    }
}
