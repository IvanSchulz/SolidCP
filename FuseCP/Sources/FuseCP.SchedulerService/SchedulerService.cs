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
using System.ServiceProcess;
using System.Threading;
using FuseCP.EnterpriseServer;

namespace FuseCP.SchedulerService;


#if NETCOREAPP
public class ServiceBase {
    protected virtual void OnStart(string[] args) { }

    public static void Run(ServiceBase[] services) {
        foreach (var service in services) service.OnStart(Environment.GetCommandLineArgs());
    }
    protected virtual void Dispose(bool disposing) { }
    public string ServiceName { get; set; }
}
#endif

public partial class SchedulerService : ServiceBase
{
    private Timer _Timer;
    private static object _isRuninng;
    #region Construcor

    public SchedulerService()
    {
        _isRuninng = new object();

        InitializeComponent();

        _Timer = new Timer(Process, null, 5000, 5000);
    }

    #endregion

    #region Methods

    protected override void OnStart(string[] args)
    {
    }

    protected static void Process(object callback)
    {
        //check running service
        if (!Monitor.TryEnter(_isRuninng))
            return;

        try
        {
            using (var scheduler = new Scheduler())
            {
                scheduler.Start();
            }
        }
        finally
        {
            Monitor.Exit(_isRuninng);
        }

    }

    #endregion
}
