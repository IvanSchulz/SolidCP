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
using System.Management;

namespace FuseCP.Providers.Utils
{
    /// <summary>
    /// Summary description for WmiHelper.
    /// </summary>
    public class WmiHelper
    {
        // namespace
        string ns = null;
        ManagementScope scope = null;

        public WmiHelper(string ns)
        {
            this.ns = ns;
        }

        public ManagementObjectCollection ExecuteQuery(string query)
        {
            ObjectQuery objectQuery = new ObjectQuery(query);

            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(WmiScope, objectQuery);
            return searcher.Get();
        }

        // execute wmi query with parameters (taken from internal class Wmi used in Hyper-V provider)
        public ManagementObjectCollection ExecuteWmiQuery(string query, params object[] args)
        {
            if (args != null && args.Length > 0)
                query = String.Format(query, args);

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(WmiScope, new ObjectQuery(query));
            return searcher.Get();
        }

        //(taken from internal class Wmi used in Hyper-V provider)
        public ManagementObject GetWmiObject(string className)
        {
            return GetWmiObject(className, null);
        }

        //(taken from internal class Wmi used in Hyper-V provider)
        public ManagementObjectCollection GetWmiObjects(string className, string filter, params object[] args)
        {
            string query = "select * from " + className;
            if (!String.IsNullOrEmpty(filter))
                query += " where " + filter;
            return ExecuteWmiQuery(query, args);
        }

        //(taken from internal class Wmi used in Hyper-V provider)
        public ManagementObject GetWmiObject(string className, string filter, params object[] args)
        {
            ManagementObjectCollection col = GetWmiObjects(className, filter, args);
            ManagementObjectCollection.ManagementObjectEnumerator enumerator = col.GetEnumerator();
            return enumerator.MoveNext() ? (ManagementObject)enumerator.Current : null;
        }

        public ManagementClass GetClass(string path)
        {
            return new ManagementClass(WmiScope, new ManagementPath(path), null);
        }

        public ManagementObject GetObject(string path)
        {
            return new ManagementObject(WmiScope, new ManagementPath(path), null);
        }

        private ManagementScope WmiScope
        {
            get
            {
                if (scope == null)
                {
                    ManagementPath path = new ManagementPath(ns);
                    scope = new ManagementScope(path, new ConnectionOptions());
                    scope.Connect();
                }
                return scope;
            }
        }

		public ManagementObject CreateInstance(string path)
		{
			ManagementClass objClass = GetClass(path);
			return objClass.CreateInstance();
		}
    }
}
