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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.Ecommerce.EnterpriseServer
{
	internal class WhoisSettings
	{
		public readonly static Hashtable Parsers;
		public readonly static Hashtable WhoisServers;

		public const string UANIC = "UANIC";
		public const string INTERNIC = "INTERNIC";
		public const string PIR = "PIR";
		public const string EDUCAUSE = "EDUCAUSE";
		public const string NOMINET = "NOMINET";
		public const string AUREGISTRY = "AUREGISTRY";
		public const string EURID = "EURID";
		public const string ROMANIAN = "ROMANIAN";
		public const string SWITCH = "SWITCH";
		public const string NEUSTAR = "NEUSTAR";
		public const string NUDOMAINLTD = "NUDOMAINLTD";
		public const string NEULEVEL = "NEULEVEL";
		public const string AFFILIAS_LTD = "AFFILIAS_LTD";
		public const string SIDN = "SIDN";
        public const string AFNIC = "AFNIC";
        public const string TIERED_ACCESS = "TIERED_ACCESS";
        public const string mTLD = "mTLD";
		public const string INTERNET_NZ = "InternetNZ";

		private WhoisSettings() { }

		static WhoisSettings()
		{
			WhoisServers = new Hashtable();
			Parsers = new Hashtable();

			InitParsers();
			InitWhoisServers();
		}

		static void InitWhoisServers()
		{
			WhoisServers.Add("ua", "whois.ua");
			WhoisServers.Add("com", "whois.verisign-grs.com");
			WhoisServers.Add("cc", "ccwhois.verisign-grs.com");
			WhoisServers.Add("net", "whois.verisign-grs.com");
			WhoisServers.Add("edu", "whois.educause.net");
			WhoisServers.Add("org", "whois.publicinterestregistry.net");
			WhoisServers.Add("uk", "whois.nic.uk");
			WhoisServers.Add("au", "whois.ausregistry.net");
			WhoisServers.Add("us", "whois.nic.us");
			WhoisServers.Add("eu", "whois.eu");
			WhoisServers.Add("ro", "whois.rotld.ro");
			WhoisServers.Add("ch", "whois.nic.ch");
			WhoisServers.Add("tv", "tvwhois.verisign-grs.com");
			WhoisServers.Add("biz", "whois.biz");
			WhoisServers.Add("info", "whois.afilias.net");
			WhoisServers.Add("nl", "whois.domain-registry.nl");
            WhoisServers.Add("fr", "whois.nic.fr");
            WhoisServers.Add("be", "whois.dns.be");
            WhoisServers.Add("name", "whois.nic.name");
            WhoisServers.Add("mobi", "whois.dotmobiregistry.net");
			WhoisServers.Add("nu", "whois.nic.nu");
			WhoisServers.Add("nz", "srs-ak.srs.net.nz");
		}

		static void InitParsers()
		{
			Parsers.Add("whois.ua", UANIC);
			Parsers.Add("whois.nic.nu", NUDOMAINLTD);
			Parsers.Add("whois.internic.net", INTERNIC);
			Parsers.Add("whois.verisign-grs.com", INTERNIC);
			Parsers.Add("ccwhois.verisign-grs.com", INTERNIC);
			Parsers.Add("whois.publicinterestregistry.net", PIR);
			Parsers.Add("whois.educause.net", EDUCAUSE);
			Parsers.Add("whois.nic.uk", NOMINET);
			Parsers.Add("whois.ausregistry.net", AUREGISTRY);
			Parsers.Add("whois.nic.us", NEUSTAR);
			Parsers.Add("whois.eu", EURID);
            Parsers.Add("whois.dns.be", EURID);
			Parsers.Add("whois.rotld.ro", ROMANIAN);
			Parsers.Add("whois.nic.ch", SWITCH);
			Parsers.Add("tvwhois.verisign-grs.com", INTERNIC);
			Parsers.Add("whois.biz", NEULEVEL);
			Parsers.Add("whois.afilias.net", AFFILIAS_LTD);
			Parsers.Add("whois.domain-registry.nl", SIDN);
            Parsers.Add("whois.nic.fr", AFNIC);
            Parsers.Add("whois.nic.name", TIERED_ACCESS);
            Parsers.Add("whois.dotmobiregistry.net", mTLD);
			Parsers.Add("srs-ak.srs.net.nz", INTERNET_NZ);
		}
	}
}
