using System;

namespace FuseCP.EnterpriseServer.Data;

interface IMigratableDbContext : IDisposable
{
	public void Migrate();
}
