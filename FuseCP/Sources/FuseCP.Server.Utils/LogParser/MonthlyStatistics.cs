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
using System.Collections;

namespace FuseCP.Providers.Utils.LogParser
{
	/// <summary>
	/// Summary description for WebSiteStatistics.
	/// </summary>
	public class MonthlyStatistics
	{
		private string statsFile = null;
		private Hashtable days = new Hashtable();


		public MonthlyStatistics(string statsFile)
		{
			// save file name
			this.statsFile = statsFile;
		}

		public MonthlyStatistics(string statsFile, bool load)
		{
			// save file name
			this.statsFile = statsFile;
			//
			if (load)
			{
			    if (File.Exists(statsFile))
			    {
			        Load();
			    }
				//else
				//{
				//    throw new ArgumentException(String.Format("File with specified name doesn't exist: {0}", statsFile), "statsFile");
				//}
			}
		}

		public StatsLine this[int day]
		{
			get { return (StatsLine)days[day]; }
			set { days[day] = value; }
		}

		public Hashtable Days
		{
			get { return days; }
		}

		private void Load()
		{
			//
			StreamReader reader = new StreamReader(statsFile);
			string line = null;
			while ((line = reader.ReadLine()) != null)
			{
				// parse line
				string[] columns = line.Split(new char[] { ' ' });
				int day = Int32.Parse(columns[0]);

				// add new stats line to the hash
				StatsLine statsLine = new StatsLine();
				statsLine.BytesSent = Int64.Parse(columns[1]);
				statsLine.BytesReceived = Int64.Parse(columns[2]);

				days.Add(day, statsLine);
			}
			reader.Close();
		}

		public void Save()
		{
			Save(Path.GetDirectoryName(statsFile));
		}

		public void Save(string dir)
		{
			// create directory if required
			//string dir = Path.GetDirectoryName(statsFile);
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			StreamWriter writer = null;

			try
			{
				writer = new StreamWriter(Path.Combine(dir, statsFile));

				foreach (int day in days.Keys)
				{
					StatsLine statsLine = (StatsLine)days[day];

					// write line
					writer.WriteLine("{0} {1} {2}", day, statsLine.BytesSent, statsLine.BytesReceived);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(String.Format("Can't open '{0}' log file", statsFile), ex);
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
		}
	}
}
