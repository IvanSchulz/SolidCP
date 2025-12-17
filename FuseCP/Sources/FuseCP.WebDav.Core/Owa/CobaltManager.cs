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
using System.IO;
using System.Threading;
using Cobalt;
using FuseCP.WebDav.Core.Interfaces.Managers;
using FuseCP.WebDav.Core.Interfaces.Owa;

namespace FuseCP.WebDav.Core.Owa
{
    public class CobaltManager : ICobaltManager
    {
        private readonly IWebDavManager _webDavManager;
        private readonly IWopiFileManager _fileManager;
        private readonly IAccessTokenManager _tokenManager;

        public CobaltManager(IWebDavManager webDavManager, IWopiFileManager fileManager,
            IAccessTokenManager tokenManager)
        {
            _webDavManager = webDavManager;
            _fileManager = fileManager;
            _tokenManager = tokenManager;
        }

        public Atom ProcessRequest(int accessTokenId, Stream requestStream)
        {
            var token = _tokenManager.GetToken(accessTokenId);

            var atomRequest = new AtomFromStream(requestStream);

            var requestBatch = new RequestBatch();

            try
            {
                var cobaltFile = _fileManager.Get(token.FilePath) ?? _fileManager.Create(accessTokenId);

                Object ctx;
                ProtocolVersion protocolVersion;

                requestBatch.DeserializeInputFromProtocol(atomRequest, out ctx, out protocolVersion);
                cobaltFile.CobaltEndpoint.ExecuteRequestBatch(requestBatch);


                foreach (var request in requestBatch.Requests)
                {

                    if (request.GetType() == typeof (PutChangesRequest) &&
                        request.PartitionId == FilePartitionId.Content && request.CompletedSuccessfully)
                    {
                        using (var saveStream = new MemoryStream())
                        {
                            CopyStream(cobaltFile, saveStream);
                            _webDavManager.UploadFile(token.FilePath, saveStream.ToArray());
                        }
                    }
                }


                return requestBatch.SerializeOutputToProtocol(protocolVersion);
            }

            catch (Exception e)
            {
                Server.Utils.Log.WriteError("Cobalt manager Process request", e);

                throw;
            }
        }

        private void CopyStream(CobaltFile file, Stream stream)
        {
            var tries = 3;

            for (int i = 0; i < tries; i++)
            {
                try
                {
                    GenericFdaStream myCobaltStream = new GenericFda(file.CobaltEndpoint, null).GetContentStream();

                    myCobaltStream.CopyTo(stream);

                    break;
                }
                catch (Exception)
                {
                    //unable to read update - save failed
                    if (i == tries - 1)
                    {
                        throw;
                    }

                    //waiting for cobalt completion
                    Thread.Sleep(50);
                }
            }
        }
    }
} 
