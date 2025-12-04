using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuseCP.Providers.Virtualization
{
    public class VirtualMachineIPAddress
    {
        public string IPAddress { get; set; }
        public string SubnetMask { get; set; }
        public string DefaultGateway { get; set; }
        public string Comments { get; set; }
        public int VLAN { get; set; }
    }
}
