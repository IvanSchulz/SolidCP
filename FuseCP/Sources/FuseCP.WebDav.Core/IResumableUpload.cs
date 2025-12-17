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

using System.IO;

namespace FuseCP.WebDav.Core
{
    namespace Client
    {
        public interface IResumableUpload
        {
            void CancelUpload();
            void CancelUpload(string lockToken);
            long GetBytesUploaded();
            Stream GetWriteStream(long startIndex, long contentLength, long resourceTotalSize);
            Stream GetWriteStream(long startIndex, long contentLength, long resourceTotalSize, string contentType);

            Stream GetWriteStream(long startIndex, long contentLength, long resourceTotalSize, string contentType,
                string lockToken);
        }

        public class WebDavResumableUpload
        {
            private long _bytesUploaded = 0;

            public void CancelUpload()
            {
            }

            public void CancelUpload(string lockToken)
            {
            }

            public long GetBytesUploaded()
            {
                return _bytesUploaded;
            }

            public Stream GetWriteStream(long startIndex, long contentLength, long resourceTotalSize)
            {
                return new MemoryStream();
            }

            public Stream GetWriteStream(long startIndex, long contentLength, long resourceTotalSize, string contentType)
            {
                return new MemoryStream();
            }

            public Stream GetWriteStream(long startIndex, long contentLength, long resourceTotalSize, string contentType,
                string lockToken)
            {
                return new MemoryStream();
            }
        }
    }
}
