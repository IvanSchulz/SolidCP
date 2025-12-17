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
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace FuseCP.Portal.ReportingServices
{
	/// <summary>
	/// This class is used to load resource string related to current module.
	/// </summary>
	public class FuseCPModuleResourceStorage : IResourceStorage
	{
		/// <summary>
		/// The module (control) currently being displayed.
		/// </summary>
		private FuseCPModuleBase module;

		/// <summary>
		/// Cunstructs the instance.
		/// </summary>
		/// <param name="module">Module containing report viewer component.</param>
		/// <exception cref="ArgumentNullException">Whem <paramref name="module"/> is null.</exception>
		public FuseCPModuleResourceStorage(FuseCPModuleBase module)
		{
			if (module == null)
			{
				throw new ArgumentNullException("module");
			}

			this.module = module;
		}

		#region IResourceStorage Members
		/// <summary>
		/// Returns string located in module resource file.
		/// </summary>
		/// <param name="resourceKey">The key, which is used to load string.</param>
		/// <returns>String stored in module resource file.</returns>
		public string GetString(string resourceKey)
		{
			return this.module.GetLocalizedString(resourceKey);
		}

		/// <summary>
		/// Returns shared string located in the global module file.
		/// </summary>
		/// <param name="resourceKey">Key, which will be used to find string in resource file.</param>
		/// <returns>String stored in shared (global) resource file.</returns>
		public string GetSharedString(string resourceKey)
		{
			return this.module.GetSharedLocalizedString(resourceKey);
		}
		#endregion
	}
}
