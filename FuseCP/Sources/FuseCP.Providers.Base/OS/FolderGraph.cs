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
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace FuseCP.Providers.OS
{
    public class FolderGraph
    {
        uint[] checkSumKeys;
        FileHash[] checkSumValues;
        Dictionary<uint, FileHash> checkSums = new Dictionary<uint, FileHash>();
        FileHash hash;

        [XmlIgnore, IgnoreDataMember]
        public Dictionary<uint, FileHash> CheckSums
        {
            get { return checkSums; }
            set { checkSums = value; }
        }

        public uint[] CheckSumKeys
        {
            get { return checkSumKeys; }
            set { checkSumKeys = value; }
        }

        public FileHash[] CheckSumValues
        {
            get { return checkSumValues; }
            set { checkSumValues = value; }
        }

        public FileHash Hash
        {
            get { return hash; }
            set { hash = value; }
        }
    }
}
