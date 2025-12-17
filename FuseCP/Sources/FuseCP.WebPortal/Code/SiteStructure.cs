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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.WebPortal
{
    public class SiteStructure
    {
        private PageCollection pages = new PageCollection();
        private Dictionary<int, PageModule> modules = new Dictionary<int, PageModule>();

		public PageCollection Pages
        {
            get { return this.pages; }
        }

        public Dictionary<int, PageModule> Modules
        {
            get { return this.modules; }
        }
    }

	public class PageCollection : NameObjectCollectionBase
	{
		public PortalPage this[string name]
		{
			get { return (PortalPage)BaseGet(name); }
			set { BaseAdd(name, value); }
		}

		public PortalPage[] Values
		{
			get { return (PortalPage[])BaseGetAllValues(typeof(PortalPage)); }
		}

		public bool ContainsKey(string key)
		{
			return BaseGet(key) != null;
		}

		public void Add(PortalPage page)
		{
			BaseAdd(page.Name, page);
		}
	}
}
