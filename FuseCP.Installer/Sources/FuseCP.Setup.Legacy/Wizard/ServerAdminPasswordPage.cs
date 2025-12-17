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

namespace FuseCP.Setup
{
	public partial class ServerAdminPasswordPage : BannerWizardPage
	{
		public ServerAdminPasswordPage()
		{
			InitializeComponent();
		}

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
			txtPassword.Text = SetupVariables.ServerAdminPassword;
			txtConfirmPassword.Text = SetupVariables.ServerAdminPassword;

			if (SetupVariables.SetupAction == SetupActions.Setup)
			{
				chkChangePassword.Visible = true;
			}
			else
			{
				SetupVariables.UpdateServerAdminPassword = true;
				chkChangePassword.Visible = false;
			}

			if (!string.IsNullOrEmpty(SetupVariables.ServerAdminPassword))
			{
				txtPassword.Text = SetupVariables.ServerAdminPassword;
				txtConfirmPassword.Text = SetupVariables.ServerAdminPassword;
			}

		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (!string.IsNullOrEmpty(SetupVariables.SetupXml))
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
					SetupVariables.UpdateServerAdminPassword = false;
					e.Cancel = false;
					return;
				}

				if (!CheckFields())
				{
					e.Cancel = true;
					return;
				}
				// Use the same password for peer account
				SetupVariables.ServerAdminPassword = SetupVariables.PeerAdminPassword = txtPassword.Text;
				//
				SetupVariables.UpdateServerAdminPassword = true;
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
