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
using FuseCP.Server.Utils;

namespace FuseCP.Providers.Utils.LogParser
{
	/// <summary>
	/// Summary description for WebSiteState.
	/// </summary>
	public class LogState
	{
		private long lastAccessed = 0;
		private long line = 0;
		private string siteFileName = null;

		public LogState(string logName)
		{
			// make file name
			siteFileName = logName;

			// open and parse site state file
			if(!File.Exists(siteFileName))
				return;

		    StreamReader reader = null;
			try
			{
				reader = new StreamReader(siteFileName);
				string s = null;

				// last accesses time
				if((s = reader.ReadLine()) != null)
					lastAccessed = Int64.Parse(s.Trim());

				// line
				if((s = reader.ReadLine()) != null)
					line = Int64.Parse(s.Trim());

				reader.Close();
                
			}
			catch(Exception ex)
			{
                Log.WriteError(ex);

			}
            finally
			{
			    if (reader != null)
			    {
			        reader.Dispose();
			    }
			}
		}

		public void Save()
		{
			// create directory if required
			string dir = Path.GetDirectoryName(siteFileName);
			if(!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			StreamWriter writer = new StreamWriter(siteFileName);
			// last accesses time
			writer.WriteLine(lastAccessed.ToString());

			// line
			writer.WriteLine(line);

			// close writer
			writer.Close();
		}

		public long LastAccessed
		{
			get { return lastAccessed; }
			set { lastAccessed = value; }
		}

		public long Line
		{
			get { return line; }
			set { line = value; }
		}
	}
}
