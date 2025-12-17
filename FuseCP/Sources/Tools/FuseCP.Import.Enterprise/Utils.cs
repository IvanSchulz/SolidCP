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

namespace FuseCP.Import.Enterprise
{
	public class Utils
	{
		private Utils()
		{

		}

		public static int ParseInt(string val, int defaultValue)
		{
			int result;
			if (!Int32.TryParse(val, out result))
			{
				result = defaultValue;
			}
			return result;
		}

		public static bool IsThreadAbortException(Exception ex)
		{
			Exception innerException = ex;
			while (innerException != null)
			{
				if (innerException is System.Threading.ThreadAbortException)
					return true;
				innerException = innerException.InnerException;
			}

			string str = ex.ToString();
			return str.Contains("System.Threading.ThreadAbortException");
		}

		#region DB

		/// <summary>
		/// Converts db value to string
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>string</returns>
		public static string GetDbString(object val)
		{
			string ret = string.Empty;
			if ((val != null) && (val != DBNull.Value))
				ret = (string)val;
			return ret;
		}

		/// <summary>
		/// Converts db value to short
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>short</returns>
		public static short GetDbShort(object val)
		{
			short ret = 0;
			if ((val != null) && (val != DBNull.Value))
				ret = (short)val;
			return ret;
		}

		/// <summary>
		/// Converts db value to int
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>int</returns>
		public static int GetDbInt32(object val)
		{
			int ret = 0;
			if ((val != null) && (val != DBNull.Value))
				ret = (int)val;
			return ret;
		}

		/// <summary>
		/// Converts db value to bool
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>bool</returns>
		public static bool GetDbBool(object val)
		{
			bool ret = false;
			if ((val != null) && (val != DBNull.Value))
				ret = Convert.ToBoolean(val);
			return ret;
		}

		/// <summary>
		/// Converts db value to decimal
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>decimal</returns>
		public static decimal GetDbDecimal(object val)
		{
			decimal ret = 0;
			if ((val != null) && (val != DBNull.Value))
				ret = (decimal)val;
			return ret;
		}


		/// <summary>
		/// Converts db value to datetime
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>DateTime</returns>
		public static DateTime GetDbDateTime(object val)
		{
			DateTime ret = DateTime.MinValue;
			if ((val != null) && (val != DBNull.Value))
				ret = (DateTime)val;
			return ret;
		}

		#endregion
	}
}
