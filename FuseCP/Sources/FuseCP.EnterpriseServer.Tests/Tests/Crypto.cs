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
using System.Text;
using System.Threading.Tasks;
using FuseCP.EnterpriseServer;

namespace FuseCP.Tests
{
	[TestClass]
	public class Crypto
	{

		[TestMethod]
		public void TestCryptoSshUrl()
		{
			var url = "ssh://test:testpassword@testhost/9015";
			var encurl = CryptoUtils.EncryptServerUrl(url);
			Assert.AreNotEqual<string>(url, encurl);
			Assert.IsTrue(encurl.StartsWith("sshencrypted://"));
			var decurl = CryptoUtils.DecryptServerUrl(encurl);
			Assert.AreEqual<string>(url, decurl);
		}

		[TestMethod]
		public void TestEncryptDecrypt()
		{
			var txt = "Hello World!";
			var entxt = CryptoUtils.Encrypt(txt);
			var detxt = CryptoUtils.Decrypt(entxt);
			Assert.AreEqual<string>(txt, detxt);
		}
	}
}
