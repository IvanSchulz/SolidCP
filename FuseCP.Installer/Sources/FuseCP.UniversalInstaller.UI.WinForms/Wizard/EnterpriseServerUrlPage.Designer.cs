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
	partial class EnterpriseServerUrlPage
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
			this.lblURL = new System.Windows.Forms.Label();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.lblIntro = new System.Windows.Forms.Label();
			this.chkBoxEmbed = new System.Windows.Forms.CheckBox();
			this.lblEnterpriseServerPath = new System.Windows.Forms.Label();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.chooseFolderButton = new System.Windows.Forms.Button();
			this.chkExpose = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// lblURL
			// 
			this.lblURL.Location = new System.Drawing.Point(3, 64);
			this.lblURL.Name = "lblURL";
			this.lblURL.Size = new System.Drawing.Size(139, 23);
			this.lblURL.TabIndex = 13;
			this.lblURL.Text = "Enterprise Server URL:";
			// 
			// txtURL
			// 
			this.txtURL.Location = new System.Drawing.Point(148, 61);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(306, 20);
			this.txtURL.TabIndex = 14;
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
			this.lblIntro.Text = "Please, specify the URL which will be used to access the Enterprise Server from t" +
    "he Portal. Click Next to continue.";
			// 
			// chkBoxEmbed
			// 
			this.chkBoxEmbed.AutoSize = true;
			this.chkBoxEmbed.Location = new System.Drawing.Point(6, 102);
			this.chkBoxEmbed.Name = "chkBoxEmbed";
			this.chkBoxEmbed.Size = new System.Drawing.Size(439, 30);
			this.chkBoxEmbed.TabIndex = 15;
			this.chkBoxEmbed.Text = "Embed Enterprise Server into Portal website (FuseCP runs faster when you em" +
	"bed\r\nEnterprise Server):";
			this.chkBoxEmbed.UseVisualStyleBackColor = true;
			this.chkBoxEmbed.CheckedChanged += new System.EventHandler(this.chkBoxEmbed_CheckedChanged);
			// 
			// lblEnterpriseServerPath
			// 
			this.lblEnterpriseServerPath.AutoSize = true;
			this.lblEnterpriseServerPath.Location = new System.Drawing.Point(3, 143);
			this.lblEnterpriseServerPath.Name = "lblEnterpriseServerPath";
			this.lblEnterpriseServerPath.Size = new System.Drawing.Size(128, 13);
			this.lblEnterpriseServerPath.TabIndex = 16;
			this.lblEnterpriseServerPath.Text = "Path to Enterprise Server:";
			// 
			// txtPath
			// 
			this.txtPath.Location = new System.Drawing.Point(148, 140);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(274, 20);
			this.txtPath.TabIndex = 17;
			// 
			// chooseFolderButton
			// 
			this.chooseFolderButton.Image = global::FuseCP.UniversalInstaller.Properties.Resources.Icons_MenuFileOpenIcon;
			this.chooseFolderButton.Location = new System.Drawing.Point(424, 138);
			this.chooseFolderButton.Name = "chooseFolderButton";
			this.chooseFolderButton.Size = new System.Drawing.Size(30, 23);
			this.chooseFolderButton.TabIndex = 18;
			this.chooseFolderButton.UseVisualStyleBackColor = true;
			this.chooseFolderButton.Click += new System.EventHandler(this.chooseFolderButton_Click);
			// 
			// chkExpose
			// 
			this.chkExpose.AutoSize = true;
			this.chkExpose.Location = new System.Drawing.Point(6, 179);
			this.chkExpose.Name = "chkExpose";
			this.chkExpose.Size = new System.Drawing.Size(210, 17);
			this.chkExpose.TabIndex = 19;
			this.chkExpose.Text = "Expose Enterprise Server Webservices";
			this.chkExpose.UseVisualStyleBackColor = true;
			// 
			// UrlPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkExpose);
			this.Controls.Add(this.chooseFolderButton);
			this.Controls.Add(this.txtPath);
			this.Controls.Add(this.lblEnterpriseServerPath);
			this.Controls.Add(this.chkBoxEmbed);
			this.Controls.Add(this.lblURL);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.lblIntro);
			this.Name = "UrlPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblURL;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.Label lblIntro;
        private System.Windows.Forms.CheckBox chkBoxEmbed;
        private System.Windows.Forms.Label lblEnterpriseServerPath;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button chooseFolderButton;
        private System.Windows.Forms.CheckBox chkExpose;
    }
}
