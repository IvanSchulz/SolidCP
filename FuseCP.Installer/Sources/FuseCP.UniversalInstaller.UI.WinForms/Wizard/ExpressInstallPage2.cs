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
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Specialized;
using System.Data.SqlClient;

using FuseCP.EnterpriseServer;
using FuseCP.Providers.Common;
using FuseCP.Providers.ResultObjects;
using FuseCP.UniversalInstaller;
using FuseCP.UniversalInstaller.Web;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class ExpressInstallPage2 : BannerWizardPage
	{
		private Thread thread;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CommonSettings Settings { get; set; }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Action Action { get; set; }
		public ExpressInstallPage2()
		{
			InitializeComponent();
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
				progressBar.Value = value;
				Update();
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
				lblProcess.Text = text;
				Update();
			}
		}
		
		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);
			string name = Settings.ComponentName;
			this.Text = string.Format("Installing {0}", name);
			this.Description = string.Format("Please wait while {0} is being installed.", name);
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
			SetProgressValue(0);

			string componentName = Settings.ComponentName;

			try
			{
				SetProgressText("Creating installation script...");

				Action?.Invoke();

				this.progressBar.Value = 100;
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				return;
			}

			SetProgressText("Completed. Click Next to continue.");
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
			AbortProcess();
		}

		private void OnWizardCancel(object sender, EventArgs e)
		{
			AbortProcess();
			this.CustomCancelHandler = false;
			Wizard.Close();
		}

		private void AbortProcess()
		{
			if (this.thread != null)
			{
				if (this.thread.IsAlive)
				{
					this.thread.Abort();
				}
				this.thread.Join();
			}
		}
	}
}
