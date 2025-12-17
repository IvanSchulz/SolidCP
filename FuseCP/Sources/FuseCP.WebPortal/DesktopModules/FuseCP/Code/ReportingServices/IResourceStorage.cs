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
	/// Base interface for classes that utilize access to the localization resources.
	/// </summary>
	public interface IResourceStorage
	{
		/// <summary>
		/// Should return a string requested by <paramref name="resourceKey"/>
		/// </summary>
		/// <param name="resourceKey"></param>
		/// <returns>String value.</returns>
		string GetString(string resourceKey);

		/// <summary>
		/// Should return a string from shared (global) resource files requested by <paramref name="resourceKey"/>
		/// </summary>
		/// <param name="resourceKey"></param>
		/// <returns></returns>
		string GetSharedString(string resourceKey);
	}
}
