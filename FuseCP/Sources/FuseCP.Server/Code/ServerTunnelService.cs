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

using System.Net;
using System.Threading.Tasks;
using FuseCP.Web.Services;
using FuseCP.Providers;
using FuseCP.Providers.OS;
using FuseCP.Providers.Virtualization;

[assembly:TunnelService(typeof(FuseCP.Server.ServerTunnelService))]

namespace FuseCP.Server
{
    public class ServerTunnelService: ServerTunnelServiceBase
    {
        public override string CryptoKey => Settings.CryptoKey;
        public override void Authenticate(string user, string password)
        {
            PasswordValidator.Validate(password);
        }
        public override async Task<TunnelSocket> GetPveVncWebSocketAsync(string vmId, VncCredentials credentials, RemoteServerSettings serverSettings, ServiceProviderSettings providerSettings)
        {
            using (var proxmox = new VirtualizationServerProxmox())
            {
                proxmox.ProviderSettings = providerSettings;
                proxmox.ServerSettings = serverSettings;
                return await proxmox.GetPveVncWebSocketAsync(vmId, credentials);
            }
        }
    }
}
