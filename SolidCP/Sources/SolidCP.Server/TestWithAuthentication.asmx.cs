using System.ComponentModel;
using FuseCP.Web.Services;
using FuseCP.Providers;
using System.Linq;

namespace FuseCP.Server
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

    // setting the policy causes the UserNamePasswordValidator in FuseCP.Web.Services to validate the password against the 
    // server password specified in web.config.
    [Policy("ServerPolicy")]

    [ToolboxItem(false)]

    public class TestWithAuthentication : HostingServiceProviderWebService
    {

        // a simple method echoing the message
        [WebMethod]
        public string Echo(string message)
        {
            return message;
        }

        // a method that receives a soap header. The soap header is written to the field settings of the base type HostingServiceProviderWebService.
        // In the client proxy, the soap header can be set by assign the header to  the SoapHeader property. The soap header is set in the client
        // in SoapHeaderMessageInspector in FuseCP.Web.Clients, and is read in the server by SoapHeaderMessageInspector in FuseCP.Web.Services

        [WebMethod, SoapHeader("settings")]
        public string EchoSettings()
        {
            return settings.Settings.FirstOrDefault() ?? string.Empty;
        }
    }
}
