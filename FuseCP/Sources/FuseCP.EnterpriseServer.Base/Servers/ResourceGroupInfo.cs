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

namespace FuseCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for ResourceGroup.
	/// </summary>
	[Serializable]
	public class ResourceGroupInfo
	{
		private int groupId;
		private string groupName;
		private int groupOrder;
		private string groupController;

		public ResourceGroupInfo()
		{
		}

		public int GroupOrder
		{
			get { return this.groupOrder; }
			set { this.groupOrder = value; }
		}

		public string GroupName
		{
			get { return this.groupName; }
			set { this.groupName = value; }
		}

		public int GroupId
		{
			get { return this.groupId; }
			set { this.groupId = value; }
		}

		public string GroupController
		{
			get { return this.groupController; }
			set { this.groupController = value; }
		}
	}
}
