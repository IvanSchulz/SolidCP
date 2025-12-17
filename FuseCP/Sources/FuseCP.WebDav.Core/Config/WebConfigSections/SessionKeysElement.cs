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

using System.Configuration;

namespace FuseCP.WebDav.Core.Config.WebConfigSections
{
    public class SessionKeysElement : ConfigurationElement
    {
        private const string KeyKey = "key";
        private const string ValueKey = "value";

        public const string AccountInfoKey = "AccountInfoSessionKey";
        public const string AuthTicketKey = "AuthTicketKey";
        public const string WebDavManagerKey = "WebDavManagerSessionKey";
        public const string UserGroupsKey = "UserGroupsKey";
        public const string WebDavRootFolderPermissionsKey = "WebDavRootFolderPermissionsKey";
        public const string PasswordResetSmsKey = "PasswordResetSmsKey";
        public const string ResourseRenderCountKey = "ResourseRenderCountSessionKey";
        public const string ItemIdSessionKey = "ItemId";
        public const string OwaEditFoldersSessionKey = "OwaEditFoldersSession";
        public const string AccountIdKey = "AccountIdKey";

        [ConfigurationProperty(KeyKey, IsKey = true, IsRequired = true)]
        public string Key
        {
            get { return (string) this[KeyKey]; }
            set { this[KeyKey] = value; }
        }

        [ConfigurationProperty(ValueKey, IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string) this[ValueKey]; }
            set { this[ValueKey] = value; }
        }
    }
}
