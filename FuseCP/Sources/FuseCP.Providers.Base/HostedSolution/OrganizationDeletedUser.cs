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

namespace FuseCP.Providers.HostedSolution
{
    public class OrganizationDeletedUser
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public ExchangeAccountType OriginAT { get; set; }

        public string StoragePath { get; set; }

        public string FolderName { get; set; }

        public string FileName { get; set; }

        public DateTime ExpirationDate { get; set; }

        public OrganizationUser User { get; set; }

        public bool IsArchiveEmpty { get; set; }
    }
}
