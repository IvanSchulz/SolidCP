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
using FuseCP.UniversalInstaller;

namespace FuseCP.Setup.Web
{
	/// <summary>
	/// Summary description for ServerBinding.
	/// </summary>
	[Serializable]
	public sealed class ServerBinding
	{
		private string ip;
		private string port;
		private string host;
		private string scheme;

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public ServerBinding()
		{
		}

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		/// <param name="ip">IP address.</param>
		/// <param name="port">TCP port.</param>
		/// <param name="host">Host header value.</param>
		public ServerBinding(string ip, string port, string host, string scheme = null, string componentId = null)
		{
			this.ip = ip;
			this.port = port;
			this.host = host;
			this.scheme = scheme ?? 
				((port != "80" && 
				(((componentId == Global.WebPortal.ComponentCode || componentId == Global.WebDavPortal.ComponentCode) &&
					Utils.IsHttps(ip, host)) ||
				Utils.IsHttpsAndNotWindows(ip, host))) ?
				Uri.UriSchemeHttps :
				Uri.UriSchemeHttp);
		}

		/// <summary>
		/// IP address.
		/// </summary>
		public string IP
		{
			get { return ip; }
			set { ip = value; }
		}

		/// <summary>
		/// TCP port.
		/// </summary>
		public string Port
		{
			get { return port; }
			set { port = value; }
		}

		/// <summary>
		/// Host header value.
		/// </summary>
		public string Host
		{
			get { return host; }
			set { host = value; }
		}

		public string Scheme {
			get { return scheme; }
			set { scheme = value; }
		}
	}
}
