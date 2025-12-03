using FuseCP.EnterpriseServer.Base.HostedSolution;
using FuseCP.WebDav.Core.Client;
using FuseCP.WebDav.Core.Entities.Owa;

namespace FuseCP.WebDav.Core.Interfaces.Owa
{
    public interface IWopiServer
    {
        CheckFileInfo GetCheckFileInfo(WebDavAccessToken token);
        byte[] GetFileBytes(int accessTokenId);
    }
}
