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
using System.Xml;

namespace FuseCP.Setup
{
	/// <summary>
	/// Shows rollback process.
	/// </summary>
	public sealed class RollBackProcess
	{
		private ProgressBar progressBar;
		
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		/// <param name="bar">Progress bar.</param>
		public RollBackProcess(ProgressBar bar)
		{
			this.progressBar = bar;
		}

		/// <summary>
		/// Runs rollback process.
		/// </summary>
		internal void Run()
		{
			progressBar.Minimum = 0;
			progressBar.Maximum = 100;
			progressBar.Value = 0;

			int actions = RollBack.Actions.Count;
			for ( int i=0; i<actions; i++ )
			{
				//reverse order
				XmlNode action = RollBack.Actions[actions-i-1];
				try
				{
					RollBack.ProcessAction(action);
				}
				catch
				{}
				progressBar.Value = Convert.ToInt32(i*100/actions);
			}
		}
	}
}
