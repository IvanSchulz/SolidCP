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
