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

namespace FuseCP.UniversalInstaller
{
	/// <summary>
	/// System user item.
	/// </summary>
	public sealed class SystemUserItem
	{
		private string name;
		private bool system;
		private string fullName;
		private string description;
		private string password;
		private bool passwordCantChange;
		private bool passwordNeverExpires;
		private bool accountDisabled;
		private string domain;
		private string[] memberOf = new string[0];

		/// <summary>
		/// Initializes a new instance of the SystemUserItem class.
		/// </summary>
		public SystemUserItem()
		{
		}

		/// <summary>
		/// Name
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		/// <summary>
		/// System
		/// </summary>
		public bool System
		{
			get { return system; }
			set { system = value; }
		}

		/// <summary>
		/// Full name
		/// </summary>
		public string FullName
		{
			get { return fullName; }
			set { fullName = value; }
		}

		/// <summary>
		/// Description
		/// </summary>
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		/// <summary>
		/// Password
		/// </summary>
		public string Password
		{
			get { return password; }
			set { password = value; }
		}

		/// <summary>
		/// User can't change password
		/// </summary>
		public bool PasswordCantChange
		{
			get { return passwordCantChange; }
			set { passwordCantChange = value; }
		}

		/// <summary>
		/// Password never expires
		/// </summary>
		public bool PasswordNeverExpires
		{
			get { return passwordNeverExpires; }
			set { passwordNeverExpires = value; }
		}

		/// <summary>
		/// Account is disabled
		/// </summary>
		public bool AccountDisabled
		{
			get { return accountDisabled; }
			set { accountDisabled = value; }
		}

		/// <summary>
		/// Member of
		/// </summary>
		public string[] MemberOf
		{
			get { return memberOf; }
			set { memberOf = value; }
		}

		/// <summary>
		/// Organizational Unit
		/// </summary>
		public string Domain
		{
			get { return domain; }
			set { domain = value; }
		}
	}
}
