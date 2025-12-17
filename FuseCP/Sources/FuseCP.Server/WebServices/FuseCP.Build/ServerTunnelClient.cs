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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuseCP.Providers;
using FuseCP.Providers.OS;
using FuseCP.Providers.Virtualization;

[assembly:TunnelClient(typeof(FuseCP.Server.Client.ServerTunnelClient))]

namespace FuseCP.Server.Client
{
    public class ServerTunnelClient: ServerTunnelClientBase
    {
        public virtual string GetCryptoKey()
        {
            if (string.IsNullOrEmpty(ServerUrl)) throw new ArgumentException("ServerUrl must be set.");

            var serviceProviderClient = new ServiceProvider();
            serviceProviderClient.Url = ServerUrl;
            serviceProviderClient.Credentials.UserName = "";
            serviceProviderClient.Credentials.Password = Password;
            return serviceProviderClient.GetCryptoKey();
        }

        public override string CryptoKey => GetCryptoKey();

        public override async Task<TunnelSocket> GetPveVncWebSocketAsync(string vmId, VncCredentials credentials, RemoteServerSettings serverSettings, ServiceProviderSettings providerSettings)
        {
            return await GetTunnel(nameof(GetPveVncWebSocketAsync), vmId, credentials, serverSettings, providerSettings);
        }
    }
}
