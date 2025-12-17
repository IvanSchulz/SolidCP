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
using System.Runtime.Caching;
using FuseCP.WebDav.Core.Config;
using FuseCP.WebDav.Core.Interfaces.Storages;

namespace FuseCP.WebDav.Core.Storages
{
    public class CacheTtlStorage : ITtlStorage
    {
       private static readonly ObjectCache Cache;

       static CacheTtlStorage()
        {
            Cache = MemoryCache.Default;
        }

        public TV Get<TV>(string id)
        {
            var value = (TV)Cache[id];

            if (!EqualityComparer<TV>.Default.Equals(value, default(TV)))
            {
                SetTtl(id, value);
            }

            return value;
        }

        public bool Add<TV>(string id, TV value)
        {
            return Cache.Add(id, value, DateTime.Now.AddMinutes(WebDavAppConfigManager.Instance.OfficeOnline.CobaltFileTtl));
        }

        public bool Delete(string id)
        {
            if (Cache.Any(x => x.Key == id))
            {
                Cache.Remove(id);

                return true;
            }

            return false;
        }

        public void SetTtl<TV>(string id, TV value)
        {
            Cache.Set(id, value, DateTime.Now.AddMinutes(WebDavAppConfigManager.Instance.OfficeOnline.CobaltFileTtl));
        }
    }
}
