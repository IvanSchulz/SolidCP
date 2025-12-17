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
using System.Windows.Forms;

namespace FuseCP.UniversalInstaller.Controls
{
	internal class ResultViewControl : UserControl
	{
		private FCPAppContext appContext;
		private bool isInitialized;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FCPAppContext AppContext
		{
			get { return appContext; }
			set { appContext = value; }
		}

		protected bool IsInitialized
		{
			get { return isInitialized; }
			set { isInitialized = value; }
		}

		public virtual void ShowControl(FCPAppContext context)
		{
			this.AppContext = context;
		}
	}
}
