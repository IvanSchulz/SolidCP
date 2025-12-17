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
using FuseCP.WebDavPortal.WebConfigSections;

namespace FuseCP.WebDav.Core.Config.Entities
{
    public class FilesToIgnoreCollection : AbstractConfigCollection, IReadOnlyCollection<FilesToIgnoreElement>
    {
        private readonly IList<FilesToIgnoreElement> _filesToIgnore;

        public FilesToIgnoreCollection()
        {
            _filesToIgnore = ConfigSection.FilesToIgnore.Cast<FilesToIgnoreElement>().ToList();
        }

        public IEnumerator<FilesToIgnoreElement> GetEnumerator()
        {
            return _filesToIgnore.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _filesToIgnore.Count; }
        }

        public bool Contains(string name)
        {
            return _filesToIgnore.Any(x => x.Name == name);
        }
    }
}
