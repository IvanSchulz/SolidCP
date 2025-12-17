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
    public class MarginWizardPage : WizardPageBase
    {
        protected MarginWizardPage()
        {
            this.proceedText = "To continue, click Next.";
            this.BackColor = SystemColors.Window;
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
                    e.Graphics.DrawString(this.ProceedText, this.Font, brush, (RectangleF) base.ClientRectangle, format);
                }
            }
        }


        [DefaultValue(typeof(Color), "Window")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                if (base.IsCurrentPage)
                {
                    base.Parent.Invalidate();
                }
            }
        }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                if (base.IsCurrentPage)
                {
                    base.Parent.Invalidate();
                }
            }
        }

        [DefaultValue("To continue, click Next.")]
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


        private string proceedText;
    }
}

