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
using System.Web;
using System.Web.Script.Serialization;

namespace FuseCP.Portal
{
    public class RdsServerStatusHandler : IHttpHandler
    {
        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            string fqdnName = context.Request.Params["fqdnName"];
            string itemIndex = context.Request.Params["itemIndex"];
            string result = ES.Services.RDS.GetRdsServerStatus(null, fqdnName);
            
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(string.Format("{0}:{1}:{2}", result, itemIndex, result.StartsWith("Online", StringComparison.InvariantCultureIgnoreCase)));
            context.Response.ContentType = "text/plain";
            context.Response.Write(json);            
        }
    }
}
