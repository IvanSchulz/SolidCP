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
using FuseCP.Providers.Virtualization;
using Newtonsoft.Json.Serialization;
using FuseCP.EnterpriseServer;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Specialized;
using FuseCP.EnterpriseServer.Code.Virtualization2012.Helpers.guacamole;

namespace FuseCP.EnterpriseServer.Code.Virtualization2012.Helpers
{
    public class GuacaHelper: ControllerBase
    {
        public GuacaHelper(ControllerBase provider) : base(provider) { }

        public string GetUrl(VirtualMachine vm)
        {

            //string iv = null;
            string[] key;
            string guacaserverurl = null;

            guacadata cookiedata = new guacadata();

            StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);

            try
            {
                key = settings["GuacamoleConnectPassword"].Split(':');
                guacaserverurl = settings["GuacamoleConnectScript"];
                cookiedata.password = settings["GuacamoleHyperVAdministratorPassword"];
                cookiedata.domain = settings["GuacamoleHyperVDomain"];
                cookiedata.hostname = settings["GuacamoleHyperVIP"];
                if (String.IsNullOrEmpty(settings["GuacamoleHyperVUser"]))
                {
                    cookiedata.username = "Administrator";
                }
                else
                {
                    cookiedata.username = settings["GuacamoleHyperVUser"];
                }
            }
            catch
            {
                return "";
            }

            cookiedata.security = "vmconnect";
            cookiedata.protocol = "rdp";
            cookiedata.port = "2179";
            cookiedata.vmhostname = vm.Hostname;
            cookiedata.timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");

            /* Als Ansatz eines pseudorandom IV mit Datum dec values drin.
            string strkey = "";
            foreach (var value in key[1])
            {
                decimal decValue = value;
                strkey = String.Format("{0} {1}", strkey, decValue.ToString());
            }
            */

            cookiedata.preconnectionblob = vm.VirtualMachineId;
            if (cookiedata.hostname == "" || cookiedata.domain == "" || cookiedata.password == "" || cookiedata.preconnectionblob == "") return "";
            string cookie = JsonConvert.SerializeObject(cookiedata);
            try
            {
                //Encryption.GenerateIV(out iv); // Random IV
                string cryptedcookie = Encryption.Encrypt(cookie, key[0], key[1]);
                //string urlstring = UrlEncodeBase64(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}|{1}", cryptedcookie, key[1])))); // Random IV mit �bergeben
                string urlstring = UrlEncodeBase64(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}", cryptedcookie))));
                return String.Format("{0}?e={1}&Resolution=", guacaserverurl, urlstring);
            }
            catch
            {
                return "";
            }

        }

        private static StringDictionary ConvertArrayToDictionary(string[] settings)
        {
            StringDictionary r = new StringDictionary();
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
            }
            return r;
        }

        private static string UrlEncodeBase64(string base64Data)
        {
            return new string(UrlEncodeBase64(base64Data.ToCharArray()));
        }

        private static char[] UrlEncodeBase64(char[] base64Data)
        {
            for (int i = 0; i < base64Data.Length; i++)
            {
                switch (base64Data[i])
                {
                    case '+':
                        base64Data[i] = '-';
                        break;
                    case '/':
                        base64Data[i] = '_';
                        break;
                }
            }
            return base64Data;
        }
    }
}
