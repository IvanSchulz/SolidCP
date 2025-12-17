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

namespace FuseCP.WebPortal
{
    public class PortalPage
    {
        private string name;
        private bool enabled;
        private bool hidden;
		private string adminSkinSrc;
        private string skinSrc;
        private List<string> roles = new List<string>();
        private List<PortalPage> pages = new List<PortalPage>();
        private PortalPage parentPage;
        private Dictionary<string, ContentPane> contentPanes = new Dictionary<string, ContentPane>();
		private string url;
        private string target;
        private string align;

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public List<string> Roles
        {
            get { return this.roles; }
        }

        public List<PortalPage> Pages
        {
            get { return this.pages; }
        }

        public PortalPage ParentPage
        {
            get { return this.parentPage; }
            set { this.parentPage = value; }
        }

        public string SkinSrc
        {
            get { return this.skinSrc; }
            set { this.skinSrc = value; }
        }

		public string AdminSkinSrc
		{
			get { return this.adminSkinSrc; }
			set { this.adminSkinSrc = value; }
		}

        public Dictionary<string, ContentPane> ContentPanes
        {
            get { return this.contentPanes; }
        }

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        public bool Hidden
        {
            get { return this.hidden; }
            set { this.hidden = value; }
        }

		public string Url
		{
			get { return this.url; }
			set { this.url = value; }
		}

        public string Target
        {
            get { return this.target; }
            set { this.target = value; }
        }

        public string Align
        {
            get { return this.align; }
            set { this.align = value; }
        }
    }
}
