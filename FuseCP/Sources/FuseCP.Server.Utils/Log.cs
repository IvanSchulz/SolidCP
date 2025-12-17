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
using System.Configuration;
using System.Diagnostics;
using System.Text;

namespace FuseCP.Server.Utils
{
    /// <summary>
    /// Application log.
    /// </summary>
    public sealed class Log
    {
        private static TraceSwitch logSeverity = new TraceSwitch("Log", "General trace switch");
        private Log()
        {
        }
        public static TraceLevel LogLevel
        {
            get => logSeverity.Level;
            set => logSeverity.Level = value;
        }

        /// <summary>
        /// Write error to the log.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="ex">Exception.</param>
        public static void WriteError(string message, Exception ex)
        {
            try
            {
                if (logSeverity.TraceError)
                {
                    StringBuilder txt = new StringBuilder();
                    txt.Append($"[{DateTime.Now:G}] ERROR: ");
                    txt.AppendLine(message);
                    while (ex != null) {
                        txt.AppendLine(ex.ToString());
                        ex = ex.InnerException;
                        if (ex != null)
                        {
                            txt.AppendLine("Inner Exception:");
                        }
                    }
                    Trace.TraceError(txt.ToString());
                }
            }
            catch { }
        }

        /// <summary>
        /// Write error to the log.
        /// </summary>
        /// <param name="ex">Exception.</param>
        public static void WriteError(Exception ex)
        {

            try
            {
                if (ex != null)
                {
                    WriteError(ex.Message, ex);
                }
            }
            catch { }
        }

        /// <summary>
        /// Write info message to log
        /// </summary>
        /// <param name="message"></param>
        public static void WriteInfo(string message, params object[] args)
        {
            try
            {
                if (logSeverity.TraceInfo)
                {
                    Trace.TraceInformation(FormatIncomingMessage(message, "INFO", args));
                }
            }
            catch { }
        }

        /// <summary>
        /// Write info message to log
        /// </summary>
        /// <param name="message"></param>
        public static void WriteWarning(string message, params object[] args)
        {
            try
            {
                if (logSeverity.TraceWarning)
                {
                    Trace.TraceWarning(FormatIncomingMessage(message, "WARNING", args));
                }
            }
            catch { }
        }

        /// <summary>
        /// Write start message to log
        /// </summary>
        /// <param name="message"></param>
        public static void WriteStart(string message, params object[] args)
        {
            try
            {
                if (logSeverity.TraceInfo)
                {
                    Trace.TraceInformation(FormatIncomingMessage(message, "START", args));
                }
            }
            catch { }
        }

        /// <summary>
        /// Write end message to log
        /// </summary>
        /// <param name="message"></param>
        public static void WriteEnd(string message, params object[] args)
        {
            try
            {
                if (logSeverity.TraceInfo)
                {
                    Trace.TraceInformation(FormatIncomingMessage(message, "END", args));
                }
            }
            catch { }
        }

        private static string FormatIncomingMessage(string message, string tag, params object[] args)
        {
            //
            if (args.Length > 0)
            {
                message = String.Format(message, args);
            }
            //
            return String.Concat(String.Format("[{0:G}] {1}: ", DateTime.Now, tag), message);
        }


    }
}
