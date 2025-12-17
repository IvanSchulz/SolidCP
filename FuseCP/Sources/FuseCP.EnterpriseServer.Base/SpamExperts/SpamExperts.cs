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

namespace FuseCP.EnterpriseServer
{
    public class SpamExperts
    {


        //string emailAddress;
        //bool isPrimary;
        //bool isUserPrincipalName;


        public bool SEEnabled
        {
            get { return SEEnabled; }
            set { SEEnabled = value; }
        }

        public string schema
        {
            get { return this.schema; }
            set { this.schema = value; }
        }

        public string url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        public string user
        {
            get { return user; }
            set { user = value; }
        }

        public string password
        {
            get { return password; }
            set { password = value; }
        }

        public string ErrorMailSubject
        {
            get { return ErrorMailSubject; }
            set { ErrorMailSubject = value; }
        }

        public string ErrorMailBody
        {
            get { return ErrorMailBody; }
            set { ErrorMailBody = "{0}.Error: {1}"; }
        }

    }
}
