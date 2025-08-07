using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyTrack.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class convert_enum_role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "role",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: new Guid("4c5d1784-f350-49da-861c-92c486b4b46c"),
                columns: new[] { "CreatedDate", "Name" },
                values: new object[] { new DateTime(2025, 8, 7, 7, 23, 9, 638, DateTimeKind.Utc).AddTicks(7540), "Guest" });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: new Guid("af2b2a21-21e7-41a2-8727-c67816796132"),
                columns: new[] { "CreatedDate", "Name" },
                values: new object[] { new DateTime(2025, 8, 7, 7, 23, 9, 638, DateTimeKind.Utc).AddTicks(7290), "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "role",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: new Guid("4c5d1784-f350-49da-861c-92c486b4b46c"),
                columns: new[] { "CreatedDate", "Name" },
                values: new object[] { new DateTime(2025, 8, 7, 4, 33, 54, 577, DateTimeKind.Utc).AddTicks(4900), 1 });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: new Guid("af2b2a21-21e7-41a2-8727-c67816796132"),
                columns: new[] { "CreatedDate", "Name" },
                values: new object[] { new DateTime(2025, 8, 7, 4, 33, 54, 577, DateTimeKind.Utc).AddTicks(4660), 0 });
        }
    }
}
