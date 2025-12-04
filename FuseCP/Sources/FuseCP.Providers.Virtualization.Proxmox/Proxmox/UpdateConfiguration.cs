using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.Providers.Virtualization.Proxmox
{
    public class UpdateConfiguration
    {
        public int cores { get; set; }
        public int memory{ get; set; }
    }
}
