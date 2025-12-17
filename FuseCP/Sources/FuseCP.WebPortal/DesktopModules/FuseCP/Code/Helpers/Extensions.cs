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
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace FuseCP.Portal.Code.Helpers
{
	public static class Extensions
	{
		/// <summary>
		/// Sets active view by the specifed id
		/// </summary>
		/// <param name="mv"></param>
		/// <param name="viewId"></param>
		public static void SetActiveViewById(this MultiView mv, string viewId)
		{
			foreach (View tab in mv.Views)
			{
				if (tab.ID.Equals(viewId))
				{
					mv.SetActiveView(tab);
					//
					break;
				}
			}
		}

		/// <summary>
		/// Filters tabs list by hosting plan quotas assigned to the package.
		/// </summary>
		/// <param name="tl"></param>
		/// <param name="packageId"></param>
		/// <returns></returns>
		public static IEnumerable<Tab> FilterTabsByHostingPlanQuotas(this List<Tab> tl, int packageId)
		{
			return from t in tl where String.IsNullOrEmpty(t.Quota)
							|| PackagesHelper.CheckGroupQuotaEnabled(packageId, t.ResourceGroup, t.Quota)
						select t;
		}


        /// <summary>
        /// Adds the specified parameter to the Query String.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramName">Name of the parameter to add.</param>
        /// <param name="paramValue">Value for the parameter to add.</param>
        /// <returns>Url with added parameter.</returns>
        public static Uri AddParameter(this Uri url, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = query.ToString();

            return new Uri(uriBuilder.ToString());
        }
	}

	public class Tab
		{
			/// <summary>
			/// Gets or sets index of the tab
			/// </summary>
			public int Index { get; set; }

			/// <summary>
			/// Gets or sets id assosicated with the tab
			/// </summary>
			public string Id { get; set; }

			/// <summary>
			/// Gets or sets localized name of the tab
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Gets or sets name of a hosting plan quota associated with the tab
			/// </summary>
			public string Quota { get; set; }

			/// <summary>
			/// Gets or sets resource group of a hosting plan quota associated with the tab
			/// </summary>
			public string ResourceGroup { get; set; }

			/// <summary>
			/// Gets or sets resource key associated with the tab for localization purposes
			/// </summary>
			public string ResourceKey { get; set; }

			/// <summary>
			/// Gets or sets identificator of the view control associated with the tab
			/// </summary>
			public string ViewId { get; set; }
		}
}
