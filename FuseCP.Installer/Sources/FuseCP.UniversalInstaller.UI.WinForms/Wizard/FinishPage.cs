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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class FinishPage : BannerWizardPage
	{
		public FinishPage()
		{
			InitializeComponent();
		}

		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);
			if (Installer.Current.HasError)
			{
				this.Text = "Setup failed";
				txtLog.Text = Installer.Current.Error.SourceException.ToString();
				//ParentForm.DialogResult = DialogResult.Abort;
			} else
			{
				this.Text = "Setup complete";
				this.txtLog.Text = string.Join(Environment.NewLine,
					new[] { "The Installer has:" }
					.Concat(Installer.Current.InstallLogs.SelectMany(log =>
					{
						bool first = true;
						return log.Split('\n')
							.Where(line => !string.IsNullOrWhiteSpace(line))
							.Select(line =>
							{
								line = line.Trim();
								line = first ? $"- {line}" : $"  {line}";
								first = false;
								return line;
							});
					})));
				//ParentForm.DialogResult = DialogResult.OK;
			}
			this.Description = "Click Finish to exit the wizard.";
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
		
		private void OnLinkClicked(object sender, LinkClickedEventArgs e)
		{
			var startInfo = new ProcessStartInfo(e.LinkText);
			startInfo.UseShellExecute = true;
			startInfo.Verb = "open";
			Process.Start(startInfo);
		}

		private void OnViewLogLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Log.ShowLogFile();
		}
	}
}
