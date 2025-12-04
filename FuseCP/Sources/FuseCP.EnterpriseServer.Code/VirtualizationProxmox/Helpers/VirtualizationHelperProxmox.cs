using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuseCP.Server.Client;
//using FuseCP.Providers.VirtualizationProxmox;

namespace FuseCP.EnterpriseServer.Code.VirtualizationProxmox
{
    public class VirtualizationHelperProxmox: ControllerBase
    {
        public VirtualizationHelperProxmox(ControllerBase provider) : base(provider) { }

        public VirtualizationServerProxmox GetVirtualizationProxy(int serviceId)
        {
            VirtualizationServerProxmox ws = new VirtualizationServerProxmox();
            ServiceProviderProxy.Init(ws, serviceId);
            return ws;
        }
    }
}
