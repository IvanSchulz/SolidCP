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

namespace FuseCP.Setup.Web
{
	/// <summary>
	/// Server states.
	/// </summary>
	[Serializable]
	public enum ServerState
	{
		/// <summary>Starting</summary>
		Starting = 1,
		/// <summary>Started</summary>
		Started = 2,
		/// <summary>Stopping</summary>
		Stopping = 3,
		/// <summary>Stopped</summary>
		Stopped = 4,
		/// <summary>Pausing</summary>
		Pausing = 5,
		/// <summary>Paused</summary>
		Paused = 6,
		/// <summary>Continuing</summary>
		Continuing = 7
	}
}
