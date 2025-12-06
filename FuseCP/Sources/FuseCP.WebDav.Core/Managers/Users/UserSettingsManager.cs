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
