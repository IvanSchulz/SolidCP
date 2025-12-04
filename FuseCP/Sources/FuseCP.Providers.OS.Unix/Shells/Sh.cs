using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuseCP.Providers.OS;

namespace FuseCP.Providers
{
    public class Sh : Shell
	{
		public override string ShellExe => "sh";
	}
}
