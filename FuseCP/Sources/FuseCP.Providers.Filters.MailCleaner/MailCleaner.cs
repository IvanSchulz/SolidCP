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
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace FuseCP.Providers.Filters
{
    public class MailCleaner : MailCleanerBase
    {
        #region Constructor

        static MailCleaner()
        {
            MailCleanerRegistryPath = "SOFTWARE\\Microsoft\\MailCleaner";
            MailCleanerVersion = "1";
        }

        #endregion
        public override string[]  Install()
        {
            // install in silence
            return new string[] { };
        }
    }
}
