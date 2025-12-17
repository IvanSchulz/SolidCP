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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;
using FuseCP.Providers;
using FuseCP.Providers.OS;
using System.Net;

namespace FuseCP.UniversalInstaller
{
	/// <summary>
	/// Utils class.
	/// </summary>
	public class CryptoUtils
	{

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private CryptoUtils()
		{
		}


		#region Crypting

		static string ComputeHash(string plainText, HashAlgorithm hash) => Cryptor.Hash(plainText, hash);

		/// <summary>
		/// Computes the SHA1 hash value
		/// </summary>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public static string ComputeSHA1(string plainText) => Cryptor.SHA1(plainText);
		public static string ComputeSHA256(string plainText) => Cryptor.SHA256(plainText);
        public static string ComputeSHAServerPassword(string password) => ComputeSHA256(password);
		public static bool IsSHA256(string hash) => Cryptor.IsSHA256(hash);
		public static bool SHAEquals(string plainText, string hash) => Cryptor.SHAEquals(plainText, hash);
		public static string CreateCryptoKey(int len) => Cryptor.CreateCryptoKey(len);
		public static string Encrypt(string key, string str) => new Cryptor(key).Encrypt(str);
		public static string EncryptServer(string key, string secret) => Encrypt(key, secret);
		public static string Decrypt(string key, string Base64String) => new Cryptor(key).Decrypt(Base64String);
		public static string GetRandomString(int length) => Cryptor.GetRandomString(length);
		#endregion


	}
}
