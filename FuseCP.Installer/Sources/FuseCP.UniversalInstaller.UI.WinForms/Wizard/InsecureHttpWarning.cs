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
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FuseCP.Providers.OS;
using FuseCP.UniversalInstaller;
using FuseCP.UniversalInstaller.Web;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class InsecureHttpWarningPage : BannerWizardPage
	{
		public InsecureHttpWarningPage()
		{
			InitializeComponent();
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CommonSettings Settings { get; set; }
		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();

			Text = "Warning Insecure Http Protocol";
			Description = "You've choosen an insecure http protocol.";

			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;
			Update();
		}

		bool iis7 => OSInfo.IsWindows && OSInfo.Windows.WebServer.Version.Major >= 7;

		bool IsHttps => (iis7 || !OSInfo.IsWindows) && Utils.IsHttps(Installer.Current.GetUrls(Settings));

		public override bool Hidden => IsHttps || OSInfo.IsWindows;

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if ((Installer.Current.Settings.Installer.IsUnattended || IsHttps) && AllowMoveNext)
				Wizard.GoNext();
		}
	}
}
