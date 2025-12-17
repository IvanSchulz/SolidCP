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
using System.Xml;
using System.Configuration;
using System.Windows.Forms;
using System.Collections;
using System.Text;
using FuseCP.Setup.Actions;
using Data = FuseCP.EnterpriseServer.Data;
using FuseCP.UniversalInstaller.Core;
using FuseCP.Providers.OS;
using FuseCP.UniversalInstaller;

namespace FuseCP.Setup
{
	public class EnterpriseServer : BaseSetup
	{
		public static object Install(object obj)
		{
			return InstallBase(obj, "1.0.1");
		}
		public static object InstallBase(object obj, string minimalInstallerVersion)
		{
			return InstallBaseRaw(obj, minimalInstallerVersion);
		}
		static object InstallBaseRaw(object obj, string minimalInstallerVersion)
		{
			var args = Utils.GetSetupParameters(obj);
			//check CS version
			var shellVersion = Utils.GetStringSetupParameter(args, "ShellVersion");
			var shellMode = Utils.GetStringSetupParameter(args, Global.Parameters.ShellMode);
			var version = new Version(shellVersion);

			var setupVariables = new SetupVariables
			{
				ConnectionString = Global.EntServer.AspNetConnectionStringFormat,
				DatabaseServer = Global.EntServer.DefaultDbServer,
				Database = Global.EntServer.DefaultDatabase,
				CreateDatabase = true,
				WebSiteIP = Global.EntServer.DefaultIP,
				WebSitePort = Global.EntServer.DefaultPort,
				WebSiteDomain = String.Empty,
				NewWebSite = true,
				NewVirtualDirectory = false,
				ConfigurationFile = "web.config",
				UpdateServerAdminPassword = true,
				ServerAdminPassword = "",
			};

			//
			InitInstall(args, setupVariables);
			//
			var eam = new EntServerActionManager(setupVariables);
			//
			eam.PrepareDistributiveDefaults();
			//
			if (shellMode.Equals(Global.SilentInstallerShell, StringComparison.OrdinalIgnoreCase))
			{
				if (version < new Version(minimalInstallerVersion))
				{
					Utils.ShowConsoleErrorMessage(Global.Messages.InstallerVersionIsObsolete, minimalInstallerVersion);
					//
					return false;
				}

				try
				{
					var success = true;
					//
					setupVariables.ServerAdminPassword = Utils.GetStringSetupParameter(args, Global.Parameters.ServerAdminPassword);
					setupVariables.Database = Utils.GetStringSetupParameter(args, Global.Parameters.DatabaseName);
					setupVariables.DatabaseServer = Utils.GetStringSetupParameter(args, Global.Parameters.DatabaseServer);
					setupVariables.DatabasePort = (int)Utils.GetSetupParameter(args, Global.Parameters.DatabasePort);
					Data.DbType dbType = Data.DbType.Unknown;
					Enum.TryParse(Utils.GetStringSetupParameter(args, Global.Parameters.DatabaseType), out dbType);
					setupVariables.DatabaseType = dbType;
					//
					switch (setupVariables.DatabaseType)
					{
						case Data.DbType.SqlServer:
							setupVariables.DbInstallConnectionString = Data.DatabaseUtils.BuildSqlServerMasterConnectionString(
								setupVariables.DatabaseServer,
								Utils.GetStringSetupParameter(args, Global.Parameters.DbServerAdmin),
								Utils.GetStringSetupParameter(args, Global.Parameters.DbServerAdminPassword)
							);
							break;
						case Data.DbType.MySql:
						case Data.DbType.MariaDb:
							setupVariables.DbInstallConnectionString = Data.DatabaseUtils.BuildMySqlMasterConnectionString(
								setupVariables.DatabaseServer,
								setupVariables.DatabasePort,
								Utils.GetStringSetupParameter(args, Global.Parameters.DbServerAdmin),
								Utils.GetStringSetupParameter(args, Global.Parameters.DbServerAdminPassword)
							);
							break;
						case Data.DbType.Sqlite:
						case Data.DbType.SqliteFX:
							setupVariables.DbInstallConnectionString =
								Data.DatabaseUtils.BuildSqliteMasterConnectionString(setupVariables.Database,
									setupVariables.InstallationFolder, setupVariables.EnterpriseServerPath, setupVariables.EmbedEnterpriseServer);
							break;
						default: break;
					}
					//
					eam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
					{
						Utils.ShowConsoleErrorMessage(e.ErrorMessage);
						//
						Log.WriteError(e.ErrorMessage);
						//
						success = false;
					});
					//
					eam.Start();
					//
					return success;
				}
				catch (Exception ex)
				{
					Log.WriteError("Failed to install the component", ex);
					//
					return false;
				}
			}
			else
			{
				if (version < new Version(minimalInstallerVersion))
				{
					MessageBox.Show(string.Format(Global.Messages.InstallerVersionIsObsolete, minimalInstallerVersion), "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return DialogResult.Cancel;
				}

				InstallerForm form = new InstallerForm();
				Wizard wizard = form.Wizard;
				wizard.SetupVariables = setupVariables;
				wizard.ActionManager = eam;

				//Unattended setup
				LoadSetupVariablesFromSetupXml(wizard.SetupVariables.SetupXml, wizard.SetupVariables);
				//create wizard pages
				var introPage = new IntroductionPage();
				var licPage = new LicenseAgreementPage();
				var page1 = new ConfigurationCheckPage();
				if (OSInfo.IsWindows)
				{
					page1.Checks.AddRange(new ConfigurationCheck[]
					{
						new ConfigurationCheck(CheckTypes.WindowsOperatingSystem, "Operating System Requirement"){ SetupVariables = setupVariables },
						new ConfigurationCheck(CheckTypes.IISVersion, "IIS Requirement"){ SetupVariables = setupVariables },
						new ConfigurationCheck(CheckTypes.ASPNET, "ASP.NET Requirement"){ SetupVariables = setupVariables }
					});
				}
				else
				{
					page1.Checks.AddRange(new ConfigurationCheck[]
					{
						new ConfigurationCheck(CheckTypes.OperatingSystem, "Operating System Requirement"){ SetupVariables = setupVariables },
						new ConfigurationCheck(CheckTypes.Net8Runtime, ".NET 8 Runtime Requirement"){ SetupVariables = setupVariables },
						new ConfigurationCheck(CheckTypes.Systemd, "Systemd Requirement"){ SetupVariables = setupVariables },
					});
				}
				var page2 = new InstallFolderPage();
				var page3 = new WebPage();
				var page4 = new InsecureHttpWarningPage();
				var page5 = new CertificatePage();
				UserAccountPage page6 = null;
				if (OSInfo.IsWindows) page6 = new UserAccountPage();
				var page7 = new DatabasePage();
				var passwordPage = new ServerAdminPasswordPage();
				//
				var page8 = new ExpressInstallPage2();
				//
				var page9 = new FinishPage();
				wizard.Controls.AddRange(new Control[] { introPage, licPage, page1, page2, page3, page4, page5 });
				if (OSInfo.IsWindows) wizard.Controls.Add(page6);
				wizard.Controls.AddRange(new Control[] { page7, passwordPage, page8, page9 });
				wizard.LinkPages();
				wizard.SelectedPage = introPage;

				//show wizard
				IWin32Window owner = args[Global.Parameters.ParentForm] as IWin32Window;
				return form.ShowModal(owner);
			}
		}

