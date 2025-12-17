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

namespace FuseCP.Providers.FTP.IIs100
{
	using FuseCP.Providers.FTP.IIs100.Config;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Ftp;
    using Microsoft.Web.Management.Ftp.Configuration;
    using Microsoft.Web.Management.Server;
    using System;
    using System.Collections;

    internal class SitesHelper
    {
        public const string FtpProtocol = "ftp";

        public static void DeserializeFtpSiteProperties(FtpSite ftpSite, PropertyBag bag)
        {
			foreach (int num in bag.ModifiedKeys)
			{
				switch (num)
				{
					case FtpSiteGlobals.Connections_UnauthenticatedTimeout:
						ftpSite.Connections.UnauthenticatedTimeout = (int)bag[num];
						break;
					case FtpSiteGlobals.Connections_ControlChannelTimeout:
						ftpSite.Connections.ControlChannelTimeout = (int)bag[num];
						break;
					case FtpSiteGlobals.Connections_DisableSocketPooling:
						ftpSite.Connections.DisableSocketPooling = (bool)bag[num];
						break;
					case FtpSiteGlobals.Connections_ServerListenBacklog:
						ftpSite.Connections.ServerListenBacklog = (int)bag[num];
						break;
					case FtpSiteGlobals.Connections_DataChannelTimeout:
						ftpSite.Connections.DataChannelTimeout = (int)bag[num];
						break;
					case FtpSiteGlobals.Connections_MinBytesPerSecond:
						ftpSite.Connections.MinBytesPerSecond = (int)bag[num];
						break;
					case FtpSiteGlobals.Connections_MaxConnections:
						ftpSite.Connections.MaxConnections = (long)bag[num];
						break;
					case FtpSiteGlobals.Connections_ResetOnMaxConnection:
						ftpSite.Connections.ResetOnMaxConnections = (bool)bag[num];
						break;
					case FtpSiteGlobals.Ssl_ServerCertHash:
						ftpSite.Security.Ssl.ServerCertHash = (string)bag[num];
						break;
					case FtpSiteGlobals.Ssl_ControlChannelPolicy:
						ftpSite.Security.Ssl.ControlChannelPolicy = (ControlChannelPolicy)bag[num];
						break;
					case FtpSiteGlobals.Ssl_DataChannelPolicy:
						ftpSite.Security.Ssl.DataChannelPolicy = (DataChannelPolicy)bag[num];
						break;
					case FtpSiteGlobals.Authentication_AnonymousEnabled:
						ftpSite.Security.Authentication.AnonymousAuthentication.Enabled = (bool)bag[num];
						break;
					case FtpSiteGlobals.Authentication_BasicEnabled:
						ftpSite.Security.Authentication.BasicAuthentication.Enabled = (bool)bag[num];
						break;
					case FtpSiteGlobals.FtpSite_AutoStart:
						ftpSite.ServerAutoStart = (bool)bag[num];
						break;
				}
			}
        }

        public static void DeserializeSiteProperties(Site site, PropertyBag bag)
        {
            foreach (int num in bag.ModifiedKeys)
            {
                switch (num)
                {
					case FtpSiteGlobals.Site_Name:
                        string str = (string) bag[num];
                        site.Name = str;
						break;
                }
            }
            Application element = site.Applications["/"];
            if (element == null)
            {
                element = site.Applications.CreateElement();
                element.Path = "/";
                site.Applications.Add(element);
            }
            VirtualDirectory directory = element.VirtualDirectories["/"];
            if (directory == null)
            {
                directory = element.VirtualDirectories.CreateElement();
                directory.Path = "/";
                element.VirtualDirectories.Add(directory);
            }
            DeserializeAppVirtualDirectoryProperties(directory, bag);
            DeserializeFtpSiteProperties(FtpHelper.GetFtpSiteElement(site), bag);
        }

        public static void DeserializeAppVirtualDirectoryProperties(VirtualDirectory vDir, PropertyBag bag)
        {
            foreach (int num in bag.ModifiedKeys)
            {
                switch (num)
                {
					case FtpSiteGlobals.AppVirtualDirectory_PhysicalPath:
						vDir.PhysicalPath = (string)bag[num];
                        break;
					case FtpSiteGlobals.AppVirtualDirectory_UserName:
                        if (PasswordExistsAndSet(bag))
                        {
                            string str2 = (string)bag[num];
                            if (!String.IsNullOrEmpty(str2))
								vDir.UserName = str2;
                        }
                        break;
					case FtpSiteGlobals.AppVirtualDirectory_Password:
                        if (PasswordExistsAndSet(bag))
                        {
							string str3 = (string)bag[FtpSiteGlobals.AppVirtualDirectory_Password];
                            if (String.IsNullOrEmpty(str3))
								goto PASS_DELETE;
							//
                            vDir.Password = str3;
                        }
                        break;
                }
                vDir.UserName = string.Empty;
            PASS_DELETE:
                vDir.GetAttribute("password").Delete();
            }
        }

