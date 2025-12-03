using System;
using FuseCP.WebDavPortal.Models.Common;

namespace FuseCP.WebDavPortal.Models
{
    public class ErrorModel
    {
        public int HttpStatusCode { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
