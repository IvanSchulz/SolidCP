// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FuseCP.Setup
{
	public sealed class OS
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct OSVERSIONINFO
		{
			public Int32 dwOSVersionInfoSize;
			public Int32 dwMajorVersion;
			public Int32 dwMinorVersion;
			public Int32 dwBuildNumber;
			public Int32 dwPlatformID;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string szCSDVersion;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct OSVERSIONINFOEX
		{
			public Int32 dwOSVersionInfoSize;
			public Int32 dwMajorVersion;
			public Int32 dwMinorVersion;
			public Int32 dwBuildNumber;
			public Int32 dwPlatformID;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string szCSDVersion;
			public short wServicePackMajor;
			public short wServicePackMinor;
			public short wSuiteMask;
			public byte wProductType;
			public byte wReserved;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SYSTEM_INFO
		{
			public Int32 dwOemID;
			public Int32 dwPageSize;
			public Int32 wProcessorArchitecture;
			public Int32 lpMinimumApplicationAddress;
			public Int32 lpMaximumApplicationAddress;
			public Int32 dwActiveProcessorMask;
			public Int32 dwNumberOrfProcessors;
			public Int32 dwProcessorType;
			public Int32 dwAllocationGranularity;
			public Int32 dwReserved;
		}
		public enum WinSuiteMask : int
		{
			VER_SUITE_SMALLBUSINESS = 1,
			VER_SUITE_ENTERPRISE = 2,
			VER_SUITE_BACKOFFICE = 4,
			VER_SUITE_COMMUNICATIONS = 8,
			VER_SUITE_TERMINAL = 16,
			VER_SUITE_SMALLBUSINESS_RESTRICTED = 32,
			VER_SUITE_EMBEDDEDNT = 64,
			VER_SUITE_DATACENTER = 128,
			VER_SUITE_SINGLEUSERTS = 256,
			VER_SUITE_PERSONAL = 512,
			VER_SUITE_BLADE = 1024,
			VER_SUITE_STORAGE_SERVER = 8192,
			VER_SUITE_COMPUTE_SERVER = 16384
		}
		public enum WinPlatform : byte
		{
			VER_NT_WORKSTATION = 1,
			VER_NT_DOMAIN_CONTROLLER = 2,
			VER_NT_SERVER = 3
		}
		public enum OSMajorVersion : byte
		{
			VER_OS_NT4 = 4,
			VER_OS_2K_XP_2K3 = 5,
			VER_OS_VISTA_LONGHORN = 6
		}

		private const Int32 SM_SERVERR2 = 89;
		private const Int32 SM_MEDIACENTER = 87;
		private const Int32 SM_TABLETPC = 86;

		[DllImport("kernel32")]
		private static extern int GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);
		[DllImport("user32")]
		private static extern int GetSystemMetrics(int nIndex);
		[DllImport("kernel32", EntryPoint = "GetVersion")]
		private static extern int GetVersionAdv(ref OSVERSIONINFO lpVersionInformation);
		[DllImport("kernel32")]
		private static extern int GetVersionEx(ref OSVERSIONINFOEX lpVersionInformation);

	
		/*public static string GetVersionEx()
		{
			OSVERSIONINFO osvi = new OSVERSIONINFO();
			OSVERSIONINFOEX xosvi = new OSVERSIONINFOEX();
			Int32 iRet = 0;
			string strDetails = string.Empty;
			osvi.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFO));
			xosvi.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));
			try
			{
				iRet = (int)System.Environment.OSVersion.Platform;
				if (iRet == 1)
				{
					iRet = GetVersionAdv(ref osvi);
					strDetails = Environment.NewLine + "Release: " + osvi.dwMajorVersion + "." + osvi.dwMinorVersion + "." + osvi.dwBuildNumber + Environment.NewLine + osvi.szCSDVersion;
					if (Len(osvi) == 0)
					{
						return "Windows 95" + strDetails;
					}
					else if (Len(osvi) == 10)
					{
						return "Windows 98" + strDetails;
					}
					else if (Len(osvi) == 9)
					{
						return "Windows ME" + strDetails;
					}
				}
				else
				{
					iRet = GetVersionEx(xosvi);
					strDetails = Environment.NewLine + "Release: " + xosvi.dwMajorVersion + "." + xosvi.dwMinorVersion + "." + xosvi.dwBuildNumber + Environment.NewLine + xosvi.szCSDVersion + " (" + xosvi.wServicePackMajor + "." + xosvi.wServicePackMinor + ")";
					if (xosvi.dwMajorVersion == (byte)OSMajorVersion.VER_OS_NT4)
					{
						return "Windows NT 4" + strDetails;
					}
					else if (xosvi.dwMajorVersion == OSMajorVersion.VER_OS_2K_XP_2K3)
					{
						if (xosvi.dwMinorVersion == 0)
						{
							if (xosvi.wProductType == WinPlatform.VER_NT_WORKSTATION)
							{
								return "Windows 2000 Pro" + strDetails;
							}
							else if (xosvi.wProductType == WinPlatform.VER_NT_SERVER)
							{
								if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_DATACENTER) == WinSuiteMask.VER_SUITE_DATACENTER)
								{
									return "Windows 2000 Datacenter Server" + strDetails;
								}
								else if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_ENTERPRISE) == WinSuiteMask.VER_SUITE_ENTERPRISE)
								{
									return "Windows 2000 Advanced Server" + strDetails;
								}
								else if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_SMALLBUSINESS) == WinSuiteMask.VER_SUITE_SMALLBUSINESS)
								{
									return "Windows 2000 Small Business Server" + strDetails;
								}
								else
								{
									return "Windows 2000 Server" + strDetails;
								}
							}
							else if (xosvi.wProductType == WinPlatform.VER_NT_DOMAIN_CONTROLLER)
							{
								if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_DATACENTER) == WinSuiteMask.VER_SUITE_DATACENTER)
								{
									return "Windows 2000 Datacenter Server Domain Controller" + strDetails;
								}
								else if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_ENTERPRISE) == WinSuiteMask.VER_SUITE_ENTERPRISE)
								{
									return "Windows 2000 Advanced Server Domain Controller" + strDetails;
								}
								else if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_SMALLBUSINESS) == WinSuiteMask.VER_SUITE_SMALLBUSINESS)
								{
									return "Windows 2000 Small Business Server Domain Controller" + strDetails;
								}
								else
								{
									return "Windows 2000 Server Domain Controller" + strDetails;
								}
							}
						}
						else if (xosvi.dwMinorVersion == 1)
						{
							if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_PERSONAL) == WinSuiteMask.VER_SUITE_PERSONAL)
							{
								return "Windows XP Home Edition" + strDetails;
							}
							else
							{
								return "Windows XP Professional Edition" + strDetails;
							}
						}
						else if (xosvi.dwMinorVersion == 2)
						{
							if (xosvi.wProductType == WinPlatform.VER_NT_WORKSTATION)
							{
								return "Windows XP Professional x64 Edition" + strDetails;
							}
							else if (xosvi.wProductType == WinPlatform.VER_NT_SERVER)
							{
								if (GetSystemMetrics(SM_SERVERR2) == 1)
								{
									return "Windows Server 2003 R2" + strDetails;
								}
								else
								{
									return "Windows Server 2003" + strDetails;
								}
							}
							else if (xosvi.wProductType == WinPlatform.VER_NT_DOMAIN_CONTROLLER)
							{
								if (GetSystemMetrics(SM_SERVERR2) == 1)
								{
									return "Windows Server 2003 R2 Domain Controller" + strDetails;
								}
								else
								{
									return "Windows Server 2003 Domain Controller" + strDetails;
								}
							}
						}
					}
					else if (xosvi.dwMajorVersion == OSMajorVersion.VER_OS_VISTA_LONGHORN)
					{
						if (xosvi.wProductType == WinPlatform.VER_NT_WORKSTATION)
						{
							if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_PERSONAL) == WinSuiteMask.VER_SUITE_PERSONAL)
							{
								return "Windows Vista (Home Premium, Home Basic, or Home Ultimate) Edition";
							}
							else
							{
								return "Windows Vista (Enterprize or Business)" + strDetails;
							}
						}
						else
						{
							return "Windows Server (Longhorn)" + strDetails;
						}
					}
				}
			}
			catch
			{
				MessageBox.Show(GetLastError.ToString);
				return string.Empty;
			}
		}*/

		public enum WindowsVersion
		{
			Unknown = 0,
			Windows95,
			Windows98,
			WindowsMe,
			WindowsNT351,
			WindowsNT4,
			Windows2000,
			WindowsXP,
			WindowsServer2003,
			WindowsVista,
			WindowsServer2008,
            Windows7,
            WindowsServer2008R2,
            Windows8,
            WindowsServer2012,
            WindowsServer2012R2,
            WindowsServer2016,
            Windows10
		}

		public static string GetName(WindowsVersion version)
		{
			string ret = string.Empty; 
			switch (version)
			{
				case WindowsVersion.Unknown:
					ret = "Unknown";
					break;
				case WindowsVersion.Windows2000:
					ret = "Windows 2000";
					break;
				case WindowsVersion.Windows95:
					ret = "Windows 95";
					break;
				case WindowsVersion.Windows98:
					ret = "Windows 98";
					break;
				case WindowsVersion.WindowsMe:
					ret = "Windows Me";
					break;
				case WindowsVersion.WindowsNT351:
					ret = "Windows NT 3.51";
					break;
				case WindowsVersion.WindowsNT4:
					ret = "Windows NT 4.0";
					break;
				case WindowsVersion.WindowsServer2003:
					ret = "Windows Server 2003";
					break;
				case WindowsVersion.WindowsServer2008:
					ret = "Windows Server 2008";
					break;
                case WindowsVersion.WindowsServer2008R2:
                    ret = "Windows Server 2008 R2";
                    break;
                case WindowsVersion.WindowsServer2012:
                    ret = "Windows Server 2012";
                    break;
                case WindowsVersion.WindowsServer2012R2:
                    ret = "Windows Server 2012 R2";
                    break;
				case WindowsVersion.WindowsVista:
					ret = "Windows Vista";
					break;
				case WindowsVersion.WindowsXP:
					ret = "Windows XP";
					break;
                case WindowsVersion.Windows7:
                    ret = "Windows 7";
                    break;
                case WindowsVersion.Windows8:
                    ret = "Windows 8";
                    break;
                case WindowsVersion.Windows10:
                    ret = "Windows 10";
                    break;
                case WindowsVersion.WindowsServer2016:
                    ret = "Windows Server 2016";
                    break;
                default:
                    ret = "Windows";
                    break;
			}
			return ret;
		}

		/// <summary>
		/// Determine OS version
		/// </summary>
		/// <returns></returns>
		public static WindowsVersion GetVersion()
		{
			WindowsVersion ret = WindowsVersion.Unknown;

			OSVERSIONINFOEX info = new OSVERSIONINFOEX();
			info.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));
			GetVersionEx(ref info);

			// Get OperatingSystem information from the system namespace.
			System.OperatingSystem osInfo = System.Environment.OSVersion;

			// Determine the platform.
			switch (osInfo.Platform)
			{
				// Platform is Windows 95, Windows 98, Windows 98 Second Edition, or Windows Me.
				case System.PlatformID.Win32Windows:
					switch (osInfo.Version.Minor)
					{
						case 0:
							ret = WindowsVersion.Windows95;
							break;
						case 10:
							ret = WindowsVersion.Windows98;
							break;
						case 90:
							ret = WindowsVersion.WindowsMe;
							break;
					}
					break;

				// Platform is Windows NT 3.51, Windows NT 4.0, Windows 2000, or Windows XP.
				case System.PlatformID.Win32NT:
					switch (osInfo.Version.Major)
					{
						case 3:
							ret = WindowsVersion.WindowsNT351;
							break;
						case 4:
							ret = WindowsVersion.WindowsNT4;
							break;
						case 5:
							switch (osInfo.Version.Minor)
							{
								case 0:
									ret = WindowsVersion.Windows2000;
									break;
								case 1:
									ret = WindowsVersion.WindowsXP;
									break;
								case 2:
									int i = GetSystemMetrics(SM_SERVERR2);
									if (i != 0)
									{
										//Server 2003 R2
										ret = WindowsVersion.WindowsServer2003;
									}
									else
									{
										if (info.wProductType == (byte)WinPlatform.VER_NT_WORKSTATION)
										{
											//XP Pro x64
											ret = WindowsVersion.WindowsXP;
										}
										else
										{
											ret = WindowsVersion.WindowsServer2003;
										}
										break;
									}
									break;
							}
							break;
						case 6:
                            switch (osInfo.Version.Minor)
                            {
                                case 0:
                                    if (info.wProductType == (byte)WinPlatform.VER_NT_WORKSTATION)
                                        ret = WindowsVersion.WindowsVista;
                                    else
                                        ret = WindowsVersion.WindowsServer2008;
                                    break;
                                case 1:
                                    if (info.wProductType == (byte)WinPlatform.VER_NT_WORKSTATION)
                                        ret = WindowsVersion.Windows7;
                                    else
                                        ret = WindowsVersion.WindowsServer2008R2;
                                    break;
                                case 2:
                                    if (info.wProductType == (byte)WinPlatform.VER_NT_WORKSTATION)
                                        ret = WindowsVersion.Windows8;
                                    else
                                        ret = WindowsVersion.WindowsServer2012;
                                    break;
                                case 3:
                                        ret = WindowsVersion.WindowsServer2012R2;
                                    break;
                            }
                            break;
                        case 10:
                            if (info.wProductType == (byte)WinPlatform.VER_NT_WORKSTATION)
                                ret = WindowsVersion.Windows10;
                            else
                                ret = WindowsVersion.WindowsServer2016;
                            break;
                    }
					break;
			}
			return ret;
		}

		/// <summary>
		/// Returns Windows directory
		/// </summary>
		/// <returns></returns>
		public static string GetWindowsDirectory()
		{
			return Environment.GetEnvironmentVariable("windir");
		}

		/// <summary>
		/// Returns Windows directory
		/// </summary>
		/// <returns></returns>
		public static string GetSystemTmpDirectory()
		{
			return Environment.GetEnvironmentVariable("TMP", EnvironmentVariableTarget.Machine);
		}
	}
}

