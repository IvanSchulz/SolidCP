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
using System.IO;
using System.Xml;
using FuseCP.UniversalInstaller;
using FuseCP.Setup.Common;

namespace FuseCP.Setup.Actions
{
	public class SetWebDavPortalWebSettingsAction : Action, IPrepareDefaultsAction
	{
		public const string LogStartMessage = "Retrieving default IP address of the component...";

		void IPrepareDefaultsAction.Run(SetupVariables vars)
		{
			//
			if (String.IsNullOrEmpty(vars.WebSitePort))
				vars.WebSitePort = Global.WebDavPortal.DefaultPort;
			//
			if (String.IsNullOrEmpty(vars.UserAccount))
				vars.UserAccount = Global.WebDavPortal.ServiceAccount;

			// By default we use public ip for the component
			if (String.IsNullOrEmpty(vars.WebSiteIP))
			{
				var serverIPs = WebUtils.GetIPv4Addresses();
				//
				if (serverIPs != null && serverIPs.Length > 0)
				{
					vars.WebSiteIP = serverIPs[0];
				}
				else
				{
					vars.WebSiteIP = Global.LoopbackIPv4;
				}
			}
		}
	}

	public class WebDavPortalActionManager : BaseActionManager
	{
		public static readonly List<Action> InstallScenario = new List<Action>
		{
			new SetCommonDistributiveParamsAction(),
			new SetWebDavPortalWebSettingsAction(),
			new EnsureServiceAccntSecured(),
			new CopyFilesAction(),
			new CopyWebConfigAction(),
			new CreateWindowsAccountAction(),
			new ConfigureAspNetTempFolderPermissionsAction(),
			new SetNtfsPermissionsAction(),
			new CreateWebApplicationPoolAction(),
			new CreateWebSiteAction(),
			new InstallLetsEncryptCertificateAction(),
			new SwitchAppPoolAspNetVersion(),
			new UpdateEnterpriseServerUrlAction(),
            new GenerateSessionValidationKeyAction(),
			new SaveComponentConfigSettingsAction()
        };

        public WebDavPortalActionManager(SetupVariables sessionVars)
			: base(sessionVars)
		{
			Initialize += new EventHandler(WebDavPortalActionManager_Initialize);
		}

		void WebDavPortalActionManager_Initialize(object sender, EventArgs e)
		{
			//
			switch (SessionVariables.SetupAction)
			{
				case SetupActions.Install: // Install
					LoadInstallationScenario();
					break;
			}
		}

		private void LoadInstallationScenario()
		{
			CurrentScenario.AddRange(InstallScenario);
		}
	}
}
