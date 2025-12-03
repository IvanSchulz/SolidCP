using System.ComponentModel.DataAnnotations;
using FuseCP.WebDavPortal.Models.Common;

namespace FuseCP.WebDavPortal.Models.Account
{
    public class PasswordResetPincodeModel
    {
        [Required]
        public string Sms { get; set; }
        public bool IsTokenExist { get; set; }
    }
}
