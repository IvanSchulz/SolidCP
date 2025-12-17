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

using System.Collections;

namespace FuseCP.WebDavPortal.Models.Common.DataTable
{
    public class JqueryDataTablesResponse
    {
        public int draw { get; private set; }
        public IEnumerable data { get; private set; }
        public int recordsTotal { get; private set; }
        public int recordsFiltered { get; private set; }

        public JqueryDataTablesResponse(int draw, IEnumerable data, int recordsFilteredCount, int recordsTotalCount)
        {
            this.draw = draw;
            this.data = data;
            this.recordsFiltered = recordsFilteredCount;
            this.recordsTotal = recordsTotalCount;
        } 
    }
}
