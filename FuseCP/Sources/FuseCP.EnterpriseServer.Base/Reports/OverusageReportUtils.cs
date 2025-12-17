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
using System.Text;
using System.Data;

namespace FuseCP.EnterpriseServer.Base.Reports
{
	public static class OverusageReportUtils
	{
		/// <summary>
		/// Returns a long value from <paramref name="value"/>.
		/// </summary>
		/// <param name="value">String representing a number.</param>
		/// <param name="detaultValue">Default value.</param>
		/// <returns>Long value.</returns>
		public static long GetLongValueOrDefault(string value, long detaultValue)
		{
			long result = 0;

			if (!long.TryParse(value, out result))
			{
				result = detaultValue;
			}

			if (result == -1)
			{
				result = long.MaxValue;
			}

			return result;
		}

		/// <summary>
		/// Return string value specified by <paramref name="column"/> or <paramref name="defaultValue"/> in case there is no such column in <see cref="DataRow"/>.
		/// </summary>
		/// <param name="dr"><see cref="DataRow"/> containing data.</param>
		/// <param name="column">Columns name to take data from.</param>
		/// <param name="defaultValue">Default value in case if there is no such <paramref name="column"/></param>
		/// <returns>String value.</returns>
		public static string GetStringOrDefault(DataRow dr, string column, string defaultValue)
		{
			string result = defaultValue;

			if (dr.Table.Columns.Contains(column))
			{
				result = dr[column].ToString();
			}

			return result;
		}
	}
}
