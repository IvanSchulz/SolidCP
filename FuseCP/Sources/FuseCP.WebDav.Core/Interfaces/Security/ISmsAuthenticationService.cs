using System;

namespace FuseCP.WebDav.Core.Interfaces.Security
{
    public interface ISmsAuthenticationService
    {
        bool VerifyResponse(Guid token, string response);
        string SendRequestMessage(string phoneTo);
        string GenerateResponse();
    }
}
