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

namespace FuseCP.Installer.Controls
{
	partial class ComponentsControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentsControl));
			this.grpDescription = new System.Windows.Forms.GroupBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.grdComponents = new System.Windows.Forms.DataGridView();
			this.btnLoadComponents = new System.Windows.Forms.Button();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colLink = new System.Windows.Forms.DataGridViewLinkColumn();
			this.grpDescription.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdComponents)).BeginInit();
			this.SuspendLayout();
			// 
			// grpDescription
			// 
			this.grpDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpDescription.Controls.Add(this.lblDescription);
			this.grpDescription.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.grpDescription.Location = new System.Drawing.Point(14, 198);
			this.grpDescription.Name = "grpDescription";
			this.grpDescription.Size = new System.Drawing.Size(379, 117);
			this.grpDescription.TabIndex = 2;
			this.grpDescription.TabStop = false;
			this.grpDescription.Text = "Description";
			// 
			// lblDescription
			// 
			this.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblDescription.Location = new System.Drawing.Point(3, 17);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(373, 97);
			this.lblDescription.TabIndex = 0;
			// 
			// grdComponents
			// 
			this.grdComponents.AllowUserToAddRows = false;
			this.grdComponents.AllowUserToDeleteRows = false;
			this.grdComponents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grdComponents.BackgroundColor = System.Drawing.SystemColors.Window;
			this.grdComponents.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.grdComponents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colVersion,
            this.colLink});
			this.grdComponents.Location = new System.Drawing.Point(14, 43);
			this.grdComponents.MultiSelect = false;
			this.grdComponents.Name = "grdComponents";
			this.grdComponents.ReadOnly = true;
			this.grdComponents.RowHeadersWidth = 21;
			this.grdComponents.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.grdComponents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grdComponents.Size = new System.Drawing.Size(379, 149);
			this.grdComponents.TabIndex = 1;
			this.grdComponents.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnInstallLinkClick);
			this.grdComponents.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnRowEnter);
			// 
			// btnLoadComponents
			// 
			this.btnLoadComponents.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadComponents.Image")));
			this.btnLoadComponents.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnLoadComponents.Location = new System.Drawing.Point(14, 9);
			this.btnLoadComponents.Name = "btnLoadComponents";
			this.btnLoadComponents.Size = new System.Drawing.Size(170, 28);
			this.btnLoadComponents.TabIndex = 0;
			this.btnLoadComponents.Text = " &View Available Components";
			this.btnLoadComponents.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnLoadComponents.Click += new System.EventHandler(this.OnLoadComponentsClick);
			// 
			// colName
			// 
			this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colName.DataPropertyName = "Component";
			this.colName.HeaderText = "Component Name";
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			// 
			// colVersion
			// 
			this.colVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colVersion.DataPropertyName = "Version";
			this.colVersion.HeaderText = "Version";
			this.colVersion.Name = "colVersion";
			this.colVersion.ReadOnly = true;
			this.colVersion.Width = 67;
			// 
			// colLink
			// 
			this.colLink.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
			this.colLink.DataPropertyName = "Id";
			this.colLink.HeaderText = "";
			this.colLink.MinimumWidth = 60;
			this.colLink.Name = "colLink";
			this.colLink.ReadOnly = true;
			this.colLink.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colLink.Text = "Install";
			this.colLink.TrackVisitedState = false;
			this.colLink.UseColumnTextForLinkValue = true;
			this.colLink.Width = 60;
			// 
			// ComponentsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnLoadComponents);
			this.Controls.Add(this.grpDescription);
			this.Controls.Add(this.grdComponents);
			this.Name = "ComponentsControl";
			this.Size = new System.Drawing.Size(406, 327);
			this.grpDescription.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdComponents)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpDescription;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.DataGridView grdComponents;
		private System.Windows.Forms.Button btnLoadComponents;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colVersion;
		private System.Windows.Forms.DataGridViewLinkColumn colLink;
	}
}
