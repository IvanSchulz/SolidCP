using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FuseCP.WebDav.Core;
using FuseCP.WebDavPortal.DependencyInjection;
using FuseCP.WebDavPortal.Models;

namespace FuseCP.WebDavPortal.Constraints
{
    public class OrganizationRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (ScpContext.User == null)
            {
                return false;
            }

            object value;
            if (!values.TryGetValue(parameterName, out value))
                return false;

            var str = value as string;
            if (str == null)
                return false;

            return ScpContext.User.OrganizationId == str;
        }
    }
}
