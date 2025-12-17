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

namespace FuseCP.UniversalInstaller
{
	public partial class AvaloniaUI : UI
	{
		public new class SetupWizard: UI.SetupWizard
		{
			public SetupWizard(UI ui): base(ui) { }

			public override UI.SetupWizard Introduction(ComponentSettings settings) => this;
			public override UI.SetupWizard Certificate(CommonSettings settings) => this;
			public override UI.SetupWizard CheckPrerequisites() => this;
			public override UI.SetupWizard ConfirmUninstall(ComponentSettings settings) => this;
			public override UI.SetupWizard Database() => this;
			public override UI.SetupWizard EmbedEnterpriseServer() => this;
			public override UI.SetupWizard Progress() => this;
			public override UI.SetupWizard Download() => this;
			public override UI.SetupWizard Finish() => this;
			public override UI.SetupWizard InsecureHttpWarning(CommonSettings settings) => this;
			public override UI.SetupWizard InstallFolder(ComponentSettings settings) => this;
			public override UI.SetupWizard LicenseAgreement() => this;
			public override UI.SetupWizard ServerAdminPassword() => this;
			public override UI.SetupWizard ServerPassword() => this;
	
			public override bool Show()
			{
				return true;
			}
		}

		bool isAvailable = false;
		public override bool IsAvailable => isAvailable;

		public override UI.SetupWizard Wizard => new SetupWizard(this);

		public override void Exit()
		{
		}

		public override string GetRootPassword()
		{
			throw new NotImplementedException();
		}

		public override void Init()
		{
			try
			{
			} catch
			{

			}
		}

		public override void RunMainUI()
		{
			throw new NotImplementedException();
		}

		public override void ShowError(Exception ex)
		{
			throw new NotImplementedException();
		}

		public override void ShowLogFile()
		{
			throw new NotImplementedException();
		}

		public override void DownloadInstallerUpdate()
		{
			throw new NotImplementedException();
		}
		public override bool CheckForInstallerUpdate(bool appStartup = false)
		{
			throw new NotImplementedException();
		}

		public override void ShowWarning(string msg) => throw new NotImplementedException();

		public override bool DownloadSetup(RemoteFile file, bool setupOnly = false)
		{
			throw new NotImplementedException();
		}
	}
}
