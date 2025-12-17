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

using FuseCP.EnterpriseServer.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FuseCP.Tests
{
	[TestClass]
	public class SafeSqlTest
	{
		public TestContext TestContext { get; set; }

		public string ReplaceWhiteSpace(string txt)
		{
			return Regex.Replace(txt.Trim(), @"\s+", " ", RegexOptions.Singleline);
		}
		[TestMethod]
		public void TestSafeSql()
		{
			var sql = @"CREATE FUNCTION
GO

'CREATE FUNCTION'
'GO'

GO

CREATE VIEW
GO;

CREATE VIEW
[GO]

CREATE VIEW
GO
-- CREATE VIEW
GO
/* CREATE VIEW */
GO
";
			int count = 0;
			var safesql = MigrationBuilderExtension.SafeSql(sql, ref count);

			Debug.WriteLine(safesql);

			var correctSafeSql = @"PRINT 'Command 3'
EXECUTE sp_executesql N'CREATE FUNCTION'
GO

'CREATE FUNCTION'
'GO'
GO

PRINT 'Command 2'
EXECUTE sp_executesql N'CREATE VIEW'
GO;

PRINT 'CREATE VIEW'
EXECUTE sp_executesql N'CREATE VIEW
[GO]

CREATE VIEW'
GO

-- CREATE VIEW
GO

/* CREATE VIEW */
GO";
			Assert.AreEqual(ReplaceWhiteSpace(safesql), ReplaceWhiteSpace(correctSafeSql));

		}

	}
}
