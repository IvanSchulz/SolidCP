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
using System.Management;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FuseCP.UniversalInstaller;
using FuseCP.UniversalInstaller.Web;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class UserAccountPage : BannerWizardPage
	{
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CommonSettings Settings { get; set; }
		SecurityUtils SecurityUtils => Installer.Current.SecurityUtils;
		public UserAccountPage()
		{
			InitializeComponent();
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string WarningText
		{
			get { return lblWarning.Text; }
			set { lblWarning.Text = value; }
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "Security Settings";
			
			string component = Settings.ComponentName;
			this.Description = string.Format("Specify {0} security settings.", component);
			
			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;

			if (!string.IsNullOrEmpty(Settings.Username))
			{
				string domain, username;
				username = Settings.Username;
				var backslash = username.IndexOf('\\');
				if (backslash >= 0)
				{
					domain = username.Substring(0, backslash);
					if (backslash + 1 < username.Length) username = username.Substring(backslash + 1);
					else username = "";
				} else
				{
					domain = "";
				}
				txtUserName.Text = username;
				txtDomain.Text = domain;
				txtPassword.Text = Settings.Password;
				txtConfirmPassword.Text = Settings.Password;
				chkUseActiveDirectory.Checked = !string.IsNullOrEmpty(domain);
			}
			else
			{
				//creating user account
				string userName = component.Replace(" ", string.Empty);
				userName = userName.Replace("FuseCP", "WP");

				txtUserName.Text = userName;
				txtDomain.Text = "mydomain.com";

				string password = Guid.NewGuid().ToString("P");
				this.txtPassword.Text = password;
				this.txtConfirmPassword.Text = password;

				if (Environment.UserDomainName != Environment.MachineName)
				{
					chkUseActiveDirectory.Checked = true;
					string domainName = SecurityUtils.GetFullDomainName(Environment.UserDomainName);
					if (!string.IsNullOrEmpty(domainName))
					{
						txtDomain.Text = domainName;
					}
				}
				else
				{
					chkUseActiveDirectory.Checked = false;
				}
			}
			UpdateControls();
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (Installer.Current.Settings.Installer.IsUnattended && AllowMoveNext)
				Wizard.GoNext();
		}

		private bool CheckSettings()
		{
			string name = txtUserName.Text;
			string password = txtPassword.Text;
			string confirm = txtConfirmPassword.Text;
			string domain = txtDomain.Text;

			if (chkUseActiveDirectory.Checked)
			{
				if (domain.Trim().Length == 0)
				{
					ShowWarning("Please enter domain name");
					return false;
				}

				string users = SecurityUtils.GetDomainUsersContainer(domain);
				if (string.IsNullOrEmpty(users))
				{
					ShowWarning("Domain not found or access denied.");
					return false;
				}

				if (!SecurityUtils.ADObjectExists(users))
				{
					ShowWarning("Domain not found or access denied.");
					return false;
				}
			}

			if (name.Trim().Length == 0)
			{
				ShowWarning("Please enter user name");
				return false;
			}

			if (password.Trim().Length == 0)
			{
				ShowWarning("Please enter password");
				return false;
			}

			if (password != confirm)
			{
				ShowWarning("The password was not correctly confirmed. Please ensure that the password and confirmation match exactly.");
				return false;
			}

			return true;
		}

		private bool ProcessSettings()
		{
			if (!CheckSettings())
			{
				return false;
			}

			if (!CheckUserAccount())
			{
				return false;
			}
			return true;
		}

		private bool CheckUserAccount()
		{
			string userName = txtUserName.Text;
			string password = txtPassword.Text;
			string domain = (chkUseActiveDirectory.Checked ? txtDomain.Text : null);
			
			if (SecurityUtils.UserExists(domain, userName))
			{
				ShowWarning(string.Format("{0} user account already exists.", userName));
				return false;
			}
			
			bool created = false;
			try
			{
				// create account
				Log.WriteStart(string.Format("Creating temp user account \"{0}\"", userName));
				SystemUserItem user = new SystemUserItem();
				user.Name = userName;
				user.FullName = userName;
				user.Description = string.Empty;
				user.MemberOf = null;
				user.Password = password;
				user.PasswordCantChange = true;
				user.PasswordNeverExpires = true;
				user.AccountDisabled = false;
				user.System = true;
				user.Domain = domain;
				SecurityUtils.CreateUser(user);
				//update log
				Log.WriteEnd("Created temp local user account");
				created = true;
			}
			catch (Exception ex)
			{
				System.Runtime.InteropServices.COMException e = ex.InnerException as System.Runtime.InteropServices.COMException;
				Log.WriteError("Create temp local user account error", ex);
				string errorMessage = "Unable to create Windows user account"; 
				if (e != null )
				{
					string errorCode = string.Format("{0:x}", e.ErrorCode);
					switch (errorCode)
					{
						case "8007089a":
							errorMessage = "Invalid username";
							break;
						case "800708c5":
							errorMessage = "The password does not meet the password policy requirements. Check the minimum password length, password complexity and password history requirements.";
							break;
						case "800708b0":
							errorMessage = "The account already exists.";
							break;
					}
				}
				ShowWarning(errorMessage);
				return false;
			}

			if (created)
			{
				Log.WriteStart(string.Format("Deleting temp local user account \"{0}\"", userName));
				try
				{
					SecurityUtils.DeleteUser(domain, userName);
				}
				catch (Exception ex)
				{
					Log.WriteError("Delete temp local user account error", ex);
				}
				Log.WriteEnd("Deleted temp local user account");
			}
			return true;
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			string userName = txtUserName.Text;
			string password = txtPassword.Text;
			string domain = (chkUseActiveDirectory.Checked ? txtDomain.Text : null);

			Log.WriteInfo(string.Format("Domain: {0}", domain));
			Log.WriteInfo(string.Format("User name: {0}", userName));


			if (!ProcessSettings())
			{
				e.Cancel = true;
				return;
			}
			if (!string.IsNullOrEmpty(domain)) Settings.Username = $"{domain}\\{userName}";
			else Settings.Username = userName;
			Settings.Password = password;

			base.OnBeforeMoveNext(e);
		}

		private void OnActiveDirectoryChanged(object sender, EventArgs e)
		{
			UpdateControls();
		}

		private void UpdateControls()
		{
			bool useAD = chkUseActiveDirectory.Checked;
			lblDomain.Visible = useAD;
			txtDomain.Visible = useAD;
			Update();
		}
	}
}
