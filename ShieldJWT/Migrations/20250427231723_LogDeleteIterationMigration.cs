using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShieldJWT.Migrations
{
    /// <inheritdoc />
    public partial class LogDeleteIterationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IntervalDaysToRemoveLogs",
                table: "company",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "log_delete_iteration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastIterationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_delete_iteration", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "log_delete_iteration");

            migrationBuilder.DropColumn(
                name: "IntervalDaysToRemoveLogs",
                table: "company");
        }
    }
}
