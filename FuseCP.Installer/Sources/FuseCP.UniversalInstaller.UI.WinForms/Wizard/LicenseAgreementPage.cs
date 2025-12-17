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
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FuseCP.Providers.OS;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class LicenseAgreementPage : BannerWizardPage
	{
		private string nextText;
		public LicenseAgreementPage()
		{
			InitializeComponent();
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();

			this.Text = "License Agreement";
			this.Description = "Please review the license terms before installing the product";

			string resource = OSInfo.IsWindows ? "FuseCP.UniversalInstaller.Resources.EULA.rtf" : "FuseCP.UniversalInstaller.Resources.EULA.Unix.rtf";

			try
			{
				var asm = GetType().Assembly;
				using (Stream stream = asm.GetManifestResourceStream(resource))
				{
					using (StreamReader sr = new StreamReader(stream))
					{
						this.txtLicense.Rtf = sr.ReadToEnd();
					}
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("License agreement error", ex);
			}

		}

		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);
			nextText = this.Wizard.NextText;
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			this.Wizard.NextText = "I &Agree";
			//unattended setup
			if (Installer.Current.Settings.Installer.IsUnattended && AllowMoveNext)
				Wizard.GoNext();
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			this.Wizard.NextText = nextText;
			base.OnBeforeMoveNext(e);
		}

		protected internal override void OnBeforeMoveBack(CancelEventArgs e)
		{
			this.Wizard.NextText = nextText;
			base.OnBeforeMoveBack(e);
		}
	}
}
