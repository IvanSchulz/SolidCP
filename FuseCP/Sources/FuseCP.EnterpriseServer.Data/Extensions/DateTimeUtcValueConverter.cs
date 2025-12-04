#if NetCore
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FuseCP.EnterpriseServer.Data.Extensions
{
	public class DateTimeUtcValueConverter : ValueConverter<DateTime, DateTime>
	{
		public DateTimeUtcValueConverter() : base(
			d => d.ToUniversalTime(),
			d => DateTime.SpecifyKind(d, DateTimeKind.Utc).ToLocalTime()
		)
		{ }
	}
}
#endif
