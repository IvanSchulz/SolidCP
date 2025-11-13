using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SolidCP.EnterpriseServer.Data.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class Run_Migrate_msSQL_Script : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlScript("Migrate_msSQL.sql");

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "UsersHome", 1 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "UsersHome", 100 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "UsersHome", 104 });

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 104);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Providers",
                columns: new[] { "ProviderID", "DisableAutoDiscovery", "DisplayName", "EditorControl", "GroupID", "ProviderName", "ProviderType" },
                values: new object[,]
                {
                    { 1, null, "Windows Server 2003", "Windows2003", 1, "Windows2003", "SolidCP.Providers.OS.Windows2003, SolidCP.Providers.OS.Windows2003" },
                    { 100, null, "Windows Server 2008", "Windows2008", 1, "Windows2008", "SolidCP.Providers.OS.Windows2008, SolidCP.Providers.OS.Windows2008" },
                    { 104, null, "Windows Server 2012", "Windows2012", 1, "Windows2012", "SolidCP.Providers.OS.Windows2012, SolidCP.Providers.OS.Windows2012" }
                });

            migrationBuilder.InsertData(
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
