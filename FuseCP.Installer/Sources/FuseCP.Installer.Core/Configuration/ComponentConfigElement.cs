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
	/// Represents a configuration element containing component info.
	/// </summary>
	public class ComponentConfigElement : ConfigurationElement
	{
		/// <summary>
		/// Creates a new instance of the ServerConfigElement class.
		/// </summary>
		public ComponentConfigElement() : this(string.Empty)
		{
		}


		/// <summary>
		/// Creates a new instance of the ServerConfigElement class.
		/// </summary>
		public ComponentConfigElement(string id)
		{
			ID = id;
		}


		[ConfigurationProperty("id", IsRequired = true, IsKey = true)]
		public string ID
		{
			get
			{
				return (string)this["id"];
			}
			set
			{
				this["id"] = value;
			}
		}

		[ConfigurationProperty("settings", IsDefaultCollection = false)]
		public KeyValueConfigurationCollection Settings
		{
			get
			{
				return (KeyValueConfigurationCollection)base["settings"];
			}
		}

		public string GetStringSetting(string key)
		{
			string ret = null;
			if (Settings[key] != null)
			{
				ret = Settings[key].Value;
			}
			return ret;
		}

		public int GetInt32Setting(string key)
		{
			int ret = 0;
			if (Settings[key] != null)
			{
				string val = Settings[key].Value;
				Int32.TryParse(val, out ret);
			}
			return ret;
		}

		public bool GetBooleanSetting(string key)
		{
			bool ret = false;
			if (Settings[key] != null)
			{
				string val = Settings[key].Value;
				Boolean.TryParse(val, out ret);
			}
			return ret;
		}

		/*
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
			//return new ModuleConfigElement();
			return null;
		}*/


		/// <summary>
		/// Creates a new ConfigurationElement. 
		/// </summary>
		/// <param name="elementName"></param>
		/// <returns></returns>
		/*protected override ConfigurationElement CreateNewElement(string elementName)
		{
			return new ModuleConfigElement(elementName);
		}*/

		/*
		/// <summary>
		///  
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		protected override Object GetElementKey(ConfigurationElement element)
		{
			//ModuleConfigElement moduleConfigElement = element as ModuleConfigElement;
			//return userConfigElement.Module;
			return null;
		}*/


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
		/// Gets or sets a child element of this ServerConfigElement object.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		/*public UserConfigElement this[int index]
		{
			get
			{
				return (UserConfigElement)BaseGet(index);
			}
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}*/

	/*	/// <summary>
		/// Gets or sets a child element of this ServerConfigElement object.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		new public UserConfigElement this[string name]
		{
			get
			{
				return (UserConfigElement)BaseGet(name);
			}
		}*/

		/// <summary>
		/// The index of the specified PluginConfigElement.
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		/*public int IndexOf(UserConfigElement connection)
		{
			return BaseIndexOf(connection);
		}*/

		/// <summary>
		/// Adds a PluginConfigElement to the ServerConfigElement instance. 
		/// </summary>
		/// <param name="c"></param>
		/*public void Add(UserConfigElement c)
		{
			BaseAdd(c);

			// Add custom code here.
		}*/
	}
}
