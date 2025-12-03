using FuseCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.EnterpriseServer.Data;

public class DynamicLibraries
{
	private static void AddEnvironmentPaths(params IEnumerable<string> newpaths)
	{
		if (OSInfo.IsLinux)
		{
			var exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var path = new[] { Environment.GetEnvironmentVariable("LD_LIBRARY_PATH") ?? string.Empty };
			newpaths = newpaths.Select(path => new DirectoryInfo(path.Replace("~", exepath)).FullName);
			var newpath = string.Join(Path.PathSeparator.ToString(), path.Concat(newpaths));

			Environment.SetEnvironmentVariable("LD_LIBRARY_PATH", newpath);
		} else if (OSInfo.IsMac)
		{
			var exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var path = new[] { Environment.GetEnvironmentVariable("DYLD_LIBRARY_PATH") ?? string.Empty };
			newpaths = newpaths.Select(path => new DirectoryInfo(path.Replace("~", exepath)).FullName);
			var newpath = string.Join(Path.PathSeparator.ToString(), path.Concat(newpaths));

			Environment.SetEnvironmentVariable("DYLD_LIBRARY_PATH", newpath);
		}
	}
	public static bool IsLinuxMusl
	{
		get
		{
			if (!OSInfo.IsLinux) return false;

			var info = new ProcessStartInfo("ldd");
			info.Arguments = "/bin/ls";
			info.RedirectStandardOutput = true;
			info.UseShellExecute = false;
			var p = Process.Start(info);
			return p.StandardOutput.ReadToEnd().Contains("musl");
		}
	}
	public static nint LoadSQLiteNativeLibrary()
	{
		nint handle = IntPtr.Zero;

#if NETCOREAPP
		string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		// Determine platform
		string libName = OSInfo.IsMac ? "libe_sqlite3.dylib"
			: OSInfo.IsLinux ? "libe_sqlite3.so"
			: "e_sqlite3.dll";

		// Try root output first
		string path = Path.Combine(baseDir, libName);

		// If not found, try runtimes subfolders
		if (!File.Exists(path))
		{
			string arch = RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant();
			string rid = OSInfo.IsMac ? "osx" : "linux";
			path = Path.Combine(baseDir, $"runtimes/{rid}-{arch}/native/{libName}");
		}

		if (File.Exists(path))
		{
			handle = NativeLibrary.Load(path);
			Console.WriteLine($"Loaded SQLite native library from: {path}");
		}
		else
		{
			Console.Error.WriteLine($"Could not find native SQLite library: {path}");
		}
#endif
		return handle;
	}
	static IntPtr? dll = null; 
	private static IntPtr ResolveSQLiteNative(Assembly assembly, string libraryName)
	{
		if (libraryName != "e_sqlite3")
			return IntPtr.Zero; // fallback for other libraries

		return dll ??= LoadSQLiteNativeLibrary();
	}

	public static void Init()
	{
#if NETCOREAPP
		var loadContext = System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
		loadContext.ResolvingUnmanagedDll += ResolveSQLiteNative;
#endif
	}
}
