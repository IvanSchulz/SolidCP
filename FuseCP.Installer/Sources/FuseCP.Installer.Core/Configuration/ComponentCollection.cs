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
using System.Collections;
using System.Configuration;
using System.Text;

namespace FuseCP.Installer.Configuration
{
	/// <summary>
	/// Represents <connections> configuration element containing a collection of child elements.
	/// </summary>
	public class ComponentCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// Creates a new instance of the ConnectionCollection class.
		/// </summary>
		public ComponentCollection()
		{
			AddElementName = "component";
		}

		/// <summary>
		/// Gets the type of the ConfigurationElementCollection.
		/// </summary>
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.AddRemoveClearMap;
			}
		}

		/// <summary>
		/// Creates a new ConfigurationElement. 
		/// </summary>
		/// <returns></returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new ComponentConfigElement();
		}


		/// <summary>
		/// Creates a new ConfigurationElement. 
		/// </summary>
		/// <param name="elementName"></param>
		/// <returns></returns>
		protected override ConfigurationElement CreateNewElement(string id)
		{
			return new ComponentConfigElement(id);
		}


		/// <summary>
		///  
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		protected override Object GetElementKey(ConfigurationElement element)
		{
			ComponentConfigElement componentConfigElement = element as ComponentConfigElement;
			return componentConfigElement.ID;
		}


		/*		public new string AddElementName
				{
					get
					{
						return base.AddElementName;
					}
					set
					{
						base.AddElementName = value; 
					}
				}

				public new string ClearElementName
				{
					get
					{ return base.ClearElementName; }

					set
					{ base.AddElementName = value; }

				}

				public new string RemoveElementName
				{
					get
					{ return base.RemoveElementName; }


				}

				public new int Count
				{

					get { return base.Count; }

				}

		*/
		/// <summary>
		/// Gets or sets a child element of this ConnectionCollection object.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public ComponentConfigElement this[int index]
		{
			get
			{
				return (ComponentConfigElement)BaseGet(index);
			}
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}

		/// <summary>
		/// Gets or sets a child element of this ConnectionCollection object.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public new ComponentConfigElement this[string id]
		{
			get
			{
				return (ComponentConfigElement)BaseGet(id);
			}
		}

		/// <summary>
		/// The index of the specified PluginConfigElement.
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		public int IndexOf(ComponentConfigElement element)
		{
			return BaseIndexOf(element);
		}

		/// <summary>
		/// Adds a PluginConfigElement to the ConnectionCollection instance. 
		/// </summary>
		/// <param name="c"></param>
		public void Add(ComponentConfigElement c)
		{
			BaseAdd(c);

			// Add custom code here.
		}

		public void Remove(ComponentConfigElement c)
		{
			if (BaseIndexOf(c) >= 0)
				BaseRemove(c.ID);
		}

		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}

		public void Remove(string id)
		{
			BaseRemove(id);
		}

		public void Clear()
		{
			BaseClear();
			// Add custom code here.
		}
	}
}
