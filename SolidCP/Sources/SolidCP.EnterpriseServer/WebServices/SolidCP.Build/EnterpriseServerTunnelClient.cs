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
