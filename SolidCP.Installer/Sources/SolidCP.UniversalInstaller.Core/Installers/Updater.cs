using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FuseCP.UniversalInstaller;  

public class Updater
{
	public Action<long, long> Progress = null;
	public Updater(Action<long, long> progress = null)
	{
		var url = GetCommandLineArgument("url");
		Installer.Current.Settings.Installer.WebServiceUrl = url;
	}

	public void Update(Action<long, long> progress)
	{
		try
		{
			Progress = progress;
			string url = GetCommandLineArgument("url");
			string targetFile = GetCommandLineArgument("target");
			//string fileToDownload = GetCommandLineArgument("file");
			string proxyServer = GetCommandLineArgument("proxy");
			string user = GetCommandLineArgument("user");
			string password = GetCommandLineArgument("password");

			if (!string.IsNullOrEmpty(proxyServer))
			{
				Installer.Current.Settings.Installer.Proxy = new ProxySettings();
				Installer.Current.Settings.Installer.Proxy.Address = proxyServer;
				if (!String.IsNullOrEmpty(user))
				{
					Installer.Current.Settings.Installer.Proxy.Username = user;
					Installer.Current.Settings.Installer.Proxy.Password = password;
				}
			}

			Installer.Current.Settings.Installer.WebServiceUrl = url;

			string destinationFile = Path.GetTempFileName();
			string baseDir = Path.GetDirectoryName(targetFile);
			string tempDir = Path.Combine(baseDir, "Temp");

			DownloadAndUnzipFile(new RemoteFile(url), destinationFile, tempDir).Wait();

			CopyDirectory(tempDir, baseDir, true);

			FileUtils.DeleteFile(destinationFile);
			Directory.Delete(tempDir, true);

			ProcessStartInfo info = new ProcessStartInfo();
			var isExe = Path.GetExtension(targetFile).Equals(".exe", StringComparison.OrdinalIgnoreCase);
			var winconsole = UI.Current.IsConsole && Providers.OS.OSInfo.IsWindows;
			var ui = $"-ui={UI.Current.GetType().Name.Replace("UI", "").ToLower()}";
			if (isExe)
			{
				info.FileName = targetFile;
				info.Arguments = $"{ui} nocheck";
			}
			else
			{
				info.FileName = Providers.OS.Shell.Standard.Find(Providers.OS.OSInfo.IsWindows ? "dotnet.exe" : "dotnet");
				info.Arguments = $"\"{targetFile}\" {ui} nocheck";
			}
			info.UseShellExecute = winconsole;
			info.CreateNoWindow = !winconsole;

			//info.WindowStyle = ProcessWindowStyle.Normal;
			Process process = Process.Start(info);
			//activate window
			if (Providers.OS.OSInfo.IsWindows && process.Handle != IntPtr.Zero)
			{
				User32.SetForegroundWindow(process.Handle);
				/*if (User32.IsIconic(process.Handle))
				{
					User32.ShowWindowAsync(process.Handle, User32.SW_RESTORE);
				}
				else
				{
					User32.ShowWindowAsync(process.Handle, User32.SW_SHOWNORMAL);
				}*/
			}

			Installer.Current.Exit();
		}
		catch (Exception ex)
		{
			if (Utils.IsThreadAbortException(ex))
				return;
			string message = ex.ToString();

			return;
		}
	}

	private void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
	{
		// Get information about the source directory
		var dir = new DirectoryInfo(sourceDir);

		// Check if the source directory exists
		if (!dir.Exists)
			throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

		// Cache directories before we start copying
		DirectoryInfo[] dirs = dir.GetDirectories();

		// Create the destination directory
		if (!Directory.Exists(destinationDir)) Directory.CreateDirectory(destinationDir);

		// Get the files in the source directory and copy to the destination directory
		foreach (FileInfo file in dir.GetFiles())
		{
			string targetFilePath = Path.Combine(destinationDir, file.Name);
			file.CopyTo(targetFilePath, true);
		}

		// If recursive and copying subdirectories, recursively call this method
		if (recursive)
		{
			foreach (DirectoryInfo subDir in dirs)
			{
				string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
				CopyDirectory(subDir.FullName, newDestinationDir, true);
			}
		}
	}

	public Releases Releases => Installer.Current.Releases;

	private async Task DownloadAndUnzipFile(RemoteFile file, string destinationFile, string destPath)
	{
		try
		{
			await Releases.GetFileAndUnzipAsync(file, destinationFile, destPath, null, Progress);
		}
		catch (Exception ex)
		{
			if (Utils.IsThreadAbortException(ex))
				return;

			throw;
		}
	}

	private static string GetCommandLineArgument(string argName)
	{
		argName = "-" + argName + ":";
		string[] args = Environment.GetCommandLineArgs();
		for (int i = 1; i < args.Length; i++)
		{
			string arg = args[i];
			if (arg.StartsWith(argName))
			{
				string text = arg.Substring(argName.Length);
				if (text.StartsWith("\"") && text.EndsWith("\""))
				{
					text = text.Substring(1, text.Length - 2);
				}
				return text;
			}
		}
		return string.Empty;
	}

}
