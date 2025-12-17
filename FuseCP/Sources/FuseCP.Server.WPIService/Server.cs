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
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;

namespace FuseCP.Server.WPIService
{
    class Server
    {
        static Mutex mutex = null;
        static void Main(string[] args)
        {
            bool onlyInstance = false;
            mutex = new Mutex(false, "Global\\{5DE133EC-49AE-4AE4-99BE-0F0A0BB5719E}", out onlyInstance);
            if (!mutex.WaitOne(0, false))  //if (!onlyInstance)
            {
                Console.WriteLine("The service is already running.");
                return;
            }
            TcpChannel ch = new TcpChannel(WPIServiceContract.PORT);
            ChannelServices.RegisterChannel(ch, true);

            WPIService wpiService = new WPIService();
            RemotingServices.Marshal(wpiService, "WPIServiceContract");


            Console.WriteLine("The service is running.");
                        
            while (!wpiService.IsFinished)
            {
                Thread.Sleep(2000);
            }

            Console.WriteLine("The service is finished.");
        }
    }
}
