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

namespace FuseCP.Providers
{
	[Flags]
	[Serializable]
	public enum NTFSPermission : int
	{
		FullControl = 0x1,// = 0x1F01FF,
		Modify = 0x2,// = 0x1301BF,
		//Execute = 0x4,// = 0x1200A9,
		//ListFolderContents = 0x8,// = 0x1200A9,
		Read = 0x10,// = 0x120089,
		Write = 0x20// = 0x100116
	}
}
