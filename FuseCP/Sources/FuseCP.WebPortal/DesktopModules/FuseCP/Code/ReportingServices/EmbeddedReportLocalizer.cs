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
using System.Reflection;
using System.IO;

namespace FuseCP.Portal.ReportingServices
{
	/// <summary>
	/// Class that is used to localize reports embedded to the assembly.
	/// </summary>
	public class EmbeddedReportLocalizer : AbstractReportLocalizer
	{
		#region Data
		/// <summary>
		/// The assembly containing the required report file.
		/// </summary>
		protected Assembly assembly;

		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a localizer.
		/// </summary>
		/// <param name="assembly">The assembly containing the report file.</param>
		/// <param name="embeddedReportName">Embedded report full name: {default namespace}.{folder path inside assembly}.{report name with extension}</param>
		/// <param name="embeddedReportId">Report identifier that will be used to load resource strings related to this report.</param>
		/// <param name="resourceStorage"><see cref="IResourceStorage"/> instance.</param>
		public EmbeddedReportLocalizer(Assembly assembly, string embeddedReportName, string embeddedReportId, IResourceStorage resourceStorage)
			: base(embeddedReportName, embeddedReportId, resourceStorage)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			this.assembly = assembly;
		}

		/// <summary>
		/// Shorter constructor that assume that report file is located in the assembly where this type is declared.
		/// </summary>
		/// <param name="embeddedReportName">Embedded report full name: {default namespace}.{folder path inside assembly}.{report name with extension}</param>
		/// <param name="embeddedReportId">Report identifier that will be used to load resource strings related to this report.</param>
		/// <param name="resourceStorage"><see cref="IResourceStorage"/> instance.</param>
		public EmbeddedReportLocalizer(string embeddedReportName, string embeddedReportId, IResourceStorage resourceStorage)
			: base(embeddedReportName, embeddedReportId, resourceStorage)
		{
			this.assembly = this.GetType().Assembly;
		}
		#endregion

		#region Algorithm Methods overloads
		/// <summary>
		/// Verify whether current assembly contains report file requested in constructor.
		/// </summary>
		/// <returns>True, if file exists. False, if not.</returns>
		public override bool IsReportExists()
		{
			bool result = false;

			foreach (string resourceName in this.assembly.GetManifestResourceNames())
			{
				if (String.Compare(resourceName, this.reportName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					result = true;
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// Returns report file stream.
		/// </summary>
		/// <returns>report file <see cref="Stream"/>.</returns>
		public override Stream GetReportStream()
		{
			return this.assembly.GetManifestResourceStream(this.reportName);
		}
		#endregion
	}

}
