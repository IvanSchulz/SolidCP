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
using System.IO;
using System.Data;
using System.Collections;
using System.Web;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace FuseCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for Utils.
    /// </summary>
    public class Utils
    {
        public static int ParseInt(string val, int defaultValue)
        {
            int result = defaultValue;
            try { result = Int32.Parse(val); }
            catch { /* do nothing */ }
            return result;
        }

        public static double ParseDouble(string val, double defaultValue)
        {
            double result = defaultValue;
            try { result = Double.Parse(val); }
            catch { /* do nothing */ }
            return result;
        }

        public static bool ParseBool(object val, bool defaultValue)
        {
            bool result = defaultValue;
            try { result = Boolean.Parse(val.ToString()); }
            catch { /* do nothing */ }
            return result;
        }


        public static bool ParseBool(string val, bool defaultValue)
        {
            bool result = defaultValue;
            try { result = Boolean.Parse(val); }
            catch { /* do nothing */ }
            return result;
        }

        public static decimal ParseDecimal(string val, decimal defaultValue)
        {
            decimal result = defaultValue;
            try { result = Decimal.Parse(val); }
            catch { /* do nothing */ }
            return result;
        }

        public static string[] ParseDelimitedString(string str, params char[] delimiter)
        {
			if (String.IsNullOrEmpty(str))
				return new string[] { };

            string[] parts = str.Split(delimiter);
            ArrayList list = new ArrayList();
            foreach (string part in parts)
                if (part.Trim() != "" && !list.Contains(part.Trim()))
                    list.Add(part);
            return (string[])list.ToArray(typeof(string));
        }


        public static string ReplaceStringVariable(string str, string variable, string value)
        {
            return ReplaceStringVariable(str, variable, value, false);
        }

        public static string ReplaceStringVariable(string str, string variable, string value, bool allowEmptyValue)
        {
            if (allowEmptyValue)
            {
                if (String.IsNullOrEmpty(str)) return str;
            }
            else
            {
                if (String.IsNullOrEmpty(str) || String.IsNullOrEmpty(value))
                    return str;
            }

            Regex re = new Regex("\\[" + variable + "\\]+", RegexOptions.IgnoreCase);
            return re.Replace(str, value);
        }

		public static string CleanIdentifier(string str)
		{
			if (String.IsNullOrEmpty(str))
				return str;

			return Regex.Replace(str, "\\W", "_");
		}

        public static string GetRandomHexString(int length)
        {
            byte[] buf = new byte[length];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buf);

            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < length; i++)
                sb.AppendFormat("{0:X2}", buf[i]);

            return sb.ToString();
        }

        public static string GetRandomString(int length)
        {
            string ptrn = "abcdefghjklmnpqrstwxyzABCDEFGHJKLMNPQRSTWXYZ0123456789";
            StringBuilder sb = new StringBuilder();

            byte[] randomBytes = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1]         << 16 |
                        randomBytes[2]         <<  8 |
                        randomBytes[3];


            Random rnd = new Random(seed);

            for (int i = 0; i < length; i++)
                sb.Append(ptrn[rnd.Next(ptrn.Length - 1)]);

            return sb.ToString();
        }

        public static DateTime ParseDate(object value)
        {
            try
            {
                return (DateTime) value;
            }
            catch(Exception )
            {
                return DateTime.MinValue;
            }
        }
    }
}
