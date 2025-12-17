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
using FuseCP.UniversalInstaller;

namespace FuseCP.Setup
{
	public class Serveraspv2 : BaseSetup
	{
		public static object Install(object obj)
		{
			return InstallBase(obj, "1.0.1");
		}

		internal static object InstallBase(object obj, string minimalInstallerVersion)
		{
			return InstallBaseRaw(obj, minimalInstallerVersion);
		}

		static object InstallBaseRaw(object obj, string minimalInstallerVersion)
		{
			Hashtable args = Utils.GetSetupParameters(obj);

            //check CS version
            string shellVersion = Utils.GetStringSetupParameter(args, Global.Parameters.ShellVersion);
            var shellMode = Utils.GetStringSetupParameter(args, Global.Parameters.ShellMode);
            Version version = new Version(shellVersion);
            //
            var setupVariables = new SetupVariables
            {
                SetupAction = SetupActions.Install,
                IISVersion = Global.IISVersion
            };
            //
            InitInstall(args, setupVariables);
            //Unattended setup
            LoadSetupVariablesFromSetupXml(setupVariables.SetupXml, setupVariables);
            //
            var sam = new ServerActionManager(setupVariables);
            // Prepare installation defaults
            sam.PrepareDistributiveDefaults();
            // Silent Installer Mode
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
                    setupVariables.ServerPassword = Utils.GetStringSetupParameter(args, Global.Parameters.ServerPassword);
                    //
                    sam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
                    {
                        Utils.ShowConsoleErrorMessage(e.ErrorMessage);
                        //
                        Log.WriteError(e.ErrorMessage);
                        //
                        success = false;
                    });
                    //
                    sam.Start();
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
                    MessageBox.Show(String.Format(Global.Messages.InstallerVersionIsObsolete, minimalInstallerVersion), "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //
                    return DialogResult.Cancel;
                }

                var form = new InstallerForm();
                var wizard = form.Wizard;
                wizard.SetupVariables = setupVariables;
                //
                wizard.ActionManager = sam;

                //create wizard pages
                var introPage = new IntroductionPage();
                var licPage = new LicenseAgreementPage();
                //
                var page1 = new ConfigurationCheckPage();
                page1.Checks.AddRange(new ConfigurationCheck[]
				{ 
					new ConfigurationCheck(CheckTypes.WindowsOperatingSystem, "Operating System Requirement"){ SetupVariables = setupVariables }, 
					new ConfigurationCheck(CheckTypes.IISVersion, "IIS Requirement"){ SetupVariables = setupVariables }, 
					new ConfigurationCheck(CheckTypes.ASPNET, "ASP.NET Requirement"){ SetupVariables = setupVariables }
				});
                //
                var page2 = new InstallFolderPage();
                var page3 = new WebPage();
                var page4 = new UserAccountPage();
                var page5 = new ServerPasswordPage();
                var page6 = new ExpressInstallPage2();
                var page7 = new FinishPage();
                //
                wizard.Controls.AddRange(new Control[] { introPage, licPage, page1, page2, page3, page4, page5, page6, page7 });
                wizard.LinkPages();
                wizard.SelectedPage = introPage;

                //show wizard
                IWin32Window owner = args["ParentForm"] as IWin32Window;
                return form.ShowModal(owner);
            }
        }

		public static object Uninstall(object obj)
		{
			return UninstallRaw(obj);
		}
		static object UninstallRaw(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);
			//
			string shellVersion = Utils.GetStringSetupParameter(args, Global.Parameters.ShellVersion);
			//
			var setupVariables = new SetupVariables
			{
				ComponentId = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentId),
				SetupAction = SetupActions.Uninstall,
				IISVersion = Global.IISVersion
			};
			//
			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			wizard.SetupVariables = setupVariables;

			AppConfig.LoadComponentSettings(wizard.SetupVariables);

			IntroductionPage page1 = new IntroductionPage();
			ConfirmUninstallPage page2 = new ConfirmUninstallPage();
			UninstallPage page3 = new UninstallPage();
			page2.UninstallPage = page3;
			FinishPage page4 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { page1, page2, page3, page4 });
			wizard.LinkPages();
			wizard.SelectedPage = page1;

			//show wizard
			IWin32Window owner = args[Global.Parameters.ParentForm] as IWin32Window;
			return form.ShowModal(owner);
		}

		public static object Setup(object obj)
		{
			return SetupRaw(obj);
		}
		static object SetupRaw(object obj)
		{
			var args = Utils.GetSetupParameters(obj);
			var shellVersion = Utils.GetStringSetupParameter(args, Global.Parameters.ShellVersion);
			//
			var setupVariables = new SetupVariables
			{
				ComponentId = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentId),
				SetupAction = SetupActions.Setup,
				IISVersion = Global.IISVersion,
				ConfigurationFile = "web.config"
			};
			//
			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			//
			wizard.SetupVariables = setupVariables;
			//
			AppConfig.LoadComponentSettings(wizard.SetupVariables);

			WebPage page1 = new WebPage();
			ServerPasswordPage page2 = new ServerPasswordPage();
			ExpressInstallPage page3 = new ExpressInstallPage();
			//create install actions
			InstallAction action = new InstallAction(ActionTypes.UpdateWebSite);
			action.Description = "Updating web site...";
			page3.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateServerPassword);
			action.Description = "Updating server password...";
			page3.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page3.Actions.Add(action);

			FinishPage page4 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { page1, page2, page3, page4 });
			wizard.LinkPages();
			wizard.SelectedPage = page1;

			//show wizard
			IWin32Window owner = args[Global.Parameters.ParentForm] as IWin32Window;
			return form.ShowModal(owner);
		}

		public static object Update(object obj)
		{
			return UpdateRaw(obj);
		}
		static object UpdateRaw(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);

			var setupVariables = new SetupVariables
			{
				ComponentId = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentId),
				SetupAction = SetupActions.Update,
				BaseDirectory = Utils.GetStringSetupParameter(args, Global.Parameters.BaseDirectory),
				UpdateVersion = Utils.GetStringSetupParameter(args, "UpdateVersion"),
				InstallerFolder = Utils.GetStringSetupParameter(args, Global.Parameters.InstallerFolder),
				Installer = Utils.GetStringSetupParameter(args, Global.Parameters.Installer),
				InstallerType = Utils.GetStringSetupParameter(args, Global.Parameters.InstallerType),
				InstallerPath = Utils.GetStringSetupParameter(args, Global.Parameters.InstallerPath)
			};

			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			//
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

			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page2.Actions.Add(action);

			FinishPage page3 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { introPage, licPage, page2, page3 });
			wizard.LinkPages();
			wizard.SelectedPage = introPage;

			//show wizard
			IWin32Window owner = args[Global.Parameters.ParentForm] as IWin32Window;
			return form.ShowModal(owner);
		}
	}
}
