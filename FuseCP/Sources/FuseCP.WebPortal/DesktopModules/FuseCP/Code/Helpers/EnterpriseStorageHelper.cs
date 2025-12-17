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

using System.Text.RegularExpressions;
using FuseCP.Providers.OS;

namespace FuseCP.Portal
{
    public class EnterpriseStorageHelper
    {
        #region Folders

        public static bool ValidateFolderName(string name)
        {
            return Regex.IsMatch(name, @"^[a-zA-Z0-9-_. ]+$");
        }

        SystemFilesPaged folders;

        public int GetEnterpriseFoldersPagedCount(int itemId, string filterValue)
        {
            return folders.RecordsCount;
        }

        public SystemFile[] GetEnterpriseFoldersPaged(int itemId, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            filterValue = filterValue ?? string.Empty;
            startRowIndex++;
            folders = ES.Services.EnterpriseStorage.GetEnterpriseFoldersPaged(itemId, false, false, false, filterValue, sortColumn, startRowIndex, maximumRows);

            return folders.PageItems;
        }

        #endregion

        #region Drive Maps

        MappedDrivesPaged mappedDrives;

        public int GetEnterpriseDriveMapsPagedCount(int itemId, string filterValue)
        {
            return mappedDrives.RecordsCount;
        }

        public MappedDrive[] GetEnterpriseDriveMapsPaged(int itemId, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            filterValue = filterValue ?? string.Empty;

            mappedDrives = ES.Services.EnterpriseStorage.GetDriveMapsPaged(itemId,
                filterValue, sortColumn, startRowIndex, maximumRows);

            return mappedDrives.PageItems;
        }

        #endregion
    }
}
