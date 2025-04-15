using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShieldJWT.Migrations
{
    /// <inheritdoc />
    public partial class InputAndOutputTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InputTime",
                table: "log",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OutputTime",
                table: "log",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputTime",
                table: "log");

            migrationBuilder.DropColumn(
                name: "OutputTime",
                table: "log");
        }
    }
}
