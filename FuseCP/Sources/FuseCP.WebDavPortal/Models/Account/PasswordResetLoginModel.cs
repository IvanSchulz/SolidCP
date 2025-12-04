using System.ComponentModel.DataAnnotations;
using FuseCP.WebDavPortal.Models.Common;
using FuseCP.WebDavPortal.Resources;

namespace FuseCP.WebDavPortal.Models.Account
{
    public class PasswordResetLoginModel 
    {
        [Required]
        [Display(ResourceType = typeof(Resources.UI), Name = "Email")]
        [EmailAddress(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "EmailInvalid",ErrorMessage = null)]
        public string Email { get; set; }
    }
}
