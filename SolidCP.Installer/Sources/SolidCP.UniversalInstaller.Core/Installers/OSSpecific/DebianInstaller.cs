using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.UniversalInstaller
{
	public class DebianInstaller : UnixInstaller
	{

		Providers.OS.Installer apt = null;
		public Providers.OS.Installer Apt
		{
			get
			{
				if (apt == null)
				{
					apt = OSInfo.Unix.Apt;
					apt.CheckInstallerInstalled();
				}
				return apt;
			}
		}

		public override void InstallNet10Runtime()
		{
			if (CheckNet10RuntimeInstalled()) return;

			if (OSInfo.OSVersion.Major < 11) throw new PlatformNotSupportedException("Cannot install NET 10 on Debian below version 11.");

			Info("Installing .NET 10 Runtime...");

			if (!HasDotnet)
			{
				/*	// install dotnet from microsoft
				var tmp = DownloadFile($"https://packages.microsoft.com/config/debian/{OSInfo.OSVersion.Major}/packages-microsoft-prod.deb");
				Shell.Exec($"dpkg -i \"{tmp}\"");
				File.Delete(tmp);
				Apt.Update(); */
				// do not install dotnet from microsoft
				var text = @"Package: dotnet* aspnet* netstandard*
Pin: origin ""packages.microsoft.com""
Pin-Priority: -10
";
				var file = "/etc/apt/preferences";
				if (!File.Exists(file)) File.WriteAllText(file, text);
				else File.AppendAllText(file, Environment.NewLine + text);
			}

			Apt.Install("aspnetcore-runtime-10.0 netcore-runtime-10.0");

			Net10RuntimeInstalled = true;

			InstallLog("Installed .NET 10 Runtime.");

			ResetHasDotnet();
		}

		public override void RemoveNet10NetRuntime()
		{
			Apt.Remove("netcore-runtime-10.0");
			ResetHasDotnet();
		}
		public override void RemoveNet10AspRuntime()
		{
			Apt.Remove("aspnetcore-runtime-10.0");
			ResetHasDotnet();
		}
	}
}
