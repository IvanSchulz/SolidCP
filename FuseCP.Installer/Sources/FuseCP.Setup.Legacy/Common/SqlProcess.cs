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
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FuseCP.Setup.Actions;
using FuseCP.UniversalInstaller.Core;
using FuseCP.EnterpriseServer.Data;

namespace FuseCP.Setup
{
	/// <summary>
	/// Shows sql script process.
	/// </summary>
	public sealed class SqlProcess
	{
		private string scriptFile; 
		private string connectionString;
		private string database;

		public event EventHandler<ActionProgressEventArgs<int>> ProgressChange;
		
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		/// <param name="file">Sql script file</param>
		/// <param name="connection">Sql connection string</param>
		/// <param name="db">Sql server database name</param>
		public SqlProcess(string file, string connection, string db)
		{
			this.scriptFile = file;
			this.connectionString = connection;
			this.database = db;
		}

		private void OnProgressChange(float ratio)
		{
			if (ProgressChange == null)
				return;
			//
			ProgressChange(this, new ActionProgressEventArgs<int>
			{
				EventData = (int)(ratio * 100 + 0.5)
			});
		}


		internal void RunSqlServer(int commandCount)
		{
			SqlConnection connection = new SqlConnection(connectionString);
			string sql;
			int i = 0;
			float n = commandCount;

			try
			{
				// iterate through delimited command text
				using (StreamReader reader = new StreamReader(scriptFile))
				{
					SqlCommand command = new SqlCommand();
					connection.Open();
					command.Connection = connection;
					command.CommandType = System.Data.CommandType.Text;
					command.CommandTimeout = 600;

					while (null != (sql = ReadNextStatementFromStream(reader)))
					{
						sql = ProcessInstallVariables(sql);
						command.CommandText = sql;
						try
						{
							command.ExecuteNonQuery();
						}
						catch (Exception ex)
						{
							throw new Exception("Error executing SQL command: " + sql, ex);
						}

						i++;
						if (commandCount != 0)
						{
							OnProgressChange((float)i / n);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Can't run SQL script " + scriptFile, ex);
			}
			finally
			{
				connection.Close();
			}
		}

		void SetCommandCount(int n) => Log.WriteInfo($"Executing {n} database commands");

		/// <summary>
		/// Executes sql script file.
		/// </summary>
		internal void Run()
		{
			//
			OnProgressChange(0);
			//
			using (var stream = new FileStream(scriptFile, FileMode.Open, FileAccess.Read))
			{
				DatabaseUtils.RunSqlScript(connectionString, stream, OnProgressChange, SetCommandCount, ProcessInstallVariables, scriptFile, database);
			}
		}

		public bool IsDelimiter(string cmd) => Regex.IsMatch(cmd, "^(GO|DELIMITER)(?=$|[^a-zA-Z])", RegexOptions.Singleline | RegexOptions.IgnoreCase);
		private string ReadNextStatementFromStream(StreamReader reader)
		{
			StringBuilder sb = new StringBuilder();
			string lineOfText;
	
			while(true) 
			{
				lineOfText = reader.ReadLine();
				if( lineOfText == null ) 
				{
					if( sb.Length > 0 ) 
					{
						return sb.ToString();
					}
					else 
					{
						return null;
					}
				}

				if(IsDelimiter(lineOfText)) 
				{
					break;
				}
				
				sb.Append(lineOfText + Environment.NewLine);
			}

			return sb.ToString();
		}

		private string ProcessInstallVariables(string input)
		{
			//replace install variables
			string output = input;
			output = output.Replace("${install.database}", database);
			return output;
		}
	}
}
