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
using System.IO;

namespace FuseCP.Portal.ReportingServices
{
	/// <summary>
	/// Class that is used to load Reporting Services report files from local file system.
	/// </summary>
	public class FileSystemReportLocalizer : AbstractReportLocalizer
	{
		/// <summary>
		/// Constructs the class to function.
		/// </summary>
		/// <param name="reportName">Actually, the full path to the report file.</param>
		/// <param name="reportIdentifier">
		/// String used to identify report file related strings in resources.
		/// The following string format is used to assosiate report and its strings: {report name}.{localiation id}
		/// </param>
		/// <param name="resourceStorage"><see cref="IResourceStorage"/> instance.</param>
		public FileSystemReportLocalizer(string reportName, string reportIdentifier, IResourceStorage resourceStorage)
			: base(reportName, reportIdentifier, resourceStorage)
		{
		}


		/// <summary>
		/// Verify if report exists for the <code>reportName</code> passed to the constructor
		/// </summary>
		/// <returns>
		/// True, if report file exists. Otherwise, false.
		/// </returns>
		public override bool IsReportExists()
		{
			FileInfo info = new FileInfo(this.reportName);
			if (info.Exists)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns <see cref="Stream"/> one can use to read report file.
		/// </summary>
		/// <returns><see cref="Stream"/> instance for the report file.</returns>
		public override Stream GetReportStream()
		{
			return File.OpenRead(this.reportName);
		}
	}
}
