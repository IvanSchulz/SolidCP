using System;
using System.Collections.Generic;
using System.Text;
using FuseCP.Providers.SharePoint;

namespace FuseCP.EnterpriseServer.Base.HostedSolution
{
	public class CalculateSharePointSitesDiskSpaceResult
	{
		public SharePointSiteDiskSpace[] Result { get; set; }
		public int ErrorCode { get; set; }
	}
}
