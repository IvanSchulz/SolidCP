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
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
#if NetCore
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
#endif

namespace FuseCP.EnterpriseServer.Data;

public static class MigrationBuilderExtension
{
	const bool UseSafeSql = true;

	struct Segment
	{
		public int Start, Length;
		public bool HasSpecialCommand;
	}

	public class CountObject
	{
		public int Count = 0;
	}

#if NetCore
	static ConditionalWeakTable<MigrationBuilder, CountObject> SafeSqlCount = new ConditionalWeakTable<MigrationBuilder, CountObject>();
	public static OperationBuilder<SqlOperation> SafeSql(this MigrationBuilder builder,
		string query, bool suppressTransaction = false)
	{
		int count;
		lock (SafeSqlCount)
		{
			count = SafeSqlCount.GetOrCreateValue(builder)?.Count ?? 0;
			var res = builder.Sql(SafeSql(query, ref count), suppressTransaction);
			SafeSqlCount.AddOrUpdate(builder, new CountObject() { Count = count });
			return res;
		}
	}
#endif
	public static string SafeSql(string query, ref int count)
	{
		if (!UseSafeSql) return query;

		List<Segment> segments = new();
		char stringDelimiter = ' ';
		bool isString = false, isDashComment = false, isCComment = false;

		int i = 0;
		int start = 0, length = 0;
		char pre = ' ';
		string preIdent = "";
		List<char> identifier = new();
		bool hasSpecialCommand = false;

		void ParseIdent()
		{
			if (identifier.Count > 0 || i >= query.Length)
			{
				var ident = new string(identifier.ToArray());
				bool isgo;
				if (isgo = ident.Equals("GO", StringComparison.OrdinalIgnoreCase) || i >= query.Length)
				{
					int end;
					if (i >= query.Length && !isgo) end = query.Length - 1;
					else end = query.LastIndexOf('\n', i - 3);

					if (end <= -1) end = 0;
					length = end - start + 1;

					if (length > 0)
					{
						segments.Add(new Segment()
						{
							Start = start,
							Length = length,
							HasSpecialCommand = hasSpecialCommand
						});
					}

					if (i < query.Length) start = query.IndexOf('\n', i);
					if (start <= -1) start = query.Length;
					hasSpecialCommand = false;
				}
				else if ((preIdent.Equals("CREATE", StringComparison.OrdinalIgnoreCase) ||
					preIdent.Equals("ALTER", StringComparison.OrdinalIgnoreCase)) &&
					(ident.Equals("FUNCTION", StringComparison.OrdinalIgnoreCase) ||
					ident.Equals("VIEW", StringComparison.OrdinalIgnoreCase) ||
					ident.Equals("PROCEDURE", StringComparison.OrdinalIgnoreCase) ||
					ident.Equals("TRIGGER", StringComparison.OrdinalIgnoreCase)) ||
					preIdent.Equals("DECLARE", StringComparison.OrdinalIgnoreCase) ||
					ident.Equals("DECLARE", StringComparison.OrdinalIgnoreCase))
				{
					hasSpecialCommand = true;
				}
				preIdent = ident;
				identifier.Clear();
			}
		}

		foreach (char ch in query)
		{
			if (!isCComment && !isDashComment)
			{
				if (ch == '\'')
				{
					if (stringDelimiter == ' ')
					{
						isString = true;
						stringDelimiter = '\'';
					}
					else if (stringDelimiter == '\'')
					{
						isString = false;
						stringDelimiter = ' ';
					}
				}
				if (ch == '"')
				{
					if (stringDelimiter == ' ')
					{
						isString = true;
						stringDelimiter = '"';
					}
					else if (stringDelimiter == '"')
					{
						isString = false;
						stringDelimiter = ' ';
					}
				}
			}
			if (!isString)
			{
				if (ch == '-' && pre == '-') isDashComment = true;
				else if (isDashComment && ch == '\n') isDashComment = false;
				else if (ch == '*' && pre == '/')
				{
					isCComment = true;
				}
				else if (isCComment && ch == '/' && pre == '*') isCComment = false;
			}
			if (!isString && !isCComment && !isDashComment)
			{
				if (char.IsLetterOrDigit(ch) || ch == '[')
				{
					identifier.Add(ch);
				}
				else
				{
					ParseIdent();
				}
			}
			pre = ch;
			i++;
		}
		ParseIdent();

		segments.Reverse();
		var str = new StringBuilder(query);

		foreach (var segment in segments)
		{
			var cmd = str.ToString(segment.Start, segment.Length)
				.Trim();
			string firstLine = null;
			var firstLineMatch = Regex.Match(cmd, "^.*?(?=\r?\n)", RegexOptions.Singleline);
			if (firstLineMatch.Success) firstLine = firstLineMatch.Value.Replace("\'", "\'\'");
			else firstLine = $"Command {count++}";
			if (segment.HasSpecialCommand)
			{
				str.Remove(segment.Start, segment.Length);
				str.Insert(segment.Start, Environment.NewLine);
				str.Insert(segment.Start, "'");
				str.Insert(segment.Start, cmd.Replace("'", "''"));
				str.Insert(segment.Start, "EXECUTE sp_executesql N'");
				str.Insert(segment.Start, Environment.NewLine);
				str.Insert(segment.Start, $"PRINT '{firstLine}'");
				str.Insert(segment.Start, Environment.NewLine);
			}
			else
			{
				str.Remove(segment.Start, segment.Length);
				if (cmd != "")
				{
					str.Insert(segment.Start, Environment.NewLine);
					str.Insert(segment.Start, cmd);
					str.Insert(segment.Start, Environment.NewLine);
				}
			}
		}

		//File.WriteAllText(@"C:\GitHub\test.sql", str.ToString().Trim());

		return str.ToString().Trim();
	}

#if NetCore
	public static OperationBuilder<SqlOperation> SqlScript(this MigrationBuilder migration, string scriptName, bool suppressTransaction = false)
	{
		return migration.SafeSql(DatabaseUtils.InstallScript(scriptName), suppressTransaction);
	}
#endif

}
