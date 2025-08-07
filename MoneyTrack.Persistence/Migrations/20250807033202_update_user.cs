using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyTrack.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeletedDate",
                table: "user",
                newName: "DeletedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "transaction",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "role",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "api_usage",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: new Guid("4c5d1784-f350-49da-861c-92c486b4b46c"),
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: new Guid("af2b2a21-21e7-41a2-8727-c67816796132"),
                column: "DeletedAt",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "role");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "api_usage");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "user",
                newName: "DeletedDate");
        }
    }
}
