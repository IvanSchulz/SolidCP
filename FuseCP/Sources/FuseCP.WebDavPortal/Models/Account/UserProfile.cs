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

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using FuseCP.WebDavPortal.CustomAttributes;
using FuseCP.WebDavPortal.Models.Common;
using FuseCP.WebDavPortal.UI.Routes;

namespace FuseCP.WebDavPortal.Models.Account
{
    public class UserProfile 
    {
        [Display(ResourceType = typeof(Resources.UI), Name = "PrimaryEmail")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "EmailInvalid", ErrorMessage = null)]
        public string PrimaryEmailAddress { get; set; }

        [Display(ResourceType = typeof(Resources.UI), Name = "DisplayName")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        public string DisplayName { get; set; }
        public string AccountName { get; set; }

        [Display(ResourceType = typeof(Resources.UI), Name = "FirstName")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        public string FirstName { get; set; }
        public string Initials { get; set; }

        [Display(ResourceType = typeof(Resources.UI), Name = "LastName")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Office { get; set; }

        [PhoneNumber(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PhoneNumberInvalid")]
        [UniqueAdPhoneNumber(AccountRouteNames.PhoneNumberIsAvailible, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "AlreadyInUse")]
        [Display(ResourceType = typeof(Resources.UI), Name = "BusinessPhone")]
        public string BusinessPhone { get; set; }

        [PhoneNumber(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PhoneNumberInvalid")]
        [UniqueAdPhoneNumber(AccountRouteNames.PhoneNumberIsAvailible, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "AlreadyInUse")]
        [Display(ResourceType = typeof(Resources.UI), Name = "Fax")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(Resources.UI), Name = "HomePhone")]
        [PhoneNumber(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PhoneNumberInvalid")]
        [UniqueAdPhoneNumber(AccountRouteNames.PhoneNumberIsAvailible, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "AlreadyInUse")]
        public string HomePhone { get; set; }

        [Display(ResourceType = typeof(Resources.UI), Name = "MobilePhone")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        [PhoneNumber(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PhoneNumberInvalid")]
        [UniqueAdPhoneNumber(AccountRouteNames.PhoneNumberIsAvailible, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "AlreadyInUse")]
        public string MobilePhone { get; set; }

        [PhoneNumber(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PhoneNumberInvalid")]
        [Display(ResourceType = typeof(Resources.UI), Name = "Pager")]
        [UniqueAdPhoneNumber(AccountRouteNames.PhoneNumberIsAvailible,ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "AlreadyInUse")]
        public string Pager { get; set; }

        [Url(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "UrlInvalid", ErrorMessage = null)]
        public string WebPage { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "EmailInvalid", ErrorMessage = null)]
        public string ExternalEmail { get; set; }

        [UIHint("CountrySelector")]
        public string Country { get; set; }

        public string Notes { get; set; }
        public DateTime PasswordExpirationDateTime { get; set; }
    }
}
