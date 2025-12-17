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
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Specialized;
using System.Data.SqlClient;

using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;
using FuseCP.Providers.OS;
using FuseCP.Providers.ResultObjects;
using FuseCP.UniversalInstaller;
using FuseCP.UniversalInstaller.Web;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class ProgressPage : BannerWizardPage
	{
		private Thread thread;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComponentSettings Settings { get; set; }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Action Action { get; set; }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

		public int Maximum { get => progressBar.Maximum; set => progressBar.Maximum = value; }
		public ProgressPage()
		{
			InitializeComponent();
			Maximum = 1000;
			//
			this.CustomCancelHandler = true;
		}

		delegate void StringCallback(string value);
		delegate void IntCallback(int value);

		private void SetProgressValue(int value)
		{
			//thread safe call
			if (InvokeRequired)
			{
				IntCallback callback = new IntCallback(SetProgressValue);
				Invoke(callback, new object[] { value });
			}
			else
			{
				if (value > 0)
				{
					if (value < 100) value = (Maximum * value / (5 * 100));
					else value = (Maximum / 5) + (int)(Maximum * 0.8 * (1 - Math.Exp(-2 * (double)(value - 100) / Installer.Current.EstimatedOutputLines)));
				}
				if (value > Maximum) value = Maximum;

				if (progressBar.Value != value)
				{
					progressBar.Value = value;
					Update();
				}
			}
		}

		private void SetProgressText(string text)
		{
			//thread safe call
			if (InvokeRequired)
			{
				StringCallback callback = new StringCallback(SetProgressText);
				Invoke(callback, new object[] { text });
			}
			else
			{
				lblProcess.Text = text.Replace("&", "&&");
				Update();
			}
		}
		
		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);
			string name = Settings.ComponentName;
			switch (Installer.Current.Settings.Installer.Action)
			{
				case SetupActions.Install:
					this.Text = string.Format("Installing {0}", name);
					this.Description = string.Format("Please wait while {0} is being installed.", name);
					break;
				case SetupActions.Setup:
					this.Text = string.Format("Configuring {0}", name);
					this.Description = string.Format("Please wait while {0} is being configured.", name);
					break;
				case SetupActions.Update:
					this.Text = string.Format("Updating {0}", name);
					this.Description = string.Format("Please wait while {0} is being updated.", name);
					break;
				case SetupActions.Uninstall:
					this.Text = string.Format("Uninstalling {0}", name);
					this.Description = string.Format("Please wait while {0} is being uninstalled.", name);
					break;
			}
			this.AllowMoveBack = false;
			this.AllowMoveNext = false;
			this.AllowCancel = false;
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			Task.Run(Start, Installer.Current.Cancel.Token);
		}

		bool progressFinished = false;

		/// <summary>
		/// Displays process progress.
		/// </summary>
		public void Start()
		{
			SetProgressValue(0);

			string componentName = Settings.ComponentName;

			int n = 0;
			try
			{
				SetProgressText("Download && Unzip component...");

				var reportProgress = () => SetProgressValue(n++);
				Installer.Current.Log.OnWrite += reportProgress;
				Installer.Current.OnInfo += SetProgressText;
				Installer.Current.OnError += ShowError;

				Installer.Current.WaitForDownloadToComplete();

				Action?.Invoke();

				progressFinished = true;

				Installer.Current.Log.OnWrite -= reportProgress;
				Installer.Current.OnInfo -= SetProgressText;
				Installer.Current.OnError -= ShowError;

				this.progressBar.Value = Maximum;

				SetProgressText("Completed. Click Next to continue.");
			}
			catch (Exception ex)
			{
				if (Installer.Current.Error == null)
				{
					Installer.Current.Error = System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex);
				} else
				{
					ShowError(ex);
				}
				this.progressBar.Value = 0;
				SetProgressText("Installation failed. Click Next to continue.");
				this.AllowMoveNext = true;
				this.AllowCancel = false;
				//ParentForm.DialogResult = DialogResult.Abort;
			}

			this.AllowMoveNext = true;
			this.AllowCancel = false;
			//unattended setup
			if (Installer.Current.Settings.Installer.IsUnattended) Wizard.GoNext();
		}


		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			if (this.Wizard != null)
			{
				this.Wizard.Cancel += new EventHandler(OnWizardCancel);
			}
			Form parentForm = FindForm();
			parentForm.FormClosing += new FormClosingEventHandler(OnFormClosing);
		}

		void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			if (!progressFinished) AbortProcess();
		}

		private void OnWizardCancel(object sender, EventArgs e)
		{
			AbortProcess();
			this.CustomCancelHandler = false;
			Wizard.Close();
		}

		private void AbortProcess()
		{
			Installer.Current.Cancel.Cancel();
		}

		/// <summary>
		/// Displays an error message box with the specified text.
		/// </summary>
		/// <param name="text">The text to display in the message box.</param>
		protected override void ShowError(string text)
		{
			MessageBox.Show(this, text, FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		protected override void ShowError()
		{
			ShowError("An unexpected error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@fusecp.com.\n\n" +
				"Make sure you include a copy of the Installer.log file from the\n" +
				"FuseCP Installer home directory.");
			SetProgressText("Rollback ...");
		}

		bool errorShown = false;
		protected void ShowError(Exception ex)
		{
			if (!errorShown)
			{
				errorShown = true;
				Log.WriteError("Error: ", ex);
				ShowError();
			}
		}
	}
}
