using FuseCP.WebDavPortal.Models.Common;

namespace FuseCP.WebDavPortal.Models
{
    public class OfficeOnlineModel 
    {
        public string Url { get; set; }
        public string FileName { get; set; }
        public string Backurl { get; set; }

        public OfficeOnlineModel(string url, string fileName, string backUrl)
        {
            Url = url;
            FileName = fileName;
            Backurl = backUrl;
        }
    }
}
