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
using System.Security.Policy;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Lifetime;
using System.IO;
using FuseCP.Providers.OS;

namespace FuseCP.Installer.Common
{
	[Serializable]
	public class AssemblyLoader : MarshalByRefObject
	{

		const bool UseLocalSetupDllForDebugging = true;
		public bool UseLocalSetupDll = false;

		public object RemoteRun(string fileName, string typeName, string methodName, object[] parameters)
		{
			Assembly assembly = null;
#if DEBUG
			if (UseLocalSetupDllForDebugging && fileName.EndsWith("Setup.dll", StringComparison.OrdinalIgnoreCase) && 
				(Debugger.IsAttached || UseLocalSetupDll))
			{
				var exe = Assembly.GetEntryAssembly();
				var path = Path.Combine(Path.GetDirectoryName(exe.Location), "Setup.dll");
				assembly = Assembly.LoadFrom(path);
			} else assembly = Assembly.LoadFrom(fileName);
#else
			assembly = Assembly.LoadFrom(fileName);
#endif
			Type type = assembly.GetType(typeName);
			MethodInfo method = type.GetMethod(methodName);
			return method.Invoke(Activator.CreateInstance(type), parameters);
		}

		public void AddTraceListener(TraceListener traceListener)
		{
			Trace.Listeners.Add(traceListener);
		}
		public static object Execute(string fileName, string typeName, string methodName, object[] parameters)
		{
			AppDomain domain = null;
		
			try
			{
				Evidence securityInfo = AppDomain.CurrentDomain.Evidence;
				AppDomainSetup info = new AppDomainSetup();
				info.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

				domain = AppDomain.CreateDomain("Remote Domain", securityInfo, info);
				domain.InitializeLifetimeService();
				//domain.UnhandledException += new UnhandledExceptionEventHandler(OnDomainUnhandledException);

				AssemblyLoader loader;

				if (!Debugger.IsAttached)
				{
					loader = (AssemblyLoader)domain.CreateInstanceAndUnwrap(
						typeof(AssemblyLoader).Assembly.FullName,
						typeof(AssemblyLoader).FullName);

					foreach (TraceListener listener in Trace.Listeners)
					{
						loader.AddTraceListener(listener);
					}
				}
				else  // don't call in separate AppDomain when debugging
				{
					loader = new AssemblyLoader();
				}

				object ret = loader.RemoteRun(fileName, typeName, methodName, parameters);
				AppDomain.Unload(domain);
				return ret;
			}
			catch (Exception ex)
			{
				if (domain != null)
				{
					AppDomain.Unload(domain);
				}
				throw;
			}
		}

		static void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Log.WriteError("Remote domain error", (Exception)e.ExceptionObject);
		}

		public static string GetShellVersion()
		{
			return Assembly.GetEntryAssembly().GetName().Version.ToString();
		}
	}
}