		public static DialogResult Uninstall(object obj)
		{
			return UninstallBase(obj);
		}

		public static DialogResult Setup(object obj)
		{
			return SetupRaw(obj);
		}
		static DialogResult SetupRaw(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);
			string shellVersion = Utils.GetStringSetupParameter(args, "ShellVersion");
			//
			var setupVariables = new SetupVariables
			{
				ComponentId = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentId),
				ConfigurationFile = "web.config",
				IISVersion = Global.IISVersion,
				SetupAction = SetupActions.Setup
			};
			//
			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			wizard.SetupVariables = setupVariables;
			//
			AppConfig.LoadComponentSettings(wizard.SetupVariables);

			//IntroductionPage page1 = new IntroductionPage();
			WebPage page1 = new WebPage();
			var page2 = new InsecureHttpWarningPage();
			CertificatePage page3 = new CertificatePage();
			ServerAdminPasswordPage page4 = new ServerAdminPasswordPage();
			ExpressInstallPage page5 = new ExpressInstallPage();
			//create install currentScenario
			InstallAction action = new InstallAction(ActionTypes.UpdateWebSite);
			action.Description = "Updating web site...";
			page5.Actions.Add(action);

			action = new InstallAction(ActionTypes.ConfigureLetsEncrypt);
			action.Description = "Configure Let's Encrypt...";
			page5.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateServerAdminPassword);
			action.Description = "Updating serveradmin password...";
			page5.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page5.Actions.Add(action);


            FinishPage page6 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { page1, page2, page3, page4, page5, page6 });
			wizard.LinkPages();
			wizard.SelectedPage = page1;

			//show wizard
			IWin32Window owner = args[Global.Parameters.ParentForm] as IWin32Window;
			return form.ShowModal(owner);
		}

		public static DialogResult Update(object obj)
		{
			return UpdateRaw(obj);
		}
		static DialogResult UpdateRaw(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);

			var setupVariables = new SetupVariables
			{
				ComponentId = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentId),
				SetupAction = SetupActions.Update,
				IISVersion = Global.IISVersion
			};

			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			wizard.SetupVariables = setupVariables;
			//
			AppConfig.LoadComponentSettings(wizard.SetupVariables);

			IntroductionPage introPage = new IntroductionPage();
			LicenseAgreementPage licPage = new LicenseAgreementPage();
			ExpressInstallPage page2 = new ExpressInstallPage();
			//create install currentScenario
			InstallAction action = new InstallAction(ActionTypes.Backup);
			action.Description = "Backing up...";
			page2.Actions.Add(action);

			action = new InstallAction(ActionTypes.DeleteFiles);
			action.Description = "Deleting files...";
			action.Path = "setup\\delete.txt";
			page2.Actions.Add(action);

			action = new InstallAction(ActionTypes.CopyFiles);
			action.Description = "Copying files...";
			page2.Actions.Add(action);

			action = new InstallAction(ActionTypes.ExecuteSql);
			action.Description = "Updating database...";
			action.Path = "setup\\update_db.sql";
			page2.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page2.Actions.Add(action);

			FinishPage page3 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { introPage, licPage, page2, page3 });
			wizard.LinkPages();
			wizard.SelectedPage = introPage;

			//show wizard
			Form parentForm = args[Global.Parameters.ParentForm] as Form;
			return form.ShowDialog(parentForm);
		}
	}
}
