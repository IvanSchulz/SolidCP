#if NETCOREAPP
using Microsoft.Extensions.DependencyInjection;

namespace FuseCP.EnterpriseServer.Code
{
	public static class Initializer
	{
		public static void Init(IServiceCollection services)
		{
			services.AddHostedService<ScheduleWorker>();
		}
	}
}
#endif
