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
using System.Text;
using System.Threading.Tasks;
#if NetCore
using Microsoft.EntityFrameworkCore;
#elif NetFX
using System.Data.Entity.Validation;
#endif

namespace FuseCP.EnterpriseServer.Data.Extensions
{
	public static class ExceptionExtensions
	{
		public static string ValidationErrorMessage(this Exception ex)
		{
#if NETFRAMEWORK
			if (ex is DbEntityValidationException evex)
			{
				return $"Validation Errors: {Environment.NewLine}" +
					string.Join(Environment.NewLine,
						evex.EntityValidationErrors
							.Select(err => string.Join(Environment.NewLine, err.ValidationErrors
								.Select(err2 => $"{err2.PropertyName} - {err2.ErrorMessage}"))));
			}
#endif
			return null;
		}
	}
}
