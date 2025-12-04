using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuseCP.EnterpriseServer.Data;

namespace FuseCP.Tests
{
	[TestClass]
	public class Initialization
	{

		[AssemblyInitialize]
		public static void Init(TestContext context)
		{
			// Init the assembly loader
			EnterpriseServer.InitAssemblyLoader();

			// Init all db flavors DbConfiguration
#if NETFRAMEWORK
			DbConfiguration.InitAllDatabaseProviders();
#else
			FuseCP.Web.Services.Configuration.CryptoKey = "1234567890";
			FuseCP.Web.Services.Configuration.EncryptionEnabled = true;
#endif
			// install the certificate
			Certificate.Install();
			Certificate.TrustAll();
			
			// Setup EnterpriseServer website & database
			EnterpriseServer.Clone();
			EnterpriseServer.SetupSqliteDb();
			var connStr = EnterpriseServer.SetupLocalDb();
			EnterpriseServer.ConfigureDatabase(connStr);

			Servers.Init(Component.EnterpriseServer);
		}

		[AssemblyCleanup]
		public static void Cleanup()
		{
			// remove the certificate
			Certificate.Remove();

			Servers.StopAll();

			EnterpriseServer.DeleteDatabases();
			EnterpriseServer.Delete();
		}
	}
}
