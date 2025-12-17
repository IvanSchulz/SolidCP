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
	partial class InsecureHttpWarningPage
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsecureHttpWarningPage));
			this.grpWebSiteSettings = new System.Windows.Forms.GroupBox();
			this.lblWebSiteDomain = new System.Windows.Forms.Label();
			this.grpWebSiteSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpWebSiteSettings
			// 
			this.grpWebSiteSettings.Controls.Add(this.lblWebSiteDomain);
			this.grpWebSiteSettings.Location = new System.Drawing.Point(30, 27);
			this.grpWebSiteSettings.Name = "grpWebSiteSettings";
			this.grpWebSiteSettings.Size = new System.Drawing.Size(396, 141);
			this.grpWebSiteSettings.TabIndex = 0;
			this.grpWebSiteSettings.TabStop = false;
			this.grpWebSiteSettings.Text = "Insecure Http Warning";
			// 
			// lblWebSiteDomain
			// 
			this.lblWebSiteDomain.Location = new System.Drawing.Point(30, 30);
			this.lblWebSiteDomain.Name = "lblWebSiteDomain";
			this.lblWebSiteDomain.Size = new System.Drawing.Size(340, 85);
			this.lblWebSiteDomain.TabIndex = 4;
			this.lblWebSiteDomain.Text = resources.GetString("lblWebSiteDomain.Text");
			// 
			// InsecureHttpWarningPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpWebSiteSettings);
			this.Name = "InsecureHttpWarningPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.grpWebSiteSettings.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpWebSiteSettings;
		private System.Windows.Forms.Label lblWebSiteDomain;
	}
}
