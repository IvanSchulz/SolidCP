// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

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
