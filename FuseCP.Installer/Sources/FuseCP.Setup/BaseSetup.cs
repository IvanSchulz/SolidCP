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
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FuseCP.UniversalInstaller;
using OS = FuseCP.Providers.OS;

namespace FuseCP.Setup;

public class BaseSetup
{
	public bool IsJsonArguments => true;
	public InstallerSettings Settings => Installer.Current.Settings;

	static AssemblyLoader loader = null;
	static BaseSetup()
	{
		//loader = AssemblyLoader.Init();
		AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
	}
	public void InitCostura()
	{
#if Costura
		CosturaUtility.Initialize();
#endif
	}

	public void Unload()
	{
		AppDomain.CurrentDomain.UnhandledException -= OnDomainUnhandledException;
		loader?.Unload();
	}
	static void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		Log.WriteError("Remote domain error", (Exception)e.ExceptionObject);
	}

	public void AssertLoadContext()
	{
		if (AssemblyLoadContext.GetLoadContext(Installer.Current.GetType().Assembly) ==
			AssemblyLoadContext.Default) throw new NotSupportedException();
	}

	bool argsParsed = false;
	public bool ParseArgs(object args)
	{
		if (OS.OSInfo.IsCore) AssertLoadContext();

		if (!argsParsed)
		{
			argsParsed = true;
			string json;
			json = args as string;
			if (json == null)
			{
				var hashtable = args as Hashtable;
				if (hashtable.Contains("ParametersJson")) json = hashtable["ParametersJson"] as string;
			}
			if (json != null)
			{
				Installer.Current.Settings = JsonConvert.DeserializeObject<InstallerSettings>(json, new VersionConverter(), new StringEnumConverter());

				if (CommonSettings != null)
				{
					CommonSettings.Version = Installer.Current.Settings.Installer.Component.Version;
				}
				else
				{
					var version = Installer.Current.Settings.Installer.Component.Version;
					Settings.EnterpriseServer.Version = Settings.Server.Version =
						Settings.WebPortal.Version = version;
				}

				UI.SetCurrent(Installer.Current.Settings.Installer.UI);
			}
			else
			{
				UI.Current.ShowWarning("You need to upgrade the Installer to install this component.");
				return false;
			}
			if (args is Hashtable hash) UI.Current.ReadArguments(hash);
		}
		return true;
	}
	public virtual Version MinimalInstallerVersion => new Version("2.0.0");
	public virtual string VersionsToUpgrade => "";
	public bool CheckInstallerVersion()
	{
		if (Settings.Installer.Version < MinimalInstallerVersion)
		{
			UI.Current.ShowWarning("You need to upgrade the Installer to install this component.");
			return false;
		}
		else return true;
	}
	public virtual CommonSettings CommonSettings => null;
	public virtual ComponentSettings ComponentSettings => (CommonSettings as ComponentSettings) ?? Installer.Current.Settings.Standalone;
	public virtual ComponentInfo Component => null;

	public virtual bool IsServer => ComponentSettings is ServerSettings;
	public virtual bool IsEnterpriseServer => ComponentSettings is EnterpriseServerSettings;
	public virtual bool IsStandalone => ComponentSettings is StandaloneSettings;
	public virtual bool IsWebPortal => ComponentSettings is WebPortalSettings;
	public virtual bool IsWebDavPortal => ComponentSettings is WebDavPortalSettings;
	public virtual UI.SetupWizard Wizard(object args, bool setup = false)
	{
		if (ParseArgs(args) && CheckInstallerVersion())
		{
			if (!Installer.Current.Settings.Installer.IsUnattended)
			{
				var wizard = UI.Current.Wizard
					.Introduction(ComponentSettings)
					.CheckPrerequisites()
					.LicenseAgreement();
				if (!IsStandalone)
				{
					if (!setup) wizard = wizard.InstallFolder(CommonSettings);
					wizard = wizard
						.Web(CommonSettings)
						.InsecureHttpWarning(CommonSettings)
						.Certificate(CommonSettings);
					if (!setup) wizard = wizard.UserAccount(CommonSettings);

					if (IsServer) wizard = wizard
						.ServerPassword();

					if (IsEnterpriseServer)
					{
						if (!setup) wizard = wizard.Database();
						wizard = wizard
							.ServerAdminPassword();
					}
					if (IsWebPortal)
					{
						wizard = wizard.EnterpriseServerUrl();
					}
				}
				else
				{
					// Set EnterpriseServer setting for embedded EnterpriseServer
					Settings.EnterpriseServer.WebSiteDomain = "";
					Settings.EnterpriseServer.WebSitePort = 9002;
					Settings.EnterpriseServer.WebSiteIp = "";
					Settings.EnterpriseServer.Username = "";
					Settings.EnterpriseServer.Password = "";
					Settings.EnterpriseServer.Urls = "http://localhost:9002";
					Settings.EnterpriseServer.ConfigureCertificateManually = true;
					Settings.WebPortal.EmbedEnterpriseServer = true;

					if (!setup) wizard = wizard.InstallFolder(Settings.Standalone);
					wizard = wizard
						.Web(Settings.WebPortal)
						.InsecureHttpWarning(Settings.WebPortal)
						.Certificate(Settings.WebPortal);
					if (!setup) wizard = wizard.UserAccount(Settings.WebPortal);
					wizard = wizard
						.ServerAdminPassword();
					if (!setup) wizard = wizard.Database();
					wizard = wizard
						.Web(Settings.Server)
						.InsecureHttpWarning(Settings.Server)
						.Certificate(Settings.Server);
					if (!setup) wizard = wizard.UserAccount(Settings.Server);
					wizard = wizard
						.ServerPassword();
					if (OS.OSInfo.IsWindows)
					{
						wizard = wizard
							.Web(Settings.WebDavPortal)
							.InsecureHttpWarning(Settings.WebDavPortal)
							.Certificate(Settings.WebDavPortal);
						if (!setup) wizard = wizard.UserAccount(Settings.WebDavPortal);
					}
				}
				return wizard;
			} else
			{
				return UI.Current.Wizard;
            }
		}
		return null;
	}

	private void RunWithInfo(Action installer)
	{
		Installer.Current.Info("Download & Unzip Component");
		installer?.Invoke();
	}

	public virtual Result InstallOrSetup(object args, string title, Action installer, bool setup = false) {
		var wizard = Wizard(args, setup);
		if (wizard == null) return Result.Abort;
		if (setup) Installer.Current.Settings.Installer.Action = SetupActions.Setup;
		else Installer.Current.Settings.Installer.Action = SetupActions.Install;
		wizard = wizard
			.RunWithProgress(title, () => RunWithInfo(installer), ComponentSettings);
		Result res = Result.OK;
		if (!Installer.Current.Settings.Installer.IsUnattended)
		{
			wizard = wizard
				.Finish();
		}
		res = wizard.Show() ? Result.OK : Result.Cancel;
		Unload();
		return res;
	}

	public bool CheckUpdate()
	{
		// Find out whether the version(s) are supported in that upgrade
		var upgradeSupported = VersionsToUpgrade.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
			.Any(x => Component?.Version == new Version(x.Trim()));
		// 
		if (!upgradeSupported)
		{
			Log.WriteInfo(
				String.Format("Could not find a suitable version to upgrade. Current version: {0}; Versions supported: {1};", Component?.Version.ToString() ?? "?", VersionsToUpgrade));
			//
			UI.Current.ShowWarning(
				"Your current software version either is not supported or could not be upgraded at this time. Please send log file from the installer to the software vendor for further research on the issue.");
			//
		}

		return upgradeSupported;
	}
	public virtual Result Update(object args, string title, Action installer)
	{
		Result res = Result.OK;
		if (ParseArgs(args))
		{
			Installer.Current.Settings.Installer.Action = SetupActions.Update;

			if (CheckUpdate())
			{
				var wizard = Wizard(args)
					.RunWithProgress(title, () => RunWithInfo(installer), ComponentSettings);
				if (!Installer.Current.Settings.Installer.IsUnattended)
                {
					wizard = wizard
						.Finish();
				}
				res = wizard.Show() ? Result.OK : Result.Cancel;
			}
			Unload();
			return res;
		}
		Unload();
		return Result.Cancel;
	}
			
	public virtual Result Uninstall(object args, string title, Action installer)
	{
		if (ParseArgs(args))
		{
			Installer.Current.Settings.Installer.Action = SetupActions.Uninstall;

			if (CheckInstallerVersion())
			{
				Result res = Result.OK;
				if (!Installer.Current.Settings.Installer.IsUnattended)
				{
					res = UI.Current.Wizard
						.Introduction(ComponentSettings)
						.ConfirmUninstall(ComponentSettings)
						.RunWithProgress(title, installer, ComponentSettings)
						.Finish()
						.Show() ? Result.OK : Result.Cancel;
				} else
				{
					res = UI.Current.Wizard
						.RunWithProgress(title, installer, ComponentSettings)
						.Show() ? Result.OK : Result.Cancel;
                }
				Unload();
				return res;
			}
		}
		Unload();
		return Result.Cancel;
	}
}
