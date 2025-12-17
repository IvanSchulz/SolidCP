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
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;
using FuseCP.Providers.Web.Iis.Common;

namespace FuseCP.Providers.Web.Compression
{

    internal static class CompressionGlobals
    {
        public const int DynamicCompression = 1;
        public const int StaticCompression = 2;
    }


    internal sealed class CompressionModuleService : ConfigurationModuleService
    {
        public const string DynamicCompression = "doDynamicCompression";
        public const string StaticCompression = "doStaticCompression";

        public PropertyBag GetSettings(ServerManager srvman, string siteId)
        {
            var config = srvman.GetApplicationHostConfiguration();
            //
            var section = config.GetSection(Constants.CompressionSection, siteId);
            //
            PropertyBag bag = new PropertyBag();
            //
            bag[CompressionGlobals.DynamicCompression] = Convert.ToBoolean(section.GetAttributeValue(DynamicCompression));
            bag[CompressionGlobals.StaticCompression] = Convert.ToBoolean(section.GetAttributeValue(StaticCompression));
            //
            return bag;
        }

        public void SetSettings(string virtualPath, bool doDynamicCompression, bool doStaticCompression)
        {
            using (var srvman = GetServerManager())
            {
                var config = srvman.GetApplicationHostConfiguration();
                //
                var section = config.GetSection(Constants.CompressionSection, virtualPath);
                //
                section.SetAttributeValue(DynamicCompression, doDynamicCompression);
                section.SetAttributeValue(StaticCompression, doStaticCompression);

                //
                srvman.CommitChanges();
            }
        }

      
    }
}
