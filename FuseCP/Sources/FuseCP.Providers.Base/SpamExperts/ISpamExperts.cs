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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuseCP.Providers.Filters
{
    public interface ISpamExperts
    {
        SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations);
        SpamExpertsResult DeleteDomainFilter(string domain);

        SpamExpertsResult AddEmailFilter(string name, string domain, string password);
        SpamExpertsResult DeleteEmailFilter(string email);

        SpamExpertsResult SetEmailFilterUserPassword(string email, string password);
        SpamExpertsResult SetDomainFilterUserPassword(string name, string password);

        SpamExpertsResult SetDomainFilterDestinations(string name, string[] destinations);

        SpamExpertsResult SetDomainFilterUser(string domain, string password, string email);
        SpamExpertsResult AddDomainFilterAlias(string domain, string alias);

        SpamExpertsResult DeleteDomainFilterAlias(string domain, string alias);
    }
}
