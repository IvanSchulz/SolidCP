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
    //[Designer(typeof(WizardPageDesigner))]
    public class BannerWizardPage : WizardPageBase
    {
        public BannerWizardPage()
        {
            this.description = "Description.";
            this.proceedText = "";
            this.textColor = SystemColors.WindowText;
            this.descriptionColor = SystemColors.WindowText;
            this.Text = "Wizard Page";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (StringFormat format = new StringFormat(StringFormat.GenericDefault))
            {
                using (SolidBrush brush = new SolidBrush(this.ForeColor))
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Far;
                    Rectangle rect = base.ClientRectangle;
                    rect.Inflate(-Control.DefaultFont.Height * 2, 0);
                    e.Graphics.DrawString(this.ProceedText, this.Font, brush, (RectangleF) rect, format);
                }
            }
        }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
                if (base.IsCurrentPage)
                {
                    ((Wizard) base.Parent).Redraw();
                }
            }
        }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Color DescriptionColor
        {
            get
            {
                return this.descriptionColor;
            }
            set
            {
                this.descriptionColor = value;
                if (base.IsCurrentPage)
                {
                    base.Parent.Invalidate();
                }
            }
        }

        [DefaultValue("")]
        public virtual string ProceedText
        {
            get
            {
                return this.proceedText;
            }
            set
            {
                this.proceedText = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "WindowText")]
        public Color TextColor
        {
            get
            {
                return this.textColor;
            }
            set
            {
                this.textColor = value;
                if (base.IsCurrentPage)
                {
                    base.Parent.Invalidate();
                }
            }
        }

        private string description;
        private string proceedText;
        private Color textColor;
        private Color descriptionColor;
    }
}

