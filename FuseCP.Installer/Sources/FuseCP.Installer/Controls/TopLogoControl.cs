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

using System.Windows.Forms;
using System.Reflection;

namespace FuseCP.Installer
{
	/// <summary>
	/// Top logo
	/// </summary>
	internal partial class TopLogoControl : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the TopLogoControl class.
		/// </summary>
		public TopLogoControl()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			lblVersion.Text = "v" + this.GetType().Assembly.GetName().Version.ToString(3);
		}

		internal void ShowProgress()
		{
			progressIcon.Visible = true;
			progressIcon.StartAnimation();
		}

		internal void HideProgress()
		{
			progressIcon.Visible = false;
			progressIcon.StopAnimation();
		}
	}
}

