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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FuseCP.UniversalInstaller.WinForms
{
    public class IntroductionPage : MarginWizardPage
    {
        public IntroductionPage()
        {
        }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComponentSettings Settings { get; set; }
		protected override void InitializePageInternal()
		{
			string product = Settings.ComponentName;
			if (string.IsNullOrEmpty(product))
				product = "FuseCP";
			this.introductionText = string.Format("This wizard will guide you through the installation of the {0} product.\n\n" +
				"It is recommended that you close all other applications before starting Setup. This will make it possible to update relevant system files without having to reboot your computer.",
				product);
			this.Text = string.Format("Welcome to the {0} Setup Wizard", product);
			
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (Installer.Current.Settings.Installer.IsUnattended && AllowMoveNext)
				Wizard.GoNext();
		}

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (StringFormat format = new StringFormat(StringFormat.GenericDefault))
            {
                using (SolidBrush brush = new SolidBrush(this.ForeColor))
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    e.Graphics.DrawString(this.IntroductionText, this.Font, brush, (RectangleF) base.ClientRectangle, format);
                }
            }
        }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string IntroductionText
        {
            get
            {
                return this.introductionText;
            }
            set
            {
                this.introductionText = value;
                base.Invalidate();
            }
        }


        private string introductionText;
    }
}

