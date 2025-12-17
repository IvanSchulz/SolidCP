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

﻿using System;
using System.Collections.Generic;

namespace FuseCP.Providers.HostedSolution
{
    public abstract class BaseReport<T> where T : BaseStatistics
	{
		private List<T> items = new List<T>();

		public List<T> Items
		{
			get { return items; }
		}

        public abstract string ToCSV();
		
		/// <summary>
		/// Converts source string into CSV string.
		/// </summary>
		/// <param name="source">Source string.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(string source)
		{
			string ret = source;
			if (!string.IsNullOrEmpty(source))
			{
				if (source.IndexOf(',') >= 0 || source.IndexOf('"') >= 0)
					ret = "\"" + source.Replace("\"", "\"\"") + "\"";
			}
			return ret;
		}

		/// <summary>
		/// Converts DateTime string into CSV string.
		/// </summary>
		/// <param name="date">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(DateTime date)
		{
			string ret = string.Empty;
			if (date != DateTime.MinValue)
			{
				ret = date.ToString("G");
			}
			return ToCsvString(ret);
		}

		/// <summary>
		/// Converts long value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(long val)
		{
			return ToCsvString(val.ToString());
		}

		/// <summary>
		/// Converts int value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(int val)
		{
			return ToCsvString(val.ToString());
		}

		/// <summary>
		/// Converts unlimited int value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string UnlimitedToCsvString(int val)
		{
			string ret = string.Empty;
			ret = (val == -1) ? "Unlimited" : val.ToString();
			return ToCsvString(ret);
		}

		/// <summary>
		/// Converts unlimited long value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string UnlimitedToCsvString(long val)
		{
			string ret = string.Empty;
			ret = (val == -1) ? "Unlimited" : val.ToString();
			return ToCsvString(ret);
		}

		/// <summary>
		/// Converts unlimited long value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string UnlimitedToCsvString(double val)
		{
			string ret = string.Empty;
			ret = (val == -1d) ? ToCsvString("Unlimited") : ToCsvString(val);
			return ret;
		}

		/// <summary>
		/// Converts double value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(double val)
		{
			return ToCsvString(val.ToString("F"));
		}

		/// <summary>
		/// Converts boolean value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(bool val, string trueValue, string falseValue)
		{
			return ToCsvString(val ? trueValue : falseValue);
		}

		/// <summary>
		/// Converts boolean value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(bool val)
		{
			return ToCsvString(val, "Enabled", "Disabled");
		}

		/// <summary>
		/// Converts value into CSV string.
		/// </summary>
		/// <param name="val">Value.</param>
		/// <returns>CSV string.</returns>
		protected static string ToCsvString(ExchangeAccountType val)
		{
			string ret = string.Empty;
			switch (val)
			{
				case ExchangeAccountType.Contact:
					ret = "Contact";
					break;
				case ExchangeAccountType.DistributionList:
					ret = "Distribution List";
					break;
				case ExchangeAccountType.Equipment:
					ret = "Equipment Mailbox";
					break;
				case ExchangeAccountType.Mailbox:
					ret = "User Mailbox";
					break;
				case ExchangeAccountType.PublicFolder:
					ret = "Public Folder";
					break;
				case ExchangeAccountType.Room:
					ret = "Room Mailbox";
					break;
				case ExchangeAccountType.User:
					ret = "User";
					break;
				default:
					ret = "Undefined";
					break;

			}
			return ToCsvString(ret);
		}

	}
}
