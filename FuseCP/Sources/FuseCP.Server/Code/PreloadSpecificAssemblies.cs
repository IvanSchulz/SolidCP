using System.Reflection;
using FuseCP.Providers.OS;

namespace FuseCP.Server
{
	public class PreloadSpecificAssemblies
	{
		public static void Init()
		{
			if (OSInfo.IsWindows && OSInfo.IsNetFX)
			{
				if (OSInfo.WindowsVersion >= WindowsVersion.WindowsServer2012)
				{
					Assembly.LoadFrom(Web.Services.Server.MapPath("~/bin/Providers/System.Management.Automation.dll"));
				} else
				{
					Assembly.LoadFrom(Web.Services.Server.MapPath("~/bin/ProvidersLegacy/System.Management.Automation.dll"));
				}
			}
		}
	}
}
