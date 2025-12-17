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
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;

namespace FuseCP.Import.Enterprise
{
	public enum ActionTypes
	{
		None,
		ImportOrganization,
		ImportOrganizationDomain,
		ImportMailbox,
		ImportContact,
		ImportGroup
	}

	public class ImportAction
	{
		public ImportAction(ActionTypes type)
		{
			this.ActionType = type;
		}

		private ActionTypes actionType;

		public ActionTypes ActionType
		{
			get { return actionType; }
			set { actionType = value; }
		}

		private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string description;

		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		private DirectoryEntry entry;

		public DirectoryEntry DirectoryEntry
		{
			get { return entry; }
			set { entry = value; }
		}

		private string path;

		public string Path
		{
			get { return path; }
			set { path = value; }
		}

		private string userName;

		public string UserName
		{
			get { return userName; }
			set { userName = value; }
		}
	}
}
