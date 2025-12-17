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

namespace FuseCP.Setup
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
			this.wizard = new FuseCP.Setup.Wizard();
			this.SuspendLayout();
			// 
			// wizard
			// 
			this.wizard.BannerImage = global::FuseCP.Setup.Legacy.Properties.Resources.BannerImage;
			this.wizard.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizard.Location = new System.Drawing.Point(0, 0);
			this.wizard.MarginImage = global::FuseCP.Setup.Legacy.Properties.Resources.MarginImage;
			this.wizard.Name = "wizard";
			this.wizard.SelectedPage = null;
			this.wizard.Size = new System.Drawing.Size(495, 358);
			this.wizard.TabIndex = 0;
			this.wizard.Finish += new System.EventHandler(this.OnWizardFinish);
			this.wizard.Cancel += new System.EventHandler(this.OnWizardCancel);
			// 
			// InstallerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(495, 358);
			this.Controls.Add(this.wizard);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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

