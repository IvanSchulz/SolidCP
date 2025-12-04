using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseCP.EnterpriseServer
{
	public abstract class SchedulerTask: ControllerAsyncBase, ISchedulerTask
	{
		public abstract void DoWork();

	}
}
