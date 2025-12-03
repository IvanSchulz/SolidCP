using System;
using System.Web;
using System.Web.Script.Serialization;
using FuseCP.Portal.ExchangeServer;
using FuseCP.Providers.OS;

namespace FuseCP.Portal
{
    public class EnterpriseFolderDataHandler : IHttpHandler
    {
        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            string folderName = context.Request.Params["folderName"];
            string itemIndex = context.Request.Params["itemIndex"];
            int itemId = Convert.ToInt32(context.Request.Params["itemId"]);

            var folder = ES.Services.EnterpriseStorage.GetEnterpriseFolderWithExtraData(itemId, folderName, true) ?? new SystemFile();

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(string.Format("{0}:{1}:{2}", EnterpriseStorageFolders.ConvertMBytesToGB(folder.Size) + " GB", itemIndex, string.IsNullOrEmpty(folder.DriveLetter) ? "not mapped" : folder.DriveLetter));
            context.Response.ContentType = "text/plain";
            context.Response.Write(json);
        }
    }
}
