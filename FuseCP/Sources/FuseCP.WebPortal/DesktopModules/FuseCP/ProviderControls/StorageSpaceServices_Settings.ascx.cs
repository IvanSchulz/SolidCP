using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FuseCP.Portal.ProviderControls
{
    public partial class StorageSpaceServices_Settings : FuseCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
            }
        }

        public void BindSettings(StringDictionary settings)
        {
        }

        public void SaveSettings(StringDictionary settings)
        {
        }
    }
}
