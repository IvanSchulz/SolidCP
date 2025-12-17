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
using FuseCP.Providers.HostedSolution;
using FuseCP.Providers.ResultObjects;

namespace FuseCP.Portal
{
    public class LyncHelper
    {

        public LyncUser[] GetLyncUsersPaged(int itemId,
           string filterColumn, string filterValue,
           int maximumRows, int startRowIndex, string sortColumn)
        {
            if (!String.IsNullOrEmpty(filterValue))
                filterValue = filterValue + "%";
            if (maximumRows == 0)
            {
                maximumRows = Int32.MaxValue;
            }

            string name = string.Empty;
            string email = string.Empty;

            if (filterColumn == "DisplayName")
                name = filterValue;
            else
                email = filterValue;


            string[] data = sortColumn.Split(' ');
            string direction = data.Length > 1 ? "DESC" : "ASC";
            LyncUsersPagedResult res =
                ES.Services.Lync.GetLyncUsersPaged(itemId, data[0], direction, startRowIndex, maximumRows);

            return res.Value.PageUsers;
        }


        public int GetLyncUsersPagedCount(int itemId, string filterColumn, string filterValue)
        {
            string name = string.Empty;
            string email = string.Empty;

            if (!string.IsNullOrEmpty(filterValue))
                filterValue = filterValue + "%";

            if (filterColumn == "DisplayName")
                name = filterValue;
            else
                email = filterValue;

            IntResult res = ES.Services.Lync.GetLyncUserCount(itemId);
            return res.Value;
        }

    }
}
