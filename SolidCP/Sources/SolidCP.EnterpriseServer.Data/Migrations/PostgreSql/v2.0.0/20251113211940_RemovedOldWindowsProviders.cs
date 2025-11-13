using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SolidCP.EnterpriseServer.Data.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class RemovedOldWindowsProviders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 770);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 771);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "UsersHome", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "UsersHome", 100 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "UsersHome", 104 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 100);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 104);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "public",
                table: "Providers",
                columns: new[] { "ProviderID", "DisableAutoDiscovery", "DisplayName", "EditorControl", "GroupID", "ProviderName", "ProviderType" },
                values: new object[,]
                {
                    { 1, null, "Windows Server 2003", "Windows2003", 1, "Windows2003", "SolidCP.Providers.OS.Windows2003, SolidCP.Providers.OS.Windows2003" },
                    { 100, null, "Windows Server 2008", "Windows2008", 1, "Windows2008", "SolidCP.Providers.OS.Windows2008, SolidCP.Providers.OS.Windows2008" },
                    { 104, null, "Windows Server 2012", "Windows2012", 1, "Windows2012", "SolidCP.Providers.OS.Windows2012, SolidCP.Providers.OS.Windows2012" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Quotas",
                columns: new[] { "QuotaID", "GroupID", "HideQuota", "ItemTypeID", "PerOrganization", "QuotaDescription", "QuotaName", "QuotaOrder", "QuotaTypeID", "ServiceQuota" },
                values: new object[,]
                {
                    { 770, 4, null, 11, null, "Mail Domains", "Mail.Domains", 1.1000000000000001, 2, true },
                    { 771, 4, null, null, null, "Mail Accounts per Domain", "Mail.Accounts.per.Domain", 1.2, 2, true }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "ServiceDefaultProperties",
                columns: new[] { "PropertyName", "ProviderID", "PropertyValue" },
                values: new object[,]
                {
                    { "UsersHome", 1, "%SYSTEMDRIVE%\\HostingSpaces" },
                    { "UsersHome", 100, "%SYSTEMDRIVE%\\HostingSpaces" },
                    { "UsersHome", 104, "%SYSTEMDRIVE%\\HostingSpaces" }
                });
        }
    }
}
