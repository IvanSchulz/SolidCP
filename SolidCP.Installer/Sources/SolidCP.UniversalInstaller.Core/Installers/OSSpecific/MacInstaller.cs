using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace SolidCP.UniversalInstaller;

public class MacInstaller : UnixInstaller
{
	Brew Brew => (Brew)OSInfo.Unix.Brew;
	public override string UnixAppRootPath => "/var/bin";
	public override void InstallNet10Runtime()
	{
		if (CheckNet10RuntimeInstalled()) return;

		string tmp = null;

		if (OSInfo.Architecture == Architecture.X64) tmp = DownloadFile("https://builds.dotnet.microsoft.com/dotnet/Sdk/10.0.0/dotnet-sdk-10.0.0-osx-x64.pkg");
		else if (OSInfo.Architecture == Architecture.Arm64) tmp = DownloadFile("https://builds.dotnet.microsoft.com/dotnet/Sdk/10.0.0/dotnet-sdk-10.0.0-osx-arm64.pkg");
		else throw new PlatformNotSupportedException("Only x64 and Arm64 architectures supported.");
	
		Info("Installing .NET 10 Runtime...");

		Shell.Exec($"installer -pkg \"{tmp}\" -target /");
		Shell.Exec("brew update");
		//Shell.Exec("brew install mono-libgdiplus");

		Net10RuntimeInstalled = true;

		InstallLog("Installed .NET 10 Runtime.");

		ResetHasDotnet();
	}

	public override void RemoveNet10AspRuntime()
	{
		//throw new NotImplementedException();
	}
	public override void RemoveNet10NetRuntime()
	{
		//throw new NotImplementedException();
	}


    public virtual void OpenAppFirewall(string app)
    {
        Shell.Exec($"sudo /usr/libexec/ApplicationFirewall/socketfilterfw --add {app}");
        Shell.Exec($"sudo /usr/libexec/ApplicationFirewall/socketfilterfw --unblockapp {app}");
    }
    public virtual void CloseAppFirewall(string app)
    {
        Shell.Exec($"sudo /usr/libexec/ApplicationFirewall/socketfilterfw --blockapp {app}");
        //Shell.Exec($"sudo /usr/libexec/ApplicationFirewall/socketfilterfw --remove {app}");
    }

    public override void OpenFirewall(int port) { }
    public override void RemoveFirewallRule(int port) { }
    public override void InstallAspNetCoreSharedServer()
    {
        base.InstallAspNetCoreSharedServer();

        OpenAppFirewall("/var/bin/AspNetCoreSharedServer");
    }

    public override void AddUnixGroup(string group)
    {
        Shell.Exec($"dscl . create /Groups/{group}");
        Shell.Exec($"dscl . create /Groups/{group} RealName \"{group}\"");
        Shell.Exec($"dscl . create /Groups/{group} Password \"*\"");
        // Get free PrimaryGroupID
        var output = Shell.Exec($"dscl . list /Groups PrimaryGroupID").Output().Result;
        var maxid = Regex.Matches(output, @"(?<=^\s*[^ \t]+\s+)[0-9]+", RegexOptions.Multiline)
            .OfType<Match>()
            .Select(m =>
            {
                if (int.TryParse(m.Value, out int v)) return v;
                return -1;
            })
            .Max();
        Shell.Exec($"dscl . create /Groups/{group} PrimaryGroupID {maxid + 100}");
    }
    public override void AddUnixUser(string user, string group, string password)
    {
        Shell.Exec($"sysadminctl -addUser {user} -fullName \"{user}\" -password \"{password}\"");
        var groups = group.Split(',');
        foreach (var g in groups)
        {
            Shell.Exec($"dscl . -append /Groups/{g.Trim()} GroupMembership {user}");
        }
    }
}
