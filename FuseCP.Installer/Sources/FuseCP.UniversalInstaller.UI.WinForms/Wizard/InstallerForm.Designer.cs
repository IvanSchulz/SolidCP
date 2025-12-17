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

namespace FuseCP.UniversalInstaller.WinForms
{
	partial class InstallerForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		private Wizard wizard;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallerForm));
			this.wizard = new FuseCP.UniversalInstaller.WinForms.Wizard();
			this.SuspendLayout();
			// 
			// wizard
			// 
			this.wizard.BannerImage = global::FuseCP.UniversalInstaller.Properties.Resources.BannerImage;
			this.wizard.CancelText = "&Cancel";
			this.wizard.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizard.FinishText = "&Finish";
			this.wizard.Location = new System.Drawing.Point(0, 0);
			this.wizard.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.wizard.MarginImage = global::FuseCP.UniversalInstaller.Properties.Resources.MarginImage;
			this.wizard.Name = "wizard";
			this.wizard.Size = new System.Drawing.Size(660, 441);
			this.wizard.TabIndex = 0;
			this.wizard.Cancel += new System.EventHandler(this.OnWizardCancel);
			this.wizard.Finish += new System.EventHandler(this.OnWizardFinish);
			// 
			// InstallerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(660, 441);
			this.Controls.Add(this.wizard);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InstallerForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Setup Wizard";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
			this.ResumeLayout(false);

		}

		#endregion


	}
}

