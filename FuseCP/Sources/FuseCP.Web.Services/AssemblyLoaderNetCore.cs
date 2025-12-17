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

#if !NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Loader;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FuseCP.Web.Services;

public class AssemblyLoaderNetCore
{
	static int Initialized = 0;
	public static void Init()
	{
		var initialized = Interlocked.Exchange(ref Initialized, 1);
		if (initialized == 0) AssemblyLoadContext.Default.Resolving += Resolve;
	}

	static string probingPaths = null;
	public static string ProbingPaths
	{
		get => probingPaths ??= Configuration.ProbingPaths;
		set { probingPaths = value; paths = null; }
	}

	static readonly string exepath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
	static string[] paths = null;
	static string[] Paths => paths != null ? paths : paths =
		(ProbingPaths ?? "")
			.Replace('\\', Path.DirectorySeparatorChar)
			.Split(';')
			.Concat(new string[] { exepath })
			.ToArray();

	public static Assembly Resolve(AssemblyLoadContext context, AssemblyName name)
	{
		return Paths
			.Select(p =>
			{
				var relativename = Path.Combine(p, $"{name.Name}.dll");
				return new
				{
					FullName = new DirectoryInfo(Path.Combine(exepath, relativename)).FullName,
					Name = relativename
				};
			})
			.Where(p => File.Exists(p.FullName))
			.Select(p => context.LoadFromAssemblyPath(p.FullName))
			.Where(assembly => assembly != null &&
				(name.Version == null || assembly.GetName().Version >= name.Version))
			.FirstOrDefault();
	}
}
#endif
