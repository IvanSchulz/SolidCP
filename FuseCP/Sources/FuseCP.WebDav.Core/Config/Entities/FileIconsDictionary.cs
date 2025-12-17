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
    public class FileIconsDictionary : AbstractConfigCollection, IReadOnlyDictionary<string, string>
    {
        private readonly IDictionary<string, string> _fileIcons;

        public FileIconsDictionary()
        {
            DefaultPath = ConfigSection.FileIcons.DefaultPath;
            FolderPath = ConfigSection.FileIcons.FolderPath;
            _fileIcons = ConfigSection.FileIcons.Cast<FileIconsElement>().ToDictionary(x => x.Extension, y => y.Path);
        }

        public string DefaultPath { get; private set; }
        public string FolderPath { get; private set; }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _fileIcons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _fileIcons.Count; }
        }

        public bool ContainsKey(string extension)
        {
            return _fileIcons.ContainsKey(extension);
        }

        public bool TryGetValue(string extension, out string path)
        {
            return _fileIcons.TryGetValue(extension, out path);
        }

        public string this[string extension]
        {
            get { return ContainsKey(extension) ? _fileIcons[extension] : DefaultPath; }
        }

        public IEnumerable<string> Keys
        {
            get { return _fileIcons.Keys; }
        }

        public IEnumerable<string> Values
        {
            get { return _fileIcons.Values; }
        }
    }
}
