using System.Collections.Generic;
using FuseCP.WebDavPortal.Models.Common;

namespace FuseCP.WebDavPortal.Models.FileSystem
{
    public class DeleteFilesModel : AjaxModel
    {
        public DeleteFilesModel()
        {
            DeletedFiles = new List<string>();
        }

        public List<string> DeletedFiles { get; set; }
    }
}
