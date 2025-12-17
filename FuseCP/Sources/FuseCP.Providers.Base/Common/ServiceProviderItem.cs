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
using System.Collections.Specialized;
using System.Xml.Serialization;
using FuseCP.Providers.Common;
using System.Runtime.Serialization;

namespace FuseCP.Providers
{
	/// <summary>
	/// Summary description for ServiceProviderItem.
	/// </summary>
	[Serializable]
    [DataContract]
	public abstract class ServiceProviderItem 
	{
		private int id;
        private int typeId = 0;
		private int packageId = -1;
		private int serviceId = -1;
		private string name;
        private string[] properties;
        private string groupName;

        private StringDictionary propsHash = null;

		public ServiceProviderItem()
		{
		}

        [DataMember]
		public int Id
		{
			get { return id; }
			set { id = value; }
		}

        [DataMember]
        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        [DataMember]
		public int PackageId
		{
			get { return packageId; }
			set { packageId = value; }
		}

        [DataMember]
		public int ServiceId
		{
			get { return serviceId; }
			set { serviceId = value; }
		}

        [DataMember]
		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}

        [DataMember]
        public string GroupName
        {
            get { return this.groupName; }
            set { this.groupName = value; }
        }

        [DataMember]
        public DateTime CreatedDate
        {
            get;
            set;
        }

        [DataMember]
        public string[] Properties
        {
            get
            {
                if (propsHash == null)
                    return null;

                properties = new string[propsHash.Count];
                int i = 0;
                foreach (string key in propsHash.Keys)
                    properties[i++] = key + "=" + propsHash[key];

                return properties;
            }
            set
            {
                if (value == null)
                    return;

                properties = value;

                // fill hash
                propsHash = new StringDictionary();
                foreach (string pair in value)
                {
                    int idx = pair.IndexOf('=');
                    string name = pair.Substring(0, idx);
                    string val = pair.Substring(idx + 1);
                    propsHash.Add(name, val);
                }
            }
        }

        [XmlIgnore]
        public string this[string propertyName]
        {
            get
            {
                if (propsHash == null)
                    propsHash = new StringDictionary();

                return propsHash[propertyName];
            }
            set
            {
                if (propsHash == null)
                    propsHash = new StringDictionary();

                propsHash[propertyName] = value;
            }
        }

		public T GetValue<T>(string propertyName)
		{
			string strValue = this[propertyName];
			//
			if (String.IsNullOrEmpty(strValue))
				return default(T);
			//
			return (T)Convert.ChangeType(strValue, typeof(T));
		}

		public void SetValue<T>(string propertyName, T propertyValue)
		{
			this[propertyName] = Convert.ToString(propertyValue);
		}
	}
}
