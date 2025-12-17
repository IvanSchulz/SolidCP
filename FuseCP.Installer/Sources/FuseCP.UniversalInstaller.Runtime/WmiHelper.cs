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

using System.Management;

namespace FuseCP.UniversalInstaller.Runtime
{
	/// <summary>
	/// Wmi helper class.
	/// </summary>
	internal sealed class WmiHelper
	{
		// namespace
		private string ns = null;
        // scope
		private ManagementScope scope = null;

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		/// <param name="ns">Namespace.</param>
		public WmiHelper(string ns)
		{
			this.ns = ns;
		}

		/// <summary>
		/// Executes specified query.
		/// </summary>
		/// <param name="query">Query to execute.</param>
		/// <returns>Resulting collection.</returns>
		internal ManagementObjectCollection ExecuteQuery(string query)
		{
			ObjectQuery objectQuery = new ObjectQuery(query);

			ManagementObjectSearcher searcher =
				new ManagementObjectSearcher(WmiScope, objectQuery);
			return searcher.Get();
		}

		/// <summary>
		/// Retreives ManagementClass class initialized to the given WMI path.
		/// </summary>
		/// <param name="path">A ManagementPath specifying which WMI class to bind to.</param>
		/// <returns>Instance of the ManagementClass class.</returns>
		internal ManagementClass GetClass(string path)
		{
			return new ManagementClass(WmiScope, new ManagementPath(path), null);
		}

		/// <summary>
		/// Retreives ManagementObject class bound to the specified WMI path.
		/// </summary>
		/// <param name="path">A ManagementPath that contains a path to a WMI object.</param>
		/// <returns>Instance of the ManagementObject class.</returns>
		internal ManagementObject GetObject(string path)
		{
			return new ManagementObject(WmiScope, new ManagementPath(path), null);
		}

		public ManagementScope WmiScope
		{
			get
			{
				if(scope == null)
				{
					ManagementPath path = new ManagementPath(ns);
					scope = new ManagementScope(path, new ConnectionOptions());
					scope.Connect();
				}
				return scope;
			}
		}
	}
}
