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
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.Net;
using System.IO;

namespace FuseCP.Tests
{
	public class Certificate
	{
		public const string Password = "123456";
		public const string CertFile = "localhost.pfx";

		public static string CertFilePath {
			get {
				var certfile = Path.Combine(Paths.Test, "Initialization", CertFile);
				var asm = Assembly.GetExecutingAssembly();
				var resxs = asm.GetManifestResourceNames();
				var localhostPfx = resxs.FirstOrDefault(r => r.EndsWith(CertFile));
				var stream = asm.GetManifestResourceStream(localhostPfx);
				if (stream == null)
					throw new FileNotFoundException($"Resource {localhostPfx} not found in assembly {asm.FullName}");
				using (var file = File.Create(certfile))
				using (stream)
					stream.CopyTo(file);
				
				return certfile;
			}
		}
		public static void Install(string certfile, string password)
		{
			X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadWrite);
			X509Certificate2 cert = new X509Certificate2(certfile, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
			if (!store.Certificates.OfType<X509Certificate2>()
				.Any(c => c.Thumbprint == cert.Thumbprint))
			{
				store.Add(cert);
			}
			store.Close();
		}

		public static void InstallLocalhostIntoMy()
		{
			var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
			var mystore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadWrite);
			mystore.Open(OpenFlags.ReadWrite);
			var certs = store.Certificates.Find(X509FindType.FindBySubjectName, "localhost", true);
			foreach (var cert in certs)
			{
				if (!mystore.Certificates.OfType<X509Certificate2>()
					.Any(c => c.Thumbprint == cert.Thumbprint))
				{
					try
					{
						mystore.Add(cert);
					}
					catch { }
				}
			}

			mystore.Close();
			store.Close();
		}

		public static void Remove(string certfile, string password)
		{
			X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadWrite);
			X509Certificate2 cert = new X509Certificate2(certfile, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
			var storecert = store.Certificates.OfType<X509Certificate2>()
				.FirstOrDefault(c => c.Thumbprint == cert.Thumbprint);
			if (storecert != null) store.Remove(storecert);
			store.Close();
		}

		public static void Install() => Install(CertFilePath, Password);

		public static void Remove() => Remove(CertFilePath, Password);

		public static void TrustAll()
		{
			// Always trust certificates
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
			Web.Clients.ClientBase.TrustAllCertificates = true;
			InstallLocalhostIntoMy();
		}
	}
}
