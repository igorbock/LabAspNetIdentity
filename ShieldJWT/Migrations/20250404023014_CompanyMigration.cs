using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShieldJWT.Migrations
{
    /// <inheritdoc />
    public partial class CompanyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdCompany",
                table: "user",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "IdCompany",
                table: "changed_password",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    CNPJ = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "EmailConfirmed", "IdCompany" },
                values: new object[] { "noreply.shieldjwt@gmail.com", true, new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.CreateIndex(
                name: "IX_user_IdCompany",
                table: "user",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_changed_password_IdCompany",
                table: "changed_password",
                column: "IdCompany");

            migrationBuilder.AddForeignKey(
                name: "FK_changed_password_company_IdCompany",
                table: "changed_password",
                column: "IdCompany",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_company_IdCompany",
                table: "user",
                column: "IdCompany",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_changed_password_company_IdCompany",
                table: "changed_password");

            migrationBuilder.DropForeignKey(
                name: "FK_user_company_IdCompany",
                table: "user");

            migrationBuilder.DropTable(
                name: "company");

            migrationBuilder.DropIndex(
                name: "IX_user_IdCompany",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_changed_password_IdCompany",
                table: "changed_password");

            migrationBuilder.DropColumn(
                name: "IdCompany",
                table: "user");

            migrationBuilder.DropColumn(
                name: "IdCompany",
                table: "changed_password");

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "EmailConfirmed" },
                values: new object[] { "noreply@gmail.com", false });
        }
    }
}
