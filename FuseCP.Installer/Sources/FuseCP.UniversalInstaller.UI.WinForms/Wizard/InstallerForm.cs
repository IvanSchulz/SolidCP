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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
#if NETFRAMEWORK
using System.Runtime.Remoting.Lifetime;
#endif

namespace FuseCP.UniversalInstaller.WinForms;

public partial class InstallerForm : Form
{
	public InstallerForm()
	{
		InitializeComponent();
		try
		{
#if NETFRAMEWORK
			LifetimeServices.LeaseTime = TimeSpan.Zero;
#endif
		}
		catch {
		}
		Log.WriteInfo("Setup wizard loaded.");
	}

	private void OnWizardCancel(object sender, EventArgs e)
	{
		Installer.Current.Cancel.Cancel();
		this.DialogResult = DialogResult.Cancel;
		Close();
	}

	private void OnWizardFinish(object sender, EventArgs e)
	{
		if (!Installer.Current.HasError) this.DialogResult = DialogResult.OK;
		else if (!Installer.Current.Cancel.IsCancellationRequested) this.DialogResult = DialogResult.Abort;
		else this.DialogResult = DialogResult.Cancel;
		Close();
	}

	public Wizard Wizard
	{
		get
		{
			return this.wizard;
		}
	}

	private void OnFormClosed(object sender, FormClosedEventArgs e)
	{
		Log.WriteInfo("Setup wizard closed.");
	}
	
	delegate DialogResult ShowModalCallback(IWin32Window owner);

	public DialogResult ShowModal(IWin32Window owner)
	{
		//thread safe call
		if (this.InvokeRequired)
		{
			ShowModalCallback callback = new ShowModalCallback(ShowModal);
			return (DialogResult)this.Invoke(callback, new object[] { owner });
		}
		else
		{
				return this.ShowDialog(owner);
		}
	}
}
