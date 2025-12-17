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
	partial class EmbedEnterpriseServerPage
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
			this.lblIntro = new System.Windows.Forms.Label();
			this.chkBoxEmbed = new System.Windows.Forms.CheckBox();
			this.chkExpose = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// lblIntro
			// 
			this.lblIntro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblIntro.Location = new System.Drawing.Point(0, 0);
			this.lblIntro.Name = "lblIntro";
			this.lblIntro.Size = new System.Drawing.Size(457, 58);
			this.lblIntro.TabIndex = 12;
			this.lblIntro.Text = "Please, specify if you want to embed EnterpriseServer into the Portal application" +
    " website. Click Next to continue.";
			// 
			// chkBoxEmbed
			// 
			this.chkBoxEmbed.AutoSize = true;
			this.chkBoxEmbed.Location = new System.Drawing.Point(0, 51);
			this.chkBoxEmbed.Name = "chkBoxEmbed";
			this.chkBoxEmbed.Size = new System.Drawing.Size(232, 17);
			this.chkBoxEmbed.TabIndex = 15;
			this.chkBoxEmbed.Text = "Embed Enterprise Server into Portal website";
			this.chkBoxEmbed.UseVisualStyleBackColor = true;
			this.chkBoxEmbed.CheckedChanged += new System.EventHandler(this.chkBoxEmbed_CheckedChanged);
			// 
			// chkExpose
			// 
			this.chkExpose.AutoSize = true;
			this.chkExpose.Location = new System.Drawing.Point(0, 88);
			this.chkExpose.Name = "chkExpose";
			this.chkExpose.Size = new System.Drawing.Size(210, 17);
			this.chkExpose.TabIndex = 19;
			this.chkExpose.Text = "Expose Enterprise Server Webservices";
			this.chkExpose.UseVisualStyleBackColor = true;
			// 
			// EmbedEnterpriseServerPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkExpose);
			this.Controls.Add(this.chkBoxEmbed);
			this.Controls.Add(this.lblIntro);
			this.Name = "EmbedEnterpriseServerPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label lblIntro;
        private System.Windows.Forms.CheckBox chkBoxEmbed;
        private System.Windows.Forms.CheckBox chkExpose;
    }
}
