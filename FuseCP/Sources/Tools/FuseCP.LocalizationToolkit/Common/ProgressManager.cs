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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.LocalizationToolkit
{
	internal class ProgressManager
	{
		private Form form;
		private ToolStripStatusLabel label;

		public ProgressManager(Form form, ToolStripStatusLabel label)
		{
			this.form = form;
			this.label = label;
		}

		public void StartProgress(string title)
		{
			form.Cursor = Cursors.WaitCursor;
			label.Text = title;
			label.Owner.Refresh();
			form.Update();
		}

		public void FinishProgress()
		{
			form.Cursor = Cursors.Default;
			label.Text = "Ready";
			label.Owner.Refresh();
			form.Update();
		}
	}

}
