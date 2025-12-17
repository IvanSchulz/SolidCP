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
    public class OpenerCollection : AbstractConfigCollection, IReadOnlyCollection<OpenerElement>
    {
        private readonly IList<OpenerElement> _targetBlankMimeTypeExtensions;

        public OpenerCollection()
        {
            _targetBlankMimeTypeExtensions = ConfigSection.TypeOpener.Cast<OpenerElement>().ToList();
        }

        public IEnumerator<OpenerElement> GetEnumerator()
        {
            return _targetBlankMimeTypeExtensions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _targetBlankMimeTypeExtensions.Count; }
        }

        public bool Contains(string extension)
        {
            return _targetBlankMimeTypeExtensions.Any(x => x.Extension == extension);
        }
    }
}
