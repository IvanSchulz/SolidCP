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
using System.Text;

namespace FuseCP.EnterpriseServer
{
    public class UsernamePolicy
    {
        bool enabled = false;
        string allowedSymbols = "a-zA-Z0-9\\.\\_";
        int minLength = -1;
        int maxLength = -1;
        string prefix = null;
        string suffix = null;

        public UsernamePolicy(string policyValue)
        {
            if (String.IsNullOrEmpty(policyValue))
                return;

            try
            {
                // parse settings
                string[] parts = policyValue.Split(';');

                enabled = Boolean.Parse(parts[0]);
                allowedSymbols += parts[1];
                minLength = Int32.Parse(parts[2]);
                maxLength = Int32.Parse(parts[3]);
                prefix = parts[4];
                suffix = parts[5];
            }
            catch { /* skip */ }
        }

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        public string AllowedSymbols
        {
            get { return this.allowedSymbols; }
            set { this.allowedSymbols = value; }
        }

        public int MinLength
        {
            get { return this.minLength; }
            set { this.minLength = value; }
        }

        public int MaxLength
        {
            get { return this.maxLength; }
            set { this.maxLength = value; }
        }

        public string Prefix
        {
            get { return this.prefix; }
            set { this.prefix = value; }
        }

        public string Suffix
        {
            get { return this.suffix; }
            set { this.suffix = value; }
        }
    }
}
