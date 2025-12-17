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
	/// Summary description for MailForwardingItem.
	/// </summary>
	[Serializable]
	public class MailAlias : MailAccount
	{
	    private string forwardTo;
        private bool includealldomainusers;
	    public string ForwardTo
		{
			get { return this.forwardTo; }
			set { this.forwardTo = value; }
		}

        public bool IncludeAllDomainUsers
        {
            get { return this.includealldomainusers; }
            set { this.includealldomainusers = value; }
        }
    }
}
