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
using System.Text;

namespace FuseCP.Providers.FTP.IIs100
{
	internal class FtpSiteGlobals
	{
		//
		public const int BindingProtocol =						0;
		//
		public const int SslCertificate_FriendlyName =			0;
		//
		public const int Authorization_Users =					1;
		//
		public const int BindingInformation =					1;
		//
		public const int SslCertificate_Hash =					1;
		//
		public const int Authorization_Roles =					2;
		//
		public const int SslCertificate_IssuedTo =				2;
		//
		public const int BindingIndex =							2;
		//
		public const int Authorization_Permission =				3;
		//
		public const int Site_Name =							100;
		//
		public const int Site_ID =								103;
		//
		public const int Site_SingleBinding =					104;
		//
		public const int Site_Bindings =						105;
		//
		public const int AppVirtualDirectory_PhysicalPath =		300;
		//
		public const int AppVirtualDirectory_UserName =			301;
		//
		public const int AppVirtualDirectory_Password =			302;
		//
		public const int AppVirtualDirectory_Password_Set =		303;
		//
		public const int FtpSite_AutoStart =					350;
		//
		public const int FtpSite_Status =						351;
		//
		public const int Connections_UnauthenticatedTimeout =	400;
		//
		public const int Connections_ControlChannelTimeout =	401;
		//
		public const int Connections_DisableSocketPooling =		402;
		//
		public const int Connections_ServerListenBacklog =		403;
		//
		public const int Connections_DataChannelTimeout =		404;
		//
		public const int Connections_MinBytesPerSecond =		405;
		//
		public const int Connections_MaxConnections =			406;
		//
		public const int Connections_ResetOnMaxConnection =		407;
		//
		public const int Ssl_ServerCertHash =					410;
		//
		public const int Ssl_ControlChannelPolicy =				411;
		//
		public const int Ssl_DataChannelPolicy =				412;
		//
		public const int Ssl_Ssl128 =							413;
		//
		public const int Authentication_AnonymousEnabled =		420;
		//
		public const int Authentication_BasicEnabled =			421;
		//
		public const int Authorization_Rule =					422;
		//
		public const string FtpServerElementName =	"ftpServer";
		//
		public const string SearchHostHeader =		"SearchHostHeader";
		//
		public const string SearchIPAddress =		"SearchIPAddress";
		//
		public const string SearchPhysicalPath =	"SearchPhysicalPath";
		//
		public const string SearchPort =			"SearchPort";
		//
		public const string SearchSiteName =		"SearchSiteName";
	}
}
