using System;
using System.Collections.Generic;
using FuseCP.EnterpriseServer.Data.Configuration;
using FuseCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace FuseCP.EnterpriseServer.Data.Configuration;

public partial class ProviderConfiguration: EntityTypeConfiguration<Provider>
{
    public override void Configure() {
        HasKey(e => e.ProviderId).HasName("PK_Provider");

#if NetCore
        Property(e => e.ProviderId).ValueGeneratedNever();

        HasOne(d => d.Group).WithMany(p => p.Providers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Providers_ResourceGroups");
#else
        HasRequired(d => d.Group).WithMany(p => p.Providers);
#endif

		#region Seed Data
		HasData(() => new Provider[] {
			//new Provider() { ProviderId = 1, DisplayName = "Windows Server 2003", EditorControl = "Windows2003", GroupId = 1, ProviderName = "Windows2003", ProviderType = "FuseCP.Providers.OS.Windows2003, FuseCP.Providers.OS.Windows2003" },
			new Provider() { ProviderId = 2, DisplayName = "Internet Information Services 6.0", EditorControl = "IIS60", GroupId = 2, ProviderName = "IIS60", ProviderType = "FuseCP.Providers.Web.IIs60, FuseCP.Providers.Web.IIs60" },
			new Provider() { ProviderId = 3, DisplayName = "Microsoft FTP Server 6.0", EditorControl = "MSFTP60", GroupId = 3, ProviderName = "MSFTP60", ProviderType = "FuseCP.Providers.FTP.MsFTP, FuseCP.Providers.FTP.IIs60" },
			new Provider() { ProviderId = 4, DisplayName = "MailEnable Server 1.x - 7.x", EditorControl = "MailEnable", GroupId = 4, ProviderName = "MailEnable", ProviderType = "FuseCP.Providers.Mail.MailEnable, FuseCP.Providers.Mail.MailEnable" },
			new Provider() { ProviderId = 5, DisplayName = "Microsoft SQL Server 2000", EditorControl = "MSSQL", GroupId = 5, ProviderName = "MSSQL", ProviderType = "FuseCP.Providers.Database.MsSqlServer, FuseCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 6, DisplayName = "MySQL Server 4.x", EditorControl = "MySQL", GroupId = 6, ProviderName = "MySQL", ProviderType = "FuseCP.Providers.Database.MySqlServer, FuseCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 7, DisplayName = "Microsoft DNS Server", EditorControl = "MSDNS", GroupId = 7, ProviderName = "MSDNS", ProviderType = "FuseCP.Providers.DNS.MsDNS, FuseCP.Providers.DNS.MsDNS" },
			new Provider() { ProviderId = 8, DisplayName = "AWStats Statistics Service", EditorControl = "AWStats", GroupId = 8, ProviderName = "AWStats", ProviderType = "FuseCP.Providers.Statistics.AWStats, FuseCP.Providers.Statistics.AWStats" },
			new Provider() { ProviderId = 9, DisplayName = "SimpleDNS Plus 4.x", EditorControl = "SimpleDNS", GroupId = 7, ProviderName = "SimpleDNS", ProviderType = "FuseCP.Providers.DNS.SimpleDNS, FuseCP.Providers.DNS.SimpleDNS" },
			new Provider() { ProviderId = 10, DisplayName = "SmarterStats 3.x", EditorControl = "SmarterStats", GroupId = 8, ProviderName = "SmarterStats", ProviderType = "FuseCP.Providers.Statistics.SmarterStats, FuseCP.Providers.Statistics.SmarterS" +
				"tats" },
			new Provider() { ProviderId = 11, DisplayName = "SmarterMail 2.x", EditorControl = "SmarterMail", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "FuseCP.Providers.Mail.SmarterMail2, FuseCP.Providers.Mail.SmarterMail2" },
			new Provider() { ProviderId = 12, DisplayName = "Gene6 FTP Server 3.x", EditorControl = "Gene6FTP", GroupId = 3, ProviderName = "Gene6FTP", ProviderType = "FuseCP.Providers.FTP.Gene6, FuseCP.Providers.FTP.Gene6" },
			new Provider() { ProviderId = 13, DisplayName = "Merak Mail Server 8.0.3 - 9.2.x", EditorControl = "Merak", GroupId = 4, ProviderName = "Merak", ProviderType = "FuseCP.Providers.Mail.Merak, FuseCP.Providers.Mail.Merak" },
			new Provider() { ProviderId = 14, DisplayName = "SmarterMail 3.x - 4.x", EditorControl = "SmarterMail", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "FuseCP.Providers.Mail.SmarterMail3, FuseCP.Providers.Mail.SmarterMail3" },
			new Provider() { ProviderId = 16, DisplayName = "Microsoft SQL Server 2005", EditorControl = "MSSQL", GroupId = 10, ProviderName = "MSSQL", ProviderType = "FuseCP.Providers.Database.MsSqlServer2005, FuseCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 17, DisplayName = "MySQL Server 5.0", EditorControl = "MySQL", GroupId = 11, ProviderName = "MySQL", ProviderType = "FuseCP.Providers.Database.MySqlServer50, FuseCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 18, DisplayName = "MDaemon 9.x - 11.x", EditorControl = "MDaemon", GroupId = 4, ProviderName = "MDaemon", ProviderType = "FuseCP.Providers.Mail.MDaemon, FuseCP.Providers.Mail.MDaemon" },
			new Provider() { ProviderId = 19, DisableAutoDiscovery = true, DisplayName = "ArGoSoft Mail Server 1.x", EditorControl = "ArgoMail", GroupId = 4, ProviderName = "ArgoMail",
				ProviderType = "FuseCP.Providers.Mail.ArgoMail, FuseCP.Providers.Mail.ArgoMail" },
			new Provider() { ProviderId = 20, DisplayName = "hMailServer 4.2", EditorControl = "hMailServer", GroupId = 4, ProviderName = "hMailServer", ProviderType = "FuseCP.Providers.Mail.hMailServer, FuseCP.Providers.Mail.hMailServer" },
			new Provider() { ProviderId = 21, DisplayName = "Ability Mail Server 2.x", EditorControl = "AbilityMailServer", GroupId = 4, ProviderName = "AbilityMailServer", ProviderType = "FuseCP.Providers.Mail.AbilityMailServer, FuseCP.Providers.Mail.AbilityMailServ" +
				"er" },
			new Provider() { ProviderId = 22, DisplayName = "hMailServer 4.3", EditorControl = "hMailServer43", GroupId = 4, ProviderName = "hMailServer43", ProviderType = "FuseCP.Providers.Mail.hMailServer43, FuseCP.Providers.Mail.hMailServer43" },
			new Provider() { ProviderId = 24, DisplayName = "ISC BIND 8.x - 9.x", EditorControl = "Bind", GroupId = 7, ProviderName = "Bind", ProviderType = "FuseCP.Providers.DNS.IscBind, FuseCP.Providers.DNS.Bind" },
			new Provider() { ProviderId = 25, DisplayName = "Serv-U FTP 6.x", EditorControl = "ServU", GroupId = 3, ProviderName = "ServU", ProviderType = "FuseCP.Providers.FTP.ServU, FuseCP.Providers.FTP.ServU" },
			new Provider() { ProviderId = 26, DisplayName = "FileZilla FTP Server 0.9", EditorControl = "FileZilla", GroupId = 3, ProviderName = "FileZilla", ProviderType = "FuseCP.Providers.FTP.FileZilla, FuseCP.Providers.FTP.FileZilla" },
			new Provider() { ProviderId = 27, DisplayName = "Hosted Microsoft Exchange Server 2007", EditorControl = "Exchange", GroupId = 12, ProviderName = "Exchange2007", ProviderType = "FuseCP.Providers.HostedSolution.Exchange2007, FuseCP.Providers.HostedSolution" },
			new Provider() { ProviderId = 28, DisplayName = "SimpleDNS Plus 5.x", EditorControl = "SimpleDNS", GroupId = 7, ProviderName = "SimpleDNS", ProviderType = "FuseCP.Providers.DNS.SimpleDNS5, FuseCP.Providers.DNS.SimpleDNS50" },
			new Provider() { ProviderId = 29, DisplayName = "SmarterMail 5.x", EditorControl = "SmarterMail50", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "FuseCP.Providers.Mail.SmarterMail5, FuseCP.Providers.Mail.SmarterMail5" },
			new Provider() { ProviderId = 30, DisplayName = "MySQL Server 5.1", EditorControl = "MySQL", GroupId = 11, ProviderName = "MySQL", ProviderType = "FuseCP.Providers.Database.MySqlServer51, FuseCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 31, DisplayName = "SmarterStats 4.x", EditorControl = "SmarterStats", GroupId = 8, ProviderName = "SmarterStats", ProviderType = "FuseCP.Providers.Statistics.SmarterStats4, FuseCP.Providers.Statistics.Smarter" +
				"Stats" },
			new Provider() { ProviderId = 32, DisplayName = "Hosted Microsoft Exchange Server 2010", EditorControl = "Exchange", GroupId = 12, ProviderName = "Exchange2010", ProviderType = "FuseCP.Providers.HostedSolution.Exchange2010, FuseCP.Providers.HostedSolution" },
			new Provider() { ProviderId = 55, DisableAutoDiscovery = true, DisplayName = "Nettica DNS", EditorControl = "NetticaDNS", GroupId = 7, ProviderName = "NetticaDNS",
				ProviderType = "FuseCP.Providers.DNS.Nettica, FuseCP.Providers.DNS.Nettica" },
			new Provider() { ProviderId = 56, DisableAutoDiscovery = true, DisplayName = "PowerDNS", EditorControl = "PowerDNS", GroupId = 7, ProviderName = "PowerDNS",
				ProviderType = "FuseCP.Providers.DNS.PowerDNS, FuseCP.Providers.DNS.PowerDNS" },
			new Provider() { ProviderId = 60, DisplayName = "SmarterMail 6.x", EditorControl = "SmarterMail60", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "FuseCP.Providers.Mail.SmarterMail6, FuseCP.Providers.Mail.SmarterMail6" },
			new Provider() { ProviderId = 61, DisplayName = "Merak Mail Server 10.x", EditorControl = "Merak", GroupId = 4, ProviderName = "Merak", ProviderType = "FuseCP.Providers.Mail.Merak10, FuseCP.Providers.Mail.Merak10" },
			new Provider() { ProviderId = 62, DisplayName = "SmarterStats 5.x +", EditorControl = "SmarterStats", GroupId = 8, ProviderName = "SmarterStats", ProviderType = "FuseCP.Providers.Statistics.SmarterStats5, FuseCP.Providers.Statistics.Smarter" +
				"Stats" },
			new Provider() { ProviderId = 63, DisplayName = "hMailServer 5.x", EditorControl = "hMailServer5", GroupId = 4, ProviderName = "hMailServer5", ProviderType = "FuseCP.Providers.Mail.hMailServer5, FuseCP.Providers.Mail.hMailServer5" },
			new Provider() { ProviderId = 64, DisplayName = "SmarterMail 7.x - 8.x", EditorControl = "SmarterMail60", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "FuseCP.Providers.Mail.SmarterMail7, FuseCP.Providers.Mail.SmarterMail7" },
			new Provider() { ProviderId = 65, DisplayName = "SmarterMail 9.x", EditorControl = "SmarterMail60", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "FuseCP.Providers.Mail.SmarterMail9, FuseCP.Providers.Mail.SmarterMail9" },
			new Provider() { ProviderId = 66, DisplayName = "SmarterMail 10.x +", EditorControl = "SmarterMail100", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "FuseCP.Providers.Mail.SmarterMail10, FuseCP.Providers.Mail.SmarterMail10" },
			new Provider() { ProviderId = 67, DisplayName = "SmarterMail 100.x +", EditorControl = "SmarterMail100x", GroupId = 4, ProviderName = "SmarterMail", ProviderType = "FuseCP.Providers.Mail.SmarterMail100, FuseCP.Providers.Mail.SmarterMail100" },
			new Provider() { ProviderId = 90, DisplayName = "Hosted Microsoft Exchange Server 2010 SP2", EditorControl = "Exchange", GroupId = 12, ProviderName = "Exchange2010SP2", ProviderType = "FuseCP.Providers.HostedSolution.Exchange2010SP2, FuseCP.Providers.HostedSoluti" +
				"on" },
			new Provider() { ProviderId = 91, DisplayName = "Hosted Microsoft Exchange Server 2013", EditorControl = "Exchange", GroupId = 12, ProviderName = "Exchange2013", ProviderType = "FuseCP.Providers.HostedSolution.Exchange2013, FuseCP.Providers.HostedSolution." +
				"Exchange2013" },
			new Provider() { ProviderId = 92, DisplayName = "Hosted Microsoft Exchange Server 2016", EditorControl = "Exchange", GroupId = 12, ProviderName = "Exchange2016", ProviderType = "FuseCP.Providers.HostedSolution.Exchange2016, FuseCP.Providers.HostedSolution." +
				"Exchange2016" },
			new Provider() { ProviderId = 93, DisplayName = "Hosted Microsoft Exchange Server 2019", EditorControl = "Exchange", GroupId = 12, ProviderName = "Exchange2016", ProviderType = "FuseCP.Providers.HostedSolution.Exchange2019, FuseCP.Providers.HostedSolution." +
				"Exchange2019" },
			//new Provider() { ProviderId = 100, DisplayName = "Windows Server 2008", EditorControl = "Windows2008", GroupId = 1, ProviderName = "Windows2008", ProviderType = "FuseCP.Providers.OS.Windows2008, FuseCP.Providers.OS.Windows2008" },
			new Provider() { ProviderId = 101, DisplayName = "Internet Information Services 7.0", EditorControl = "IIS70", GroupId = 2, ProviderName = "IIS70", ProviderType = "FuseCP.Providers.Web.IIs70, FuseCP.Providers.Web.IIs70" },
			new Provider() { ProviderId = 102, DisplayName = "Microsoft FTP Server 7.0", EditorControl = "MSFTP70", GroupId = 3, ProviderName = "MSFTP70", ProviderType = "FuseCP.Providers.FTP.MsFTP, FuseCP.Providers.FTP.IIs70" },
			new Provider() { ProviderId = 103, DisplayName = "Hosted Organizations", EditorControl = "Organizations", GroupId = 13, ProviderName = "Organizations", ProviderType = "FuseCP.Providers.HostedSolution.OrganizationProvider, FuseCP.Providers.HostedS" +
				"olution" },
			//new Provider() { ProviderId = 104, DisplayName = "Windows Server 2012", EditorControl = "Windows2012", GroupId = 1, ProviderName = "Windows2012", ProviderType = "FuseCP.Providers.OS.Windows2012, FuseCP.Providers.OS.Windows2012" },
			new Provider() { ProviderId = 105, DisplayName = "Internet Information Services 8.0", EditorControl = "IIS70", GroupId = 2, ProviderName = "IIS80", ProviderType = "FuseCP.Providers.Web.IIs80, FuseCP.Providers.Web.IIs80" },
			new Provider() { ProviderId = 106, DisplayName = "Microsoft FTP Server 8.0", EditorControl = "MSFTP70", GroupId = 3, ProviderName = "MSFTP80", ProviderType = "FuseCP.Providers.FTP.MsFTP80, FuseCP.Providers.FTP.IIs80" },
			new Provider() { ProviderId = 110, DisplayName = "Cerberus FTP Server 6.x", EditorControl = "CerberusFTP6", GroupId = 3, ProviderName = "CerberusFTP6", ProviderType = "FuseCP.Providers.FTP.CerberusFTP6, FuseCP.Providers.FTP.CerberusFTP6" },
			new Provider() { ProviderId = 111, DisplayName = "Windows Server 2016", EditorControl = "Windows2008", GroupId = 1, ProviderName = "Windows2016", ProviderType = "FuseCP.Providers.OS.Windows2016, FuseCP.Providers.OS.Windows2016" },
			new Provider() { ProviderId = 112, DisplayName = "Internet Information Services 10.0", EditorControl = "IIS70", GroupId = 2, ProviderName = "IIS100", ProviderType = "FuseCP.Providers.Web.IIs100, FuseCP.Providers.Web.IIs100" },
			new Provider() { ProviderId = 113, DisplayName = "Microsoft FTP Server 10.0", EditorControl = "MSFTP70", GroupId = 3, ProviderName = "MSFTP100", ProviderType = "FuseCP.Providers.FTP.MsFTP100, FuseCP.Providers.FTP.IIs100" },
			new Provider() { ProviderId = 160, DisplayName = "IceWarp Mail Server", EditorControl = "IceWarp", GroupId = 4, ProviderName = "IceWarp", ProviderType = "FuseCP.Providers.Mail.IceWarp, FuseCP.Providers.Mail.IceWarp" },
			new Provider() { ProviderId = 200, DisplayName = "Hosted Windows SharePoint Services 3.0", EditorControl = "HostedSharePoint30", GroupId = 20, ProviderName = "HostedSharePoint30", ProviderType = "FuseCP.Providers.HostedSolution.HostedSharePointServer, FuseCP.Providers.Hoste" +
				"dSolution" },
			new Provider() { ProviderId = 201, DisplayName = "Hosted MS CRM 4.0", EditorControl = "CRM", GroupId = 21, ProviderName = "CRM", ProviderType = "FuseCP.Providers.HostedSolution.CRMProvider, FuseCP.Providers.HostedSolution" },
			new Provider() { ProviderId = 202, DisplayName = "Microsoft SQL Server 2008", EditorControl = "MSSQL", GroupId = 22, ProviderName = "MsSQL", ProviderType = "FuseCP.Providers.Database.MsSqlServer2008, FuseCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 203, DisableAutoDiscovery = true, DisplayName = "BlackBerry 4.1", EditorControl = "BlackBerry", GroupId = 31, ProviderName = "BlackBerry 4.1",
				ProviderType = "FuseCP.Providers.HostedSolution.BlackBerryProvider, FuseCP.Providers.HostedSol" +
				"ution" },
			new Provider() { ProviderId = 204, DisableAutoDiscovery = true, DisplayName = "BlackBerry 5.0", EditorControl = "BlackBerry5", GroupId = 31, ProviderName = "BlackBerry 5.0",
				ProviderType = "FuseCP.Providers.HostedSolution.BlackBerry5Provider, FuseCP.Providers.HostedSo" +
				"lution" },
			new Provider() { ProviderId = 205, DisableAutoDiscovery = true, DisplayName = "Office Communications Server 2007 R2", EditorControl = "OCS", GroupId = 32, ProviderName = "OCS",
				ProviderType = "FuseCP.Providers.HostedSolution.OCS2007R2, FuseCP.Providers.HostedSolution" },
			new Provider() { ProviderId = 206, DisableAutoDiscovery = true, DisplayName = "OCS Edge server", EditorControl = "OCS_Edge", GroupId = 32, ProviderName = "OCSEdge",
				ProviderType = "FuseCP.Providers.HostedSolution.OCSEdge2007R2, FuseCP.Providers.HostedSolution" },
			new Provider() { ProviderId = 208, DisplayName = "Hosted SharePoint Foundation 2010", EditorControl = "HostedSharePoint30", GroupId = 20, ProviderName = "HostedSharePoint2010", ProviderType = "FuseCP.Providers.HostedSolution.HostedSharePointServer2010, FuseCP.Providers.H" +
				"ostedSolution" },
			new Provider() { ProviderId = 209, DisplayName = "Microsoft SQL Server 2012", EditorControl = "MSSQL", GroupId = 23, ProviderName = "MsSQL", ProviderType = "FuseCP.Providers.Database.MsSqlServer2012, FuseCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 250, DisplayName = "Microsoft Lync Server 2010 Multitenant Hosting Pack", EditorControl = "Lync", GroupId = 41, ProviderName = "Lync2010", ProviderType = "FuseCP.Providers.HostedSolution.Lync2010, FuseCP.Providers.HostedSolution" },
			new Provider() { ProviderId = 300, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V", EditorControl = "HyperV", GroupId = 30, ProviderName = "HyperV",
				ProviderType = "FuseCP.Providers.Virtualization.HyperV, FuseCP.Providers.Virtualization.HyperV" },
			new Provider() { ProviderId = 301, DisplayName = "MySQL Server 5.5", EditorControl = "MySQL", GroupId = 11, ProviderName = "MySQL", ProviderType = "FuseCP.Providers.Database.MySqlServer55, FuseCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 302, DisplayName = "MySQL Server 5.6", EditorControl = "MySQL", GroupId = 11, ProviderName = "MySQL", ProviderType = "FuseCP.Providers.Database.MySqlServer56, FuseCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 303, DisplayName = "MySQL Server 5.7", EditorControl = "MySQL", GroupId = 11, ProviderName = "MySQL", ProviderType = "FuseCP.Providers.Database.MySqlServer57, FuseCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 304, DisplayName = "MySQL Server 8.0", EditorControl = "MySQL", GroupId = 90, ProviderName = "MySQL", ProviderType = "FuseCP.Providers.Database.MySqlServer80, FuseCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 305, DisplayName = "MySQL Server 8.1", EditorControl = "MySQL", GroupId = 90, ProviderName = "MySQL", ProviderType = "FuseCP.Providers.Database.MySqlServer81, FuseCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 306, DisplayName = "MySQL Server 8.2", EditorControl = "MySQL", GroupId = 90, ProviderName = "MySQL", ProviderType = "FuseCP.Providers.Database.MySqlServer82, FuseCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 307, DisplayName = "MySQL Server 8.3", EditorControl = "MySQL", GroupId = 90, ProviderName = "MySQL", ProviderType = "FuseCP.Providers.Database.MySqlServer83, FuseCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 308, DisplayName = "MySQL Server 8.4", EditorControl = "MySQL", GroupId = 90, ProviderName = "MySQL", ProviderType = "FuseCP.Providers.Database.MySqlServer84, FuseCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 320, DisplayName = "MySQL Server 9.0", EditorControl = "MySQL", GroupId = 90, ProviderName = "MySQL", ProviderType = "FuseCP.Providers.Database.MySqlServer90, FuseCP.Providers.Database.MySQL" },
			new Provider() { ProviderId = 350, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V 2012 R2", EditorControl = "HyperV2012R2", GroupId = 33, ProviderName = "HyperV2012R2",
				ProviderType = "FuseCP.Providers.Virtualization.HyperV2012R2, FuseCP.Providers.Virtualization." +
				"HyperV2012R2" },
			new Provider() { ProviderId = 351, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V Virtual Machine Management", EditorControl = "HyperVvmm", GroupId = 33, ProviderName = "HyperVvmm",
				ProviderType = "FuseCP.Providers.Virtualization.HyperVvmm, FuseCP.Providers.Virtualization.Hyp" +
				"erVvmm" },
			new Provider() { ProviderId = 352, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V 2016", EditorControl = "HyperV2012R2", GroupId = 33, ProviderName = "HyperV2016",
				ProviderType = "FuseCP.Providers.Virtualization.HyperV2016, FuseCP.Providers.Virtualization.Hy" +
				"perV2016" },
			new Provider() { ProviderId = 370, DisableAutoDiscovery = true, DisplayName = "Proxmox Virtualization (remote)", EditorControl = "Proxmox", GroupId = 167, ProviderName = "Proxmox (remote)",
				ProviderType = "FuseCP.Providers.Virtualization.Proxmoxvps, FuseCP.Providers.Virtualization.Pr" +
				"oxmoxvps" },
			new Provider() { ProviderId = 371, DisableAutoDiscovery = false, DisplayName = "Proxmox Virtualization", EditorControl = "Proxmox", GroupId = 167, ProviderName = "Proxmox",
				ProviderType = "FuseCP.Providers.Virtualization.ProxmoxvpsLocal, FuseCP.Providers.Virtualizati" +
				"on.Proxmoxvps" },
			new Provider() { ProviderId = 400, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V For Private Cloud", EditorControl = "HyperVForPrivateCloud", GroupId = 40, ProviderName = "HyperVForPC",
				ProviderType = "FuseCP.Providers.VirtualizationForPC.HyperVForPC, FuseCP.Providers.Virtualizat" +
				"ionForPC.HyperVForPC" },
			new Provider() { ProviderId = 410, DisplayName = "Microsoft DNS Server 2012+", EditorControl = "MSDNS", GroupId = 7, ProviderName = "MSDNS.2012", ProviderType = "FuseCP.Providers.DNS.MsDNS2012, FuseCP.Providers.DNS.MsDNS2012" },
			new Provider() { ProviderId = 500, DisplayName = "Unix System", EditorControl = "Unix", GroupId = 1, ProviderName = "UnixSystem", ProviderType = "FuseCP.Providers.OS.Unix, FuseCP.Providers.OS.Unix" },
			new Provider() { ProviderId = 600, DisplayName = "Enterprise Storage Windows 2012", EditorControl = "EnterpriseStorage", GroupId = 44, ProviderName = "EnterpriseStorage2012", ProviderType = "FuseCP.Providers.EnterpriseStorage.Windows2012, FuseCP.Providers.EnterpriseSto" +
				"rage.Windows2012" },
			new Provider() { ProviderId = 700, DisplayName = "Storage Spaces Windows 2012", EditorControl = "StorageSpaceServices", GroupId = 49, ProviderName = "StorageSpace2012", ProviderType = "FuseCP.Providers.StorageSpaces.Windows2012, FuseCP.Providers.StorageSpaces.Win" +
				"dows2012" },
			new Provider() { ProviderId = 1201, DisplayName = "Hosted MS CRM 2011", EditorControl = "CRM2011", GroupId = 21, ProviderName = "CRM", ProviderType = "FuseCP.Providers.HostedSolution.CRMProvider2011, FuseCP.Providers.HostedSoluti" +
				"on.CRM2011" },
			new Provider() { ProviderId = 1202, DisplayName = "Hosted MS CRM 2013", EditorControl = "CRM2011", GroupId = 24, ProviderName = "CRM", ProviderType = "FuseCP.Providers.HostedSolution.CRMProvider2013, FuseCP.Providers.HostedSoluti" +
				"on.Crm2013" },
			new Provider() { ProviderId = 1203, DisplayName = "Microsoft SQL Server 2014", EditorControl = "MSSQL", GroupId = 46, ProviderName = "MsSQL", ProviderType = "FuseCP.Providers.Database.MsSqlServer2014, FuseCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 1205, DisplayName = "Hosted MS CRM 2015", EditorControl = "CRM2011", GroupId = 24, ProviderName = "CRM", ProviderType = "FuseCP.Providers.HostedSolution.CRMProvider2015, FuseCP.Providers.HostedSoluti" +
				"on.Crm2015" },
			new Provider() { ProviderId = 1206, DisplayName = "Hosted MS CRM 2016", EditorControl = "CRM2011", GroupId = 24, ProviderName = "CRM", ProviderType = "FuseCP.Providers.HostedSolution.CRMProvider2016, FuseCP.Providers.HostedSoluti" +
				"on.Crm2016" },
			new Provider() { ProviderId = 1301, DisplayName = "Hosted SharePoint Foundation 2013", EditorControl = "HostedSharePoint30", GroupId = 20, ProviderName = "HostedSharePoint2013", ProviderType = "FuseCP.Providers.HostedSolution.HostedSharePointServer2013, FuseCP.Providers.H" +
				"ostedSolution.SharePoint2013" },
			new Provider() { ProviderId = 1306, DisplayName = "Hosted SharePoint Foundation 2016", EditorControl = "HostedSharePoint30", GroupId = 20, ProviderName = "HostedSharePoint2016", ProviderType = "FuseCP.Providers.HostedSolution.HostedSharePointServer2016, FuseCP.Providers.H" +
				"ostedSolution.SharePoint2016" },
			new Provider() { ProviderId = 1401, DisplayName = "Microsoft Lync Server 2013 Enterprise Edition", EditorControl = "Lync", GroupId = 41, ProviderName = "Lync2013", ProviderType = "FuseCP.Providers.HostedSolution.Lync2013, FuseCP.Providers.HostedSolution.Lync" +
				"2013" },
			new Provider() { ProviderId = 1402, DisplayName = "Microsoft Lync Server 2013 Multitenant Hosting Pack", EditorControl = "Lync", GroupId = 41, ProviderName = "Lync2013HP", ProviderType = "FuseCP.Providers.HostedSolution.Lync2013HP, FuseCP.Providers.HostedSolution.Ly" +
				"nc2013HP" },
			new Provider() { ProviderId = 1403, DisplayName = "Microsoft Skype for Business Server 2015", EditorControl = "SfB", GroupId = 52, ProviderName = "SfB2015", ProviderType = "FuseCP.Providers.HostedSolution.SfB2015, FuseCP.Providers.HostedSolution.SfB20" +
				"15" },
			new Provider() { ProviderId = 1404, DisplayName = "Microsoft Skype for Business Server 2019", EditorControl = "SfB", GroupId = 52, ProviderName = "SfB2019", ProviderType = "FuseCP.Providers.HostedSolution.SfB2019, FuseCP.Providers.HostedSolution.SfB20" +
				"19" },
			new Provider() { ProviderId = 1501, DisableAutoDiscovery = true, DisplayName = "Remote Desktop Services Windows 2012", EditorControl = "RDS", GroupId = 45, ProviderName = "RemoteDesktopServices2012",
				ProviderType = "FuseCP.Providers.RemoteDesktopServices.Windows2012,FuseCP.Providers.RemoteDesk" +
				"topServices.Windows2012" },
			new Provider() { ProviderId = 1502, DisableAutoDiscovery = true, DisplayName = "Remote Desktop Services Windows 2016", EditorControl = "RDS", GroupId = 45, ProviderName = "RemoteDesktopServices2012",
				ProviderType = "FuseCP.Providers.RemoteDesktopServices.Windows2016,FuseCP.Providers.RemoteDesk" +
				"topServices.Windows2016" },
			new Provider() { ProviderId = 1503, DisableAutoDiscovery = true, DisplayName = "Remote Desktop Services Windows 2019", EditorControl = "RDS", GroupId = 45, ProviderName = "RemoteDesktopServices2019",
				ProviderType = "FuseCP.Providers.RemoteDesktopServices.Windows2019,FuseCP.Providers.RemoteDesk" +
				"topServices.Windows2019" },
			new Provider() { ProviderId = 1504, DisableAutoDiscovery = true, DisplayName = "Remote Desktop Services Windows 2022", EditorControl = "RDS", GroupId = 45, ProviderName = "RemoteDesktopServices2022",
				ProviderType = "FuseCP.Providers.RemoteDesktopServices.Windows2022,FuseCP.Providers.RemoteDesk" +
				"topServices.Windows2022" },
			new Provider() { ProviderId = 1505, DisableAutoDiscovery = true, DisplayName = "Remote Desktop Services Windows 2025", EditorControl = "RDS", GroupId = 45, ProviderName = "RemoteDesktopServices2025",
				ProviderType = "FuseCP.Providers.RemoteDesktopServices.Windows2025,FuseCP.Providers.RemoteDesk" +
				"topServices.Windows2025" },
			new Provider() { ProviderId = 1550, DisplayName = "MariaDB 10.1", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB101, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1552, DisplayName = "Hosted SharePoint Enterprise 2013", EditorControl = "HostedSharePoint30", GroupId = 73, ProviderName = "HostedSharePoint2013Ent", ProviderType = "FuseCP.Providers.HostedSolution.HostedSharePointServer2013Ent, FuseCP.Provider" +
				"s.HostedSolution.SharePoint2013Ent" },
			new Provider() { ProviderId = 1560, DisplayName = "MariaDB 10.2", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB102, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1570, DisplayName = "MariaDB 10.3", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB",
				ProviderType = "FuseCP.Providers.Database.MariaDB103, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1571, DisplayName = "MariaDB 10.4", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB104, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1572, DisplayName = "MariaDB 10.5", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB105, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1573, DisplayName = "MariaDB 10.6", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB106, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1574, DisplayName = "MariaDB 10.7", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB107, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1575, DisplayName = "MariaDB 10.8", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB108, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1576, DisplayName = "MariaDB 10.9", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB109, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1577, DisplayName = "MariaDB 10.10", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB1010, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1578, DisplayName = "MariaDB 10.11", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB1011, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1579, DisplayName = "MariaDB 11.0", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB110, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1580, DisplayName = "MariaDB 11.1", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB111, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1581, DisplayName = "MariaDB 11.2", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB112, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1582, DisplayName = "MariaDB 11.3", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB113, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1583, DisplayName = "MariaDB 11.4", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB114, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1584, DisplayName = "MariaDB 11.5", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB115, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1585, DisplayName = "MariaDB 11.6", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB116, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1586, DisplayName = "MariaDB 11.7", EditorControl = "MariaDB", GroupId = 50, ProviderName = "MariaDB", ProviderType = "FuseCP.Providers.Database.MariaDB117, FuseCP.Providers.Database.MariaDB" },
			new Provider() { ProviderId = 1601, DisableAutoDiscovery = true, DisplayName = "Mail Cleaner", EditorControl = "MailCleaner", GroupId = 61, ProviderName = "MailCleaner",
				ProviderType = "FuseCP.Providers.Filters.MailCleaner, FuseCP.Providers.Filters.MailCleaner" },
			new Provider() { ProviderId = 1602, DisableAutoDiscovery = true, DisplayName = "SpamExperts Mail Filter", EditorControl = "SpamExperts", GroupId = 61, ProviderName = "SpamExperts",
				ProviderType = "FuseCP.Providers.Filters.SpamExperts, FuseCP.Providers.Filters.SpamExperts" },
			new Provider() { ProviderId = 1701, DisplayName = "Microsoft SQL Server 2016", EditorControl = "MSSQL", GroupId = 71, ProviderName = "MsSQL", ProviderType = "FuseCP.Providers.Database.MsSqlServer2016, FuseCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 1702, DisplayName = "Hosted SharePoint Enterprise 2016", EditorControl = "HostedSharePoint30", GroupId = 73, ProviderName = "HostedSharePoint2016Ent", ProviderType = "FuseCP.Providers.HostedSolution.HostedSharePointServer2016Ent, FuseCP.Provider" +
				"s.HostedSolution.SharePoint2016Ent" },
			new Provider() { ProviderId = 1703, DisplayName = "SimpleDNS Plus 6.x", EditorControl = "SimpleDNS", GroupId = 7, ProviderName = "SimpleDNS", ProviderType = "FuseCP.Providers.DNS.SimpleDNS6, FuseCP.Providers.DNS.SimpleDNS60" },
			new Provider() { ProviderId = 1704, DisplayName = "Microsoft SQL Server 2017", EditorControl = "MSSQL", GroupId = 72, ProviderName = "MsSQL",
				ProviderType = "FuseCP.Providers.Database.MsSqlServer2017, FuseCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 1705, DisplayName = "Microsoft SQL Server 2019", EditorControl = "MSSQL", GroupId = 74, ProviderName = "MsSQL",
				ProviderType = "FuseCP.Providers.Database.MsSqlServer2019, FuseCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 1706, DisplayName = "Microsoft SQL Server 2022", EditorControl = "MSSQL", GroupId = 75, ProviderName = "MsSQL", ProviderType = "FuseCP.Providers.Database.MsSqlServer2022, FuseCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 1707, DisplayName = "Microsoft SQL Server 2025", EditorControl = "MSSQL", GroupId = 76, ProviderName = "MsSQL", ProviderType = "FuseCP.Providers.Database.MsSqlServer2025, FuseCP.Providers.Database.SqlServer" },
			new Provider() { ProviderId = 1711, DisplayName = "Hosted SharePoint 2019", EditorControl = "HostedSharePoint30", GroupId = 73, ProviderName = "HostedSharePoint2019", ProviderType = "FuseCP.Providers.HostedSolution.HostedSharePointServer2019, FuseCP.Providers.H" +
				"ostedSolution.SharePoint2019" },
			new Provider() { ProviderId = 1800, DisplayName = "Windows Server 2019", EditorControl = "Windows2012", GroupId = 1, ProviderName = "Windows2019", ProviderType = "FuseCP.Providers.OS.Windows2019, FuseCP.Providers.OS.Windows2019" },
			new Provider() { ProviderId = 1801, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V 2019", EditorControl = "HyperV2012R2", GroupId = 33, ProviderName = "HyperV2019",
				ProviderType = "FuseCP.Providers.Virtualization.HyperV2019, FuseCP.Providers.Virtualization.Hy" +
				"perV2019" },
			new Provider() { ProviderId = 1802, DisplayName = "Windows Server 2022", EditorControl = "Windows2012", GroupId = 1, ProviderName = "Windows2022", ProviderType = "FuseCP.Providers.OS.Windows2022, FuseCP.Providers.OS.Windows2022" },
			new Provider() { ProviderId = 1803, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V 2022", EditorControl = "HyperV2012R2", GroupId = 33, ProviderName = "HyperV2022",
				ProviderType = "FuseCP.Providers.Virtualization.HyperV2022, FuseCP.Providers.Virtualization.Hy" +
				"perV2022" },
			new Provider() { ProviderId = 1804, DisplayName = "Windows Server 2025", EditorControl = "Windows2012", GroupId = 1, ProviderName = "Windows2025",
				ProviderType = "FuseCP.Providers.OS.Windows2025, FuseCP.Providers.OS.Windows2025" },
			new Provider() { ProviderId = 1805, DisableAutoDiscovery = true, DisplayName = "Microsoft Hyper-V 2025", EditorControl = "HyperV2012R2", GroupId = 33, ProviderName = "HyperV2025",
				ProviderType = "FuseCP.Providers.Virtualization.HyperV2025, FuseCP.Providers.Virtualization.Hy" +
				"perV2025" },
			new Provider() { ProviderId = 1901, DisplayName = "SimpleDNS Plus 8.x", EditorControl = "SimpleDNS", GroupId = 7, ProviderName = "SimpleDNS", ProviderType = "FuseCP.Providers.DNS.SimpleDNS8, FuseCP.Providers.DNS.SimpleDNS80" },
			new Provider() { ProviderId = 1902, DisplayName = "Microsoft DNS Server 2016", EditorControl = "MSDNS", GroupId = 7, ProviderName = "MSDNS.2016", ProviderType = "FuseCP.Providers.DNS.MsDNS2016, FuseCP.Providers.DNS.MsDNS2016" },
			new Provider() { ProviderId = 1903, DisplayName = "SimpleDNS Plus 9.x", EditorControl = "SimpleDNS", GroupId = 7, ProviderName = "SimpleDNS", ProviderType = "FuseCP.Providers.DNS.SimpleDNS9, FuseCP.Providers.DNS.SimpleDNS90" },
			new Provider() { ProviderId = 1910, DisplayName = "vsftpd FTP Server 3", EditorControl = "vsftpd", GroupId = 3, ProviderName = "vsftpd", ProviderType = "FuseCP.Providers.FTP.VsFtp3, FuseCP.Providers.FTP.VsFtp" },
			new Provider() { ProviderId = 1911, DisplayName = "Apache Web Server 2.4", EditorControl = "Apache", GroupId = 2, ProviderName = "Apache", ProviderType = "FuseCP.Providers.Web.Apache24, FuseCP.Providers.Web.Apache" }
		});
		#endregion
	}
}
