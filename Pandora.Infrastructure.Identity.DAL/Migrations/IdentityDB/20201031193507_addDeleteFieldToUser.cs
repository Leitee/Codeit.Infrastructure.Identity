using Microsoft.EntityFrameworkCore.Migrations;

namespace Pandora.Infrastructure.Identity.DAL.Migrations.IdentityDB
{
    public partial class addDeleteFieldToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                schema: "IDENTITY",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                schema: "IDENTITY",
                table: "Users");
        }
    }
}
