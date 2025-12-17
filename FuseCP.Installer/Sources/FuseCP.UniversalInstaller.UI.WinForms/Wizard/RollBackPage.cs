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
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FuseCP.UniversalInstaller.WinForms
{
	public partial class RollBackPage : BannerWizardPage
	{
		private Thread thread;
		
		public RollBackPage()
		{
			InitializeComponent();
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CommonSettings Settings { get; set; }

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			string name = Settings.ComponentName;
			this.Text = "Rolling back";
			this.Description = string.Format("Please wait while {0} is being rolled back.", name);
			this.AllowMoveBack = false;
			this.AllowMoveNext = false;
			this.AllowCancel = false;
			thread = new Thread(new ThreadStart(this.Start));
			thread.Start();
		}

		/// <summary>
		/// Displays process progress.
		/// </summary>
		public void Start()
		{
			this.Update();
			try
			{
				this.progressBar.Value = 0;
				this.lblProcess.Text = "Rolling back...";
				Log.WriteStart("Rolling back");
				/*
				Wizard.ActionManager.TotalProgressChanged += new EventHandler<ProgressEventArgs>((object sender, ProgressEventArgs e) =>
				{
					this.progressBar.Value = e.Value;
				});
				*/
				//Wizard.ActionManager.Rollback();
				//
				Log.WriteEnd("Rolled back");
				this.progressBar.Value = 100;
			}
			catch(Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				ShowError();
				this.Wizard.Close();
			}
			this.lblProcess.Text = "Rollback completed. Click Finish to exit setup.";
			this.AllowMoveNext = true;
			this.AllowCancel = true;
		}
	}
}
