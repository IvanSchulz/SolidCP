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
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace FuseCP.Server.Utils
{
    public class EventLogTraceListener : TraceListener
    {
        private EventLog eventLog;
        private bool nameSet;

        public EventLog EventLog
        {
            get
            {
                return this.eventLog;
            }
            set
            {
                this.eventLog = value;
            }
        }

        public override string Name
        {
            get
            {
                if (!this.nameSet && (this.eventLog != null))
                {
                    this.nameSet = true;
                    base.Name = this.eventLog.Source;
                }
                return base.Name;
            }
            set
            {
                this.nameSet = true;
                base.Name = value;
            }
        }

        public EventLogTraceListener(EventLog eventLog)
            : base((eventLog != null) ? eventLog.Source : string.Empty)
        {
            this.eventLog = eventLog;
        }

        public EventLogTraceListener(string source)
        {
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, source);
            }

            this.eventLog = new EventLog();
            this.eventLog.Source = source;
            this.eventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
        }

        public EventLogTraceListener() : this("FuseCP") { }

        private EventInstance CreateEventInstance(TraceEventType severity, int id)
        {
            if (id > 0xffff)
            {
                id = 0xffff;
            }
            if (id < 0)
            {
                id = 0;
            }
            EventInstance instance = new EventInstance((long)id, 0);
            if ((severity == TraceEventType.Error) || (severity == TraceEventType.Critical))
            {
                instance.EntryType = EventLogEntryType.Error;
                return instance;
            }
            if (severity == TraceEventType.Warning)
            {
                instance.EntryType = EventLogEntryType.Warning;
                return instance;
            }
            instance.EntryType = EventLogEntryType.Information;
            return instance;
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, params object[] data)
        {
            if ((base.Filter == null) || base.Filter.ShouldTrace(eventCache, source, severity, id, null, null, null, data))
            {
                EventInstance instance = this.CreateEventInstance(severity, id);
                StringBuilder builder = new StringBuilder();
                if (data != null)
                {
                    for (int num = 0; num < data.Length; num++)
                    {
                        if (num != 0)
                        {
                            builder.Append(", ");
                        }
                        if (data[num] != null)
                        {
                            builder.Append(data[num].ToString());
                        }
                    }
                }
                this.eventLog.WriteEvent(instance, new object[] { builder.ToString() });
            }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, object data)
        {
            if ((base.Filter == null) || base.Filter.ShouldTrace(eventCache, source, severity, id, null, null, data, null))
            {
                EventInstance instance = this.CreateEventInstance(severity, id);
                this.eventLog.WriteEvent(instance, new object[] { data });
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string message)
        {
            if ((base.Filter == null) || base.Filter.ShouldTrace(eventCache, source, severity, id, message, null, null, null))
            {
                EventInstance instance = this.CreateEventInstance(severity, id);
                this.eventLog.WriteEvent(instance, new object[] { message });
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string format, params object[] args)
        {
            if ((base.Filter == null) || base.Filter.ShouldTrace(eventCache, source, severity, id, format, args, null, null))
            {
                EventInstance instance1 = this.CreateEventInstance(severity, id);
                if (args == null)
                {
                    this.eventLog.WriteEvent(instance1, new object[] { format });
                }
                else if (string.IsNullOrEmpty(format))
                {
                    string[] textArray1 = new string[args.Length];
                    for (int num1 = 0; num1 < args.Length; num1++)
                    {
                        textArray1[num1] = args[num1].ToString();
                    }
                    this.eventLog.WriteEvent(instance1, textArray1);
                }
                else
                {
                    this.eventLog.WriteEvent(instance1, new object[] { string.Format(CultureInfo.InvariantCulture, format, args) });
                }
            }
        }

        public override void Write(string message)
        {
            if (this.eventLog != null)
            {
                this.eventLog.WriteEntry(message);
            }
        }

        public override void WriteLine(string message)
        {
            this.Write(message);
        }

        public override void Close()
        {
            if (this.eventLog != null)
            {
                this.eventLog.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Close();
            }
        }
    }
}
