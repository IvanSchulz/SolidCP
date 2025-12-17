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

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class ConfirmUninstallPage : BannerWizardPage
	{
		public ConfirmUninstallPage()
		{
			InitializeComponent();
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComponentSettings Settings { get; set; }
		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "Confirm Removal";
			string name = Settings.ComponentName;
			this.Description = string.Format("Setup Wizard is ready to uninstall {0}.", name);
		}

		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);
			this.txtActions.Text = Installer.Current.GetUninstallLog(Settings);
		}

		/*
		private string GetUninstallActions(string componentId)
		{
			StringBuilder sb = new StringBuilder();
			try
			{
				List<InstallAction> actions = UninstallPage.GetUninstallActions(componentId);
				foreach (InstallAction action in actions)
				{
					sb.AppendLine(action.Log);
				}
				//add external currentScenario
				foreach (InstallAction extAction in UninstallPage.Actions)
				{
					sb.AppendLine(extAction.Log);
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("Uninstall error", ex);
			}
			return sb.ToString();
		}*/
	}
}
