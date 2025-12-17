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
using System.Data;

namespace FuseCP.Portal
{
    /// <summary>
    /// Summary description for SchedulesHelper
    /// </summary>
    public class SchedulesHelper
    {
        public DataSet GetRawSchedules()
        {
            return ES.Services.Scheduler.GetSchedules(PanelSecurity.SelectedUserId);
        }

        DataSet dsSchedulesPaged;

        public int GetSchedulesPagedCount(bool recursive, string filterColumn, string filterValue)
        {
            return (int)dsSchedulesPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetSchedulesPaged(int maximumRows, int startRowIndex, string sortColumn,
            bool recursive, string filterColumn, string filterValue)
        {
            dsSchedulesPaged = ES.Services.Scheduler.GetSchedulesPaged(PanelSecurity.PackageId, recursive, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsSchedulesPaged.Tables[1];
        }
    }
}
