using SolidCP.EnterpriseServer.Data;
using SolidCP.Providers;
using SolidCP.Providers.Common;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using SolidCP.Providers.Web;
using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SolidCP.UniversalInstaller;

public abstract partial class Installer
{
	public virtual string EnterpriseServerSiteId => $"{SolidCP}EnterpriseServer";
	public virtual string UnixEnterpriseServerServiceId => "solidcp-enterpriseserver";
	public virtual void InstallEnterpriseServerPrerequisites() { }
	public virtual void RemoveEnterpriseServerPrerequisites() { }
	public virtual void CreateEnterpriseServerUser() => CreateUser(Settings.EnterpriseServer);
	public virtual void RemoveEnterpriseServerUser() => RemoveUser(Settings.EnterpriseServer.Username);
	public virtual void SetEnterpriseServerFilePermissions()
	{
		if (!OSInfo.IsWindows) SetFilePermissions(Settings.EnterpriseServer.InstallPath);
		else
		{
			var user = Settings.WebPortal.EmbedEnterpriseServer ? Settings.WebPortal.Username : Settings.EnterpriseServer.Username;
			if (!string.IsNullOrEmpty(user))
			{
				SetFilePermissions(Settings.EnterpriseServer.InstallPath, user);
			}
			if (Settings.WebPortal.EmbedEnterpriseServer)
			{
				user = Settings.EnterpriseServer.Username;
				if (!string.IsNullOrEmpty(user))
				{
					SetFilePermissions(Settings.EnterpriseServer.InstallPath, user);
				}
			}
		}
	}
	public virtual void SetEnterpriseServerFileOwner()
	{
		var user = string.IsNullOrEmpty(Settings.EnterpriseServer.Username) && Settings.WebPortal.EmbedEnterpriseServer ?
			Settings.WebPortal.Username : Settings.EnterpriseServer.Username;
		SetFileOwner(Settings.EnterpriseServer.InstallPath, user, SolidCPGroup);
	}
	public virtual void InstallEnterpriseServer()
	{
		ResetEstimatedOutputLines();
		CountInstallDatabaseStatements();
		InstallEnterpriseServerPrerequisites();
		CopyEnterpriseServer(true, this.StandardInstallFilter);
		CreateEnterpriseServerUser();
		InstallDatabase();
		ConfigureEnterpriseServer();
		InstallSchedulerService();
		SetEnterpriseServerFilePermissions();
		SetEnterpriseServerFileOwner();
		InstallEnterpriseServerWebsite();
	}
	public virtual void UpdateEnterpriseServer()
	{
		ResetEstimatedOutputLines();
		CountUpdateDatabaseStatements();
		InstallEnterpriseServerPrerequisites();
		RemoveSchedulerService();
		DisableEnterpriseServerWebsite();
		CopyEnterpriseServer(true, StandardUpdateFilter);
		UpdateDatabase();
		UpdateEnterpriseServerConfig();
		ConfigureEnterpriseServer();
		InstallSchedulerService();
		SetEnterpriseServerFilePermissions();
		SetEnterpriseServerFileOwner();
		EnableEnterpriseServerWebsite();
	}
	public void SetupEnterpriseServer()
	{
		ResetEstimatedOutputLines();
		RemoveEnterpriseServerWebsite();
		ConfigureEnterpriseServer();
		InstallEnterpriseServerWebsite();
	}
	public virtual void UpdateEnterpriseServerConfig()
	{
		var configFile = Path.Combine(Settings.EnterpriseServer.InstallPath, "Web.config");
		var config = XElement.Load(configFile);
		var sections = config.Element("configSections");
		// Remove WSE3
		sections
			?.Elements("section")
			.FirstOrDefault(e => e.Attribute("name")?.Value == "microsoft.web.services3")
			?.Remove();
		config
			.Elements("microsoft.web.services3")
			.FirstOrDefault()
			?.Remove();
		// Add SwaggerWCF
		sections.Add(new XElement("section",
			new XAttribute("name", "swaggerwcf"),
			new XAttribute("type", "SwaggerWcf.Configuration.SwaggerWcfSection, SwaggerWcf")));
		var swaggerwcf = XElement.Parse(@"<swaggerwcf>
		<settings>
			<setting name=""InfoDescription"" value=""SolidCP EnterpriseServer Service"" />
			<setting name=""InfoVersion"" value=""2.0.0"" />
			<setting name=""InfoTermsOfService"" value=""Terms of Service"" />
			<setting name=""InfoTitle"" value=""SolidCP EnterpriseServer Service"" />
			<setting name=""InfoContactName"" value=""SolidCP"" />
			<setting name=""InfoContactUrl"" value=""http://solidcp.com/forum"" />
			<setting name=""InfoContactEmail"" value=""support@solidcp.com"" />
			<setting name=""InfoLicenseUrl"" value=""https://github.com/FuseCP/SolidCP/blob/master/LICENSE.txt"" />
			<setting name=""InfoLicenseName"" value=""Creative Commons Share-alike"" />
		</settings>
	</swaggerwcf>");
		config.Add(swaggerwcf);

		// Update connectionString
		var connectionStrings = config.Element("connectionStrings");
		var cstringElement = connectionStrings.Elements("add").FirstOrDefault(e => e.Attribute("name")?.Value == "EnterpriseServer");
		var cstringAttr = cstringElement?.Attribute("connectionString");
		cstringAttr.SetValue($"DbType=SqlServer;{cstringAttr.Value}");
		cstringElement.Attribute("providerName")?.Remove();

		// Adjust appSettings
		var appSettings = config.Element("appSettings");
		appSettings.Add(XElement.Parse(@"<add key=""ExposeWebServices"" value=""true"" />"));

		// Adjust httpRuntime
		var systemWeb = config.Element("system.web");
		var httpRuntime = systemWeb?.Element("httpRuntime");
		if (httpRuntime == null) systemWeb.Add(httpRuntime = new XElement("httpRuntime"));
		var requestLength = httpRuntime.Attribute("maxRequestLength");
		if (requestLength == null)
		{
			httpRuntime.Add(requestLength = new XAttribute("maxRequestLength", 4194304));
		}
		else requestLength.SetValue(4194304);
		systemWeb?.Element("webServices")?.Remove();

		// Block bin_dotnet
		var location = XElement.Parse(@"<location path=""bin_dotnet"">
        <system.webServer>
            <handlers>
                <add name=""DisallowServe"" path=""*.*"" verb=""*"" type=""System.Web.HttpNotFoundHandler"" />
                <!-- Return 404 instead of 403 -->
            </handlers>
        </system.webServer>
    </location>");
		config.Add(location);

		// Add system.webServers
		var systemWebServer = XElement.Parse(@"<system.webServer>
		<modules runAllManagedModulesForAllRequests=""true"" />
        <!-- hide the bin_dotnet folder -->
        <security>
            <requestFiltering>
                <hiddenSegments>
                    <add segment=""bin_dotnet"" />
                </hiddenSegments>
                <requestLimits maxAllowedContentLength=""4194304"" />
            </requestFiltering>
        </security>
        <httpErrors errorMode=""Detailed"" />
		<!--
        To browse web app root directory during debugging, set the value below to true.
        Set to false before deployment to avoid disclosing web app folder information.
      -->
	</system.webServer>");
		if (config.Element("system.webServer") == null) config.Add(systemWebServer);
		else config.Element("system.webServer").ReplaceWith(systemWebServer);

		// Add WCF certificate
		var systemServiceModel = config.Element("system.serviceModel");
		var environment = XElement.Parse(@"<serviceHostingEnvironment aspNetCompatibilityEnabled=""true"" />");
		if (systemServiceModel.Element("serviceHostingEnvironment") == null) systemServiceModel.Add(environment);
		var behaviors = systemServiceModel?.Element("behaviors");
		if (behaviors == null)
		{
			systemServiceModel.Add(behaviors = new XElement("behaviors"));
		}
		var serviceBehaviors = XElement.Parse(@"<serviceBehaviors>
				<behavior>
					<serviceCredentials>
						<serviceCertificate storeLocation=""LocalMachine"" storeName=""My"" x509FindType=""FindBySubjectName"" findValue=""localhost"" />
					</serviceCredentials>
				</behavior>
			</serviceBehaviors>");
		behaviors?.Add(serviceBehaviors);

		// Runtime
		var runtime = config.Element("runtime");
		if (runtime == null) config.Add(runtime = new XElement("runtime"));

		runtime.ReplaceWith(XElement.Parse(@"<runtime>
		<assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">
			<probing privatePath=""bin/Code;bin/netstandard"" />
            <dependentAssembly>
                <assemblyIdentity name=""System.Runtime.CompilerServices.Unsafe""
                    publicKeyToken=""b03f5f7f11d50a3a""
                    culture=""neutral"" />
                <bindingRedirect oldVersion=""4.0.0.0-6.0.3.0"" newVersion=""6.0.3.0"" />
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""Microsoft.Identity.Client""
                    publicKeyToken=""0a613f4dd989e8ae""
                    culture=""neutral"" />
                <bindingRedirect oldVersion=""4.0.0.0-4.66.1.0"" newVersion=""4.67.2.0"" />
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""System.Collections.Immutable"" publicKeyToken=""b03f5f7f11d50a3a"" culture=""neutral"" />
                <bindingRedirect oldVersion=""0.0.0.0-10.0.0.0"" newVersion=""10.0.0.0"" />
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""System.Memory"" publicKeyToken=""cc7b13ffcd2ddd51"" culture=""neutral"" />
                <bindingRedirect oldVersion=""0.0.0.0-4.0.5.0"" newVersion=""4.0.5.0"" />
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""System.Buffers"" publicKeyToken=""cc7b13ffcd2ddd51"" culture=""neutral"" />
                <bindingRedirect oldVersion=""0.0.0.0-4.0.5.0"" newVersion=""4.0.5.0"" />
            </dependentAssembly>
        </assemblyBinding>
	</runtime>"));

		File.WriteAllText(configFile, config.ToString());
	}

	public virtual string ConnectionString()
	{
		var settings = Settings.EnterpriseServer;
		var connstr = settings.DbInstallConnectionString;
		if (settings.DatabaseType == EnterpriseServer.Data.DbType.Sqlite ||
			settings.DatabaseType == EnterpriseServer.Data.DbType.SqliteFX)
		{
			var csb = new ConnectionStringBuilder(connstr);
			if (csb["Data Source"] != null)
			{
				csb["Data Source"] = Path.Combine(Settings.EnterpriseServer.InstallPath, (string)csb["Data Source"]);
			}
			connstr = csb.ConnectionString;
		}
		return connstr;
	}

	public virtual void InstallDatabase()
	{
		Transaction(() =>
		{
			Info("Install Database...");
			var settings = Settings.EnterpriseServer;
			var connstr = ConnectionString();
			if (string.IsNullOrEmpty(connstr) ||
				!DatabaseUtils.CheckSqlConnection(connstr)) throw new DataException("Unable to connect to database.");
			if (settings.DatabaseWindowsAuthentication)
			{
				settings.DatabaseUser = settings.DatabaseName;
				settings.DatabasePassword = Utils.GetRandomString(20);
				settings.DatabaseWindowsAuthentication = false;
			}
			var user = settings.DatabaseUser ?? settings.DatabaseName;
			var password = settings.DatabasePassword ?? Utils.GetRandomString(20);
			var db = settings.DatabaseName;

			Log.WriteLine($"Connection String: {connstr}; Database: {db}; User: {user}; Password {password}");

			DatabaseUtils.InstallFreshDatabase(connstr, db, user, password, progress => Log.Write("."));

			Log.WriteLine();

			if (string.IsNullOrEmpty(settings.CryptoKey)) settings.CryptoKey = Utils.GetRandomString(20);

			var cryptor = new Cryptor(settings.CryptoKey);
			DatabaseUtils.SetServerAdminPassword(connstr, db, cryptor.Encrypt(settings.ServerAdminPassword));

			InstallLog("Installed Database");
		})
			.WithRollback(DeleteDatabase);
	}


	public virtual void UpdateDatabase()
	{
		Info("Update Database");
		var settings = Settings.EnterpriseServer;
		var configFile = Path.Combine(Settings.EnterpriseServer.InstallPath, "Web.config");
		var config = XElement.Load(configFile);
		var connectionStrings = config.Element("connectionStrings");
		var cstringElement = connectionStrings.Elements("add").FirstOrDefault(e => e.Attribute("name")?.Value == "EnterpriseServer");
		var cstringAttr = cstringElement?.Attribute("connectionString");
		var cstring = cstringAttr?.Value;
		string databaseName;
		var dataNewVersion = cstring.IndexOf("DbType=", StringComparison.OrdinalIgnoreCase) >= 0;
		if (!dataNewVersion)
		{
			cstring = $"DbType=SqlServer;{cstring}";
			cstringAttr.SetValue(cstring);
			cstringElement.Attribute("providerName")?.Remove();
			File.WriteAllText(configFile, config.ToString());
			var csb = new ConnectionStringBuilder(cstring);
			databaseName = (csb["Initial Catalog"] ?? csb["Database"]) as string;
			settings.DbInstallConnectionString = cstring;
			settings.DatabaseName = databaseName;
			settings.DatabaseUser = (csb["Uid"] ?? csb["User Id"]) as string;
			settings.DatabasePassword = (csb["Pwd"] ?? csb["Password"]) as string;
			settings.DatabaseType = EnterpriseServer.Data.DbType.SqlServer;
		} else
		{
			var csb = new ConnectionStringBuilder(cstring);
			databaseName = (csb["Initial Catalog"] ?? csb["Database"]) as string;
			settings.DbInstallConnectionString ??= cstring;
			settings.DatabaseName ??= databaseName;
			settings.DatabaseUser ??= (csb["Uid"] ?? csb["User Id"] ?? csb["User"] ?? csb["Username"]) as string;
			settings.DatabasePassword ??= (csb["Pwd"] ?? csb["Password"]) as string;
			settings.DatabaseType = (EnterpriseServer.Data.DbType)Enum.Parse(typeof(EnterpriseServer.Data.DbType), (csb["DbType"] ?? "SqlServer") as string, true);
			if (settings.DatabaseType == EnterpriseServer.Data.DbType.Sqlite ||
				settings.DatabaseType == EnterpriseServer.Data.DbType.SqliteFX)
			{
				if (csb["Data Source"] != null)
				{
					csb["Data Source"] = Path.GetFullPath(Path.Combine(Settings.EnterpriseServer.InstallPath, (string)csb["Data Source"]));
				}
				cstring = csb.ConnectionString;
			}
		}

		DatabaseUtils.UpdateDatabase(cstring, settings.DatabaseName,
			settings.DatabaseUser, settings.DatabasePassword, dataNewVersion);

		InstallLog("Updated Database");
	}
	public virtual void DeleteDatabase()
	{
		Info("Delete Database");
		var settings = Settings.EnterpriseServer;
		var connstr = ConnectionString();
		DatabaseUtils.DeleteDatabase(connstr, settings.DatabaseName);
		if (string.IsNullOrEmpty(settings.DatabaseUser))
		{
			settings.DatabaseUser = settings.DatabaseName;
		}
		try
		{
			DatabaseUtils.DeleteUser(connstr, settings.DatabaseUser);
			DatabaseUtils.DeleteLogin(connstr, settings.DatabaseUser);
		}
		catch { }

		InstallLog("Deleted Database");
	}
	public virtual void CountInstallDatabaseStatements()
	{
		var settings = Settings.EnterpriseServer;
		var connstr = ConnectionString();
		if (string.IsNullOrEmpty(settings.DatabaseUser))
		{
			settings.DatabaseUser = settings.DatabaseName;
			settings.DatabasePassword = Utils.GetRandomString(32);
		}
		var user = settings.DatabaseUser;
		var password = settings.DatabasePassword;
		var db = settings.DatabaseName;
		DatabaseUtils.InstallFreshDatabase(connstr, db, user, password, null,
			count => DatabaseStatements = count, null, "", true);
	}
	public virtual void CountUpdateDatabaseStatements()
	{
		var settings = Settings.EnterpriseServer;
		var connstr = ConnectionString();
		var user = settings.DatabaseUser;
		var password = settings.DatabasePassword;
		var db = settings.DatabaseName;
		DatabaseUtils.UpdateDatabase(connstr, db, user, password, true, null,
			count => DatabaseStatements = count, null, "", true);
	}
	public virtual void ResetEstimatedOutputLines(Func<int> calculateEstimatedOutputLines = null)
	{
		DatabaseStatements = 0;
		estimatedOutputLines = null;
		CalculateEstimateOutputLines = calculateEstimatedOutputLines;
	}
	public virtual void InstallEnterpriseServerWebsite()
	{
		var web = Settings.EnterpriseServer.InstallPath;
		var dll = Path.Combine(web, "bin_dotnet", "SolidCP.EnterpriseServer.dll");
		InstallWebsite(EnterpriseServerSiteId,
			web,
			Settings.EnterpriseServer,
			SolidCPUnixGroup,
			dll,
			"SolidCP.EnterpriseServer service, the EnterpriseServer for the SolidCP control panel.",
			UnixEnterpriseServerServiceId);
	}

	public virtual void EnableEnterpriseServerWebsite() { }
	public virtual void DisableEnterpriseServerWebsite() { }
    public virtual string SchedulerServiceId => "SolidCP.SchedulerService";
    public virtual void InstallSchedulerService() { }
    public virtual void RemoveSchedulerService()
    {
        RemoveService(SchedulerServiceId);
    }
    public virtual void ConfigureSchedulerService()
    {
        ConfigureSchedulerServiceNetFX();
    }
    public virtual void ConfigureSchedulerServiceNetFX()
    {
        if (Settings.EnterpriseServer.RunOnNetCore) return;

        var binFolder = (Settings.EnterpriseServer.RunOnNetCore ||
            Settings.WebPortal.RunOnNetCore && Settings.WebPortal.EmbedEnterpriseServer) ?
                "bin_dotnet" : Path.Combine("bin", "Code");
        var config = Path.Combine(Settings.EnterpriseServer.InstallPath, binFolder, $"SolidCP.SchedulerService.{(Settings.EnterpriseServer.RunOnNetCore ? "dll" : "exe")}.config");

        var xml = XElement.Load(config);

        var conStrings = xml.Element("connectionStrings");
        if (conStrings == null) xml.Add(conStrings = new XElement("connectionStrings"));
        var appSettings = xml.Element("appSettings");
        if (appSettings == null) xml.Add(appSettings = new XElement("appSettings"));

        var conStrElement = conStrings.Elements("add").FirstOrDefault(e => e.Attribute("name")?.Value == "EnterpriseServer");
        var esConfFile = Path.GetFullPath(Path.Combine(Settings.EnterpriseServer.InstallPath, "Web.config"));
        if (File.Exists(esConfFile))
        {
            var esConf = XElement.Load(esConfFile);
            var esAppSettings = esConf.Element("appSettings");

            foreach (var esSetting in esAppSettings.Elements("add"))
            {
                var key = esSetting.Attribute("key")?.Value;
                var setting = appSettings.Elements("add").FirstOrDefault(s => s.Attribute("key")?.Value == key);
                if (setting == null)
                {
                    appSettings.Add(esSetting);
                }
                else
                {
                    setting.Attribute("value").SetValue(esSetting.Attribute("value")?.Value);
                }
            }

            var esConStrings = esConf.Element("connectionStrings");
            conStrings.ReplaceWith(esConStrings);

            File.WriteAllText(config, xml.ToString());
        }
    }
    public virtual void RemoveEnterpriseServerWebsite() {
		RemoveWebsite(EnterpriseServerSiteId, Settings.EnterpriseServer);
	}
	public virtual void RemoveEnterpriseServerFolder()
	{
		var dir = Settings.EnterpriseServer.InstallPath;
		if (Directory.Exists(dir)) Directory.Delete(dir, true);
		InstallLog("Removed EnterpriseServer files");
	}

	public virtual void RemoveEnterpriseServer()
	{
		RemoveEnterpriseServerWebsite();
		DeleteDatabase();
		RemoveEnterpriseServerFolder();
		RemoveEnterpriseServerUser();
	}
	public virtual void ReadEnterpriseServerConfiguration() => ReadEnterpriseServerConfigurationNetFX();

	public virtual void ReadEnterpriseServerConfigurationNetFX()
	{

		var confFile = Path.Combine(Settings.EnterpriseServer.InstallPath, "Web.config");

		if (!File.Exists(confFile)) return;

		Settings.EnterpriseServer = new EnterpriseServerSettings();

		var webconf = XElement.Load(confFile);
		var configuration = webconf.Element("configuration");

		// server certificate
		var cert = configuration?.Element("system.serviceModel/behaviors/serviceBehaviors/behavior/serviceCredentials/serviceCertificate");
		if (cert != null)
		{
			Settings.EnterpriseServer.CertificateStoreLocation = cert.Attribute("storeLocation")?.Value;
			Settings.EnterpriseServer.CertificateStoreName = cert.Attribute("storeName")?.Value;
			Settings.EnterpriseServer.CertificateFindType = cert.Attribute("x509FindType")?.Value;
			Settings.EnterpriseServer.CertificateFindValue = cert.Attribute("findValue")?.Value;
		}

		// connection string
		var cstring = configuration?.Element("connectionStrings").Elements("add").FirstOrDefault(e => e.Attribute("name")?.Value == "EnterpriseServer");

		string server, user, password;
		bool windowsAuthentication, trustCertificate;
		EnterpriseServer.Data.DbType dbtype;
		ParseConnectionString(cstring?.Attribute("value")?.Value, out dbtype, out server, out user, out password, out windowsAuthentication,
			out trustCertificate);

		Settings.EnterpriseServer.DatabaseType = dbtype;
		Settings.EnterpriseServer.DatabaseServer = server;
		Settings.EnterpriseServer.DatabaseUser = user;
		Settings.EnterpriseServer.DatabasePassword = password;
		Settings.EnterpriseServer.DatabaseWindowsAuthentication = windowsAuthentication;
		Settings.EnterpriseServer.DatabaseTrustServerCertificate = trustCertificate;

		// CryptoKey
		var cryptoKey = configuration?.Elements("appSettings/add").FirstOrDefault(e => e.Attribute("key")?.Value == "CryptoKey");
		Settings.EnterpriseServer.CryptoKey = cryptoKey?.Attribute("value")?.Value;
	}

	public virtual void ConfigureEnterpriseServerNetFX(bool webPortalEmbedded = false)
	{
		var settings = Settings.EnterpriseServer;

		var confFile = webPortalEmbedded ? Path.Combine(Settings.WebPortal.InstallPath, "Web.config") :
			Path.Combine(Settings.EnterpriseServer.InstallPath, "Web.config");

		if (!File.Exists(confFile)) return;

		var configuration = XElement.Load(confFile);

		// server certificate
		if (!webPortalEmbedded) ConfigureCertificateNetFX(settings, configuration);

		var appSettings = configuration.Element("appSettings");

		if (webPortalEmbedded)
		{
			// read CryptoKey
			var esConfFile = Path.GetFullPath(Path.Combine(Settings.WebPortal.InstallPath, Settings.WebPortal.EnterpriseServerPath, "Web.config"));
			if (File.Exists(esConfFile))
			{
				var esConf = XElement.Load(esConfFile);
				var esAppSettings = esConf.Element("appSettings");


				var esCryptoKey = esAppSettings?.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == "SolidCP.CryptoKey");
				if (esCryptoKey != null)
				{
					settings.CryptoKey = esCryptoKey.Attribute("value")?.Value;
				}

				foreach (var esSetting in esAppSettings.Elements("add"))
				{
					var key = esSetting.Attribute("key")?.Value;
					var setting = appSettings.Elements("add").FirstOrDefault(s => s.Attribute("key")?.Value == key);
					if (setting == null)
					{
						appSettings.Add(esSetting);
					}
					else
					{
						setting.Attribute("value").SetValue(esSetting.Attribute("value")?.Value);
					}
				}

				var esConStrings = esConf.Element("connectionStrings");
				if (esConStrings != null)
				{
					var esConString = esConStrings.Elements("add").FirstOrDefault(e => e.Attribute("name")?.Value == "EnterpriseServer");
					var esCstring = esConString?.Attribute("connectionString")?.Value;
					if (esCstring != null)
					{
						var csb = new ConnectionStringBuilder(esCstring);
						if ((csb["DbType"] as string)?.Contains("Sqlite") == true)
						{
							if (csb["Data Source"] != null)
							{
								csb["Data Source"] = Path.Combine(Settings.WebPortal.EnterpriseServerPath, (string)csb["Data Source"]);
							}
							esConString.Attribute("connectionString").SetValue(csb.ConnectionString);
						}
					}
				}
				var conStrings = configuration.Element("connectionStrings");
				conStrings.ReplaceWith(esConStrings);

				var esSwagger = esConf.Element("swaggerwcf");
				var swagger = configuration.Element("swaggerwcf");
				swagger.ReplaceWith(esSwagger);
			}
			else throw new NotSupportedException("EnterpriseServer settings file not found. You must install EnterpriseServer prior to install WebPortal with embedded WnterpriseServer.");
		}

		// CryptoKey
		if (string.IsNullOrEmpty(settings.CryptoKey))
		{
			// generate random crypto key
			settings.CryptoKey = CryptoUtils.GetRandomString(20);
		}

		var cryptoKey = appSettings.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == "SolidCP.CryptoKey");
		if (cryptoKey == null)
		{
			cryptoKey = new XElement("add", new XAttribute("key", "SolidCP.CryptoKey"), new XAttribute("value", settings.CryptoKey));
			appSettings.Add(cryptoKey);
		}
		else
		{
			cryptoKey.Attribute("value").SetValue(settings.CryptoKey);
		}

		if (!webPortalEmbedded)
		{
			// Connection String
			var connectionStrings = configuration.Element("connectionStrings");
			var cstring = connectionStrings.Elements("add").FirstOrDefault(e => e.Attribute("name")?.Value == "EnterpriseServer");
			var connectionString = DatabaseUtils.BuildConnectionString(settings.DatabaseType, settings.DatabaseServer,
				settings.DatabasePort, settings.DatabaseName, settings.DatabaseUser, settings.DatabasePassword, Settings.WebPortal.EnterpriseServerPath, webPortalEmbedded && Settings.WebPortal.EmbedEnterpriseServer);
			cstring.Attribute("connectionString").SetValue(connectionString);

			// Swagger Version
			var swaggerwcf = configuration.Element("swaggerwcf");
			var swagsetting = swaggerwcf?.Element("settings");
			var versionInfo = swagsetting?.Elements("setting").FirstOrDefault(e => e.Attribute("name")?.Value == "InfoVersion");
			if (versionInfo != null)
			{
				versionInfo.Attribute("value").SetValue(Settings.EnterpriseServer.Version.ToString());
			}
		}

		configuration.Save(confFile);
	}
	public virtual void ConfigureEnterpriseServerNetCore() => ConfigureAppsettings(Settings.EnterpriseServer);
	public virtual void ConfigureEnterpriseServer()
	{
		ConfigureEnterpriseServerNetCore();
		ConfigureEnterpriseServerNetFX();
        ConfigureSchedulerService();
        InstallLog("Configured Enterprise Server.");
	}
	public virtual void CopyEnterpriseServer(bool clearDestination = false, Func<string, string> filter = null)
	{
		filter ??= SetupFilter;
		var websitePath = Settings.EnterpriseServer.InstallPath;
		InstallWebRootPath = Path.GetDirectoryName(websitePath);
		CopyFiles(ComponentTempPath, websitePath, clearDestination, filter);
	}
}