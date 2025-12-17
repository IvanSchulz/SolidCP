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

namespace FuseCP.Providers.Web.Iis.Extensions
{
    using Handlers;
    using Common;
    using Microsoft.Web.Administration;
    using System.Collections.Generic;

    internal sealed class ExtensionsModuleService : ConfigurationModuleService
    {
        public SettingPair[] GetExtensionsInstalled(ServerManager srvman)
        {
            var settings = new List<SettingPair>();
			var config = srvman.GetApplicationHostConfiguration();

            var handlersSection = (HandlersSection) config.GetSection(Constants.HandlersSection, typeof (HandlersSection));

            var executalbesToLookFor = new[]
            {
                // Perl
                new KeyValuePair<string, string>(Constants.PerlPathSetting, "\\perl.exe"),
                // Php
                new KeyValuePair<string, string>(Constants.Php4PathSetting, "\\php.exe"),
                new KeyValuePair<string, string>(Constants.PhpPathSetting, "\\php-cgi.exe"),
                // Classic ASP
                new KeyValuePair<string, string>(Constants.AspPathSetting, @"\inetsrv\asp.dll"),
                // ASP.NET
                new KeyValuePair<string, string>(Constants.AspNet11PathSetting, @"\Framework\v1.1.4322\aspnet_isapi.dll"),
                new KeyValuePair<string, string>(Constants.AspNet20PathSetting, @"\Framework\v2.0.50727\aspnet_isapi.dll"),
                new KeyValuePair<string, string>(Constants.AspNet40PathSetting, @"\Framework\v4.0.30319\aspnet_isapi.dll"),
                // ASP.NET x64
                new KeyValuePair<string, string>(Constants.AspNet20x64PathSetting, @"\Framework64\v2.0.50727\aspnet_isapi.dll"),
                new KeyValuePair<string, string>(Constants.AspNet40x64PathSetting, @"\Framework64\v4.0.30319\aspnet_isapi.dll"),
            };

            foreach (var handler in handlersSection.Handlers)
            {
                foreach (var valuePair in executalbesToLookFor)
                {
                    var key = valuePair.Key;
                    if (handler.ScriptProcessor.EndsWith(valuePair.Value) && !settings.Exists(s => s.Name == key))
                    {
						settings.Add(new SettingPair{Name = valuePair.Key, Value = handler.ScriptProcessor});
                    }
                }
            }

            return settings.ToArray();
        }
    }
}
