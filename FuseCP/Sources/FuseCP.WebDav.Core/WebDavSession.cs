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
using System.Net;

namespace FuseCP.WebDav.Core
{
    /// <summary>
    /// WebDav.Client Namespace.
    /// </summary>
    /// <see cref="http://doc.webdavsystem.com/ITHit.WebDAV.Client.html"/>
    namespace Client
    {
        public class WebDavSession
        {
            public NetworkCredential Credentials { get; set; }

            /// <summary>
            ///     Returns IFolder corresponding to path.
            /// </summary>
            /// <param name="path">Path to the folder.</param>
            /// <returns>Folder corresponding to requested path.</returns>
            public IFolder OpenFolder(string path)
            {
                var folder = new WebDavFolder();
                folder.SetCredentials(Credentials);
                folder.Open(path);
                return folder;
            }

            /// <summary>
            ///     Returns IFolder corresponding to path.
            /// </summary>
            /// <param name="path">Path to the folder.</param>
            /// <returns>Folder corresponding to requested path.</returns>
            public IFolder OpenFolder(Uri path)
            {
                var folder = new WebDavFolder();
                folder.SetCredentials(Credentials);
                folder.Open(path);
                return folder;
            }

            /// <summary>
            ///     Returns IFolder corresponding to path.
            /// </summary>
            /// <param name="path">Path to the folder.</param>
            /// <returns>Folder corresponding to requested path.</returns>
            public IFolder OpenFolderPaged(string path)
            {
                var folder = new WebDavFolder();
                folder.SetCredentials(Credentials);
                folder.OpenPaged(path);
                return folder;
            }

            /// <summary>
            ///     Returns IResource corresponding to path.
            /// </summary>
            /// <param name="path">Path to the resource.</param>
            /// <returns>Resource corresponding to requested path.</returns>
            public IResource OpenResource(string path)
            {
                return OpenResource(new Uri(path));
            }

            /// <summary>
            ///     Returns IResource corresponding to path.
            /// </summary>
            /// <param name="path">Path to the resource.</param>
            /// <returns>Resource corresponding to requested path.</returns>
            public IResource OpenResource(Uri path)
            {
                IFolder folder = OpenFolder(path);
                return folder.GetResource(path.Segments[path.Segments.Length - 1]);
            }
        }
    }
}
