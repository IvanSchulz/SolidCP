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
	[Serializable]
	public class ProviderInfo
	{
		private int providerId;
		private int groupId;
		private string providerName;
		private string editorControl;
		private string displayName;
		private string providerType;
		private bool installed;


		public ProviderInfo()
		{
		}

		public string DisplayName
		{
			get { return this.displayName; }
			set { this.displayName = value; }
		}

		public int ProviderId
		{
			get { return this.providerId; }
			set { this.providerId = value; }
		}

		public int GroupId
		{
			get { return this.groupId; }
			set { this.groupId = value; }
		}

		public string ProviderName
		{
			get { return this.providerName; }
			set { this.providerName = value; }
		}

		public string EditorControl
		{
			get { return this.editorControl; }
			set { this.editorControl = value; }
		}

		public string ProviderType
		{
			get { return this.providerType; }
			set { this.providerType = value; }
		}

		public bool Installed
		{
			get { return this.installed; }
			set { this.installed = value; }
		}
		public string ProviderGroupedName { get; set; }
        public bool DisableAutoDiscovery
        {
            get;
            set;
        }
	}
}