        public static ArrayList GetAllBindings(BindingCollection bindings)
        {
            ArrayList list = new ArrayList();
            int num = 0;
            foreach (Binding binding in bindings)
            {
                PropertyBag bag = new PropertyBag();
                bag[FtpSiteGlobals.BindingProtocol] = binding.Protocol;
                bag[FtpSiteGlobals.BindingInformation] = binding.BindingInformation;
				bag[FtpSiteGlobals.BindingIndex] = num;
                list.Add(bag);
                num++;
            }
            return list;
        }

        public static ArrayList GetFtpBindings(BindingCollection bindings)
        {
            ArrayList list = new ArrayList();
            foreach (Binding binding in bindings)
            {
                if (string.Equals(binding.Protocol.Trim(), "ftp", StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(binding.BindingInformation);
                }
            }
            return list;
        }

        private static bool PasswordExistsAndSet(PropertyBag bag)
        {
			return (bag.Contains(FtpSiteGlobals.AppVirtualDirectory_Password_Set) && ((bool)bag[FtpSiteGlobals.AppVirtualDirectory_Password_Set]));
        }

        public static void SerializeFtpSiteProperties(FtpSite ftpSite, PropertyBag bag)
        {
			bag[FtpSiteGlobals.FtpSite_AutoStart] = ftpSite.ServerAutoStart;
			bag[FtpSiteGlobals.Connections_UnauthenticatedTimeout] = ftpSite.Connections.UnauthenticatedTimeout;
			bag[FtpSiteGlobals.Connections_ControlChannelTimeout] = ftpSite.Connections.ControlChannelTimeout;
			bag[FtpSiteGlobals.Connections_DisableSocketPooling] = ftpSite.Connections.DisableSocketPooling;
			bag[FtpSiteGlobals.Connections_ServerListenBacklog] = ftpSite.Connections.ServerListenBacklog;
			bag[FtpSiteGlobals.Connections_DataChannelTimeout] = ftpSite.Connections.DataChannelTimeout;
			bag[FtpSiteGlobals.Connections_MinBytesPerSecond] = ftpSite.Connections.MinBytesPerSecond;
			bag[FtpSiteGlobals.Connections_MaxConnections] = ftpSite.Connections.MaxConnections;
			bag[FtpSiteGlobals.Connections_ResetOnMaxConnection] = ftpSite.Connections.ResetOnMaxConnections;
			bag[FtpSiteGlobals.Ssl_ServerCertHash] = ftpSite.Security.Ssl.ServerCertHash;
			bag[FtpSiteGlobals.Ssl_ControlChannelPolicy] = (int)ftpSite.Security.Ssl.ControlChannelPolicy;
			bag[FtpSiteGlobals.Ssl_DataChannelPolicy] = (int)ftpSite.Security.Ssl.DataChannelPolicy;
			bag[FtpSiteGlobals.Ssl_Ssl128] = ftpSite.Security.Ssl.Ssl128;
			bag[FtpSiteGlobals.Authentication_AnonymousEnabled] = ftpSite.Security.Authentication.AnonymousAuthentication.Enabled;
			bag[FtpSiteGlobals.Authentication_BasicEnabled] = ftpSite.Security.Authentication.BasicAuthentication.Enabled;
        }

        public static PropertyBag SerializeSite(Site site)
        {
            PropertyBag bag = new PropertyBag();
            bag[FtpSiteGlobals.Site_Name] = site.Name;
			bag[FtpSiteGlobals.Site_ID] = site.Id;
            Application application = site.Applications["/"];
			bag[FtpSiteGlobals.AppVirtualDirectory_PhysicalPath] = string.Empty;
            if (application != null)
            {
                VirtualDirectory directory = application.VirtualDirectories["/"];
                if (directory != null)
                {
					bag[FtpSiteGlobals.AppVirtualDirectory_PhysicalPath] = directory.PhysicalPath;
					bag[FtpSiteGlobals.AppVirtualDirectory_UserName] = directory.UserName;
					bag[FtpSiteGlobals.AppVirtualDirectory_Password] = directory.Password;
                }
            }
			bag[FtpSiteGlobals.Site_Bindings] = GetFtpBindings(site.Bindings);
            FtpSite ftpSiteElement = FtpHelper.GetFtpSiteElement(site);
			bag[FtpSiteGlobals.FtpSite_Status] = (int)ftpSiteElement.State;
            return bag;
        }

        public static PropertyBag SerializeSiteDefaults(ServerManager serverManager)
        {
            FtpSite ftpSiteDefaultElement = FtpHelper.GetFtpSiteDefaultElement(serverManager.SiteDefaults);
            PropertyBag bag = new PropertyBag(true);
            SerializeFtpSiteProperties(ftpSiteDefaultElement, bag);
            return bag;
        }
    }
}

