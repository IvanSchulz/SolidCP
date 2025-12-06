using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FuseCP.WebDav.Core.Security.Authentication.Principals;
using FuseCP.WebDav.Core.Scp.Framework;

namespace FuseCP.WebDav.Core
{
    public class ScpContext
    {
        public static ScpPrincipal User { get { return HttpContext.Current.User as ScpPrincipal; } }
        public static FCP Services { get { return FCP.Services; } }
    }
}
