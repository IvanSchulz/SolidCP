namespace AnyCPUAppHost;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml.Linq;

class Program
{
	// P/Invoke constants and structs
	private const int STARTF_USESTDHANDLES = 0x00000100;
	private const uint CREATE_NO_WINDOW = 0x08000000;
	private const int HANDLE_FLAG_INHERIT = 0x00000001;
	private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct STARTUPINFO
	{
		public int cb;
		public string lpReserved;
		public string lpDesktop;
		public string lpTitle;
		public int dwX, dwY, dwXSize, dwYSize, dwXCountChars, dwYCountChars, dwFillAttribute;
		public int dwFlags;
		public short wShowWindow;
		public short cbReserved2;
		public IntPtr lpReserved2;
		public IntPtr hStdInput, hStdOutput, hStdError;
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct PROCESS_INFORMATION
	{
		public IntPtr hProcess, hThread;
		public uint dwProcessId, dwThreadId;
	}

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr GetStdHandle(int nStdHandle);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool SetHandleInformation(IntPtr hObject, int dwMask, int dwFlags);

	[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
	private static extern bool CreateProcessW(
		string lpApplicationName,
		string lpCommandLine,
		IntPtr lpProcessAttributes,
		IntPtr lpThreadAttributes,
		bool bInheritHandles,
		uint dwCreationFlags,
		IntPtr lpEnvironment,
		string lpCurrentDirectory,
		[In] ref STARTUPINFO lpStartupInfo,
		out PROCESS_INFORMATION lpProcessInformation);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool CloseHandle(IntPtr hObject);

	[DllImport("kernel32.dll", SetLastError = true)]
	static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

	[DllImport("kernel32.dll", SetLastError = true)]
	static extern bool GetExitCodeProcess(IntPtr hProcess, out uint lpExitCode);

	const uint INFINITE = 0xFFFFFFFF;

	// STD handles
	private const int STD_INPUT_HANDLE = -10;
	private const int STD_OUTPUT_HANDLE = -11;
	private const int STD_ERROR_HANDLE = -12;

	static async Task<int> Main()
	{
		await DotnetInstaller.Install();

		string args = System.Environment.CommandLine;
		args = Regex.Replace(args, @"^(""[^""]+""|[^\s]+)\s*", ""); // remove own exe path
		string dll = Path.ChangeExtension(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath, ".dll");
		string childCmd = $"dotnet \"{dll}\" {args}";

		// Get std handles (these are handles owned by dotnet.exe)
		IntPtr hStdIn = GetStdHandle(STD_INPUT_HANDLE);
		IntPtr hStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
		IntPtr hStdErr = GetStdHandle(STD_ERROR_HANDLE);

		if (hStdOut == IntPtr.Zero || hStdOut == INVALID_HANDLE_VALUE)
		{
			Console.Error.WriteLine("No valid stdout handle found. Are you running inside a console?");
			return 1;
		}

		// Make sure handles are inheritable
		bool inOK = SetHandleInformation(hStdIn, HANDLE_FLAG_INHERIT, HANDLE_FLAG_INHERIT);
		bool outOK = SetHandleInformation(hStdOut, HANDLE_FLAG_INHERIT, HANDLE_FLAG_INHERIT);
		bool errOK = SetHandleInformation(hStdErr, HANDLE_FLAG_INHERIT, HANDLE_FLAG_INHERIT);

		if (!outOK || !errOK || !inOK)
		{
			int err = Marshal.GetLastWin32Error();
			Console.Error.WriteLine($"Failed to mark std handles inheritable. Win32Err={err}");
			// continue: we might still try but likely will fail
		}

		STARTUPINFO si = new STARTUPINFO();
		si.cb = Marshal.SizeOf(si);
		si.dwFlags = STARTF_USESTDHANDLES;
		si.hStdInput = hStdIn;
		si.hStdOutput = hStdOut;
		si.hStdError = hStdErr;

		PROCESS_INFORMATION pi;

		// CreateProcess expects the commandline as a single string; applicationName can be null
		string cmdLine = childCmd;

		bool created = CreateProcessW(
			null,
			cmdLine,
			IntPtr.Zero,
			IntPtr.Zero,
			true,                // inherit handles
			0,                   // creation flags (0 -> normal)
			IntPtr.Zero,
			null,
			ref si,
			out pi
		);

		// Restore inherit flags (cleanup) - remove inherit bit
		SetHandleInformation(hStdIn, HANDLE_FLAG_INHERIT, 0);
		SetHandleInformation(hStdOut, HANDLE_FLAG_INHERIT, 0);
		SetHandleInformation(hStdErr, HANDLE_FLAG_INHERIT, 0);

		if (!created)
		{
			int err = Marshal.GetLastWin32Error();
			Console.Error.WriteLine($"CreateProcessW failed: Win32Err={err}");
			return 2;
		}

		// Wait until process exits
		WaitForSingleObject(pi.hProcess, INFINITE);

		// Retrieve exit code
		uint exitCode = 0;
		if (!GetExitCodeProcess(pi.hProcess, out exitCode))
		{
			int err = Marshal.GetLastWin32Error();
			Console.Error.WriteLine($"GetExitCodeProcess failed: {err}");
			exitCode = 0xFFFFFFFF;
		}

		//Console.WriteLine($"Child exited with code {exitCode}");

		return (int)exitCode;
	}
}