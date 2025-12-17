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

namespace  FuseCP.Providers.FTP.IIs100
{
	using FuseCP.Providers.FTP.IIs100.Config;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Ftp.Configuration;
    using Microsoft.Web.Management.Server;
    using System;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Web.UI;

    internal static class FtpHelper
    {
        public const string ConfigurationError = "ConfigurationError";
        public const string FtpProtocol = "ftp";
        private const string GetSettingsExceptionError = "GetSettingsExceptionError";
        private const string SiteIsNotFtpSiteExceptionError = "SiteIsNotFtpSiteExceptionError";
        private const string szOID_ENHANCED_KEY_USAGE = "2.5.29.37";
        private const string szOID_PKIX_KP_SERVER_AUTH = "1.3.6.1.5.5.7.3.1";

        public static bool CanAuthenticateServer(X509Certificate2 certificate)
        {
            bool flag = false;
            foreach (X509Extension extension in certificate.Extensions)
            {
                if (string.Equals(extension.Oid.Value, "2.5.29.37", StringComparison.Ordinal))
                {
                    flag = true;
                    X509EnhancedKeyUsageExtension extension2 = extension as X509EnhancedKeyUsageExtension;
                    if (extension2 != null)
                    {
                        OidEnumerator enumerator = extension2.EnhancedKeyUsages.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            if (string.Equals(enumerator.Current.Value, "1.3.6.1.5.5.7.3.1", StringComparison.Ordinal))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return !flag;
        }

        public static string ConvertDistinguishedNameToString(X500DistinguishedName dnString)
        {
            string name = dnString.Name;
            bool flag = false;
            string[] strArray = dnString.Decode(X500DistinguishedNameFlags.UseNewLines).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length > 0)
            {
                flag = true;
                string pairAndValue = string.Empty;
                for (int i = 0; i < strArray.Length; i++)
                {
                    pairAndValue = strArray[i];
                    Pair pair = ConvertStringToPair(pairAndValue);
                    if (string.Equals((string) pair.First, "CN", StringComparison.OrdinalIgnoreCase))
                    {
                        name = (string) pair.Second;
                        flag = false;
                        break;
                    }
                }
            }
            else
            {
                name = (string) ConvertStringToPair(name).Second;
                flag = false;
            }
            if (flag)
            {
                name = dnString.Name;
            }
            return name;
        }

        private static Pair ConvertStringToPair(string pairAndValue)
        {
            Pair pair = new Pair(pairAndValue, pairAndValue);
            int length = -1;
            length = pairAndValue.IndexOf("=", StringComparison.Ordinal);
            if ((length != -1) && (pairAndValue.Length >= (length + 1)))
            {
                string x = pairAndValue.Substring(0, length);
                string y = pairAndValue.Substring(length + 1);
                pair = new Pair(x, y);
            }
            return pair;
        }

        public static ConfigurationSection GetAppHostSection(ServerManager serverManager, string sectionName, Type type, ManagementConfigurationPath configPath)
        {
            if ((serverManager == null) || (configPath == null))
            {
                throw new ArgumentNullException("ConfigurationError");
            }

			Configuration applicationHostConfiguration = serverManager.GetApplicationHostConfiguration();
            string effectiveConfigurationPath = configPath.GetEffectiveConfigurationPath(ManagementScope.Server);
            ConfigurationSection section = applicationHostConfiguration.GetSection(sectionName, type, effectiveConfigurationPath);
            if (section == null)
            {
                throw new NullReferenceException("ConfigurationError");
            }
            return section;
        }

        public static FtpSite GetFtpSite(ManagementConfigurationPath configPath, ServerManager serverManager)
        {
            FtpSite ftpSiteDefaultElement = null;
            if (configPath.PathType == ConfigurationPathType.Server)
            {
                ftpSiteDefaultElement = GetFtpSiteDefaultElement(serverManager.SiteDefaults);
            }
            else
            {
                Site site = serverManager.Sites[configPath.SiteName];
                if (site == null)
                {
                    WebManagementServiceException exception = new WebManagementServiceException("GetSettingsExceptionError", string.Empty);
                    throw exception;
                }
                if (!IsFtpSite(site))
                {
                    WebManagementServiceException exception2 = new WebManagementServiceException("SiteIsNotFtpSiteExceptionError", string.Empty);
                    throw exception2;
                }
                ftpSiteDefaultElement = GetFtpSiteElement(site);
            }
            if (ftpSiteDefaultElement == null)
            {
                WebManagementServiceException exception3 = new WebManagementServiceException("GetSettingsExceptionError", string.Empty);
                throw exception3;
            }
            return ftpSiteDefaultElement;
        }

        public static FtpSite GetFtpSiteDefaultElement(SiteDefaults siteDefaults)
        {
            return (FtpSite) siteDefaults.GetChildElement("ftpServer", typeof(FtpSite));
        }

        public static FtpSite GetFtpSiteElement(Site site)
        {
            return (FtpSite) site.GetChildElement("ftpServer", typeof(FtpSite));
        }

        public static bool IsFtpSite(Site site)
        {
            foreach (Binding binding in site.Bindings)
            {
                if (string.Equals(binding.Protocol, "ftp", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

