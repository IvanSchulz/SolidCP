using Ninject;
using System.Web.SessionState;
using FuseCP.WebDav.Core.Interfaces.Managers;
using FuseCP.WebDav.Core.Interfaces.Managers.Users;
using FuseCP.WebDav.Core.Interfaces.Owa;
using FuseCP.WebDav.Core.Interfaces.Security;
using FuseCP.WebDav.Core.Interfaces.Services;
using FuseCP.WebDav.Core.Interfaces.Storages;
using FuseCP.WebDav.Core.Managers;
using FuseCP.WebDav.Core.Managers.Users;
using FuseCP.WebDav.Core.Owa;
using FuseCP.WebDav.Core.Security.Authentication;
using FuseCP.WebDav.Core.Security.Authorization;
using FuseCP.WebDav.Core.Security.Cryptography;
using FuseCP.WebDav.Core.Services;
using FuseCP.WebDav.Core.Storages;
using FuseCP.WebDavPortal.DependencyInjection.Providers;

namespace FuseCP.WebDavPortal.DependencyInjection
{
    public class PortalDependencies
    {
        public static void Configure(IKernel kernel)
        {
            kernel.Bind<HttpSessionState>().ToProvider<HttpSessionStateProvider>();
            kernel.Bind<ICryptography>().To<CryptoUtils>();
            kernel.Bind<IAuthenticationService>().To<FormsAuthenticationService>();
            kernel.Bind<IWebDavManager>().To<WebDavManager>();
            kernel.Bind<IAccessTokenManager>().To<AccessTokenManager>();
            kernel.Bind<IWopiServer>().To<WopiServer>();
            kernel.Bind<IWopiFileManager>().To<CobaltSessionManager>();
            kernel.Bind<IWebDavAuthorizationService>().To<WebDavAuthorizationService>();
            kernel.Bind<ICobaltManager>().To<CobaltManager>();
            kernel.Bind<ITtlStorage>().To<CacheTtlStorage>();
            kernel.Bind<IUserSettingsManager>().To<UserSettingsManager>();
            kernel.Bind<ISmsDistributionService>().To<TwillioSmsDistributionService>();
            kernel.Bind<ISmsAuthenticationService>().To<SmsAuthenticationService>();
        }
    }
}
