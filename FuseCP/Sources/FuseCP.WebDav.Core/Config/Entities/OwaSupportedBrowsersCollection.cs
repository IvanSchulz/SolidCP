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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FuseCP.WebDav.Core.Config.WebConfigSections;

namespace FuseCP.WebDav.Core.Config.Entities
{
    public class OwaSupportedBrowsersCollection : AbstractConfigCollection, IReadOnlyDictionary<string, int>
    {
        private readonly IDictionary<string, int> _browsers;

        public OwaSupportedBrowsersCollection()
        {
            _browsers = ConfigSection.OwaSupportedBrowsers.Cast<OwaSupportedBrowsersElement>().ToDictionary(x => x.Browser, y => y.Version);
        }

        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
        {
            return _browsers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _browsers.Count; }
        }

        public bool ContainsKey(string browser)
        {
            return _browsers.ContainsKey(browser);
        }

        public bool TryGetValue(string browser, out int version)
        {
            return _browsers.TryGetValue(browser, out version);
        }

        public int this[string browser]
        {
            get { return ContainsKey(browser) ? _browsers[browser] : 0; }
        }

        public IEnumerable<string> Keys
        {
            get { return _browsers.Keys; }
        }

        public IEnumerable<int> Values
        {
            get { return _browsers.Values; }
        } 
    }
}
