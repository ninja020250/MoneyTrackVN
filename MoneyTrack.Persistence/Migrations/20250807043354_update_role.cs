using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyTrack.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update_role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: new Guid("4c5d1784-f350-49da-861c-92c486b4b46c"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { "System", new DateTime(2025, 8, 7, 4, 33, 54, 577, DateTimeKind.Utc).AddTicks(4900) });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: new Guid("af2b2a21-21e7-41a2-8727-c67816796132"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { "System", new DateTime(2025, 8, 7, 4, 33, 54, 577, DateTimeKind.Utc).AddTicks(4660) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: new Guid("4c5d1784-f350-49da-861c-92c486b4b46c"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: new Guid("af2b2a21-21e7-41a2-8727-c67816796132"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
