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
using System.IO;
using System.Security.Policy;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
#if NETFRAMEWORK
using System.Runtime.Remoting.Lifetime;
#else
using System.Runtime.Loader;
#endif

namespace FuseCP.VmConfig
{
	[Serializable]
	internal class ModuleLoader : MarshalByRefObject
	{

#if NETCOREAPP
		internal AssemblyLoadContext LoadContext { get; private set; }
#endif
		internal ExecutionResult RemoteRun(string typeName, ref ExecutionContext context)
		{
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");

			string[] parts = typeName.Split(',');
			if (parts == null || parts.Length != 2)
				throw new ArgumentException("Incorrect type name " + typeName);
 
			Assembly assembly = typeof(ModuleLoader).Assembly;
			string fileName = parts[1].Trim();
			if (!fileName.EndsWith(".dll"))
				fileName = fileName + ".dll"; 

			string path = Path.Combine(Path.GetDirectoryName(assembly.Location), fileName);
#if NETFRAMEWORK
			assembly = Assembly.LoadFrom(path);
#else
			assembly = LoadContext.LoadFromAssemblyPath(path);
#endif
			Type type = assembly.GetType(parts[0].Trim());
			//Type type = Type.GetType(typeName);
			if (type == null)
			{
				throw new Exception(string.Format("Type {0} not found", typeName));
			}

			IProvisioningModule module = Activator.CreateInstance(type) as IProvisioningModule;
			if (module == null)
			{
				throw new Exception(string.Format("Module {0} not found", typeName));
			}

			return module.Run(ref context);
		}

		internal void AddTraceListener(TraceListener traceListener)
		{
			Trace.Listeners.Add(traceListener);
		}

		internal static ExecutionResult Run(string typeName, ref ExecutionContext context)
		{
			AppDomain domain = null;
			ModuleLoader loader = null;
			try
			{
#if NETFRAMEWORK
				Evidence securityInfo = AppDomain.CurrentDomain.Evidence;
				AppDomainSetup info = new AppDomainSetup();
				info.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

				domain = AppDomain.CreateDomain("Remote Domain", securityInfo, info);
				ILease lease = domain.GetLifetimeService() as ILease;
				if (lease != null)
				{
					lease.InitialLeaseTime = TimeSpan.Zero;
				}
				domain.UnhandledException += new UnhandledExceptionEventHandler(OnDomainUnhandledException);
				loader = (ModuleLoader)domain.CreateInstanceAndUnwrap(
					typeof(ModuleLoader).Assembly.FullName,
					typeof(ModuleLoader).FullName);
#else
				loader = new ModuleLoader();
				loader.LoadContext = new AssemblyLoadContext("ModuleLoader", true);
#endif
				foreach (TraceListener listener in Trace.Listeners)
				{
					loader.AddTraceListener(listener);
				}

				ExecutionResult ret = loader.RemoteRun(typeName, ref context);
#if NETFRAMEWORK
				AppDomain.Unload(domain);
#else
				loader.LoadContext.Unload();
				loader.LoadContext = null;
#endif
				return ret;
			}
			catch (Exception)
			{
#if NETFRAMEWORK
				if (domain != null)
				{
					AppDomain.Unload(domain);
				}
#else
				loader.LoadContext.Unload();
				loader.LoadContext = null;
#endif
				throw;
			}
		}

		static void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			ServiceLog.WriteError("Remote application domain error", (Exception)e.ExceptionObject);
		}
	}
}
