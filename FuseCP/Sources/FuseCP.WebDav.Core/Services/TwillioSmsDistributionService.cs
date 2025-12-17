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

using Twilio;
using FuseCP.WebDav.Core.Config;
using FuseCP.WebDav.Core.Interfaces.Services;
using Twilio.Rest.Api.V2010.Account;

namespace FuseCP.WebDav.Core.Services
{
    public class TwillioSmsDistributionService : ISmsDistributionService
    {
        //private readonly TwilioRestClient _twilioRestClient;

        public TwillioSmsDistributionService()
        {
            //_twilioRestClient = new TwilioRestClient(WebDavAppConfigManager.Instance.TwilioParameters.AccountSid, WebDavAppConfigManager.Instance.TwilioParameters.AuthorizationToken);
               
        }


        public bool SendMessage(string phoneFrom, string phone, string message)
        {
            string accountSid = WebDavAppConfigManager.Instance.TwilioParameters.AccountSid;
            string authToken = WebDavAppConfigManager.Instance.TwilioParameters.AuthorizationToken;

            TwilioClient.Init(accountSid, authToken);

            var result = MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber(phoneFrom),
                to: new Twilio.Types.PhoneNumber(phone)
            );

            return string.IsNullOrEmpty(result.Status.ToString()) == false;


        }

        public bool SendMessage(string phone, string message)
        {
            string accountSid = WebDavAppConfigManager.Instance.TwilioParameters.AccountSid;
            string authToken = WebDavAppConfigManager.Instance.TwilioParameters.AuthorizationToken;

            TwilioClient.Init(accountSid, authToken);

            var result = MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber(WebDavAppConfigManager.Instance.TwilioParameters.PhoneFrom),
                to: new Twilio.Types.PhoneNumber(phone)
            );

            return string.IsNullOrEmpty(result.Status.ToString()) == false;
        }
    }
}
