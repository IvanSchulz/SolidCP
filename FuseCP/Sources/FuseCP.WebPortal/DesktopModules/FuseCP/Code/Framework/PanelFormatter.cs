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
using System.Text;
using System.Globalization;
using System.Web;

using FuseCP.EnterpriseServer;

namespace FuseCP.Portal
{
    /// <summary>
    /// Summary description for PanelFormatter.
    /// </summary>
    public class PanelFormatter
    {
		public static string GetYesNo(bool flag)
		{
			return PortalUtils.GetSharedLocalizedString(Utils.ModuleName, "YesNo." + flag.ToString());
		}

		public static string GetLocalizedResourceGroupName(string groupName)
		{
			return PortalUtils.GetSharedLocalizedString(Utils.ModuleName, String.Format("ResourceGroup.{0}", groupName));
		}

        public static string GetDisplaySizeInBytes(long size)
        {
            if (size >= 0x400 && size < 0x100000)
                // kilobytes
                return Convert.ToString((int)Math.Round(((float)size / 1024))) + "K";
            else if (size >= 0x100000 && size < 0x40000000)
                // megabytes
                return Convert.ToString((int)Math.Round(((float)size / 1024 / 1024))) + "M";
            else if (size >= 0x40000000 && size < 0x10000000000)
                // gigabytes
                return Convert.ToString((int)Math.Round(((float)size / 1024 / 1024 / 1024))) + "G";
            else
                return size.ToString();
        }

        public static string GetUserRoleName(int roleId)
        {
            string roleKey = ((UserRole)roleId).ToString();
            return PortalUtils.GetSharedLocalizedString(Utils.ModuleName, "PanelRole." + roleKey);
        }

        public static string GetAccountStatusName(int statusId)
        {
            string statusKey = ((UserStatus)statusId).ToString();
			return PortalUtils.GetSharedLocalizedString(Utils.ModuleName, "AccountStatus." + statusKey);
        }

        public static string GetPackageStatusName(int statusId)
        {
            string statusKey = ((PackageStatus)statusId).ToString();
			return PortalUtils.GetSharedLocalizedString(Utils.ModuleName, "PackageStatus." + statusKey);
        }

        public static string FormatDate(DateTime date)
        {
            return date.ToString("d");
        }

        public static DateTime ParseDate(string strDate, DateTime defValue)
        {
            try
            {
                return DateTime.Parse(strDate);
            }
            catch
            {
                return defValue;
            }
        }

        public static string FormatMoney(decimal val)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.CurrencySymbol = "";

            return val.ToString("C", nfi);
        }

        public static decimal ParseMoney(string val)
        {
            return ParseMoney(val, 0);
        }

        public static decimal ParseMoney(string val, decimal defaultValue)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            decimal result = defaultValue;
            try { result = Decimal.Parse(val, nfi); }
            catch { /* do nothing */ }
            return result;
        }
    }
}
