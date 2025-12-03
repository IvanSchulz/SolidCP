using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using FuseCP.WebDavPortal.DependencyInjection;
using FuseCP.WebDavPortal.Models;
using FuseCP.WebDavPortal.UI.Routes;

namespace FuseCP.WebDavPortal.CustomAttributes
{
    public class LdapAuthorizationAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(AccountRouteNames.Login, null);
        }
    }
}
