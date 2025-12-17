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
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using Microsoft.Win32;
using FuseCP.Providers;

namespace FuseCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for CryptoUtils.
	/// </summary>
	public class CryptoUtils
	{
		public static string CryptoKey => ConfigSettings.CryptoKey;
		public static bool EncryptionEnabled => ConfigSettings.EncryptionEnabled;

		static Cryptor cryptor = null;
		public static Cryptor Cryptor => cryptor ??= new Cryptor(CryptoKey); 

		public static string EncryptServerPassword(string password) => Encrypt(password);

		public static string Encrypt(string InputText) => EncryptionEnabled ? Cryptor.Encrypt(InputText) : InputText;
		public static string Decrypt(string InputText) => EncryptionEnabled ? Cryptor.Decrypt(InputText) : InputText;
		public static string SHA1(string plainText) => Cryptor.SHA1(plainText);
		public static string SHA256(string plainText) => Cryptor.SHA256(plainText);
		public static bool IsSHA256(string hash) => Cryptor.IsSHA256(hash);
		public static bool SHAEquals(string plainText, string hash) => Cryptor.SHAEquals(plainText, hash);

		public static string DecryptServerUrl(string serverUrl)
		{
			if (!string.IsNullOrEmpty(serverUrl) && serverUrl.StartsWith("sshencrypted://"))
			{
				serverUrl = Decrypt(serverUrl.Substring("sshencrypted://".Length));
			}
			return serverUrl;
		}

		public static string EncryptServerUrl(string serverUrl)
		{
			if (!string.IsNullOrEmpty(serverUrl) && serverUrl.StartsWith("ssh://")) {
				serverUrl = $"sshencrypted://{Encrypt(serverUrl)}";
			}
			return serverUrl;
		}
	}
}
