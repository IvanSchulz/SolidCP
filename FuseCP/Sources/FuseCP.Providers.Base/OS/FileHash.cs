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
    public class FileHash
    {
        FileHash[] filesArray = null;
        List<FileHash> files = new List<FileHash>();
        string name;
        string fullName;
        uint checkSum;
        bool isFolder;

        public FileHash[] FilesArray
        {
            get { return filesArray; }
            set { filesArray = value; }
        }

        [XmlIgnore, IgnoreDataMember]
        public List<FileHash> Files
        {
            get { return files; }
            set { files = value; }
        }

        public bool IsFolder
        {
            get { return isFolder; }
            set { isFolder = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public uint CheckSum
        {
            get { return checkSum; }
            set { checkSum = value; }
        }
    }
}
