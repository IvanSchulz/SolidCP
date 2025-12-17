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

namespace FuseCP.Providers.FTP.IIs100.Config
{
    using System;

    [Flags]
    public enum FtpLogExtFileFlags
    {
        BytesRecv = 0x2000,
        BytesSent = 0x1000,
        ClientIP = 4,
        ClientPort = 0x2000000,
        ComputerName = 0x20,
        Date = 1,
        FtpStatus = 0x400,
        FtpSubStatus = 0x200000,
        FullPath = 0x800000,
        Host = 0x100000,
        Info = 0x1000000,
        Method = 0x80,
        ServerIP = 0x40,
        ServerPort = 0x8000,
        Session = 0x400000,
        SiteName = 0x10,
        Time = 2,
        TimeTaken = 0x4000,
        UriStem = 0x100,
        UserName = 8,
        Win32Status = 0x800
    }
}

