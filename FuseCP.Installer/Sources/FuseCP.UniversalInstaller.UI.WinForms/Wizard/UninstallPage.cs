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
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FuseCP.Providers.OS;
using FuseCP.UniversalInstaller;
using FuseCP.UniversalInstaller.Core;
using FuseCP.EnterpriseServer.Data;

namespace FuseCP.UniversalInstaller.WinForms;

public partial class UninstallPage : BannerWizardPage
{
	private Thread thread;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public CommonSettings Settings { get; set; }
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Action Action { get; set; }
	public UninstallPage()
	{
		InitializeComponent();
	}

	protected override void InitializePageInternal()
	{
		string name = Settings.ComponentName;
		this.Text = string.Format("Uninstalling {0}", name);
		this.Description = string.Format("Please wait while {0} is being uninstalled.", name);
		this.AllowMoveBack = false;
		this.AllowMoveNext = false;
		this.AllowCancel = false;
	}

	protected internal override void OnAfterDisplay(EventArgs e)
	{
		base.OnAfterDisplay(e);
		thread = new Thread(new ThreadStart(this.Start));
		thread.Start();
	}

	/// <summary>
	/// Displays process progress.
	/// </summary>
	public void Start()
	{
		this.progressBar.Value = 0;

		string component = Settings.ComponentName;
		string componentId = Settings.ComponentCode;
		Version iisVersion = OSInfo.IsWindows ? OSInfo.Windows.WebServer?.Version : null;
		bool iis7 = iisVersion?.Major >= 7;

		try
		{
			this.lblProcess.Text = "Creating uninstall script...";
			this.Update();

			//default actions

			//process actions
			//this.progressBar.Value = progress * 100 / actions.Count;
			this.Update();

			try
			{
				Action?.Invoke();
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Uninstall error", ex);
			}

			this.progressBar.Value = 100;

		}
		catch (Exception ex)
		{
			if (Utils.IsThreadAbortException(ex))
				return;

			ShowError();
			this.Wizard.Close();
		}

		this.lblProcess.Text = "Completed. Click Next to continue.";
		this.AllowMoveNext = true;
		this.AllowCancel = true;
	}
}
