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

namespace FuseCP.Providers.FTP.IIs100.Config
{
    using Microsoft.Web.Administration;
    using System;

    internal class FileHandlingElement : ConfigurationElement
    {
        public bool AllowReadUploadsInProgress
        {
            get
            {
                return (bool) base["allowReadUploadsInProgress"];
            }
            set
            {
                base["allowReadUploadsInProgress"] = value;
            }
        }

        public bool AllowReplaceOnRename
        {
            get
            {
                return (bool) base["allowReplaceOnRename"];
            }
            set
            {
                base["allowReplaceOnRename"] = value;
            }
        }

        public bool KeepPartialUploads
        {
            get
            {
                return (bool) base["keepPartialUploads"];
            }
            set
            {
                base["keepPartialUploads"] = value;
            }
        }
    }
}

