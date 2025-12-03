using FuseCP.WebDav.Core.Entities.Account;
using FuseCP.WebDav.Core.Entities.Account.Enums;

namespace FuseCP.WebDav.Core.Interfaces.Managers.Users
{
    public interface IUserSettingsManager
    {
        UserPortalSettings GetUserSettings(int accountId);
        void UpdateSettings(int accountId, UserPortalSettings settings);
        void ChangeWebDavViewType(int accountId, FolderViewTypes type);
    }
}
