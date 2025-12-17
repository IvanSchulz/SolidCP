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
using System.Configuration;

namespace FuseCP.AWStats.Viewer
{
	/// <summary>
	/// Summary description for AuthenticationProvider.
	/// </summary>
	public abstract class AuthenticationProvider
	{
		private static AuthenticationProvider objProvider;

		public static AuthenticationProvider Instance
		{
			get
			{
				if (objProvider == null)
				{
                    string providerType = ConfigurationManager.AppSettings["AWStats.AuthenticationProvider"];
					if (providerType == null || providerType == "")
						throw new Exception("AuthenticationProvider implementation type is not specified");

					try
					{
						// instantiate provider
						objProvider = (AuthenticationProvider)Activator.CreateInstance(Type.GetType(providerType));
					}
					catch (Exception ex)
					{
						throw new Exception(String.Format("Can not instantiate '{0}' authentication provider",
							providerType), ex);
					}
				}
				return objProvider;
			}
		}

		public abstract AuthenticationResult AuthenticateUser(string domain, string username, string password);
	}

	public enum AuthenticationResult
	{
		OK,
		WrongUsername,
		WrongPassword,
		DomainNotFound
	}
}
