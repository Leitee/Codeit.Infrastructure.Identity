using Microsoft.EntityFrameworkCore.Migrations;

namespace Codeit.Infrastructure.Identity.DAL.Migrations.IdentityDB
{
    public partial class addSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileId",
                schema: "IDENTITY",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SettingsId",
                schema: "IDENTITY",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserSettings",
                schema: "IDENTITY",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThemeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SettingsID", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SettingsId",
                schema: "IDENTITY",
                table: "Users",
                column: "SettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserSettings_SettingsId",
                schema: "IDENTITY",
                table: "Users",
                column: "SettingsId",
                principalSchema: "IDENTITY",
                principalTable: "UserSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserSettings_SettingsId",
                schema: "IDENTITY",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserSettings",
                schema: "IDENTITY");

            migrationBuilder.DropIndex(
                name: "IX_Users_SettingsId",
                schema: "IDENTITY",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                schema: "IDENTITY",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                schema: "IDENTITY",
                table: "Users");
        }
    }
}
