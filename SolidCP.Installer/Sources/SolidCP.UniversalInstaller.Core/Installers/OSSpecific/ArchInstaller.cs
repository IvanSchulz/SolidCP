using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SolidCP.UniversalInstaller
{
	public class ArchInstaller : UnixInstaller
	{

		public override void InstallNet10Runtime()
		{
			if (CheckNet10RuntimeInstalled()) return;

			Info("Installing .NET 10 Runtime...");

			OSInstaller.Install("aspnetcore-runtime-10.0;dotnet-runtime-10.0");

			Net10RuntimeInstalled = true;

			InstallLog("Installed .NET 10 Runtime.");

			ResetHasDotnet();
		}

		public override void RemoveNet10NetRuntime()
		{
			OSInstaller.Remove("dotnet-runtime-8.0");

			ResetHasDotnet();
		}
		public override void RemoveNet10AspRuntime()
		{
			OSInstaller.Remove("aspnetcore-runtime-8.0");

			ResetHasDotnet();
		}
	}
}

