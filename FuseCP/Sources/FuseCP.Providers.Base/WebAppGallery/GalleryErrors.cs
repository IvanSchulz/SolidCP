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

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.Providers.WebAppGallery
{
	public static class GalleryErrors
	{
        // FCP Server
        public const string ProcessingFeedXMLError = "ProcessingFeedXMLError"; // + exception
        public const string PackageFileNotFound = "PackageFileNotFound"; // + exception
        public const string ProcessingPackageError = "ProcessingPackageError"; // + exception
        public const string PackageInstallationError = "PackageInstallationError"; // + exception

        // application requirements
        public const string PackageDoesNotMeetRequirements = "PackageDoesNotMeetRequirements";
        public const string AspNet20Required = "AspNet20Required";
        public const string AspNet40Required = "AspNet40Required";
        public const string PhpRequired = "PhpRequired";
        public const string DatabaseRequired = "DatabaseRequired";
        public const string SQLRequired = "SQLRequired";
        public const string MySQLRequired = "MySQLRequired";
        public const string MariaDBRequired = "MariaDBRequired";

        // Common
        public const string MsDeployIsNotInstalled = "MsDeployIsNotInstalled";
        public const string GeneralError = "GeneralError"; // + exception message

        // Languages
        public const string GetLanguagesError = "GetLanguagesError";
        
        // Categories
        public const string GetCategoriesError = "GetCategoriesError";

        // Applications
        public const string GetApplicationsError = "GetApplicationsError";
        public const string GetApplicationError = "GetApplicationError";
        public const string GetApplicationParametersError = "GetApplicationParametersError";

        // Install app
        public const string WebApplicationNotFound = "WebApplicationNotFound"; // + app id
        public const string WebSiteNotFound = "WebSiteNotFound"; // + web site name
        public const string AppPathParameterNotFound = "AppPathParameterNotFound";
        public const string DatabaseServiceIsNotAvailable = "DatabaseServiceIsNotAvailable";
        public const string DatabaseServerExternalAddressIsEmpty = "DatabaseServerExternalAddressIsEmpty";
        public const string DatabaseAdminUsernameNotSpecified = "DatabaseAdminUsernameNotSpecified";
        public const string DatabaseAdminPasswordNotSpecified = "DatabaseAdminPasswordNotSpecified";
        public const string DatabaseCreationError = "DatabaseCreationError";
        public const string DatabaseCreationException = "DatabaseCreationException"; // + exception message
        public const string DatabaseUserCreationError = "DatabaseUserCreationError";
        public const string DatabaseUserCreationException = "DatabaseUserCreationException"; // + exception message
        public const string DatabaseUserCannotAccessDatabase = "DatabaseUserCannotAccessDatabase"; // + username
        public const string ApplicationInstallationError = "ApplicationInstallationError";
	}

}
