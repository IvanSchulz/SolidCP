using FuseCP.Providers.StorageSpaces;

namespace FuseCP.EnterpriseServer
{
    public interface IStorageSpaceSelector
    {
        StorageSpace FindBest(string groupName, long quotaSizeInBytes);
    }
}
