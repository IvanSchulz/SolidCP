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
using System.Text;

namespace FuseCP.WIXInstaller.Common
{
    class InMemoryStringLogListener : TraceListener
    {
        private const string Format = "[{0}] Message: {1}";
        static private StringBuilder m_Ctx;
        string m_id;
        static InMemoryStringLogListener()
        {
            m_Ctx = new StringBuilder();
        }
        public InMemoryStringLogListener(string InstanceID)
            : base(InstanceID)
        {
            m_id = InstanceID;
        }
        public override void Write(string Value)
        {
            WriteLog(Value);
        }
        public override void WriteLine(string Value)
        {
            WriteLog(Value + Environment.NewLine);
        }
        [Conditional("DEBUG")]
        private void WriteLog(string Value)
        {
            m_Ctx.Append(string.Format(Format, m_id, Value));
        }
    }
}
