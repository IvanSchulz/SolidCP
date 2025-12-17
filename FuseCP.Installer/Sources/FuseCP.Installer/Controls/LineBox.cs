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

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FuseCP.Installer
{
	/// <summary>
	/// 3D line box.
	/// </summary>
	internal partial class LineBox : Control
	{
		/// <summary>
		/// Initializes a new instance of the LineBox class.
		/// </summary>
		public LineBox() : base()
		{
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.FixedHeight, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.StandardClick, false);
			SetStyle(ControlStyles.Selectable, false);
			this.TabStop = false;
		}
		
		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Rectangle rectangle = this.ClientRectangle;
			Pen lightPen = new Pen(ControlPaint.Light(this.BackColor, 1));
			Pen darkPen = new Pen(ControlPaint.Dark(this.BackColor, 0));
			graphics.DrawLine(darkPen, rectangle.X, rectangle.Y, rectangle.X+rectangle.Width, rectangle.Y);
			graphics.DrawLine(lightPen, rectangle.X, rectangle.Y+1, rectangle.X+rectangle.Width, rectangle.Y+1);
			base.OnPaint(e);
		}

		/// <summary>
		/// Gets the default size of the control.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(10, 2);
			}
		}
 
		
		/// <summary>
		/// Performs the work of setting the specified bounds of this control.
		/// </summary>
		/// <param name="x">The new Left property value of the control.</param>
		/// <param name="y">The new Right property value of the control.</param>
		/// <param name="width">The new Width property value of the control.</param>
		/// <param name="height">The new Height property value of the control.</param>
		/// <param name="specified">A bitwise combination of the BoundsSpecified values.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, width, 2, specified);
		} 
	}
}
