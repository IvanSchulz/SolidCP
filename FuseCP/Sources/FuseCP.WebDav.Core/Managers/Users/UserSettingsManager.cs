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

using FuseCP.WebDav.Core.Entities.Account;
using FuseCP.WebDav.Core.Entities.Account.Enums;
using FuseCP.WebDav.Core.Helper;
using FuseCP.WebDav.Core.Interfaces.Managers.Users;
using FuseCP.WebDav.Core.Scp.Framework;

namespace FuseCP.WebDav.Core.Managers.Users
{
    public class UserSettingsManager : IUserSettingsManager
    {
        public UserPortalSettings GetUserSettings(int accountId)
        {
            string xml = FCP.Services.EnterpriseStorage.GetWebDavPortalUserSettingsByAccountId(accountId);

            if (string.IsNullOrEmpty(xml))
            {
                return new UserPortalSettings();
            }

            return SerializeHelper.Deserialize<UserPortalSettings>(xml);
        }

        public void UpdateSettings(int accountId, UserPortalSettings settings)
        {
            var xml = SerializeHelper.Serialize(settings);

            FCP.Services.EnterpriseStorage.UpdateWebDavPortalUserSettings(accountId, xml);
        }

        public void ChangeWebDavViewType(int accountId, FolderViewTypes type)
        {
            var settings = GetUserSettings(accountId);

            settings.WebDavViewType = type;

            UpdateSettings(accountId, settings);
        }
    }
}
