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
using System.Windows.Forms;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class ServerPasswordPage : BannerWizardPage
	{
		public ServerPasswordPage()
		{
			InitializeComponent();
		}
		public ServerSettings Settings => Installer.Current.Settings.Server;
		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "Set Server Password";
			this.Description = "Specify a new password for this server.";
			
			chkChangePassword.Checked = true;

			if (Installer.Current.Settings.Installer.Action == SetupActions.Setup)
			{
				chkChangePassword.Visible = true;
			}
			else
			{
				Settings.UpdateServerPassword = true;
				chkChangePassword.Visible = false;
			}
			if (!string.IsNullOrEmpty(Settings.ServerPassword))
			{
				txtPassword.Text = Settings.ServerPassword;
				txtConfirmPassword.Text = Settings.ServerPassword;
			}
		}


		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);

			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (Installer.Current.Settings.Installer.IsUnattended && AllowMoveNext)
				Wizard.GoNext();
		}
		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			try
			{
				if (!chkChangePassword.Checked)
				{
					//if we don't want to change password during setup 
					Settings.UpdateServerPassword = false;
					e.Cancel = false;
					return;
				}

				if (!CheckFields())
				{
					e.Cancel = true;
					return;
				}
				//
				if (Installer.Current.Settings.Installer.Action == SetupActions.Setup)
				{
					Settings.ServerPassword = Utils.ComputeSHAServerPassword(txtPassword.Text);
				}
				else
				{
					Settings.ServerPassword = txtPassword.Text;
				}
				Settings.UpdateServerPassword = true;
			}
			catch
			{
				this.AllowMoveNext = false;
				ShowError("Unable to set server password.");
				return;
			}
			base.OnBeforeMoveNext(e);
		}

		private bool CheckFields()
		{
			string password = txtPassword.Text;

			if (password.Trim().Length == 0)
			{
				ShowWarning("Please enter password");
				return false;
			}

			if (password != txtConfirmPassword.Text)
			{
				ShowWarning("The password was not correctly confirmed. Please ensure that the password and confirmation match exactly.");
				return false;
			}
			return true;
		}

		private void OnChangePasswordChecked(object sender, EventArgs e)
		{
			bool enabled = chkChangePassword.Checked;
			txtPassword.Enabled = enabled;
			txtConfirmPassword.Enabled = enabled;
		}
	}
}
