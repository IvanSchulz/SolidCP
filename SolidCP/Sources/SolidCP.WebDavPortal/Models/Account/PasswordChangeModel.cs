using System.ComponentModel.DataAnnotations;
using FuseCP.Providers.HostedSolution;
using FuseCP.WebDavPortal.Models.Common;
using FuseCP.WebDavPortal.Models.Common.EditorTemplates;

namespace FuseCP.WebDavPortal.Models.Account
{
    public class PasswordChangeModel 
    {
        [Display(ResourceType = typeof (Resources.UI), Name = "OldPassword")]
        [Required(ErrorMessageResourceType = typeof (Resources.Messages), ErrorMessageResourceName = "Required")]
        public string OldPassword { get; set; }

        [UIHint("PasswordEditor")]
        public PasswordEditor PasswordEditor { get; set; }


        public PasswordChangeModel()
        {
            PasswordEditor = new PasswordEditor();
        }
    }
}
