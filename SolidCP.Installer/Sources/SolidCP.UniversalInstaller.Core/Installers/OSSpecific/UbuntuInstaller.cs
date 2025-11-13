using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SolidCP.UniversalInstaller
{
	public class UbuntuInstaller : DebianInstaller
	{

		public override void InstallNet10Runtime()
		{
			if (CheckNet10RuntimeInstalled()) return;

			if (OSInfo.OSVersion.Major < 20) throw new PlatformNotSupportedException("Cannot install NET 10 on Ubuntu below version 20.4. Please install NET 10 runtime manually.");

			bool installFromMicrosoftFeed = false;

			if (!HasDotnet)
			{
				if (OSInfo.Architecture == Architecture.Arm64)
				{
					if (OSInfo.OSVersion.Major < 23) throw new PlatformNotSupportedException("NET 10 not supported on this platform. Arm64 is only supported on Ubuntu 23 and above. Please install NET 10 runtime manually.");
					// install from ubuntu
					installFromMicrosoftFeed = false;
				}
				else if (OSInfo.Architecture == Architecture.X86) throw new PlatformNotSupportedException("NET 10 not supported on this platform. Please install NET 10 runtime manually.");
				else if (OSInfo.Architecture == Architecture.Arm ||
					OSInfo.OSVersion.Major >= 24) installFromMicrosoftFeed = false;
				else installFromMicrosoftFeed = true;
			}
			else installFromMicrosoftFeed = false;

			Info("Installing .NET 10 Runtime...");

			if (installFromMicrosoftFeed)
			{
				// install dotnet from microsoft
				Apt.Install("wget");
				Shell.ExecScript(@"
# Get Ubuntu version
declare repo_version=$(if command -v lsb_release &> /dev/null; then lsb_release -r -s; else grep -oP '(?<=^VERSION_ID=).+' /etc/os-release | tr -d '""'; fi)

# Download Microsoft signing key and repository
wget https://packages.microsoft.com/config/ubuntu/$repo_version/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

# Install Microsoft signing key and repository
dpkg -i packages-microsoft-prod.deb

# Clean up
rm packages-microsoft-prod.deb
");
				Apt.Update();
			}

			Apt.Install("aspnetcore-runtime-10.0 netcore-runtime-10.0");

			Net10RuntimeInstalled = true;

			InstallLog("Installed .NET 10 Runtime.");

			ResetHasDotnet();
		}
	}
}
