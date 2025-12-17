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
using System.Globalization;
using FuseCP.WebDav.Core.Config;
using FuseCP.WebDav.Core.Interfaces.Security;
using FuseCP.WebDav.Core.Interfaces.Services;

namespace FuseCP.WebDav.Core.Security.Authentication
{
    public class SmsAuthenticationService : ISmsAuthenticationService
    {
        private ISmsDistributionService _smsService;

        public SmsAuthenticationService(ISmsDistributionService smsService)
        {
            _smsService = smsService;
        }

        public bool VerifyResponse( Guid token, string response)
        {
            var accessToken = ScpContext.Services.Organizations.GetPasswordresetAccessToken(token);

            if (accessToken == null)
            {
                return false;
            }

            return string.Compare(accessToken.SmsResponse, response, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public string SendRequestMessage(string phoneTo)
        {
            var response = GenerateResponse();

            var result = _smsService.SendMessage(WebDavAppConfigManager.Instance.TwilioParameters.PhoneFrom, phoneTo, response);

            return result ? response : string.Empty;
        }

        public string GenerateResponse()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());

            return random.Next(10000, 99999).ToString(CultureInfo.InvariantCulture);
        }
    }
}
