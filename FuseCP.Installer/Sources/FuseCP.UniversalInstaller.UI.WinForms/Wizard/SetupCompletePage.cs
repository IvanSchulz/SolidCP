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
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class SetupCompletePage : BannerWizardPage
	{
		public SetupCompletePage()
		{
			InitializeComponent();
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CommonSettings Settings { get; set; }
		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);
			this.Text = "Completing FuseCP Setup";
			this.Description = "Setup has finished configuration of FuseCP";
			this.AllowMoveBack = false;
			this.AllowCancel = false;
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (Installer.Current.Settings.Installer.IsUnattended)
				Wizard.GoNext();
		}
		
		private void OnViewLogLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Log.ShowLogFile();
		}

		private void OnLoginClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string ip = Settings.WebSiteIp;
			string domain = Settings.WebSiteDomain;
			string port = Settings.WebSitePort.ToString();
			string url = GetApplicationUrl(ip, domain, port);
			Process.Start(url);
		}

		private string GetApplicationUrl(string ip, string domain, string port)
		{
			string url = ip;
			if (String.IsNullOrEmpty(domain))
			{
				if (!String.IsNullOrEmpty(port) && port != "80")
					url += ":" + port;
			}
			else
			{
				url = domain;
				if (!String.IsNullOrEmpty(port) && port != "80")
					url += ":" + port;
			}
			return "http://"+url;
		}
	}
}
