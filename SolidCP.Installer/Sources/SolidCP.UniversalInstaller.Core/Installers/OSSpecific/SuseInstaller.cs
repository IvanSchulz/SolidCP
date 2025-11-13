using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SolidCP.UniversalInstaller
{
	public class SuseInstaller : UnixInstaller
	{

		public override void InstallNet10Runtime()
		{
			if (CheckNet10RuntimeInstalled()) return;

			throw new NotSupportedException("NET 10 Runtime must be installed.");
		}

		public override void RemoveNet10AspRuntime()
		{
			throw new NotImplementedException();
		}
		public override void RemoveNet10NetRuntime()
		{
			throw new NotImplementedException();
		}
	}
}

