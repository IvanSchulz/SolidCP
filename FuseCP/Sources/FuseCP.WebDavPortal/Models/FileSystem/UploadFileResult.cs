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

namespace FuseCP.WebDavPortal.Models.FileSystem
{
    public class UploadFileResult
    {
        private string _error;

        public string error
        {
            get { return _error; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _error = value;
                    deleteUrl = String.Empty;
                    thumbnailUrl = String.Empty;
                    url = String.Empty;
                }
            }
        }

        public string name { get; set; }

        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string deleteUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string deleteType { get; set; }


        public string FullPath { get; set; }
        public string SavedFileName { get; set; }

        public string Title { get; set; }
    }
}
