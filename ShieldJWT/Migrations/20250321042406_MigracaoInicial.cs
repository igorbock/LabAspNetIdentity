using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShieldJWT.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "changed_password",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NewHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ConfimationCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Confirmed = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_changed_password", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "Id", "Email", "EmailConfirmed", "Hash", "Username" },
                values: new object[] { 1, "noreply@gmail.com", false, "", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "changed_password");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
