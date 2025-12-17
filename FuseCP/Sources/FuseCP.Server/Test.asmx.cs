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

using System.ComponentModel;
using FuseCP.Web.Services;
using FuseCP.Providers;
using System.Linq;

namespace FuseCP.Server
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]

    public class Test : HostingServiceProviderWebService
    {

        // a simple method echoing the message
        [WebMethod]
        public string Echo(string message)
        {
            return message;
        }

        // a method that receives a soap header. The soap header is written to the field settings of the base type HostingServiceProviderWebService.
        // In the client proxy, the soap header can be set by assign the header to  the SoapHeader property. The soap header is set in the client
        // in SoapHeaderMessageInspector in FuseCP.Web.Clients, and is read in the server by SoapHeaderMessageInspector in FuseCP.Web.Services

        [WebMethod, SoapHeader("settings")]
        public string EchoSettings()
        {
            return settings.Settings.FirstOrDefault() ?? string.Empty;
        }

        [WebMethod]
        public void Touch() { }
    }
}
