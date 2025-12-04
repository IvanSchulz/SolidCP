using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.Providers.Virtualization
{
    public class VirtualMachineData
    {
        public VirtualMachine VM { get; set; }
        public PSObject RawObject { get; set; }
    }
}
