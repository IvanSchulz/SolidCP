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
using System.Reflection;
using FuseCP.Web.Clients;

namespace FuseCP.Tests
{
	public class Server
	{
		public const string AssemblyUrl = $"assembly://{Paths.App}.Server";
		public const string Password = "cRDtpNCeBiql5KOQsKVyrA0sAiA=";
		public static void Init()
		{
			AssemblyLoader.Init(@$"..\{Paths.App}.Server\bin;..\{Paths.App}.Server\bin\Lazy;..\{Paths.App}.Server\bin\netstandard", "none", false);

			try
			{
				var aserver = Assembly.Load("FuseCP.Server");
				if (aserver != null)
				{
					var validatorType = aserver.GetType("FuseCP.Server.PasswordValidator");
					var init = validatorType.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);
					init.Invoke(null, new object[0]);
				}
			}
			catch (Exception ex) { }

		}
	}
}
