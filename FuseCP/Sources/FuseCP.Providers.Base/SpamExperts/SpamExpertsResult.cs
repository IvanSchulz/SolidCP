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
    [Serializable]
    public enum SpamExpertsStatus
    {
        Success = 0,
        Error = 1,
        AlreadyExists = 2,
        NotFound = 3,
        None = 4,
        PermissionsError = 5
    }

    [Serializable]
    public class SpamExpertsResult
    {
        public string Result { get; set; }
        public SpamExpertsStatus Status { get; set; }

        public SpamExpertsResult() { Status = SpamExpertsStatus.None; Result = null; }

        public SpamExpertsResult(SpamExpertsStatus status, string result)
        {
            this.Status = status;
            this.Result = result;
        }

        public static SpamExpertsResult None => new SpamExpertsResult(SpamExpertsStatus.None, null);

    }

    public class SpamExpertsUser
    {
        public string id { get; set; }
        public string username { get; set; }

        public string parentid { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string status { get; set; }
    }

    public enum SpamExpertsUserRole
    {
        domain,
        email
    }
}
