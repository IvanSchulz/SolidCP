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
using System.Linq;
using FuseCP.WebDavPortal.Models.Common.DataTable;

namespace FuseCP.WebDavPortal.Helpers
{
    public class DataTableHelper
    {
        public static JqueryDataTablesResponse ProcessRequest<TEntity>(IEnumerable<TEntity> entities, JqueryDataTableRequest request) where TEntity : JqueryDataTableBaseEntity
        {
            IOrderedEnumerable<TEntity> orderedEntities = null;

            foreach (var order in request.Orders)
            {
                var closure = order;

                if (orderedEntities == null)
                {
                    orderedEntities = order.Ascending ? entities.OrderBy(x => x[closure.Column]) : entities.OrderByDescending(x => x[closure.Column]);
                }
                else
                {
                    orderedEntities = order.Ascending ? orderedEntities.ThenBy(x => x[closure.Column]) : orderedEntities.ThenByDescending(x => x[closure.Column]);
                }
            }

            if (orderedEntities == null)
            {
                orderedEntities = entities.OrderBy(x=>x[0]);
            }

            var itemsPaged = orderedEntities.Skip(request.Start).Take(request.Count).ToList();
            var totalCount = orderedEntities.Count();


            return new JqueryDataTablesResponse(
                request.Draw,
                itemsPaged,
                totalCount,
                totalCount);
        } 
    }
}
