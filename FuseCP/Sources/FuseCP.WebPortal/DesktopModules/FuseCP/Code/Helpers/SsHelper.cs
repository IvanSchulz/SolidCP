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

using FuseCP.Providers.StorageSpaces;

namespace FuseCP.Portal
{
    public class SsHelper
    {
        #region Storage Space Levels

        StorageSpaceLevelPaged ssLevels;

        public int GetStorageSpaceLevelsPagedCount(string filterValue)
        {
            return ssLevels.RecordsCount;
        }

        public StorageSpaceLevel[] GetStorageSpaceLevelsPaged(int maximumRows, int startRowIndex, string sortColumn, string filterValue)
        {
            ssLevels = ES.Services.StorageSpaces.GetStorageSpaceLevelsPaged("Name", filterValue, sortColumn, startRowIndex, maximumRows);

            return ssLevels.Levels;
        }

        #endregion 

        #region Storage Spaces

        StorageSpacesPaged sSpaces;

        public int GetStorageSpacePagedCount(string filterValue)
        {
            return sSpaces.RecordsCount;
        }

        public StorageSpace[] GetStorageSpacePaged(int maximumRows, int startRowIndex, string sortColumn, string filterValue)
        {
            sSpaces = ES.Services.StorageSpaces.GetStorageSpacesPaged("Name", filterValue, sortColumn, startRowIndex, maximumRows);

            return sSpaces.Spaces;
        }

        #endregion 
    }
}
