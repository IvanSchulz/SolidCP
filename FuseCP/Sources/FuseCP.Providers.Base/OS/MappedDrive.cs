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

namespace FuseCP.Providers.OS
{
    /// <summary>
    /// Summary description for MappedDrive.
    /// </summary>
    [Serializable]
    public class MappedDrive
    {
        private string path;
        private string labelAs;
        private string driveLetter;
        private SystemFile folder;

        public MappedDrive()
        {
        }

        public MappedDrive(string path, string labelAs, string driveLetter)
        {
            this.Path = path;
            this.LabelAs = labelAs;
            this.DriveLetter = driveLetter;
            //SystemFile folder = null;
        }

        public string Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        public string LabelAs
        {
            get { return this.labelAs; }
            set { this.labelAs = value; }
        }

        public string DriveLetter
        {
            get { return this.driveLetter; }
            set { this.driveLetter = value; }
        }

        public SystemFile Folder
        {
            get { return this.folder; }
            set { this.folder = value; }
        }
    }
}
