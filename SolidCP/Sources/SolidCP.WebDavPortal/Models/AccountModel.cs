using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FuseCP.Providers.HostedSolution;
using FuseCP.WebDavPortal.Models.Common;

namespace FuseCP.WebDavPortal.Models
{
    public class AccountModel
    {
        [Required]
        [Display(Name = @"Login")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = @"Password")]
        public string Password { get; set; }

        public string LdapError { get; set; }

        public bool PasswordResetEnabled { get; set; }
    }
}
