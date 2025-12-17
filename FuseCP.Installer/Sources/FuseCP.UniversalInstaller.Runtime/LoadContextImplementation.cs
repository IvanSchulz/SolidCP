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
using System.IO;
#if NETCOREAPP
using System.Runtime.Loader;
#endif
using FuseCP.Providers.OS;

namespace FuseCP.UniversalInstaller;

[Serializable]
public class LoadContextImplementation : MarshalByRefObject, ILoadContext
{
	const bool AlwaysUseLoadContext = true;
	const bool UseLocalSetupDllForDebugging = true;
	public bool UseLocalSetupDll = false;
	public bool UseSeparateAppDomain = true;

#if NETCOREAPP
	public AssemblyLoadContext AssemblyLoadContext { get; set; } = null;
#endif

	public virtual object RemoteRun(string fileName, string typeName, string methodName, object[] parameters)
	{
		Assembly assembly = null;

		var basePath = AppDomain.CurrentDomain.BaseDirectory;
		var localSetup = Path.Combine(basePath, Path.GetFileName(fileName));
		if (File.Exists(localSetup)) fileName = localSetup;

#if NETCOREAPP
		assembly = AssemblyLoadContext.LoadFromAssemblyPath(fileName);
#else
				
		assembly = Assembly.LoadFrom(fileName);
#endif
		
		Type type = assembly.GetType(typeName);
		MethodInfo method = type.GetMethod(methodName, new Type[] { typeof(string) });
		return method.Invoke(Activator.CreateInstance(type), parameters);
	}

	public void AddTraceListener(TraceListener traceListener)
	{
		Trace.Listeners.Add(traceListener);
	}
	public virtual object Execute(string fileName, string typeName, string methodName, object[] parameters)

	{
#if NETFRAMEWORK
		AppDomain domain = null;
		//LoadContextImplementation loader;
		object loader;
		try
		{
			/* Evidence securityInfo = AppDomain.CurrentDomain.Evidence;
			AppDomainSetup info = new AppDomainSetup();
			info.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

			domain = AppDomain.CreateDomain("Remote Domain", securityInfo, info); */
			object securityInfo;
			var evidenceProperty = typeof(AppDomain).GetProperty("Evidence");
			securityInfo = evidenceProperty.GetValue(AppDomain.CurrentDomain);
			var domainSetupType = Type.GetType("System.AppDomainSetup, mscorlib");
			object info = Activator.CreateInstance(domainSetupType);
			var appBaseProperty = domainSetupType.GetProperty("ApplicationBase");
			appBaseProperty.SetValue(info, AppDomain.CurrentDomain.BaseDirectory);

			var createDomainMethod = typeof(AppDomain).GetMethod("CreateDomain", new Type[] { typeof(string), Type.GetType("System.Security.Policy.Evidence, mscorlib"), domainSetupType });
			domain = createDomainMethod.Invoke(null, new object[] { "Remote Domain", securityInfo, info }) as AppDomain;

			domain.InitializeLifetimeService();
			//domain.UnhandledException += new UnhandledExceptionEventHandler(OnDomainUnhandledException);

			if (UseSeparateAppDomain && !Debugger.IsAttached)
			{
				/* loader = (AssemblyLoader)domain.CreateInstanceAndUnwrap(
					typeof(AssemblyLoader).Assembly.FullName,
					typeof(AssemblyLoader).FullName); */
				var createInstanceAndUnwrapMethod = typeof(AppDomain).GetMethod("CreateInstanceAndUnwrap", new Type[] { typeof(string), typeof(string) });
				/*loader = (LoadContextImplementation)createInstanceAndUnwrapMethod.Invoke(domain, new object[] {
					typeof(LoadContextImplementation).Assembly.FullName,
					typeof(LoadContextImplementation).FullName
				});*/
				loader = createInstanceAndUnwrapMethod.Invoke(domain, new object[] {
					"Setup2",
					"FuseCP.UniversalInstaller.RemoteRunner"
				});

				/*foreach (TraceListener listener in Trace.Listeners)
				{
					loader.AddTraceListener(listener);
				}*/
			}
			else  // don't call in separate AppDomain when debugging
			{
				loader = Activator.CreateInstance(Type.GetType("FuseCP.UniversalInstaller.RemoteRunner, Setup2"));
			}
			var remoteRun = loader.GetType().GetMethod("RemoteRun", new Type[] { typeof(string), typeof(string), typeof(string), typeof(object[]) });
			object ret = remoteRun.Invoke(loader, new object[] { fileName, typeName, methodName, parameters });
			AppDomain.Unload(domain);
			
			Installer.Current.LoadSettings();
			Installer.Current.SaveSettings();
			return ret;
		}
		catch (Exception ex)
		{
			if (domain != null) AppDomain.Unload(domain);

			throw;
		}
#elif NETCOREAPP
		AssemblyLoadContext loadContext = null;
		try
		{
			if (!Debugger.IsAttached || AlwaysUseLoadContext) loadContext = AssemblyLoadContext = new SetupAssemblyLoadContext();
			else loadContext = AssemblyLoadContext = AssemblyLoadContext.Default;

			var res = RemoteRun(fileName, typeName, methodName, parameters);
			if (!Debugger.IsAttached || AlwaysUseLoadContext) loadContext.Unload();
			return res;
		} catch (Exception ex)
		{
			UI.Current.ShowError(ex);
			if (loadContext != null && loadContext != AssemblyLoadContext.Default) loadContext.Unload();
			return Result.Cancel;
		}
#endif
	}

	static void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		Log.WriteError("Remote domain error", (Exception)e.ExceptionObject);
	}

	public string GetShellVersion()
	{
		return Installer.Current.GetEntryAssembly().GetName().Version.ToString();
	}
}
