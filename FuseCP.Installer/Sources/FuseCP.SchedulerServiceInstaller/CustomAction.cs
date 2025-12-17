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
using System.Configuration.Install;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
//using Microsoft.Deployment.WindowsInstaller;
using WixToolset.Dtf.WindowsInstaller;
using FuseCP.Setup;

namespace FuseCP.SchedulerServiceInstaller
{
    public class CustomActions
    {
        public const string CustomDataDelimiter = "-=del=-";

        [CustomAction]
        public static ActionResult CheckConnection(Session session)
        {
            string testConnectionString = session["AUTHENTICATIONTYPE"].Equals("Windows Authentication") ? GetConnectionString(session["SERVERNAME"], "master") : GetConnectionString(session["SERVERNAME"], "master", session["LOGIN"], session["PASSWORD"]);

            testConnectionString = testConnectionString.Replace(CustomDataDelimiter, ";");

            if (CheckConnection(testConnectionString))
            {
                session["CORRECTCONNECTION"] = "1";
                session["CONNECTIONSTRING"] = session["AUTHENTICATIONTYPE"].Equals("Windows Authentication") ? GetConnectionString(session["SERVERNAME"], session["DATABASENAME"]) : GetConnectionString(session["SERVERNAME"], session["DATABASENAME"], session["LOGIN"], session["PASSWORD"]);
            }
            else
            {
                session["CORRECTCONNECTION"] = "0";
            }

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult FinalizeInstall(Session session)
        {
            var connectionString = GetCustomActionProperty(session, "ConnectionString").Replace(CustomDataDelimiter, ";");
            var serviceFolder = GetCustomActionProperty(session, "ServiceFolder");
            var previousConnectionString = GetCustomActionProperty(session, "PreviousConnectionString").Replace(CustomDataDelimiter, ";");
            var previousCryptoKey = GetCustomActionProperty(session, "PreviousCryptoKey");

            if (string.IsNullOrEmpty(serviceFolder))
            {
                return ActionResult.Success;
            }

            connectionString = string.IsNullOrEmpty(previousConnectionString)
                ? connectionString
                : previousConnectionString;

            ChangeConfigString("/configuration/connectionStrings/add[@name='EnterpriseServer']", "connectionString", connectionString, serviceFolder);
            ChangeConfigString("/configuration/appSettings/add[@key='FuseCP.CryptoKey']", "value", previousCryptoKey, serviceFolder);
            InstallService(serviceFolder);

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult FinalizeUnInstall(Session session)
        {
            UnInstallService();

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult PreInstallationAction(Session session)
        {
            session["SKIPCONNECTIONSTRINGSTEP"] = "0";

            session["SERVICEFOLDER"] = session["PI_SCHEDULER_INSTALL_DIR"];

            var servicePath = SecurityUtils.GetServicePath("FuseCP Scheduler");

            if (!string.IsNullOrEmpty(servicePath))
            {
                string path = Path.Combine(servicePath, "web.config");

                if (File.Exists(path))
                {
                    using (var reader = new StreamReader(path))
                    {
                        string content = reader.ReadToEnd();
                        var pattern = new Regex(@"(?<=<add key=""FuseCP.CryptoKey"" .*?value\s*=\s*"")[^""]+(?="".*?>)");
                        Match match = pattern.Match(content);
                        session["PREVIOUSCRYPTOKEY"] = match.Value;

                        var connectionStringPattern = new Regex(@"(?<=<add name=""EnterpriseServer"" .*?connectionString\s*=\s*"")[^""]+(?="".*?>)");
                        match = connectionStringPattern.Match(content);
                        session["PREVIOUSCONNECTIONSTRING"] = match.Value.Replace(";", CustomDataDelimiter);
                    }

                    session["SKIPCONNECTIONSTRINGSTEP"] = "1";

                    if (string.IsNullOrEmpty(session["SERVICEFOLDER"]))
                    {
                        session["SERVICEFOLDER"] = servicePath;
                    }
                } 

            }

            return ActionResult.Success;
        }

        private static void InstallService(string installFolder)
        {
            try
            {
                var schedulerService =
                    ServiceController.GetServices().FirstOrDefault(
                        s => s.DisplayName.Equals("FuseCP Scheduler", StringComparison.CurrentCultureIgnoreCase));

                if (schedulerService != null)
                {
                    StopService(schedulerService.ServiceName);

                    SecurityUtils.DeleteService(schedulerService.ServiceName);
                }

                ManagedInstallerClass.InstallHelper(new[] { "/i", Path.Combine(installFolder, "FuseCP.SchedulerService.exe") });

                StartService("FuseCP Scheduler");
            }
            catch (Exception)
            {
            }
        }

        private static void UnInstallService()
        {
            try
            {
                var schedulerService =
                    ServiceController.GetServices().FirstOrDefault(
                        s => s.DisplayName.Equals("FuseCP Scheduler", StringComparison.CurrentCultureIgnoreCase));

                if (schedulerService != null)
                {
                    StopService(schedulerService.ServiceName);

                    SecurityUtils.DeleteService(schedulerService.ServiceName);
                }
            }
            catch (Exception)
            {
            }
        }

        private static void ChangeConfigString(string nodePath, string attrToChange, string value, string installFolder)
        {
            string path = Path.Combine(installFolder, "web.config");

            if (!File.Exists(path))
            {
                return;
            }

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(path);

            XmlElement node = xmldoc.SelectSingleNode(nodePath) as XmlElement;

            if (node != null)
            {
                node.SetAttribute(attrToChange, value);

                xmldoc.Save(path);
            }
        }


        private static void StopService(string serviceName)
        {
            var sc = new ServiceController(serviceName);

            if (sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
            }
        }

        private static void StartService(string serviceName)
        {
            var sc = new ServiceController(serviceName);

            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
            }
        }

        private static string GetConnectionString(string serverName, string databaseName)
        {
            return string.Format("Server={0};database={1};Trusted_Connection=true;", serverName, databaseName).Replace(";", CustomDataDelimiter);
        }

        private static string GetConnectionString(string serverName, string databaseName, string login, string password)
        {
            return string.Format("Server={0};database={1};uid={2};password={3};", serverName, databaseName, login, password).Replace(";", CustomDataDelimiter);
        }

        private static bool CheckConnection(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            bool result = true;

            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return result;
        }

        private static string GetCustomActionProperty(Session session, string key)
        {
            if (session.CustomActionData.ContainsKey(key))
            {
                return session.CustomActionData[key].Replace("-=-", ";");
            }

            return string.Empty;
        }
    }
}
