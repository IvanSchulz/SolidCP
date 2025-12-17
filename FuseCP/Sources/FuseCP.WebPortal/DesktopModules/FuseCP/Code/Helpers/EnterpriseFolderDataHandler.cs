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
using System.Web;
using System.Web.Script.Serialization;
using FuseCP.Portal.ExchangeServer;
using FuseCP.Providers.OS;

namespace FuseCP.Portal
{
    public class EnterpriseFolderDataHandler : IHttpHandler
    {
        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            string folderName = context.Request.Params["folderName"];
            string itemIndex = context.Request.Params["itemIndex"];
            int itemId = Convert.ToInt32(context.Request.Params["itemId"]);

            var folder = ES.Services.EnterpriseStorage.GetEnterpriseFolderWithExtraData(itemId, folderName, true) ?? new SystemFile();

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(string.Format("{0}:{1}:{2}", EnterpriseStorageFolders.ConvertMBytesToGB(folder.Size) + " GB", itemIndex, string.IsNullOrEmpty(folder.DriveLetter) ? "not mapped" : folder.DriveLetter));
            context.Response.ContentType = "text/plain";
            context.Response.Write(json);
        }
    }
}
