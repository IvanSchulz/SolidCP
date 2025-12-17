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
using Microsoft.Win32;

namespace FuseCP.VmConfig
{
	/// <summary>
	/// Registry helper class.
	/// </summary>
	public sealed class RegistryUtils
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private RegistryUtils()
		{
		}

		/// <summary>
		/// Retrieves the specified value from the subkey.
		/// </summary>
		/// <param name="subkey">Subkey.</param>
		/// <param name="name">Name of value to retrieve.</param>
		/// <returns>The data associated with name.</returns>
		public static string GetRegistryKeyStringValue(string subkey, string name)
		{
			string ret = null;
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			if (rk != null)
			{
				ret = (string)rk.GetValue(name, string.Empty);
			}
			return ret;
		}

		/// <summary>
		/// Retrieves the specified value from the subkey.
		/// </summary>
		/// <param name="subkey">Subkey.</param>
		/// <param name="name">Name of value to retrieve.</param>
		/// <returns>The data associated with name.</returns>
		public static int GetRegistryKeyInt32Value(string subkey, string name)
		{
			int ret = 0;
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			if (rk != null)
			{
				ret = (int)rk.GetValue(name, 0);
			}
			return ret;
		}

		/// <summary>
		/// Retrieves the specified value from the subkey.
		/// </summary>
		/// <param name="subkey">Subkey.</param>
		/// <param name="name">Name of value to retrieve.</param>
		/// <returns>The data associated with name.</returns>
		public static bool GetRegistryKeyBooleanValue(string subkey, string name)
		{
			bool ret = false;
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			if (rk != null)
			{
				string strValue = (string)rk.GetValue(name, "False");
				ret = Boolean.Parse(strValue);
			}
			return ret;
		}

		public static bool RegistryKeyExist(string subkey)
		{
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			return (rk != null);
		}


		/// <summary>
		/// Deletes a registry subkey and any child subkeys.
		/// </summary>
		/// <param name="subkey">Subkey to delete.</param>
		public static void DeleteRegistryKey(string subkey)
		{
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			if (rk != null)
				root.DeleteSubKeyTree(subkey);
		}

		/// <summary>
		/// Sets the specified value to the subkey.
		/// </summary>
		/// <param name="subkey">Subkey.</param>
		/// <param name="name">Name of value to store data in.</param>
		/// <param name="value">Data to store. </param>
		public static void SetRegistryKeyStringValue(string subkey, string name, string value)
		{
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.CreateSubKey(subkey);
			if (rk != null)
			{
				rk.SetValue(name, value);
			}
		}

		/// <summary>
		/// Sets the specified value to the subkey.
		/// </summary>
		/// <param name="subkey">Subkey.</param>
		/// <param name="name">Name of value to store data in.</param>
		/// <param name="value">Data to store. </param>
		public static void SetRegistryKeyInt32Value(string subkey, string name, int value)
		{
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.CreateSubKey(subkey);
			if (rk != null)
			{
				rk.SetValue(name, value);
			}
		}

		/// <summary>
		/// Sets the specified value to the subkey.
		/// </summary>
		/// <param name="subkey">Subkey.</param>
		/// <param name="name">Name of value to store data in.</param>
		/// <param name="value">Data to store. </param>
		public static void SetRegistryKeyBooleanValue(string subkey, string name, bool value)
		{
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.CreateSubKey(subkey);
			if (rk != null)
			{
				rk.SetValue(name, value);
			}
		}

		/// <summary>
		/// Return the list of sub keys for the specified registry key.
		/// </summary>
		/// <param name="subkey">The name of the registry key</param>
		/// <returns>The array of subkey names.</returns>
		public static string[] GetRegistrySubKeys(string subkey)
		{
			string[] ret = new string[0];
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			if (rk != null)
				ret = rk.GetSubKeyNames();

			return ret;
		}

		public static int GetSubKeyCount(string subkey)
		{
			int ret = 0;
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			if (rk != null)
				ret = rk.SubKeyCount;

			return ret;
		}

		/// <summary>
		/// Return the list of value names for the specified registry key.
		/// </summary>
		/// <param name="subkey">The name of the registry key</param>
		/// <returns>The array of value names.</returns>
		public static string[] GetRegistryKeyValueNames(string subkey)
		{
			string[] ret = new string[0];
			RegistryKey root = Registry.LocalMachine;
			using (RegistryKey rk = root.OpenSubKey(subkey))
			{
				if (rk != null)
					ret = rk.GetValueNames();
			}
			return ret;
		}

		/// <summary>
		/// Deletes registry key value.
		/// </summary>
		public static void DeleteRegistryKeyValue(string subkey, string valueName)
		{
			RegistryKey root = Registry.LocalMachine;
			using (RegistryKey rk = root.OpenSubKey(subkey, true))
			{
				if (rk != null)
					rk.DeleteValue(valueName);
			}
		}
	}
}
