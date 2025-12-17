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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FuseCP.Providers.OS
{
    public class TunnelApplication
    {
        public string Name;
        public IPAddress Client;
    }

    public class ListeningTask
    {
        // CancellationToken Cancel = CancellationToken.None;
        public Task Task;
        public TunnelSocket Incoming, Tunnel;
        public int ListeningPort = -1;
        public DateTime LastActivity => Incoming.LastActivity;
    }

    /// <summary>
    /// Used to forward a local port on the FuseCP.WebPanel server to a port on a server where FuseCP.Server is running.
    /// The forwarding is done over WebSockets.
    /// </summary>
    public class PortTunnels
    {
        public readonly static ConcurrentDictionary<TunnelApplication, Task<ListeningTask>> Listeners = new ConcurrentDictionary<TunnelApplication, Task<ListeningTask>>();
        public static async Task<ListeningTask> Listen(string applicationName, IPAddress clientAddress, Task<TunnelSocket> tunnel)
        {
            var application = new TunnelApplication()
            {
                Name = applicationName,
                Client = clientAddress
            };
            return await Listeners.GetOrAdd(application, async app =>
            {
                var incoming = new TunnelSocket(clientAddress);
                var port = await incoming.ListenAsync(clientAddress);
                var task = Task.Factory.StartNew(async () => await incoming.Transmit(await tunnel));
                var listeningTask = new ListeningTask()
                {
                    Incoming = incoming,
                    ListeningPort = port,
                    Tunnel = await tunnel,
                    Task = task
                };
                return listeningTask;
            });
        }
    }
}
