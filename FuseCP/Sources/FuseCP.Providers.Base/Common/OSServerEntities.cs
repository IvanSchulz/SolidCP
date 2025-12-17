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
using System.Collections.Generic;
using System.Text;

namespace FuseCP.Providers.OS
{
    public class TerminalSession
    {
        private int sessionId;
        private string username;
        private string status;

        public int SessionId
        {
            get { return this.sessionId; }
            set { this.sessionId = value; }
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public string Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
    }

    public class OSProcess
    {
        private int pid;
        private string name;
        private string username;
        private float cpuUsage;
        private long memUsage;

        public int Pid
        {
            get { return this.pid; }
            set { this.pid = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Arguments { get; set; }
        public string Command { get; set; }
        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public float CpuUsage
        {
            get { return this.cpuUsage; }
            set { this.cpuUsage = value; }
        }

        public long MemUsage
        {
            get { return this.memUsage; }
            set { this.memUsage = value; }
        }
    }

    public class OSService
    {
        private string id;
        private string name;
        private OSServiceStatus status;
        private bool canStop;
        private bool canPauseAndContinue;

        public OSService()
        {
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public OSServiceStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        public bool CanStop
        {
            get { return this.canStop; }
            set { this.canStop = value; }
        }

        public bool CanPauseAndContinue
        {
            get { return this.canPauseAndContinue; }
            set { this.canPauseAndContinue = value; }
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Description { get; set; }
    }

    public enum OSServiceStatus
    {
        ContinuePending = 1,
        Paused = 2,
        PausePending = 3,
        Running = 4,
        StartPending = 5,
        Stopped = 6,
        StopPending = 7
    }

    public class SystemLogEntriesPaged
    {
        private int count;
        private SystemLogEntry[] entries;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public SystemLogEntry[] Entries
        {
            get { return entries; }
            set { entries = value; }
        }
    }

    public class SystemLogEntry
    {
        private SystemLogEntryType entryType;
        private DateTime created;
        private string source;
        private string category;
        private long eventId;
        private string userName;
        private string machineName;
        private string message;

        public SystemLogEntryType EntryType
        {
            get { return entryType; }
            set { entryType = value; }
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string MachineName
        {
            get { return machineName; }
            set { machineName = value; }
        }

        public long EventID
        {
            get { return eventId; }
            set { eventId = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }

    public enum SystemLogEntryType
    {
        Information,
        Warning,
        Error,
        SuccessAudit,
        FailureAudit
    }    
}
