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

namespace FuseCP.Providers.Mail
{
	/// <summary>
	/// Summary description for MailGroupItem.
	/// </summary>
	[Serializable]
	public class MailGroup : ServiceProviderItem
	{
		private string[] members = new string[0];
		private bool enabled;
        private bool hidefromgal;
        private bool enablechat;
        private bool internalonly;
        private bool includealldomainusers;
        private string displayname;
        private bool allowsending;

		public MailGroup()
		{
		}

		public bool Enabled
		{
			get { return this.enabled; }
			set { this.enabled = value; }
		}

		public string[] Members
		{
			get { return this.members; }
			set { this.members = value; }
		}

        public bool HideFromGAL
				{
            get { return this.hidefromgal; }
            set { this.hidefromgal = value; }
        }

        public bool EnableChat
				{
            get { return this.enablechat; }
            set { this.enablechat = value; }
        }
        public bool InternalOnly
				{
            get { return this.internalonly; }
            set { this.internalonly = value; }
        }
        public bool IncludeAllDomainUsers
        {
            get { return this.includealldomainusers; }
            set { this.includealldomainusers = value; }
        }
        public string DisplayName
        {
            get { return this.displayname; }
            set { this.displayname = value; }
        }
        public bool AllowSending
        {
            get { return this.allowsending; }
            set { this.allowsending = value; }
        }
    }
}
