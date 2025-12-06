using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using FuseCP.Providers;
using FuseCP.Providers.OS;
using FuseCP.Portal;
using FuseCP.WebPortal;
using System.Text.RegularExpressions;


namespace FuseCP.Providers.Virtualization.NoVNC
{
    public partial class vnc : System.Web.UI.Page
    {
        public const string DEFAULT_PAGE = "~/Default.aspx";
        public const string PAGE_ID_PARAM = "pid";
        public const string ITEM_ID_PARAM = "item";

        public string Host { get; set; }
        public int Port { get; set; }
        public string Path { get; set; }
        public string VmName { get; set; }
        public string Password { get; set; }

        public string EscapedJSString(string text)
        {
            text = text.Replace(@"\", @"\\")
                .Replace(@"'", @"\'");
            return $"'{text}'";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var loggedIn = PanelSecurity.LoggedUser != null;
            if (!loggedIn)
            {
                // redirect to login page
                string returnUrl = Request.RawUrl;
                Response.Redirect(DEFAULT_PAGE + "?" + PAGE_ID_PARAM + "=" +
                    PortalConfiguration.SiteSettings["LoginPage"] + "&ReturnUrl=" + Server.UrlEncode(returnUrl));
            }

            var baseUrl = Regex.Match(Request.Path, "(?<=^/).*(?=/.*?.aspx)")?.Value;

            Host = EscapedJSString(Request.Url.Host);
            Port = Request.Url.Port;

            var item = Request.QueryString[ITEM_ID_PARAM];
            if (item != null)
            {
                var itemId = int.Parse(item);
                var vm = Portal.ES.Services.Packages.GetPackageItem(itemId) as VirtualMachine;
                if (vm != null) Page.Title = VmName = vm.Name;
                else Page.Title = VmName = "noVNC";

                var vncCredentials = Portal.ES.Services.Proxmox.GetPveVncCredentials(itemId);
                if (vncCredentials != null)
                {
                    Path = EscapedJSString($"{baseUrl}/websocket?user={Uri.EscapeDataString(PanelSecurity.LoggedUser.Username)}&item={itemId}&vncpassword={Uri.EscapeDataString(vncCredentials.Password)}");
                    Password = EscapedJSString(vncCredentials.Password);

                    //var url = $"{baseUrl}/vnc.html?autoconnect=true&host={Request.Url.Host}&port={Request.Url.Port}&password={Uri.EscapeDataString(vncCredentials.Password)}&path={Uri.EscapeDataString(path)}";
                    //Response.Redirect(url);
                }
            }

            var htmlText = File.ReadAllText(Server.MapPath("~/novnc/vnc.html"));

            var match = Regex.Match(htmlText, @"<head[^/>]*?>(?<head>.*?)</head>.*?<body[^/>]*?>(?<body>.*?)</body>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            string head = match.Groups["head"].Value;
            string body = match.Groups["body"].Value;
            bodyHtml.Text = body;
            // remove title & scripts from head
            head = Regex.Replace(head, @"(?:<title>.*?</title>|<script[^>]*(?<!/)>.*?</script>|<script[^>]*?/>)(?:[ \t]*?\r?\n)?", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            headHtml.Text = head;
        }
    }
}
