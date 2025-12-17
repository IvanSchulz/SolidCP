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
using System.Management;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

using FuseCP.Server.Utils;
using FuseCP.Providers.Utils;
using FuseCP.Providers.OS;

namespace FuseCP.Providers.DNS
{
	public class MsDNS2012: HostingServiceProviderBase, IDnsServer
    {

        #region Properties
        protected TimeSpan ExpireLimit
		{
			get { return ProviderSettings.GetTimeSpan( "ExpireLimit" ); }
		}

        protected TimeSpan MinimumTTL
		{
            get { return ProviderSettings.GetTimeSpan("MinimumTTL"); }
		}

        protected TimeSpan RefreshInterval
		{
            get { return ProviderSettings.GetTimeSpan("RefreshInterval"); }
		}

        protected TimeSpan RetryDelay
		{
            get { return ProviderSettings.GetTimeSpan("RetryDelay"); }
		}

		protected bool AdMode
		{
			get { return ProviderSettings.GetBool( "AdMode" ); }
		}

        public int DNSRecordDefaultTTL
        {
            get { return ProviderSettings.GetInt("RecordDefaultTTL"); }
        }

        public int DNSRecordMinimumTTL
        {
            get { return ProviderSettings.GetInt("RecordMinimumTTL"); }
        }

        #endregion

        private PowerShellHelper ps = null;
		private bool bulkRecords;

		public MsDNS2012()
		{
			// Create PowerShell helper
			ps = new PowerShellHelper();
			/* FIX: the following code is useless.
				if( !this.IsInstalled() )
				return; */
		}

		#region Zones

		public virtual string[] GetZones()
		{
			return ps.Get_DnsServerZone_Names();
		}

		public virtual bool ZoneExists( string zoneName )
		{
			return ps.ZoneExists( zoneName );
		}

		public virtual DnsRecord[] GetZoneRecords( string zoneName )
		{
			return ps.GetZoneRecords( zoneName );
		}

		public virtual void AddPrimaryZone( string zoneName, string[] secondaryServers )
		{
			ps.Add_DnsServerPrimaryZone( zoneName, secondaryServers, AdMode);

            // remove ns records
            ps.Remove_DnsServerResourceRecords(zoneName, "NS");
		}

		public virtual void AddSecondaryZone( string zoneName, string[] masterServers )
		{
			ps.Add_DnsServerSecondaryZone( zoneName, masterServers );
        }

		public virtual void DeleteZone( string zoneName )
		{
			try
			{
				ps.Remove_DnsServerZone( zoneName );
			}
			catch( Exception ex )
			{
				Log.WriteError( ex );
			}
		}

		public virtual void AddZoneRecord( string zoneName, DnsRecord record )
		{
			try
			{
				string name = record.RecordName;
				if( String.IsNullOrEmpty( name ) )
					name = ".";

				if( record.RecordType == DnsRecordType.A )
					ps.Add_DnsServerResourceRecordA( zoneName, name, record.RecordData );
				else if( record.RecordType == DnsRecordType.AAAA )
					ps.Add_DnsServerResourceRecordAAAA( zoneName, name, record.RecordData );
				else if( record.RecordType == DnsRecordType.CNAME )
					ps.Add_DnsServerResourceRecordCName( zoneName, name, record.RecordData );
				else if( record.RecordType == DnsRecordType.MX )
					ps.Add_DnsServerResourceRecordMX( zoneName, name, record.RecordData, (ushort)record.MxPriority );
				else if( record.RecordType == DnsRecordType.NS )
					ps.Add_DnsServerResourceRecordNS( zoneName, name, record.RecordData );
				else if( record.RecordType == DnsRecordType.TXT )
					ps.Add_DnsServerResourceRecordTXT( zoneName, name, record.RecordData );
				else if( record.RecordType == DnsRecordType.SRV )
					ps.Add_DnsServerResourceRecordSRV( zoneName, name, record.RecordData, (ushort)record.SrvPort, (ushort)record.SrvPriority, (ushort)record.SrvWeight );
                else if (record.RecordType == DnsRecordType.PTR)
                    ps.Add_DnsServerResourceRecordPTR(zoneName, name, record.RecordData );
                else
					throw new Exception( "Unknown record type" );
			}
			catch( Exception ex )
			{
				// log exception
				Log.WriteError( ex );
			}
		}

		public virtual void AddZoneRecords( string zoneName, DnsRecord[] records )
		{
			bulkRecords = true;
			try
			{
				foreach( DnsRecord record in records )
					AddZoneRecord( zoneName, record );
			}
			finally
			{
				bulkRecords = false;
			}

			UpdateSoaRecord( zoneName );
		}

		public virtual void DeleteZoneRecord( string zoneName, DnsRecord record )
		{
			try
			{
				string rrType;
				if( !RecordTypes.rrTypeFromRecord.TryGetValue( record.RecordType, out rrType ) )
					throw new Exception( "Unknown record type" );
				ps.Remove_DnsServerResourceRecord( zoneName, record);
			}
			catch( Exception ex )
			{
				// log exception
				Log.WriteError( ex );
			}
		}

		public virtual void DeleteZoneRecords( string zoneName, DnsRecord[] records )
		{
			foreach( DnsRecord record in records )
				DeleteZoneRecord( zoneName, record );
		}

		#endregion

		#region SOA Record
		public virtual void UpdateSoaRecord( string zoneName, string host, string primaryNsServer, string primaryPerson )
		{
            try
            {
                ps.Update_DnsServerResourceRecordSOA(zoneName, ExpireLimit, MinimumTTL, primaryNsServer, RefreshInterval, primaryPerson, RetryDelay, null);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
            }
        }

        private void UpdateSoaRecord(string zoneName)
        {
            if (bulkRecords)
                return;

            try
            {
                ps.Update_DnsServerResourceRecordSOA(zoneName, ExpireLimit, MinimumTTL, null, RefreshInterval, null, RetryDelay, null);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
            }
        }

		#endregion


		public override void DeleteServiceItems( ServiceProviderItem[] items )
		{
			foreach( ServiceProviderItem item in items )
			{
				if( item is DnsZone )
				{
					try
					{
						// delete DNS zone
						DeleteZone( item.Name );
					}
					catch( Exception ex )
					{
						Log.WriteError( String.Format( "Error deleting '{0}' MS DNS zone", item.Name ), ex );
					}
				}
			}
		}

		protected virtual bool IsDNSInstalled() => ps.Test_DnsServer();

        public override bool IsInstalled()
		{
			if (!OSInfo.IsWindows) return false;

			return IsDNSInstalled() && WindowsVersion.WindowsServer2012 <= OSInfo.WindowsVersion &&
				OSInfo.WindowsVersion < WindowsVersion.WindowsServer2016;
		}
	}
}
