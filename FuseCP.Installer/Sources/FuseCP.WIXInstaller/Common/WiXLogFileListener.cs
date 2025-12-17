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
using System.IO;
using System.Text;
//using Microsoft.Deployment.WindowsInstaller;
using WixToolset.Dtf.WindowsInstaller;

namespace FuseCP.WIXInstaller.Common
{
    public class WiXLogFileListener : TraceListener
    {
        public const uint FileFlushSize = 4096;
        public const string DefaultLogFile = "FCPInstallation.log.txt";
        public static string LogFile { get; private set; }
        private StringBuilder m_Ctx;
        static WiXLogFileListener()
        {
            LogFile = Path.Combine(Path.GetTempPath() + DefaultLogFile);
        }
        public WiXLogFileListener(string LogFileName = DefaultLogFile)
            : base("WiXLogFileListener")
        {
            m_Ctx = new StringBuilder();
        }
        ~WiXLogFileListener()
        {
            Dispose(false);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Flush(true);
        }
        public override void Write(string Value)
        {
            m_Ctx.Append(Value);
            Flush();
        }
        public override void WriteLine(string Value)
        {
            m_Ctx.AppendLine(Value);
            Flush();
        }
        private void Flush(bool Force = false)
        {
            if(m_Ctx.Length >= FileFlushSize || Force)
            {
                using (var FileCtx = new StreamWriter(LogFile, true))
                {
                    FileCtx.Write(m_Ctx.ToString());
                }
                m_Ctx.Clear();
            }
        }
    }
}
