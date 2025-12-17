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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using FuseCP.Providers.OS;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class InstallFolderPage : BannerWizardPage
	{
		public InstallFolderPage()
		{
			InitializeComponent();
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComponentSettings Settings { get; set; }

		private long spaceRequired = 0;

		protected override void InitializePageInternal()
		{
			this.Text = "Choose Install Location";
			string component = Settings.ComponentName;
			this.Description = string.Format("Choose the folder in which to install {0}.", component);
			this.lblIntro.Text = string.Format("Setup will install {0} in the following folder. To install in a different folder, click Browse and select another folder. Click Next to continue.", component);
			this.AllowMoveBack = false;
			this.AllowMoveNext = true;
			this.AllowCancel = true;

			//UpdateSpaceRequiredInformation();
			lblSpaceRequired.Visible = lblSpaceRequiredValue.Visible = false;

			if (!string.IsNullOrEmpty(Settings.InstallPath))
			{
				txtFolder.Text = Settings is StandaloneSettings ?
					Settings.InstallPath : Path.GetDirectoryName(Settings.InstallPath);
			}
		}

		/*private void UpdateSpaceRequiredInformation()
		{
			try
			{
				spaceRequired = 0;
				lblSpaceRequiredValue.Text = string.Empty;
				if (!string.IsNullOrEmpty(Settings.InstallFolder))
				{
					spaceRequired = FileUtils.CalculateFolderSize(Settings.InstallFolder);
					lblSpaceRequiredValue.Text = FileUtils.SizeToMB(spaceRequired);
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("I/O error:", ex);
			}
		}*/

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			////unattended setup
			if (Installer.Current.Settings.Installer.IsUnattended && AllowMoveNext)
				Wizard.GoNext();
		}

		private void OnBrowseClick(object sender, System.EventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.RootFolder = Environment.SpecialFolder.MyComputer;
			dialog.SelectedPath = txtFolder.Text;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				txtFolder.Text = dialog.SelectedPath;
			}
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			try
			{
				string installFolder = Settings is StandaloneSettings ?
					txtFolder.Text : Path.Combine(this.txtFolder.Text, Settings.InstallFolder);
				Log.WriteInfo(string.Format("Destination folder \"{0}\" selected", installFolder));

				if (!Directory.Exists(installFolder))
				{
					Log.WriteStart(string.Format("Creating a new folder \"{0}\"", installFolder));
					Directory.CreateDirectory(installFolder);
					Log.WriteStart("Created a new folder");
				}
				Settings.InstallPath = installFolder;
				if (Settings is StandaloneSettings)
				{
					Installer.Current.Settings.Server.InstallPath = Path.Combine(installFolder, Installer.Current.ServerFolder);
					Installer.Current.Settings.EnterpriseServer.InstallPath = Path.Combine(installFolder, Installer.Current.EnterpriseServerFolder);
					Installer.Current.Settings.WebDavPortal.InstallPath = Path.Combine(installFolder, Installer.Current.WebDavPortalFolder);
					Installer.Current.Settings.WebPortal.InstallPath = Path.Combine(installFolder, Installer.Current.WebPortalFolder);
				}
				base.OnBeforeMoveNext(e);
			}
			catch (Exception ex)
			{
				Log.WriteError("Folder create error", ex);
				e.Cancel = true;
				ShowError("Unable to create the folder.");
			}
		}

		private void OnFolderChanged(object sender, EventArgs e)
		{
			UpdateFreeSpaceInformation();

		}

		private void UpdateFreeSpaceInformation()
		{
			lblSpaceAvailableValue.Text = string.Empty;

			this.AllowMoveNext = false;
			try
			{
				if (string.IsNullOrEmpty(txtFolder.Text))
					return;
				string path = Path.GetFullPath(txtFolder.Text);
				string drive = Path.GetPathRoot(path);
				ulong freeBytesAvailable, totalBytes, freeBytes;
				if (FileUtils.GetDiskFreeSpace(drive, out freeBytesAvailable, out totalBytes, out freeBytes))
				{
					long freeSpace = Convert.ToInt64(freeBytesAvailable);
					lblSpaceAvailableValue.Text = FileUtils.SizeToMB(freeSpace);
					this.AllowMoveNext = (freeSpace >= spaceRequired);
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("I/O error:", ex);
				this.AllowMoveNext = false;
			}
		}
	}
}
