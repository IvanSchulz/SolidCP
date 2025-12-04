#if !NETFRAMEWORK

namespace FuseCP.Server
{
	public static class Program
	{

		public static void Main(string[] args)
		{
			//if (!Debugger.IsAttached) Debugger.Launch();
			PasswordValidator.Init();
			FuseCP.Web.Services.StartupCore.Init(args);
			FuseCP.Server.Utils.Log.LogLevel = FuseCP.Web.Services.Configuration.TraceLevel;
		}
	}
}

#endif
