using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Windows.Forms;

namespace AnyCPUAppHost;

public class DotnetInstaller
{
	public static readonly string[] Runtimes =	{
		"Microsoft.AspNetCore.App",
		"Microsoft.NETCore.App",
		"Microsoft.WindowsDesktop.App"
	};
	public static readonly Version Version = new Version(8, 0);
	public const bool WinError = true;

	public static bool IsRemoteConsole =>
		!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PSRemotingProtocolVersion")) ||
		!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PSSenderInfo")) ||
		!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WSManStackVersion")) ||
		!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SSH_CLIENT")) ||
		!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SSH_TTY"));

	public static void Error(string msg)
	{
		if (!WinError || IsRemoteConsole) Console.Error.WriteLine(msg);
		else MessageBox.Show(msg);
	}

	public static Shell Shell => Shell.Standard;
	public static (string Name, Version Versoin)[] DotnetRuntimes()
	{
		var output = Shell.Exec("dotnet --list-runtimes").Output().Result ?? "";
		return Regex.Matches(output, @"^\s*(?<name>[^\s]+)\s+(?<version>[^\s]+)\s+\[(?<location>.+?)\]\s*$", RegexOptions.Multiline)
			.OfType<Match>()
			.Select(m =>
			{
				Version.TryParse(m.Groups["version"].Value, out Version v);
				return (m.Groups["name"].Value, v);
			})
			.ToArray();
	}

	public static bool IsDotnetInstalled(Version minVersion, params string[] names)
	{
		var runtimes = DotnetRuntimes();
		return names.All(name => runtimes.Any(r => r.Name == name && r.Versoin >= minVersion));
	}

	private static async Task DownloadAndInstall(string url)
	{
		const string args = "/install /quiet /norestart";
		var tempFile = Path.GetTempFileName() + ".exe";
		try
		{
			Console.WriteLine($"Downloading {url}...");
			using (HttpClient client = new HttpClient())
			{
				using (var src = await client.GetStreamAsync(url))
				using (var dest = File.Create(tempFile))
						await src.CopyToAsync(dest);
			}
			Console.WriteLine("Installing...");
			var installProcess = Shell.ExecAsync($"\"{tempFile}\" {args}");
			var exitCode = await installProcess.ExitCode();
			if (exitCode != 0)
			{
				Error($"Installation failed with exit code {exitCode}");
			}
			Console.WriteLine("Installation completed successfully.");
		}
		finally
		{
			if (File.Exists(tempFile)) File.Delete(tempFile);
		}
	}

	public static async Task InstallFromWinGetAsync(string packageName)
	{
		if (Shell.Find("winget") == null)
		{
			Error("Winget is not available on this system.");
			return;
		}
		Console.WriteLine("Installing via winget...");
		var wingetArgs = $"install {packageName} -e --silent";
		var wingetProcess = Shell.ExecAsync($"winget {wingetArgs}");
		var exitCode = await wingetProcess.ExitCode();
		if (exitCode != 0)
		{
			if (exitCode == -1978335189)
			{
				Console.WriteLine("Dotnet already installed");
			}
			else
			{
				Error($"Winget installation failed with exit code {exitCode}");
			}
		}
		else
		{
			Console.WriteLine("Installation completed successfully via winget.");
		}
	}
	public static async Task InstallDotnet(Version version)
	{
		if (Shell.Find("winget") != null)
		{
			if (Runtimes.Contains("Microsoft.WindowsDesktop.App")) await InstallFromWinGetAsync($"Microsoft.DotNet.DesktopRuntime.{version.Major}");
			if (Runtimes.Contains("Microsoft.NETCore.App")) await InstallFromWinGetAsync($"Microsoft.DotNet.Runtime.{version.Major}");
			if (Runtimes.Contains("Microsoft.AspNetCore.App")) await InstallFromWinGetAsync($"Microsoft.DotNet.AspNetCore.{version.Major}");
		}
		else
		{
			string url;
			var arch = RuntimeInformation.ProcessArchitecture;
			if (arch != Architecture.X64 && arch != Architecture.X86 && arch != Architecture.Arm64)
			{
				Error($"Dotnet installation is not supported on {arch} architecture.");
			}

			var latest = version.Major switch
			{
				8 => "8.0.21",
				9 => "9.0.10",
				10 => "10.0.0",
				_ => ""
			};
			if (Runtimes.Contains("Microsoft.WindowsDesktop.App")) await DownloadAndInstall($"https://builds.dotnet.microsoft.com/dotnet/WindowsDesktop/{latest}/windowsdesktop-runtime-{latest}-win-{arch.ToString().ToLowerInvariant()}.exe");
			if (Runtimes.Contains("Microsoft.NETCore.App")) await DownloadAndInstall($"https://builds.dotnet.microsoft.com/dotnet/Runtime/{latest}/dotnet-runtime-{latest}-win-{arch.ToString().ToLowerInvariant()}.exe");
			if (Runtimes.Contains("Microsoft.AspNetCore.App")) await DownloadAndInstall($"https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/{latest}/aspnetcore-runtime-{latest}-win-{arch.ToString().ToLowerInvariant()}.exe");
		}
	}

	public static async Task Install()
	{
		if (!IsDotnetInstalled(Version, Runtimes))
		{
			Console.WriteLine(".NET runtimes not found. Installing...");
			await DotnetInstaller.InstallDotnet(DotnetInstaller.Version);
		}
	}
}