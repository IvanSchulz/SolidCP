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
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.WebPortal
{
    public class PageModule
    {
        private int moduleId;
        private string title;
        private string moduleDefinitionID;
        private string iconFile;
        private string containerSrc;
		private string adminContainerSrc;
        private List<string> viewRoles = new List<string>();
        private List<string> editRoles = new List<string>();
        private List<string> readOnlyRoles = new List<string>();
        private Hashtable settings = new Hashtable();
		private XmlDocument xmlModuleData = new XmlDocument();
        private PortalPage page;

        public string ModuleDefinitionID
        {
            get { return this.moduleDefinitionID; }
            set { this.moduleDefinitionID = value; }
        }

        public string IconFile
        {
            get { return this.iconFile; }
            set { this.iconFile = value; }
        }

        public string ContainerSrc
        {
            get { return this.containerSrc; }
            set { this.containerSrc = value; }
        }

		public string AdminContainerSrc
		{
			get { return this.adminContainerSrc; }
			set { this.adminContainerSrc = value; }
		}

        public List<string> ViewRoles
        {
            get { return this.viewRoles; }
        }

        public List<string> EditRoles
        {
            get { return this.editRoles; }
        }

        public List<string> ReadOnlyRoles
        {
            get { return this.readOnlyRoles; }
        }


        public Hashtable Settings
        {
            get { return this.settings; }
        }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public int ModuleId
        {
            get { return this.moduleId; }
            set { this.moduleId = value; }
        }

        public PortalPage Page
        {
            get { return this.page; }
            set { this.page = value; }
        }

		public XmlNodeList SelectNodes(string xpath)
		{
            if(xmlModuleData.DocumentElement == null)
                return null;

			return xmlModuleData.DocumentElement.SelectNodes(xpath);
		}

		public void LoadXmlModuleData(string xml)
		{
			try
			{
				xmlModuleData.LoadXml(xml);
			}
			catch
			{
			}
		}
    }
}
