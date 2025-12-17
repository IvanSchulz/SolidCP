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

namespace FuseCP.WebDav.Core
{
    namespace Client
    {
        public interface IConnectionSettings
        {
            bool AllowWriteStreamBuffering { get; set; }
            bool SendChunked { get; set; }
            int TimeOut { get; set; }
        }

        public class WebDavConnectionSettings
        {
            private int _timeOut = 30000;

            public WebDavConnectionSettings()
            {
                SendChunked = false;
                AllowWriteStreamBuffering = false;
            }

            public bool AllowWriteStreamBuffering { get; set; }
            public bool SendChunked { get; set; }

            public int TimeOut
            {
                get { return _timeOut; }
                set { _timeOut = value; }
            }
        }
    }
}
