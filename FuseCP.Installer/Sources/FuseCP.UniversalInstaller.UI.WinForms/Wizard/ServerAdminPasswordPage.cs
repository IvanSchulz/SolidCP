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
	public partial class ServerAdminPasswordPage : BannerWizardPage
	{
		public ServerAdminPasswordPage()
		{
			InitializeComponent();
		}

		public EnterpriseServerSettings Settings => Installer.Current.Settings.EnterpriseServer;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string NoteText
		{
			get { return lblNote.Text; }
			set { lblNote.Text = value; }
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "Set Administrator Password";
			this.Description = "Specify a new password for the serveradmin account.";
			
			chkChangePassword.Checked = true;
			txtPassword.Text = Settings.ServerAdminPassword;
			txtConfirmPassword.Text = Settings.ServerAdminPassword;

			if (Installer.Current.Settings.Installer.Action == SetupActions.Setup)
			{
				chkChangePassword.Visible = true;
			}
			else
			{
				Settings.UpdateServerAdminPassword = true;
				chkChangePassword.Visible = false;
			}

			if (!string.IsNullOrEmpty(Settings.ServerAdminPassword))
			{
				txtPassword.Text = Settings.ServerAdminPassword;
				txtConfirmPassword.Text = Settings.ServerAdminPassword;
			}

		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (Installer.Current.Settings.Installer.IsUnattended)
				Wizard.GoNext();
		}


		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);

			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			try
			{
				if (!chkChangePassword.Checked)
				{
					//if we don't want to change password during setup 
					Settings.UpdateServerAdminPassword = false;
					e.Cancel = false;
					return;
				}
				else
				{
					Settings.UpdateServerAdminPassword = true;
				}

				if (!CheckFields())
				{
					e.Cancel = true;
					return;
				}
				Settings.ServerAdminPassword = txtPassword.Text;
				//
				Settings.UpdateServerAdminPassword = true;
			}
			catch
			{
				this.AllowMoveNext = false;
				ShowError("Unable to reset password.");
				return;
			}
			base.OnBeforeMoveNext(e);
		}

		private bool CheckFields()
		{
			string password = txtPassword.Text;

			if (string.IsNullOrEmpty(password) || password.Trim().Length == 0)
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
