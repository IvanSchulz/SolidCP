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
using System.Threading.Tasks;
using FuseCP.Providers.OS;
using FuseCP.Providers.Virtualization;

[assembly:TunnelClient(typeof(FuseCP.EnterpriseServer.Client.EnterpriseServerTunnelClient))]

namespace FuseCP.EnterpriseServer.Client
{
    public class EnterpriseServerTunnelClient : EnterpriseServerTunnelClientBase
    {
        public string GetCryptoKey()
        {
            var system = new esSystem() { Url = ServerUrl };
            system.Credentials.UserName = Username;
            system.Credentials.Password = Password;
            return system.GetCryptoKey();
        }
        public override string CryptoKey => GetCryptoKey();
        public override async Task<TunnelSocket> GetPveVncWebSocketAsync(int serviceItemId, VncCredentials credentials)
        {
            return await GetTunnel(nameof(GetPveVncWebSocketAsync), serviceItemId, credentials);
        }
    }
}
