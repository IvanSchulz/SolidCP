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
using System.Configuration;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FuseCP.EnterpriseServer;

namespace FuseCP.Import.Enterprise
{
	public partial class ConnectForm : Form
	{
		private Thread connectThread;

		public ConnectForm()
		{
			InitializeComponent();
			if (DesignMode)
			{
				return;
			}
			InitializeForm();
			UpdateFormState();
		}

		Controller EnterpriseServer => new Controller();
		private void InitializeForm()
		{
			animatedIcon.Images.Add(FuseCP.Import.Enterprise.Properties.Resources.ProgressImage1);
			animatedIcon.Images.Add(FuseCP.Import.Enterprise.Properties.Resources.ProgressImage2);
			animatedIcon.Images.Add(FuseCP.Import.Enterprise.Properties.Resources.ProgressImage3);
			animatedIcon.Images.Add(FuseCP.Import.Enterprise.Properties.Resources.ProgressImage4);
			animatedIcon.Images.Add(FuseCP.Import.Enterprise.Properties.Resources.ProgressImage5);
			animatedIcon.Images.Add(FuseCP.Import.Enterprise.Properties.Resources.ProgressImage6);
			animatedIcon.Images.Add(FuseCP.Import.Enterprise.Properties.Resources.ProgressImage7);
			animatedIcon.Images.Add(FuseCP.Import.Enterprise.Properties.Resources.ProgressImage8);
			animatedIcon.LastFrame = 8;
		}

		private void OnConnectClick(object sender, EventArgs e)
		{
			DisableForm();
			animatedIcon.StartAnimation();
			ThreadStart threadDelegate = new ThreadStart(Connect);
			connectThread = new Thread(threadDelegate);
			connectThread.Start();
		}

		

		public string Username
		{
			get { return txtUserName.Text; }
		}
		
		public string Password
		{
			get { return txtPassword.Text; }
		}

		private void Connect()
		{
			int status = -1;
			try
			{
				using (var ES = EnterpriseServer)
				{
					status = ES.UserController.AuthenticateUser(txtUserName.Text, txtPassword.Text, string.Empty);
					if (status == 0)
					{
						UserInfo userInfo = ES.UserController.GetUser(txtUserName.Text);
						ES.SecurityContext.SetThreadPrincipal(userInfo);
					}
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("Authentication error", ex);
				status = -1;
			}
			finally
			{
				animatedIcon.StopAnimation();
			}

			string errorMessage = "Check configuration settings.";
			if (status != 0)
			{
				switch (status)
				{
					case BusinessErrorCodes.ERROR_USER_WRONG_USERNAME:
					case BusinessErrorCodes.ERROR_USER_WRONG_PASSWORD:
						errorMessage = "Wrong username or password.";
						break;
					case BusinessErrorCodes.ERROR_USER_ACCOUNT_CANCELLED:
						errorMessage = "Account cancelled.";
						break;
					case BusinessErrorCodes.ERROR_USER_ACCOUNT_PENDING:
						errorMessage = "Account pending.";
						break;
					case -1:
						errorMessage = "Authentication error.";
						break;
				}
				MessageBox.Show(this,
					string.Format("Cannot connect to the Enterprise Server.\n{0}", errorMessage),
					Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				EnableForm();
				return;
			}
			else
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void DisableForm()
		{
			foreach(Control ctrl in Controls)
			{
				ctrl.Enabled = false;
			}
			btnCancel.Enabled = true;
		}

		private void EnableForm()
		{
			foreach (Control ctrl in Controls)
			{
				ctrl.Enabled = true;
			}
		}

		private void UpdateFormState()
		{
			btnConnect.Enabled = (txtUserName.Text.Length > 0 );
		}

		private void OnCancelClick(object sender, EventArgs e)
		{
			if (connectThread != null)
			{
				connectThread.Abort();
			}
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void OnServerNameTextChanged(object sender, EventArgs e)
		{
			UpdateFormState();
		}

		private void OnUserNameTextChanged(object sender, EventArgs e)
		{
			UpdateFormState();
		}
	}
}
