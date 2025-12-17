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

namespace FuseCP.UniversalInstaller.Core.DotnetHost
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				var stores = CertificateStoreInfo.GetStores();
				foreach (var store in stores)
				{
					Console.WriteLine(store.Name);
					Console.WriteLine(store.Location);
				}
			}
			else if (args.Length == 4)
			{
				StoreName name;
				StoreLocation location;
				X509FindType findType;
				string findValue = args[3];

				if (Enum.TryParse<StoreName>(args[0], out name) &&
					Enum.TryParse<StoreLocation>(args[1], out location) &&
					Enum.TryParse<X509FindType>(args[2], out findType))
				{
					if (CertificateStoreInfo.ExistsDirect(location, name, findType, findValue))
					{
						Console.WriteLine("Certificate found");
						Environment.Exit(0);
					}
					else
					{
						Console.WriteLine("Certificate not found.");
						Environment.Exit(-1);
					}
				}
				else
				{
					Console.WriteLine("Invalid parameters.");
					Environment.Exit(-2);
				}

			}
		}
	}
}
