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

namespace FuseCP.Providers.HostedSolution
{
    public class OrganizationPasswordSettings
    {
        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }
        public int EnforcePasswordHistory { get; set; }
        public int MaxPasswordAge { get; set; }

        public bool LockoutSettingsEnabled { get; set; }
        public int AccountLockoutDuration { get; set; }
        public int AccountLockoutThreshold { get; set; }
        public int ResetAccountLockoutCounterAfter { get; set; }

        public bool PasswordComplexityEnabled { get; set; }
        public int UppercaseLettersCount { get; set; }
        public int NumbersCount { get; set; }
        public int SymbolsCount { get; set; }
    }
}
