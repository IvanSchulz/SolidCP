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
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FuseCP.EnterpriseServer.Data.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class v200 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "BindConfigPath", 24 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "BindReloadBatch", 24 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExpireLimit", 24 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "MinimumTTL", 24 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "NameServers", 24 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RecordDefaultTTL", 24 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RecordMinimumTTL", 24 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RefreshInterval", 24 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ResponsiblePerson", 24 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RetryDelay", 24 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ZoneFileNameTemplate", 24 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ZonesFolderPath", 24 });

            migrationBuilder.DeleteData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "2.0.0.228");

            migrationBuilder.DeleteData(
                table: "ResourceGroups",
                keyColumn: "GroupID",
                keyValue: 42);

            migrationBuilder.AlterColumn<string>(
                name: "ServerUrl",
                table: "Servers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCore",
                table: "Servers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OSPlatform",
                table: "Servers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "PasswordIsSHA256",
                table: "Servers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<double>(
                name: "QuotaOrder",
                table: "Quotas",
                type: "float",
                nullable: false,
                defaultValue: 1.0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "DomainTypeID",
                table: "ExchangeOrganizationDomains",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsVIP",
                table: "ExchangeAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSubDomain",
                table: "Domains",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPreviewDomain",
                table: "Domains",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "HostingAllowed",
                table: "Domains",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "TempIds",
                columns: table => new
                {
                    Key = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Scope = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempIds", x => x.Key);
                });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 1,
                column: "StatusIDchangeDate",
                value: new DateTime(2024, 10, 12, 19, 29, 19, 927, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 91,
                column: "DisableAutoDiscovery",
                value: null);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 370,
                columns: new[] { "DisplayName", "ProviderName" },
                values: new object[] { "Proxmox Virtualization (remote)", "Proxmox (remote)" });

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 600,
                column: "DisableAutoDiscovery",
                value: null);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 700,
                column: "DisableAutoDiscovery",
                value: null);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1504,
                column: "ProviderType",
                value: "FuseCP.Providers.RemoteDesktopServices.Windows2022,FuseCP.Providers.RemoteDesktopServices.Windows2022");

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1505,
                column: "ProviderType",
                value: "FuseCP.Providers.RemoteDesktopServices.Windows2025,FuseCP.Providers.RemoteDesktopServices.Windows2025");

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1570,
                column: "DisableAutoDiscovery",
                value: null);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1571,
                column: "DisableAutoDiscovery",
                value: null);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1572,
                column: "DisableAutoDiscovery",
                value: null);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1704,
                column: "DisableAutoDiscovery",
                value: null);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1705,
                column: "DisableAutoDiscovery",
                value: null);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1804,
                column: "DisableAutoDiscovery",
                value: null);

            migrationBuilder.InsertData(
                table: "Providers",
                columns: new[] { "ProviderID", "DisableAutoDiscovery", "DisplayName", "EditorControl", "GroupID", "ProviderName", "ProviderType" },
                values: new object[,]
                {
                    { 305, null, "MySQL Server 8.1", "MySQL", 90, "MySQL", "FuseCP.Providers.Database.MySqlServer81, FuseCP.Providers.Database.MySQL" },
                    { 306, null, "MySQL Server 8.2", "MySQL", 90, "MySQL", "FuseCP.Providers.Database.MySqlServer82, FuseCP.Providers.Database.MySQL" },
                    { 307, null, "MySQL Server 8.3", "MySQL", 90, "MySQL", "FuseCP.Providers.Database.MySqlServer83, FuseCP.Providers.Database.MySQL" },
                    { 308, null, "MySQL Server 8.4", "MySQL", 90, "MySQL", "FuseCP.Providers.Database.MySqlServer84, FuseCP.Providers.Database.MySQL" },
                    { 320, null, "MySQL Server 9.0", "MySQL", 90, "MySQL", "FuseCP.Providers.Database.MySqlServer90, FuseCP.Providers.Database.MySQL" },
                    { 371, false, "Proxmox Virtualization", "Proxmox", 167, "Proxmox", "FuseCP.Providers.Virtualization.ProxmoxvpsLocal, FuseCP.Providers.Virtualization.Proxmoxvps" },
                    { 500, null, "Unix System", "Unix", 1, "UnixSystem", "FuseCP.Providers.OS.Unix, FuseCP.Providers.OS.Unix" },
                    { 1573, null, "MariaDB 10.6", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB106, FuseCP.Providers.Database.MariaDB" },
                    { 1574, null, "MariaDB 10.7", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB107, FuseCP.Providers.Database.MariaDB" },
                    { 1575, null, "MariaDB 10.8", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB108, FuseCP.Providers.Database.MariaDB" },
                    { 1576, null, "MariaDB 10.9", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB109, FuseCP.Providers.Database.MariaDB" },
                    { 1577, null, "MariaDB 10.10", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB1010, FuseCP.Providers.Database.MariaDB" },
                    { 1578, null, "MariaDB 10.11", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB1011, FuseCP.Providers.Database.MariaDB" },
                    { 1579, null, "MariaDB 11.0", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB110, FuseCP.Providers.Database.MariaDB" },
                    { 1580, null, "MariaDB 11.1", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB111, FuseCP.Providers.Database.MariaDB" },
                    { 1581, null, "MariaDB 11.2", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB112, FuseCP.Providers.Database.MariaDB" },
                    { 1582, null, "MariaDB 11.3", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB113, FuseCP.Providers.Database.MariaDB" },
                    { 1583, null, "MariaDB 11.4", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB114, FuseCP.Providers.Database.MariaDB" },
                    { 1584, null, "MariaDB 11.5", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB115, FuseCP.Providers.Database.MariaDB" },
                    { 1585, null, "MariaDB 11.6", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB116, FuseCP.Providers.Database.MariaDB" },
                    { 1586, null, "MariaDB 11.7", "MariaDB", 50, "MariaDB", "FuseCP.Providers.Database.MariaDB117, FuseCP.Providers.Database.MariaDB" },
                    { 1910, null, "vsftpd FTP Server 3", "vsftpd", 3, "vsftpd", "FuseCP.Providers.FTP.VsFtp3, FuseCP.Providers.FTP.VsFtp" },
                    { 1911, null, "Apache Web Server 2.4", "Apache", 2, "Apache", "FuseCP.Providers.Web.Apache24, FuseCP.Providers.Web.Apache" }
                });

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 2,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 3,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 4,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 12,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 13,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 14,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 15,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 18,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 19,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 20,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 24,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 25,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 26,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 27,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 28,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 29,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 30,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 31,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 32,
                column: "QuotaOrder",
                value: 9.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 33,
                column: "QuotaOrder",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 34,
                column: "QuotaOrder",
                value: 11.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 35,
                column: "QuotaOrder",
                value: 12.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 36,
                column: "QuotaOrder",
                value: 13.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 37,
                column: "QuotaOrder",
                value: 14.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 38,
                column: "QuotaOrder",
                value: 15.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 39,
                column: "QuotaOrder",
                value: 16.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 40,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 41,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 42,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 43,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 44,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 45,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 47,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 48,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 49,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 50,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 51,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 52,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 53,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 54,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 55,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 57,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 58,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 59,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 60,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 61,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 62,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 63,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 64,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 65,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 66,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 67,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 68,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 69,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 70,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 71,
                column: "QuotaOrder",
                value: 9.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 72,
                column: "QuotaOrder",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 73,
                column: "QuotaOrder",
                value: 11.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 74,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 75,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 77,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 78,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 79,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 80,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 81,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 83,
                column: "QuotaOrder",
                value: 9.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 84,
                column: "QuotaOrder",
                value: 11.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 85,
                column: "QuotaOrder",
                value: 13.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 86,
                column: "QuotaOrder",
                value: 15.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 87,
                column: "QuotaOrder",
                value: 17.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 88,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 94,
                column: "QuotaOrder",
                value: 17.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 96,
                column: "QuotaOrder",
                value: 18.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 97,
                column: "QuotaOrder",
                value: 20.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 100,
                column: "QuotaOrder",
                value: 19.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 102,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 103,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 104,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 105,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 106,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 107,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 108,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 110,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 111,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 112,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 113,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 114,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 115,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 200,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 203,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 204,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 205,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 206,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 207,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 208,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 209,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 210,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 211,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 212,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 213,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 214,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 215,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 216,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 217,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 218,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 219,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 220,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 221,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 222,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 223,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 224,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 225,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 230,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 300,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 301,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 302,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 303,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 304,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 305,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 306,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 307,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 308,
                column: "QuotaOrder",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 309,
                column: "QuotaOrder",
                value: 11.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 310,
                column: "QuotaOrder",
                value: 13.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 311,
                column: "QuotaOrder",
                value: 14.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 312,
                column: "QuotaOrder",
                value: 9.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 313,
                column: "QuotaOrder",
                value: 15.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 314,
                column: "QuotaOrder",
                value: 16.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 315,
                column: "QuotaOrder",
                value: 17.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 316,
                column: "QuotaOrder",
                value: 18.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 317,
                column: "QuotaOrder",
                value: 19.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 318,
                column: "QuotaOrder",
                value: 12.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 319,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 320,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 321,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 322,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 323,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 324,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 325,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 326,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 327,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 328,
                column: "QuotaOrder",
                value: 9.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 329,
                column: "QuotaOrder",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 330,
                column: "QuotaOrder",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 331,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 332,
                column: "QuotaOrder",
                value: 21.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 333,
                column: "QuotaOrder",
                value: 22.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 334,
                column: "QuotaOrder",
                value: 23.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 344,
                column: "QuotaOrder",
                value: 9.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 345,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 346,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 347,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 348,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 349,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 350,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 351,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 352,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 353,
                column: "QuotaOrder",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 354,
                column: "QuotaOrder",
                value: 11.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 355,
                column: "QuotaOrder",
                value: 13.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 356,
                column: "QuotaOrder",
                value: 14.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 357,
                column: "QuotaOrder",
                value: 9.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 358,
                column: "QuotaOrder",
                value: 15.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 359,
                column: "QuotaOrder",
                value: 16.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 360,
                column: "QuotaOrder",
                value: 17.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 361,
                column: "QuotaOrder",
                value: 18.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 362,
                column: "QuotaOrder",
                value: 19.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 363,
                column: "QuotaOrder",
                value: 12.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 364,
                column: "QuotaOrder",
                value: 19.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 365,
                column: "QuotaOrder",
                value: 20.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 366,
                column: "QuotaOrder",
                value: 21.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 367,
                column: "QuotaOrder",
                value: 22.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 368,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 369,
                column: "QuotaOrder",
                value: 23.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 370,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 371,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 372,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 373,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 374,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 375,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 376,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 377,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 378,
                column: "QuotaOrder",
                value: 9.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 379,
                column: "QuotaOrder",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 380,
                column: "QuotaOrder",
                value: 11.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 400,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 409,
                column: "QuotaOrder",
                value: 13.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 410,
                column: "QuotaOrder",
                value: 12.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 411,
                column: "QuotaOrder",
                value: 13.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 420,
                column: "QuotaOrder",
                value: 24.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 421,
                column: "QuotaOrder",
                value: 25.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 422,
                column: "QuotaOrder",
                value: 26.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 423,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 424,
                column: "QuotaOrder",
                value: 27.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 425,
                column: "QuotaOrder",
                value: 29.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 426,
                column: "QuotaOrder",
                value: 28.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 428,
                column: "QuotaOrder",
                value: 31.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 429,
                column: "QuotaOrder",
                value: 30.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 430,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 431,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 447,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 448,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 450,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 451,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 452,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 453,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 460,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 461,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 462,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 463,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 464,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 465,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 466,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 467,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 468,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 470,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 471,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 472,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 473,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 474,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 475,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 476,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 491,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 495,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 496,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 550,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 551,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 552,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 553,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 554,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 555,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 556,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 557,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 558,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 559,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 560,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 561,
                column: "QuotaOrder",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 562,
                column: "QuotaOrder",
                value: 11.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 563,
                column: "QuotaOrder",
                value: 13.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 564,
                column: "QuotaOrder",
                value: 14.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 565,
                column: "QuotaOrder",
                value: 9.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 566,
                column: "QuotaOrder",
                value: 15.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 567,
                column: "QuotaOrder",
                value: 16.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 568,
                column: "QuotaOrder",
                value: 17.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 569,
                column: "QuotaOrder",
                value: 18.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 570,
                column: "QuotaOrder",
                value: 19.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 571,
                column: "QuotaOrder",
                value: 12.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 572,
                column: "QuotaOrder",
                value: 20.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 573,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 574,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 575,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 576,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 577,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 578,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 579,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 581,
                column: "QuotaOrder",
                value: 12.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 582,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 583,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 584,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 585,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 586,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 587,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 588,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 589,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 590,
                column: "QuotaOrder",
                value: 9.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 591,
                column: "QuotaOrder",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 592,
                column: "QuotaOrder",
                value: 11.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 673,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 674,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 675,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 676,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 677,
                column: "QuotaOrder",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 678,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 679,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 680,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 681,
                column: "QuotaOrder",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 682,
                column: "QuotaOrder",
                value: 11.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 683,
                column: "QuotaOrder",
                value: 13.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 684,
                column: "QuotaOrder",
                value: 14.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 685,
                column: "QuotaOrder",
                value: 9.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 686,
                column: "QuotaOrder",
                value: 15.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 687,
                column: "QuotaOrder",
                value: 16.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 688,
                column: "QuotaOrder",
                value: 17.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 689,
                column: "QuotaOrder",
                value: 18.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 690,
                column: "QuotaOrder",
                value: 19.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 691,
                column: "QuotaOrder",
                value: 12.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 692,
                column: "QuotaOrder",
                value: 20.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 701,
                columns: new[] { "ItemTypeID", "QuotaOrder" },
                values: new object[] { 71, 1.0 });

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 702,
                columns: new[] { "ItemTypeID", "QuotaOrder" },
                values: new object[] { 72, 2.0 });

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 703,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 704,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 705,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 706,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 707,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 711,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 712,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 713,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 714,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 715,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 716,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 717,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 721,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 722,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 723,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 724,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 725,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 726,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 727,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 728,
                column: "QuotaOrder",
                value: 14.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 729,
                column: "QuotaOrder",
                value: 32.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 730,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 731,
                column: "QuotaOrder",
                value: 31.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 732,
                column: "QuotaOrder",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 733,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 734,
                column: "QuotaOrder",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 735,
                column: "QuotaOrder",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 736,
                column: "QuotaOrder",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 737,
                column: "QuotaOrder",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 738,
                column: "QuotaOrder",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 750,
                column: "QuotaOrder",
                value: 22.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 751,
                column: "QuotaOrder",
                value: 23.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 752,
                column: "QuotaOrder",
                value: 24.0);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 753,
                column: "QuotaOrder",
                value: 2.0);

            migrationBuilder.InsertData(
                table: "Quotas",
                columns: new[] { "QuotaID", "GroupID", "HideQuota", "ItemTypeID", "PerOrganization", "QuotaDescription", "QuotaName", "QuotaOrder", "QuotaTypeID", "ServiceQuota" },
                values: new object[,]
                {
                    { 381, 41, null, null, null, "Phone Numbers", "Lync.PhoneNumbers", 12.0, 2, false },
                    { 754, 4, true, null, null, "Allow changes to access controls", "Mail.AllowAccessControls", 9.0, 1, false }
                });

            migrationBuilder.InsertData(
                table: "ResourceGroups",
                columns: new[] { "GroupID", "GroupController", "GroupName", "GroupOrder", "ShowGroup" },
                values: new object[,]
                {
                    { 76, "FuseCP.EnterpriseServer.DatabaseServerController", "MsSQL2025", 10, true },
                    { 91, "FuseCP.EnterpriseServer.DatabaseServerController", "MySQL9", 12, true }
                });

            migrationBuilder.UpdateData(
                table: "Schedule",
                keyColumn: "ScheduleID",
                keyValue: 1,
                columns: new[] { "FromTime", "NextRun", "StartTime", "ToTime" },
                values: new object[] { new DateTime(2000, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2010, 7, 16, 14, 53, 2, 470, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 30, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Schedule",
                keyColumn: "ScheduleID",
                keyValue: 2,
                columns: new[] { "FromTime", "NextRun", "StartTime", "ToTime" },
                values: new object[] { new DateTime(2000, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2010, 7, 16, 14, 53, 2, 477, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "ScheduleTasks",
                columns: new[] { "TaskID", "RoleID", "TaskType" },
                values: new object[] { "SCHEDULE_TASK_CHECK_WEBSITES_SSL", 3, "FuseCP.EnterpriseServer.CheckWebsitesSslTask, FuseCP.EnterpriseServer.Code" });

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "HtmlBody", "AccountSummaryLetter", 1 },
                column: "PropertyValue",
                value: "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Account Summary Information</title>\r\n    <style type=\"text/css\">\r\n		.Summary { background-color: ##ffffff; padding: 5px; }\r\n		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { font-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font-size: 1.3em; color: ##1F4978; }\r\n        .Summary TABLE { border: solid 1px ##e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; font-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n\r\n<a name=\"top\"></a>\r\n<div class=\"Header\">\r\n	Hosting Account Information\r\n</div>\r\n\r\n<ad:if test=\"#Signup#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n\r\n<p>\r\nNew user account has been created and below you can find its summary information.\r\n</p>\r\n\r\n<h1>Control Panel URL</h1>\r\n<table>\r\n    <thead>\r\n        <tr>\r\n            <th>Control Panel URL</th>\r\n            <th>Username</th>\r\n            <th>Password</th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n        <tr>\r\n            <td><a href=\"http://panel.HostingCompany.com\">http://panel.HostingCompany.com</a></td>\r\n            <td>#user.Username#</td>\r\n            <td>#user.Password#</td>\r\n        </tr>\r\n    </tbody>\r\n</table>\r\n</ad:if>\r\n\r\n<h1>Hosting Spaces</h1>\r\n<p>\r\n    The following hosting spaces have been created under your account:\r\n</p>\r\n<ad:foreach collection=\"#Spaces#\" var=\"Space\" index=\"i\">\r\n<h2>#Space.PackageName#</h2>\r\n<table>\r\n	<tbody>\r\n		<tr>\r\n			<td class=\"Label\">Hosting Plan:</td>\r\n			<td>\r\n				<ad:if test=\"#not(isnull(Plans[Space.PlanId]))#\">#Plans[Space.PlanId].PlanName#<ad:else>System</ad:if>\r\n			</td>\r\n		</tr>\r\n		<ad:if test=\"#not(isnull(Plans[Space.PlanId]))#\">\r\n		<tr>\r\n			<td class=\"Label\">Purchase Date:</td>\r\n			<td>\r\n# Space.PurchaseDate#\r\n			</td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Disk Space, MB:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.Diskspace\" /></td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Bandwidth, MB/Month:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.Bandwidth\" /></td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Maximum Number of Domains:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.Domains\" /></td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Maximum Number of Sub-Domains:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.SubDomains\" /></td>\r\n		</tr>\r\n		</ad:if>\r\n	</tbody>\r\n</table>\r\n</ad:foreach>\r\n\r\n<ad:if test=\"#Signup#\">\r\n<p>\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n</p>\r\n\r\n<p>\r\nBest regards,<br />\r\nFuseCP.<br />\r\nWeb Site: <a href=\"https://fusecp.com\">https://fusecp.com</a><br />\r\nE-Mail: <a href=\"mailto:support@fusecp.com\">support@fusecp.com</a>\r\n</p>\r\n</ad:if>\r\n\r\n<ad:template name=\"NumericQuota\">\r\n	<ad:if test=\"#space.Quotas.ContainsKey(quota)#\">\r\n		<ad:if test=\"#space.Quotas[quota].QuotaAllocatedValue isnot -1#\">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if>\r\n	<ad:else>\r\n		0\r\n	</ad:if>\r\n</ad:template>\r\n\r\n</div>\r\n</body>\r\n</html>");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "DomainLookupLetter", 1 },
                column: "PropertyValue",
                value: "=================================\r\n   MX and NS Changes Information\r\n=================================\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nPlease, find below details of MX and NS changes.\r\n\r\n\r\n<ad:foreach collection=\"#Domains#\" var=\"Domain\" index=\"i\">\r\n\r\n# Domain.DomainName# - #DomainUsers[Domain.PackageId].FirstName# #DomainUsers[Domain.PackageId].LastName#\r\n Registrar:      #iif(isnull(Domain.Registrar), \"\", Domain.Registrar)#\r\n ExpirationDate: #iif(isnull(Domain.ExpirationDate), \"\", Domain.ExpirationDate)#\r\n\r\n        <ad:foreach collection=\"#Domain.DnsChanges#\" var=\"DnsChange\" index=\"j\">\r\n            DNS:       #DnsChange.DnsServer#\r\n            Type:      #DnsChange.Type#\r\n	    Status:    #DnsChange.Status#\r\n            Old Value: #DnsChange.OldRecord.Value#\r\n            New Value: #DnsChange.NewRecord.Value#\r\n\r\n    	</ad:foreach>\r\n</ad:foreach>\r\n\r\n\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards\r\n");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "SMSBody", "OrganizationUserPasswordRequestLetter", 1 },
                column: "PropertyValue",
                value: "\r\nUser have been created. Password request url:\r\n# passwordResetLink#");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "OrganizationUserPasswordRequestLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password request notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nYour account have been created. In order to create a password for your account, please follow next link:\r\n\r\n# passwordResetLink#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "UserPasswordExpirationLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password expiration notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nYour password expiration date is #user.PasswordExpirationDateTime#. You can reset your own password by visiting the following page:\r\n\r\n# passwordResetLink#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "PasswordResetLinkSmsBody", "UserPasswordResetLetter", 1 },
                column: "PropertyValue",
                value: "Password reset link:\r\n# passwordResetLink#\r\n");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "UserPasswordResetLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password reset notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nWe received a request to reset the password for your account. If you made this request, click the link below. If you did not make this request, you can ignore this email.\r\n\r\n# passwordResetLink#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "HtmlBody", "UserPasswordResetPincodeLetter", 1 },
                column: "PropertyValue",
                value: "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Password reset notification</title>\r\n    <style type=\"text/css\">\r\n		.Summary { background-color: ##ffffff; padding: 5px; }\r\n		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { font-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font-size: 1.3em; color: ##1F4978; } \r\n        .Summary TABLE { border: solid 1px ##e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; font-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n<div class=\"Header\">\r\n<img src=\"#logoUrl#\">\r\n</div>\r\n<h1>Password reset notification</h1>\r\n\r\n<ad:if test=\"#user#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n</ad:if>\r\n\r\n<p>\r\nWe received a request to reset the password for your account. Your password reset pincode:\r\n</p>\r\n\r\n# passwordResetPincode#\r\n\r\n<p>\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n</p>\r\n\r\n<p>\r\nBest regards\r\n</p>\r\n</div>\r\n</body>");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "PasswordResetPincodeSmsBody", "UserPasswordResetPincodeLetter", 1 },
                column: "PropertyValue",
                value: "\r\nYour password reset pincode:\r\n# passwordResetPincode#");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "UserPasswordResetPincodeLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password reset notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nWe received a request to reset the password for your account. Your password reset pincode:\r\n\r\n# passwordResetPincode#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "VerificationCodeLetter", 1 },
                column: "PropertyValue",
                value: "=================================\r\n   Verification code\r\n=================================\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nto complete the sign in, enter the verification code on the device.\r\n\r\nVerification code\r\n# verificationCode#\r\n\r\nBest regards,\r\n");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "PublishingProfile", "WebPolicy", 1 },
                column: "PropertyValue",
                value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<publishData>\r\n<ad:if test=\"#WebSite.WebDeploySitePublishingEnabled#\">\r\n	<publishProfile\r\n		profileName=\"#WebSite.Name# - Web Deploy\"\r\n		publishMethod=\"MSDeploy\"\r\n		publishUrl=\"#WebSite[\"WmSvcServiceUrl\"]#:#WebSite[\"WmSvcServicePort\"]#\"\r\n		msdeploySite=\"#WebSite.Name#\"\r\n		userName=\"#WebSite.WebDeployPublishingAccount#\"\r\n		userPWD=\"#WebSite.WebDeployPublishingPassword#\"\r\n		destinationAppUrl=\"http://#WebSite.Name#/\"\r\n		<ad:if test=\"#Not(IsNull(MsSqlDatabase)) and Not(IsNull(MsSqlUser))#\">SQLServerDBConnectionString=\"server=#MsSqlServerExternalAddress#;Initial Catalog=#MsSqlDatabase.Name#;uid=#MsSqlUser.Name#;pwd=#MsSqlUser.Password#\"</ad:if>\r\n		<ad:if test=\"#Not(IsNull(MySqlDatabase)) and Not(IsNull(MySqlUser))#\">mySQLDBConnectionString=\"server=#MySqlAddress#;database=#MySqlDatabase.Name#;uid=#MySqlUser.Name#;pwd=#MySqlUser.Password#\"</ad:if>\r\n		<ad:if test=\"#Not(IsNull(MariaDBDatabase)) and Not(IsNull(MariaDBUser))#\">MariaDBDBConnectionString=\"server=#MariaDBAddress#;database=#MariaDBDatabase.Name#;uid=#MariaDBUser.Name#;pwd=#MariaDBUser.Password#\"</ad:if>\r\n		hostingProviderForumLink=\"https://fusecp.com/support\"\r\n		controlPanelLink=\"https://panel.fusecp.com/\"\r\n	/>\r\n</ad:if>\r\n<ad:if test=\"#IsDefined(\"FtpAccount\")#\">\r\n	<publishProfile\r\n		profileName=\"#WebSite.Name# - FTP\"\r\n		publishMethod=\"FTP\"\r\n		publishUrl=\"ftp://#FtpServiceAddress#\"\r\n		ftpPassiveMode=\"True\"\r\n		userName=\"#FtpAccount.Name#\"\r\n		userPWD=\"#FtpAccount.Password#\"\r\n		destinationAppUrl=\"http://#WebSite.Name#/\"\r\n		<ad:if test=\"#Not(IsNull(MsSqlDatabase)) and Not(IsNull(MsSqlUser))#\">SQLServerDBConnectionString=\"server=#MsSqlServerExternalAddress#;Initial Catalog=#MsSqlDatabase.Name#;uid=#MsSqlUser.Name#;pwd=#MsSqlUser.Password#\"</ad:if>\r\n		<ad:if test=\"#Not(IsNull(MySqlDatabase)) and Not(IsNull(MySqlUser))#\">mySQLDBConnectionString=\"server=#MySqlAddress#;database=#MySqlDatabase.Name#;uid=#MySqlUser.Name#;pwd=#MySqlUser.Password#\"</ad:if>\r\n		<ad:if test=\"#Not(IsNull(MariaDBDatabase)) and Not(IsNull(MariaDBUser))#\">MariaDBDBConnectionString=\"server=#MariaDBAddress#;database=#MariaDBDatabase.Name#;uid=#MariaDBUser.Name#;pwd=#MariaDBUser.Password#\"</ad:if>\r\n		hostingProviderForumLink=\"https://fusecp.com/support\"\r\n		controlPanelLink=\"https://panel.fusecp.com/\"\r\n    />\r\n</ad:if>\r\n</publishData>\r\n\r\n<!--\r\nControl Panel:\r\nUsername: #User.Username#\r\nPassword: #User.Password#\r\n\r\nTechnical Contact:\r\nsupport@fusecp.com\r\n-->");

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.0",
                column: "BuildDate",
                value: new DateTime(2010, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.0.1.0",
                column: "BuildDate",
                value: new DateTime(2010, 7, 16, 12, 53, 3, 563, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.0.2.0",
                column: "BuildDate",
                value: new DateTime(2010, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.1.0.9",
                column: "BuildDate",
                value: new DateTime(2010, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.1.2.13",
                column: "BuildDate",
                value: new DateTime(2011, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.2.0.38",
                column: "BuildDate",
                value: new DateTime(2011, 7, 13, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.2.1.6",
                column: "BuildDate",
                value: new DateTime(2012, 3, 29, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.5.1",
                column: "BuildDate",
                value: new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.InsertData(
                table: "Versions",
                columns: new[] { "DatabaseVersion", "BuildDate" },
                values: new object[,]
                {
                    { "1.4.9", new DateTime(2024, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { "2.0.0", new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Providers",
                columns: new[] { "ProviderID", "DisableAutoDiscovery", "DisplayName", "EditorControl", "GroupID", "ProviderName", "ProviderType" },
                values: new object[] { 1707, null, "Microsoft SQL Server 2025", "MSSQL", 76, "MsSQL", "FuseCP.Providers.Database.MsSqlServer2025, FuseCP.Providers.Database.SqlServer" });

            migrationBuilder.InsertData(
                table: "Quotas",
                columns: new[] { "QuotaID", "GroupID", "HideQuota", "ItemTypeID", "PerOrganization", "QuotaDescription", "QuotaName", "QuotaOrder", "QuotaTypeID", "ServiceQuota" },
                values: new object[,]
                {
                    { 120, 91, null, 75, null, "Databases", "MySQL9.Databases", 1.0, 2, false },
                    { 121, 91, null, 76, null, "Users", "MySQL9.Users", 2.0, 2, false },
                    { 122, 91, null, null, null, "Database Backups", "MySQL9.Backup", 4.0, 1, false },
                    { 123, 91, null, null, null, "Max Database Size", "MySQL9.MaxDatabaseSize", 3.0, 3, false },
                    { 124, 91, null, null, null, "Database Restores", "MySQL9.Restore", 5.0, 1, false },
                    { 125, 91, null, null, null, "Database Truncate", "MySQL9.Truncate", 6.0, 1, false },
                    { 760, 76, null, 79, null, "Databases", "MsSQL2025.Databases", 1.0, 2, false },
                    { 761, 76, null, 80, null, "Users", "MsSQL2025.Users", 2.0, 2, false },
                    { 762, 76, null, null, null, "Max Database Size", "MsSQL2025.MaxDatabaseSize", 3.0, 3, false },
                    { 763, 76, null, null, null, "Database Backups", "MsSQL2025.Backup", 5.0, 1, false },
                    { 764, 76, null, null, null, "Database Restores", "MsSQL2025.Restore", 6.0, 1, false },
                    { 765, 76, null, null, null, "Database Truncate", "MsSQL2025.Truncate", 7.0, 1, false },
                    { 766, 76, null, null, null, "Max Log Size", "MsSQL2025.MaxLogSize", 4.0, 3, false }
                });

            migrationBuilder.InsertData(
                table: "ScheduleTaskParameters",
                columns: new[] { "ParameterID", "TaskID", "DataTypeID", "DefaultValue", "ParameterOrder" },
                values: new object[,]
                {
                    { "BCC_MAIL", "SCHEDULE_TASK_CHECK_WEBSITES_SSL", "String", "admin@mydomain.com", 3 },
                    { "ERROR_MAIL_BODY", "SCHEDULE_TASK_CHECK_WEBSITES_SSL", "MultiString", "Hello, <br>we cannot verify the SSL certificate for the domain [domain]. <br><br>Error message: [error] <br><br>Please check if the website is available.", 11 },
                    { "ERROR_MAIL_SUBJECT", "SCHEDULE_TASK_CHECK_WEBSITES_SSL", "String", "Certificate error or website is unavailable", 10 },
                    { "EXPIRATION_MAIL_BODY", "SCHEDULE_TASK_CHECK_WEBSITES_SSL", "MultiString", "Hello, <br>Your certificate for the [domain] will expire in [expires_in_days] days (on [expires_on_date]).", 5 },
                    { "EXPIRATION_MAIL_SUBJECT", "SCHEDULE_TASK_CHECK_WEBSITES_SSL", "String", "Website certificate expiration notice", 4 },
                    { "SEND_14_DAYS_BEFORE_EXPIRATION", "SCHEDULE_TASK_CHECK_WEBSITES_SSL", "Boolean", "true", 7 },
                    { "SEND_30_DAYS_BEFORE_EXPIRATION", "SCHEDULE_TASK_CHECK_WEBSITES_SSL", "Boolean", "true", 6 },
                    { "SEND_BCC", "SCHEDULE_TASK_CHECK_WEBSITES_SSL", "Boolean", "false", 2 },
                    { "SEND_MAIL_TO_CUSTOMER", "SCHEDULE_TASK_CHECK_WEBSITES_SSL", "Boolean", "true", 1 },
                    { "SEND_SSL_ERROR", "SCHEDULE_TASK_CHECK_WEBSITES_SSL", "Boolean", "false", 9 },
                    { "SEND_TODAY_EXPIRED", "SCHEDULE_TASK_CHECK_WEBSITES_SSL", "Boolean", "true", 8 }
                });

            migrationBuilder.InsertData(
                table: "ScheduleTaskViewConfiguration",
                columns: new[] { "ConfigurationID", "TaskID", "Description", "Environment" },
                values: new object[] { "ASP_NET", "SCHEDULE_TASK_CHECK_WEBSITES_SSL", "~/DesktopModules/FuseCP/ScheduleTaskControls/CheckWebsitesSslView.ascx", "ASP.NET" });

            migrationBuilder.InsertData(
                table: "ServiceDefaultProperties",
                columns: new[] { "PropertyName", "ProviderID", "PropertyValue" },
                values: new object[,]
                {
                    { "ExternalAddress", 305, "localhost" },
                    { "InstallFolder", 305, "%PROGRAMFILES%\\MySQL\\MySQL Server 8.0" },
                    { "InternalAddress", 305, "localhost,3306" },
                    { "RootLogin", 305, "root" },
                    { "RootPassword", 305, "" },
                    { "sslmode", 305, "True" },
                    { "ExternalAddress", 306, "localhost" },
                    { "InstallFolder", 306, "%PROGRAMFILES%\\MySQL\\MySQL Server 8.0" },
                    { "InternalAddress", 306, "localhost,3306" },
                    { "RootLogin", 306, "root" },
                    { "RootPassword", 306, "" },
                    { "sslmode", 306, "True" },
                    { "ExternalAddress", 307, "localhost" },
                    { "InstallFolder", 307, "%PROGRAMFILES%\\MySQL\\MySQL Server 8.0" },
                    { "InternalAddress", 307, "localhost,3306" },
                    { "RootLogin", 307, "root" },
                    { "RootPassword", 307, "" },
                    { "sslmode", 307, "True" },
                    { "ExternalAddress", 308, "localhost" },
                    { "InstallFolder", 308, "%PROGRAMFILES%\\MySQL\\MySQL Server 8.0" },
                    { "InternalAddress", 308, "localhost,3306" },
                    { "RootLogin", 308, "root" },
                    { "RootPassword", 308, "" },
                    { "sslmode", 308, "True" },
                    { "ExternalAddress", 320, "localhost" },
                    { "InstallFolder", 320, "%PROGRAMFILES%\\MySQL\\MySQL Server 9.0" },
                    { "InternalAddress", 320, "localhost,3306" },
                    { "RootLogin", 320, "root" },
                    { "RootPassword", 320, "" },
                    { "sslmode", 320, "True" },
                    { "LogDir", 500, "/var/log" },
                    { "UsersHome", 500, "/var/www/HostingSpaces" },
                    { "ExternalAddress", 1573, "localhost" },
                    { "InstallFolder", 1573, "%PROGRAMFILES%\\MariaDB 10.6" },
                    { "InternalAddress", 1573, "localhost" },
                    { "RootLogin", 1573, "root" },
                    { "RootPassword", 1573, "" },
                    { "ExternalAddress", 1574, "localhost" },
                    { "InstallFolder", 1574, "%PROGRAMFILES%\\MariaDB 10.7" },
                    { "InternalAddress", 1574, "localhost" },
                    { "RootLogin", 1574, "root" },
                    { "RootPassword", 1574, "" },
                    { "ExternalAddress", 1575, "localhost" },
                    { "InstallFolder", 1575, "%PROGRAMFILES%\\MariaDB 10.8" },
                    { "InternalAddress", 1575, "localhost" },
                    { "RootLogin", 1575, "root" },
                    { "RootPassword", 1575, "" },
                    { "ExternalAddress", 1576, "localhost" },
                    { "InstallFolder", 1576, "%PROGRAMFILES%\\MariaDB 10.9" },
                    { "InternalAddress", 1576, "localhost" },
                    { "RootLogin", 1576, "root" },
                    { "RootPassword", 1576, "" },
                    { "ExternalAddress", 1577, "localhost" },
                    { "InstallFolder", 1577, "%PROGRAMFILES%\\MariaDB 10.10" },
                    { "InternalAddress", 1577, "localhost" },
                    { "RootLogin", 1577, "root" },
                    { "RootPassword", 1577, "" },
                    { "ExternalAddress", 1578, "localhost" },
                    { "InstallFolder", 1578, "%PROGRAMFILES%\\MariaDB 10.11" },
                    { "InternalAddress", 1578, "localhost" },
                    { "RootLogin", 1578, "root" },
                    { "RootPassword", 1578, "" },
                    { "ExternalAddress", 1579, "localhost" },
                    { "InstallFolder", 1579, "%PROGRAMFILES%\\MariaDB 11.0" },
                    { "InternalAddress", 1579, "localhost" },
                    { "RootLogin", 1579, "root" },
                    { "RootPassword", 1579, "" },
                    { "ExternalAddress", 1580, "localhost" },
                    { "InstallFolder", 1580, "%PROGRAMFILES%\\MariaDB 11.1" },
                    { "InternalAddress", 1580, "localhost" },
                    { "RootLogin", 1580, "root" },
                    { "RootPassword", 1580, "" },
                    { "ExternalAddress", 1581, "localhost" },
                    { "InstallFolder", 1581, "%PROGRAMFILES%\\MariaDB 11.2" },
                    { "InternalAddress", 1581, "localhost" },
                    { "RootLogin", 1581, "root" },
                    { "RootPassword", 1581, "" },
                    { "ExternalAddress", 1582, "localhost" },
                    { "InstallFolder", 1582, "%PROGRAMFILES%\\MariaDB 11.3" },
                    { "InternalAddress", 1582, "localhost" },
                    { "RootLogin", 1582, "root" },
                    { "RootPassword", 1582, "" },
                    { "ExternalAddress", 1583, "localhost" },
                    { "InstallFolder", 1583, "%PROGRAMFILES%\\MariaDB 11.4" },
                    { "InternalAddress", 1583, "localhost" },
                    { "RootLogin", 1583, "root" },
                    { "RootPassword", 1583, "" },
                    { "ExternalAddress", 1584, "localhost" },
                    { "InstallFolder", 1584, "%PROGRAMFILES%\\MariaDB 11.5" },
                    { "InternalAddress", 1584, "localhost" },
                    { "RootLogin", 1584, "root" },
                    { "RootPassword", 1584, "" },
                    { "ExternalAddress", 1585, "localhost" },
                    { "InstallFolder", 1585, "%PROGRAMFILES%\\MariaDB 11.6" },
                    { "InternalAddress", 1585, "localhost" },
                    { "RootLogin", 1585, "root" },
                    { "RootPassword", 1585, "" },
                    { "ExternalAddress", 1586, "localhost" },
                    { "InstallFolder", 1586, "%PROGRAMFILES%\\MariaDB 11.7" },
                    { "InternalAddress", 1586, "localhost" },
                    { "RootLogin", 1586, "root" },
                    { "RootPassword", 1586, "" },
                    { "ConfigFile", 1910, "/etc/vsftpd.conf" },
                    { "ConfigFile", 1911, "/etc/apache2/apache2.conf" },
                    { "ConfigPath", 1911, "/etc/apache2" }
                });

            migrationBuilder.InsertData(
                table: "ServiceItemTypes",
                columns: new[] { "ItemTypeID", "Backupable", "CalculateBandwidth", "CalculateDiskspace", "DisplayName", "Disposable", "GroupID", "Importable", "Searchable", "Suspendable", "TypeName", "TypeOrder" },
                values: new object[,]
                {
                    { 90, true, false, true, "MySQL9Database", true, 91, true, true, false, "FuseCP.Providers.Database.SqlDatabase, FuseCP.Providers.Base", 20 },
                    { 91, true, false, false, "MySQL9User", true, 91, true, true, false, "FuseCP.Providers.Database.SqlUser, FuseCP.Providers.Base", 21 },
                    { 205, true, false, true, "MsSQL2025Database", true, 76, true, true, false, "FuseCP.Providers.Database.SqlDatabase, FuseCP.Providers.Base", 1 },
                    { 206, true, false, false, "MsSQL2025User", true, 76, true, true, false, "FuseCP.Providers.Database.SqlUser, FuseCP.Providers.Base", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TempIds_Created_Scope_Level",
                table: "TempIds",
                columns: new[] { "Created", "Scope", "Level" });

            StoredProceduresUp(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            StoredProceduresDown(migrationBuilder);

            migrationBuilder.DropTable(
                name: "TempIds");

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 371);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1707);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 381);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 754);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 760);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 761);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 762);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 763);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 764);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 765);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 766);

            migrationBuilder.DeleteData(
                table: "ScheduleTaskParameters",
                keyColumns: new[] { "ParameterID", "TaskID" },
                keyValues: new object[] { "BCC_MAIL", "SCHEDULE_TASK_CHECK_WEBSITES_SSL" });

            migrationBuilder.DeleteData(
                table: "ScheduleTaskParameters",
                keyColumns: new[] { "ParameterID", "TaskID" },
                keyValues: new object[] { "ERROR_MAIL_BODY", "SCHEDULE_TASK_CHECK_WEBSITES_SSL" });

            migrationBuilder.DeleteData(
                table: "ScheduleTaskParameters",
                keyColumns: new[] { "ParameterID", "TaskID" },
                keyValues: new object[] { "ERROR_MAIL_SUBJECT", "SCHEDULE_TASK_CHECK_WEBSITES_SSL" });

            migrationBuilder.DeleteData(
                table: "ScheduleTaskParameters",
                keyColumns: new[] { "ParameterID", "TaskID" },
                keyValues: new object[] { "EXPIRATION_MAIL_BODY", "SCHEDULE_TASK_CHECK_WEBSITES_SSL" });

            migrationBuilder.DeleteData(
                table: "ScheduleTaskParameters",
                keyColumns: new[] { "ParameterID", "TaskID" },
                keyValues: new object[] { "EXPIRATION_MAIL_SUBJECT", "SCHEDULE_TASK_CHECK_WEBSITES_SSL" });

            migrationBuilder.DeleteData(
                table: "ScheduleTaskParameters",
                keyColumns: new[] { "ParameterID", "TaskID" },
                keyValues: new object[] { "SEND_14_DAYS_BEFORE_EXPIRATION", "SCHEDULE_TASK_CHECK_WEBSITES_SSL" });

            migrationBuilder.DeleteData(
                table: "ScheduleTaskParameters",
                keyColumns: new[] { "ParameterID", "TaskID" },
                keyValues: new object[] { "SEND_30_DAYS_BEFORE_EXPIRATION", "SCHEDULE_TASK_CHECK_WEBSITES_SSL" });

            migrationBuilder.DeleteData(
                table: "ScheduleTaskParameters",
                keyColumns: new[] { "ParameterID", "TaskID" },
                keyValues: new object[] { "SEND_BCC", "SCHEDULE_TASK_CHECK_WEBSITES_SSL" });

            migrationBuilder.DeleteData(
                table: "ScheduleTaskParameters",
                keyColumns: new[] { "ParameterID", "TaskID" },
                keyValues: new object[] { "SEND_MAIL_TO_CUSTOMER", "SCHEDULE_TASK_CHECK_WEBSITES_SSL" });

            migrationBuilder.DeleteData(
                table: "ScheduleTaskParameters",
                keyColumns: new[] { "ParameterID", "TaskID" },
                keyValues: new object[] { "SEND_SSL_ERROR", "SCHEDULE_TASK_CHECK_WEBSITES_SSL" });

            migrationBuilder.DeleteData(
                table: "ScheduleTaskParameters",
                keyColumns: new[] { "ParameterID", "TaskID" },
                keyValues: new object[] { "SEND_TODAY_EXPIRED", "SCHEDULE_TASK_CHECK_WEBSITES_SSL" });

            migrationBuilder.DeleteData(
                table: "ScheduleTaskViewConfiguration",
                keyColumns: new[] { "ConfigurationID", "TaskID" },
                keyValues: new object[] { "ASP_NET", "SCHEDULE_TASK_CHECK_WEBSITES_SSL" });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 305 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 305 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 305 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 305 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 305 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "sslmode", 305 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 306 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 306 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 306 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 306 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 306 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "sslmode", 306 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 307 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 307 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 307 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 307 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 307 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "sslmode", 307 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 308 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 308 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 308 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 308 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 308 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "sslmode", 308 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 320 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 320 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 320 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 320 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 320 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "sslmode", 320 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "LogDir", 500 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "UsersHome", 500 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1573 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1573 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1573 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1573 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1573 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1574 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1574 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1574 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1574 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1574 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1575 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1575 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1575 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1575 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1575 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1576 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1576 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1576 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1576 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1576 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1577 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1577 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1577 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1577 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1577 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1578 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1578 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1578 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1578 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1578 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1579 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1579 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1579 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1579 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1579 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1580 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1580 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1580 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1580 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1580 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1581 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1581 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1581 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1581 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1581 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1582 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1582 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1582 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1582 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1582 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1583 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1583 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1583 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1583 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1583 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1584 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1584 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1584 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1584 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1584 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1585 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1585 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1585 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1585 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1585 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1586 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1586 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1586 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1586 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1586 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ConfigFile", 1910 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ConfigFile", 1911 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ConfigPath", 1911 });

            migrationBuilder.DeleteData(
                table: "ServiceItemTypes",
                keyColumn: "ItemTypeID",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "ServiceItemTypes",
                keyColumn: "ItemTypeID",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "ServiceItemTypes",
                keyColumn: "ItemTypeID",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "ServiceItemTypes",
                keyColumn: "ItemTypeID",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.4.9");

            migrationBuilder.DeleteData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "2.0.0");

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 320);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 500);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1573);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1574);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1575);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1576);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1577);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1578);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1579);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1580);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1581);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1582);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1583);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1584);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1585);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1586);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1910);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1911);

            migrationBuilder.DeleteData(
                table: "ResourceGroups",
                keyColumn: "GroupID",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "ResourceGroups",
                keyColumn: "GroupID",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "ScheduleTasks",
                keyColumn: "TaskID",
                keyValue: "SCHEDULE_TASK_CHECK_WEBSITES_SSL");

            migrationBuilder.DropColumn(
                name: "IsCore",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "OSPlatform",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "PasswordIsSHA256",
                table: "Servers");

            migrationBuilder.AlterColumn<string>(
                name: "ServerUrl",
                table: "Servers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "QuotaOrder",
                table: "Quotas",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 1.0);

            migrationBuilder.AlterColumn<int>(
                name: "DomainTypeID",
                table: "ExchangeOrganizationDomains",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsVIP",
                table: "ExchangeAccounts",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsSubDomain",
                table: "Domains",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPreviewDomain",
                table: "Domains",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "HostingAllowed",
                table: "Domains",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 1,
                column: "StatusIDchangeDate",
                value: new DateTime(2024, 12, 17, 12, 54, 59, 933, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 91,
                column: "DisableAutoDiscovery",
                value: true);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 370,
                columns: new[] { "DisplayName", "ProviderName" },
                values: new object[] { "Proxmox Virtualization", "Proxmox" });

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 600,
                column: "DisableAutoDiscovery",
                value: true);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 700,
                column: "DisableAutoDiscovery",
                value: true);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1504,
                column: "ProviderType",
                value: "FuseCP.Providers.RemoteDesktopServices.Windows2019,FuseCP.Providers.RemoteDesktopServices.Windows2019");

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1505,
                column: "ProviderType",
                value: "FuseCP.Providers.RemoteDesktopServices.Windows2025,FuseCP.Providers.RemoteDesktopServices.Windows2019");

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1570,
                column: "DisableAutoDiscovery",
                value: true);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1571,
                column: "DisableAutoDiscovery",
                value: true);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1572,
                column: "DisableAutoDiscovery",
                value: true);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1704,
                column: "DisableAutoDiscovery",
                value: true);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1705,
                column: "DisableAutoDiscovery",
                value: true);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1804,
                column: "DisableAutoDiscovery",
                value: true);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 2,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 3,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 4,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 12,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 13,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 14,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 15,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 18,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 19,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 20,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 24,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 25,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 26,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 27,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 28,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 29,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 30,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 31,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 32,
                column: "QuotaOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 33,
                column: "QuotaOrder",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 34,
                column: "QuotaOrder",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 35,
                column: "QuotaOrder",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 36,
                column: "QuotaOrder",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 37,
                column: "QuotaOrder",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 38,
                column: "QuotaOrder",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 39,
                column: "QuotaOrder",
                value: 16);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 40,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 41,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 42,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 43,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 44,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 45,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 47,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 48,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 49,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 50,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 51,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 52,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 53,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 54,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 55,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 57,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 58,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 59,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 60,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 61,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 62,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 63,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 64,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 65,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 66,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 67,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 68,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 69,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 70,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 71,
                column: "QuotaOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 72,
                column: "QuotaOrder",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 73,
                column: "QuotaOrder",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 74,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 75,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 77,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 78,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 79,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 80,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 81,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 83,
                column: "QuotaOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 84,
                column: "QuotaOrder",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 85,
                column: "QuotaOrder",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 86,
                column: "QuotaOrder",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 87,
                column: "QuotaOrder",
                value: 17);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 88,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 94,
                column: "QuotaOrder",
                value: 17);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 96,
                column: "QuotaOrder",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 97,
                column: "QuotaOrder",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 100,
                column: "QuotaOrder",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 102,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 103,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 104,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 105,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 106,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 107,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 108,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 110,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 111,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 112,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 113,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 114,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 115,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 200,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 203,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 204,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 205,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 206,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 207,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 208,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 209,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 210,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 211,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 212,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 213,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 214,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 215,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 216,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 217,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 218,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 219,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 220,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 221,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 222,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 223,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 224,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 225,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 230,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 300,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 301,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 302,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 303,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 304,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 305,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 306,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 307,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 308,
                column: "QuotaOrder",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 309,
                column: "QuotaOrder",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 310,
                column: "QuotaOrder",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 311,
                column: "QuotaOrder",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 312,
                column: "QuotaOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 313,
                column: "QuotaOrder",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 314,
                column: "QuotaOrder",
                value: 16);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 315,
                column: "QuotaOrder",
                value: 17);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 316,
                column: "QuotaOrder",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 317,
                column: "QuotaOrder",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 318,
                column: "QuotaOrder",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 319,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 320,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 321,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 322,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 323,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 324,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 325,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 326,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 327,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 328,
                column: "QuotaOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 329,
                column: "QuotaOrder",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 330,
                column: "QuotaOrder",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 331,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 332,
                column: "QuotaOrder",
                value: 21);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 333,
                column: "QuotaOrder",
                value: 22);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 334,
                column: "QuotaOrder",
                value: 23);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 344,
                column: "QuotaOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 345,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 346,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 347,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 348,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 349,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 350,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 351,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 352,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 353,
                column: "QuotaOrder",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 354,
                column: "QuotaOrder",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 355,
                column: "QuotaOrder",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 356,
                column: "QuotaOrder",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 357,
                column: "QuotaOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 358,
                column: "QuotaOrder",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 359,
                column: "QuotaOrder",
                value: 16);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 360,
                column: "QuotaOrder",
                value: 17);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 361,
                column: "QuotaOrder",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 362,
                column: "QuotaOrder",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 363,
                column: "QuotaOrder",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 364,
                column: "QuotaOrder",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 365,
                column: "QuotaOrder",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 366,
                column: "QuotaOrder",
                value: 21);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 367,
                column: "QuotaOrder",
                value: 22);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 368,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 369,
                column: "QuotaOrder",
                value: 23);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 370,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 371,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 372,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 373,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 374,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 375,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 376,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 377,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 378,
                column: "QuotaOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 379,
                column: "QuotaOrder",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 380,
                column: "QuotaOrder",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 400,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 409,
                column: "QuotaOrder",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 410,
                column: "QuotaOrder",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 411,
                column: "QuotaOrder",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 420,
                column: "QuotaOrder",
                value: 24);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 421,
                column: "QuotaOrder",
                value: 25);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 422,
                column: "QuotaOrder",
                value: 26);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 423,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 424,
                column: "QuotaOrder",
                value: 27);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 425,
                column: "QuotaOrder",
                value: 29);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 426,
                column: "QuotaOrder",
                value: 28);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 428,
                column: "QuotaOrder",
                value: 31);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 429,
                column: "QuotaOrder",
                value: 30);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 430,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 431,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 447,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 448,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 450,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 451,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 452,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 453,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 460,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 461,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 462,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 463,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 464,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 465,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 466,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 467,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 468,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 470,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 471,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 472,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 473,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 474,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 475,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 476,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 491,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 495,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 496,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 550,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 551,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 552,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 553,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 554,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 555,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 556,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 557,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 558,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 559,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 560,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 561,
                column: "QuotaOrder",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 562,
                column: "QuotaOrder",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 563,
                column: "QuotaOrder",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 564,
                column: "QuotaOrder",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 565,
                column: "QuotaOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 566,
                column: "QuotaOrder",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 567,
                column: "QuotaOrder",
                value: 16);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 568,
                column: "QuotaOrder",
                value: 17);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 569,
                column: "QuotaOrder",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 570,
                column: "QuotaOrder",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 571,
                column: "QuotaOrder",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 572,
                column: "QuotaOrder",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 573,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 574,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 575,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 576,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 577,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 578,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 579,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 581,
                column: "QuotaOrder",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 582,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 583,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 584,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 585,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 586,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 587,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 588,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 589,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 590,
                column: "QuotaOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 591,
                column: "QuotaOrder",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 592,
                column: "QuotaOrder",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 673,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 674,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 675,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 676,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 677,
                column: "QuotaOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 678,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 679,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 680,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 681,
                column: "QuotaOrder",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 682,
                column: "QuotaOrder",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 683,
                column: "QuotaOrder",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 684,
                column: "QuotaOrder",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 685,
                column: "QuotaOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 686,
                column: "QuotaOrder",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 687,
                column: "QuotaOrder",
                value: 16);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 688,
                column: "QuotaOrder",
                value: 17);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 689,
                column: "QuotaOrder",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 690,
                column: "QuotaOrder",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 691,
                column: "QuotaOrder",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 692,
                column: "QuotaOrder",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 701,
                columns: new[] { "ItemTypeID", "QuotaOrder" },
                values: new object[] { 39, 1 });

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 702,
                columns: new[] { "ItemTypeID", "QuotaOrder" },
                values: new object[] { 40, 2 });

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 703,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 704,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 705,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 706,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 707,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 711,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 712,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 713,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 714,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 715,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 716,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 717,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 721,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 722,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 723,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 724,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 725,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 726,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 727,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 728,
                column: "QuotaOrder",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 729,
                column: "QuotaOrder",
                value: 32);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 730,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 731,
                column: "QuotaOrder",
                value: 31);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 732,
                column: "QuotaOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 733,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 734,
                column: "QuotaOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 735,
                column: "QuotaOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 736,
                column: "QuotaOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 737,
                column: "QuotaOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 738,
                column: "QuotaOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 750,
                column: "QuotaOrder",
                value: 22);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 751,
                column: "QuotaOrder",
                value: 23);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 752,
                column: "QuotaOrder",
                value: 24);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 753,
                column: "QuotaOrder",
                value: 2);

            migrationBuilder.InsertData(
                table: "Quotas",
                columns: new[] { "QuotaID", "GroupID", "HideQuota", "ItemTypeID", "PerOrganization", "QuotaDescription", "QuotaName", "QuotaOrder", "QuotaTypeID", "ServiceQuota" },
                values: new object[] { 95, 2, null, null, null, "Web Application Gallery", "Web.WebAppGallery", 1, 1, false });

            migrationBuilder.InsertData(
                table: "ResourceGroups",
                columns: new[] { "GroupID", "GroupController", "GroupName", "GroupOrder", "ShowGroup" },
                values: new object[] { 42, "FuseCP.EnterpriseServer.HeliconZooController", "HeliconZoo", 2, true });

            migrationBuilder.UpdateData(
                table: "Schedule",
                keyColumn: "ScheduleID",
                keyValue: 1,
                columns: new[] { "FromTime", "NextRun", "StartTime", "ToTime" },
                values: new object[] { new DateTime(2000, 1, 1, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2010, 7, 16, 12, 53, 2, 470, DateTimeKind.Utc), new DateTime(2000, 1, 1, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 1, 11, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Schedule",
                keyColumn: "ScheduleID",
                keyValue: 2,
                columns: new[] { "FromTime", "NextRun", "StartTime", "ToTime" },
                values: new object[] { new DateTime(2000, 1, 1, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2010, 7, 16, 12, 53, 2, 477, DateTimeKind.Utc), new DateTime(2000, 1, 1, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 1, 11, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "ServiceDefaultProperties",
                columns: new[] { "PropertyName", "ProviderID", "PropertyValue" },
                values: new object[,]
                {
                    { "BindConfigPath", 24, "c:\\BIND\\dns\\etc\\named.conf" },
                    { "BindReloadBatch", 24, "c:\\BIND\\dns\\reload.bat" },
                    { "ExpireLimit", 24, "1209600" },
                    { "MinimumTTL", 24, "86400" },
                    { "NameServers", 24, "ns1.yourdomain.com;ns2.yourdomain.com" },
                    { "RecordDefaultTTL", 24, "86400" },
                    { "RecordMinimumTTL", 24, "3600" },
                    { "RefreshInterval", 24, "3600" },
                    { "ResponsiblePerson", 24, "hostmaster.[DOMAIN_NAME]" },
                    { "RetryDelay", 24, "600" },
                    { "ZoneFileNameTemplate", 24, "db.[domain_name].txt" },
                    { "ZonesFolderPath", 24, "c:\\BIND\\dns\\zones" }
                });

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "HtmlBody", "AccountSummaryLetter", 1 },
                column: "PropertyValue",
                value: "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Account Summary Information</title>\r\n    <style type=\"text/css\">\r\n		.Summary { background-color: ##ffffff; padding: 5px; }\r\n		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { font-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font-size: 1.3em; color: ##1F4978; }\r\n        .Summary TABLE { border: solid 1px ##e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; font-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n\r\n<a name=\"top\"></a>\r\n<div class=\"Header\">\r\n	Hosting Account Information\r\n</div>\r\n\r\n<ad:if test=\"#Signup#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n\r\n<p>\r\nNew user account has been created and below you can find its summary information.\r\n</p>\r\n\r\n<h1>Control Panel URL</h1>\r\n<table>\r\n    <thead>\r\n        <tr>\r\n            <th>Control Panel URL</th>\r\n            <th>Username</th>\r\n            <th>Password</th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n        <tr>\r\n            <td><a href=\"http://panel.HostingCompany.com\">http://panel.HostingCompany.com</a></td>\r\n            <td>#user.Username#</td>\r\n            <td>#user.Password#</td>\r\n        </tr>\r\n    </tbody>\r\n</table>\r\n</ad:if>\r\n\r\n<h1>Hosting Spaces</h1>\r\n<p>\r\n    The following hosting spaces have been created under your account:\r\n</p>\r\n<ad:foreach collection=\"#Spaces#\" var=\"Space\" index=\"i\">\r\n<h2>#Space.PackageName#</h2>\r\n<table>\r\n	<tbody>\r\n		<tr>\r\n			<td class=\"Label\">Hosting Plan:</td>\r\n			<td>\r\n				<ad:if test=\"#not(isnull(Plans[Space.PlanId]))#\">#Plans[Space.PlanId].PlanName#<ad:else>System</ad:if>\r\n			</td>\r\n		</tr>\r\n		<ad:if test=\"#not(isnull(Plans[Space.PlanId]))#\">\r\n		<tr>\r\n			<td class=\"Label\">Purchase Date:</td>\r\n			<td>\r\n				#Space.PurchaseDate#\r\n			</td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Disk Space, MB:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.Diskspace\" /></td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Bandwidth, MB/Month:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.Bandwidth\" /></td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Maximum Number of Domains:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.Domains\" /></td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Maximum Number of Sub-Domains:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.SubDomains\" /></td>\r\n		</tr>\r\n		</ad:if>\r\n	</tbody>\r\n</table>\r\n</ad:foreach>\r\n\r\n<ad:if test=\"#Signup#\">\r\n<p>\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n</p>\r\n\r\n<p>\r\nBest regards,<br />\r\nFuseCP.<br />\r\nWeb Site: <a href=\"https://fusecp.com\">https://fusecp.com</a><br />\r\nE-Mail: <a href=\"mailto:support@fusecp.com\">support@fusecp.com</a>\r\n</p>\r\n</ad:if>\r\n\r\n<ad:template name=\"NumericQuota\">\r\n	<ad:if test=\"#space.Quotas.ContainsKey(quota)#\">\r\n		<ad:if test=\"#space.Quotas[quota].QuotaAllocatedValue isnot -1#\">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if>\r\n	<ad:else>\r\n		0\r\n	</ad:if>\r\n</ad:template>\r\n\r\n</div>\r\n</body>\r\n</html>");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "DomainLookupLetter", 1 },
                column: "PropertyValue",
                value: "=================================\r\n   MX and NS Changes Information\r\n=================================\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nPlease, find below details of MX and NS changes.\r\n\r\n\r\n<ad:foreach collection=\"#Domains#\" var=\"Domain\" index=\"i\">\r\n\r\n #Domain.DomainName# - #DomainUsers[Domain.PackageId].FirstName# #DomainUsers[Domain.PackageId].LastName#\r\n Registrar:      #iif(isnull(Domain.Registrar), \"\", Domain.Registrar)#\r\n ExpirationDate: #iif(isnull(Domain.ExpirationDate), \"\", Domain.ExpirationDate)#\r\n\r\n        <ad:foreach collection=\"#Domain.DnsChanges#\" var=\"DnsChange\" index=\"j\">\r\n            DNS:       #DnsChange.DnsServer#\r\n            Type:      #DnsChange.Type#\r\n	    Status:    #DnsChange.Status#\r\n            Old Value: #DnsChange.OldRecord.Value#\r\n            New Value: #DnsChange.NewRecord.Value#\r\n\r\n    	</ad:foreach>\r\n</ad:foreach>\r\n\r\n\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards\r\n");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "SMSBody", "OrganizationUserPasswordRequestLetter", 1 },
                column: "PropertyValue",
                value: "\r\nUser have been created. Password request url:\r\n#passwordResetLink#");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "OrganizationUserPasswordRequestLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password request notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nYour account have been created. In order to create a password for your account, please follow next link:\r\n\r\n#passwordResetLink#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "UserPasswordExpirationLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password expiration notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nYour password expiration date is #user.PasswordExpirationDateTime#. You can reset your own password by visiting the following page:\r\n\r\n#passwordResetLink#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "PasswordResetLinkSmsBody", "UserPasswordResetLetter", 1 },
                column: "PropertyValue",
                value: "Password reset link:\r\n#passwordResetLink#\r\n");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "UserPasswordResetLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password reset notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nWe received a request to reset the password for your account. If you made this request, click the link below. If you did not make this request, you can ignore this email.\r\n\r\n#passwordResetLink#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "HtmlBody", "UserPasswordResetPincodeLetter", 1 },
                column: "PropertyValue",
                value: "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Password reset notification</title>\r\n    <style type=\"text/css\">\r\n		.Summary { background-color: ##ffffff; padding: 5px; }\r\n		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { font-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font-size: 1.3em; color: ##1F4978; } \r\n        .Summary TABLE { border: solid 1px ##e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; font-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n<div class=\"Header\">\r\n<img src=\"#logoUrl#\">\r\n</div>\r\n<h1>Password reset notification</h1>\r\n\r\n<ad:if test=\"#user#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n</ad:if>\r\n\r\n<p>\r\nWe received a request to reset the password for your account. Your password reset pincode:\r\n</p>\r\n\r\n#passwordResetPincode#\r\n\r\n<p>\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n</p>\r\n\r\n<p>\r\nBest regards\r\n</p>\r\n</div>\r\n</body>");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "PasswordResetPincodeSmsBody", "UserPasswordResetPincodeLetter", 1 },
                column: "PropertyValue",
                value: "\r\nYour password reset pincode:\r\n#passwordResetPincode#");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "UserPasswordResetPincodeLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password reset notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nWe received a request to reset the password for your account. Your password reset pincode:\r\n\r\n#passwordResetPincode#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "VerificationCodeLetter", 1 },
                column: "PropertyValue",
                value: "=================================\r\n   Verification code\r\n=================================\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nto complete the sign in, enter the verification code on the device.\r\n\r\nVerification code\r\n#verificationCode#\r\n\r\nBest regards,\r\n");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "PublishingProfile", "WebPolicy", 1 },
                column: "PropertyValue",
                value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<publishData>\r\n<ad:if test=\"#WebSite.WebDeploySitePublishingEnabled#\">\r\n	<publishProfile\r\n		profileName=\"#WebSite.Name# - Web Deploy\"\r\n		publishMethod=\"MSDeploy\"\r\n		publishUrl=\"#WebSite[\"WmSvcServiceUrl\"]#:#WebSite[\"WmSvcServicePort\"]#\"\r\n		msdeploySite=\"#WebSite.Name#\"\r\n		userName=\"#WebSite.WebDeployPublishingAccount#\"\r\n		userPWD=\"#WebSite.WebDeployPublishingPassword#\"\r\n		destinationAppUrl=\"http://#WebSite.Name#/\"\r\n		<ad:if test=\"#Not(IsNull(MsSqlDatabase)) and Not(IsNull(MsSqlUser))#\">SQLServerDBConnectionString=\"server=#MsSqlServerExternalAddress#;database=#MsSqlDatabase.Name#;uid=#MsSqlUser.Name#;pwd=#MsSqlUser.Password#\"</ad:if>\r\n		<ad:if test=\"#Not(IsNull(MySqlDatabase)) and Not(IsNull(MySqlUser))#\">mySQLDBConnectionString=\"server=#MySqlAddress#;database=#MySqlDatabase.Name#;uid=#MySqlUser.Name#;pwd=#MySqlUser.Password#\"</ad:if>\r\n		<ad:if test=\"#Not(IsNull(MariaDBDatabase)) and Not(IsNull(MariaDBUser))#\">MariaDBDBConnectionString=\"server=#MariaDBAddress#;database=#MariaDBDatabase.Name#;uid=#MariaDBUser.Name#;pwd=#MariaDBUser.Password#\"</ad:if>\r\n		hostingProviderForumLink=\"https://fusecp.com/support\"\r\n		controlPanelLink=\"https://panel.fusecp.com/\"\r\n	/>\r\n</ad:if>\r\n<ad:if test=\"#IsDefined(\"FtpAccount\")#\">\r\n	<publishProfile\r\n		profileName=\"#WebSite.Name# - FTP\"\r\n		publishMethod=\"FTP\"\r\n		publishUrl=\"ftp://#FtpServiceAddress#\"\r\n		ftpPassiveMode=\"True\"\r\n		userName=\"#FtpAccount.Name#\"\r\n		userPWD=\"#FtpAccount.Password#\"\r\n		destinationAppUrl=\"http://#WebSite.Name#/\"\r\n		<ad:if test=\"#Not(IsNull(MsSqlDatabase)) and Not(IsNull(MsSqlUser))#\">SQLServerDBConnectionString=\"server=#MsSqlServerExternalAddress#;database=#MsSqlDatabase.Name#;uid=#MsSqlUser.Name#;pwd=#MsSqlUser.Password#\"</ad:if>\r\n		<ad:if test=\"#Not(IsNull(MySqlDatabase)) and Not(IsNull(MySqlUser))#\">mySQLDBConnectionString=\"server=#MySqlAddress#;database=#MySqlDatabase.Name#;uid=#MySqlUser.Name#;pwd=#MySqlUser.Password#\"</ad:if>\r\n		<ad:if test=\"#Not(IsNull(MariaDBDatabase)) and Not(IsNull(MariaDBUser))#\">MariaDBDBConnectionString=\"server=#MariaDBAddress#;database=#MariaDBDatabase.Name#;uid=#MariaDBUser.Name#;pwd=#MariaDBUser.Password#\"</ad:if>\r\n		hostingProviderForumLink=\"https://fusecp.com/support\"\r\n		controlPanelLink=\"https://panel.fusecp.com/\"\r\n    />\r\n</ad:if>\r\n</publishData>\r\n\r\n<!--\r\nControl Panel:\r\nUsername: #User.Username#\r\nPassword: #User.Password#\r\n\r\nTechnical Contact:\r\nsupport@fusecp.com\r\n-->");

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.0",
                column: "BuildDate",
                value: new DateTime(2010, 4, 9, 22, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.0.1.0",
                column: "BuildDate",
                value: new DateTime(2010, 7, 16, 10, 53, 3, 563, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.0.2.0",
                column: "BuildDate",
                value: new DateTime(2010, 9, 2, 22, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.1.0.9",
                column: "BuildDate",
                value: new DateTime(2010, 11, 15, 23, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.1.2.13",
                column: "BuildDate",
                value: new DateTime(2011, 4, 14, 22, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.2.0.38",
                column: "BuildDate",
                value: new DateTime(2011, 7, 12, 22, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.2.1.6",
                column: "BuildDate",
                value: new DateTime(2012, 3, 28, 22, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "1.5.1",
                column: "BuildDate",
                value: new DateTime(2024, 12, 16, 23, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.InsertData(
                table: "Versions",
                columns: new[] { "DatabaseVersion", "BuildDate" },
                values: new object[] { "2.0.0.228", new DateTime(2012, 12, 6, 23, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Providers",
                columns: new[] { "ProviderID", "DisableAutoDiscovery", "DisplayName", "EditorControl", "GroupID", "ProviderName", "ProviderType" },
                values: new object[] { 135, true, "Web Application Engines", "HeliconZoo", 42, "HeliconZoo", "FuseCP.Providers.Web.HeliconZoo.HeliconZoo, FuseCP.Providers.Web.HeliconZoo" });
        }
    }
}
