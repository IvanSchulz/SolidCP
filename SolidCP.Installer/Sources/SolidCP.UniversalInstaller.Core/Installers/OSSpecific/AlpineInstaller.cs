using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SolidCP.UniversalInstaller
{
    public class AlpineInstaller : UnixInstaller
    {
        public override void InstallNet10Runtime()
        {
            if (CheckNet10RuntimeInstalled()) return;

			Info("Installing .NET 10 Runtime...");

			OSInstaller.Install("dotnet10-runtime, aspnetcore10-runtime");
		}

		public override void RemoveNet10AspRuntime()
		{
			OSInstaller.Remove("aspnetcore10-runtime");
		}
		public override void RemoveNet10NetRuntime()
		{
			OSInstaller.Remove("dotnet10-runtime");
		}
	}
}
