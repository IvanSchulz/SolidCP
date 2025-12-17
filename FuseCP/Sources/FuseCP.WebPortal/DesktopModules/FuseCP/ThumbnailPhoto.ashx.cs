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
using FuseCP.EnterpriseServer;
using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.Common;
using FuseCP.Providers.ResultObjects;
using System.Drawing;
using System.IO;

namespace FuseCP.Portal
{
    /// <summary>
    /// Summary description for ThumbnailPhoto
    /// </summary>
    public class ThumbnailPhoto : IHttpHandler
    {
        public HttpContext Context = null;

        public int Param(string key)
        {
            string val = Context.Request.QueryString[key];
            if (val == null)
            {
                val = Context.Request.Form[key];
                if (val == null) return 0;
            }

            int res = 0;
            int.TryParse(val, out res);

            return res;
        }



        public void ProcessRequest(HttpContext context)
        {
            Context = context;

            int ItemID = Param("ItemID");
            int AccountID = Param("AccountID");

            BytesResult res = ES.Services.ExchangeServer.GetPicture(ItemID, AccountID);
            if ((res!=null)&&(res.IsSuccess) && (res.Value != null))
            {
                context.Response.ContentType = "image/jpeg";
                context.Response.BinaryWrite(res.Value);
            }
            else
            {
				context.Response.Redirect(PortalUtils.GetThemedImage("empty.gif"), true);
			}
		}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
