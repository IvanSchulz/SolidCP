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

#if !NETSTANDARD

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

#if NETFRAMEWORK
using System.Data.Entity;
#elif NETCOREAPP
using Microsoft.EntityFrameworkCore;
#endif

namespace FuseCP.EnterpriseServer.Data
{
	public static class DynamicFunctions
	{
		public static Expression<Func<T, bool>> ColumnLike<T>(IEnumerable<T> items, string columnName, string likeExpression)
		{
			var param = Expression.Parameter(typeof(T));
			var property = Expression.Property(param, columnName);
			var likeExpressionParameter = Expression.Constant(likeExpression);
#if NETFRAMEWORK
			var type = typeof(DbFunctions);
			var likeMethod = type.GetMethod("Like", new[] { typeof(string), typeof(string) });
			// TODO use variable, not constant.
			var call = Expression.Call(null, likeMethod, property, likeExpressionParameter);
#elif NETCOREAPP
			// Get the Like Method from EF.Functions
			var efLikeMethod = typeof(DbFunctionsExtensions).GetMethod("Like",
				BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
				null,
				new[] { typeof(DbFunctions), typeof(string), typeof(string) },
				null);

			var efFunctions = Expression.Property(null, typeof(EF), nameof(EF.Functions));
			// ?all the method with all the required arguments
			var call = Expression.Call(efLikeMethod, efFunctions, property, likeExpressionParameter);
#endif
			return Expression.Lambda<Func<T, bool>>(call, param);
		}
	}

}
#endif
