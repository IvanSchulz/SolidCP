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

namespace FuseCP.Installer.Common
{
	internal class ProgressManager
	{
		private Form form;
		private ToolStripStatusLabel label;
		private Cursor cursor;

		public ProgressManager(Form form, ToolStripStatusLabel label)
		{
			this.form = form;
			this.label = label;
		}

		public void StartProgress(string title)
		{
			cursor = form.Cursor;
			form.Cursor = Cursors.WaitCursor;
			label.Text = title;
			if (label.Owner != null)
				label.Owner.Refresh();
		}

		public void FinishProgress()
		{
			form.Cursor = cursor;
			label.Text = "Ready";
			if ( label.Owner != null ) 
				label.Owner.Refresh();
		}
	}

}
