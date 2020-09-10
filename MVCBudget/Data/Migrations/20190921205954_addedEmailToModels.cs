using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCBudget.Data.Migrations
{
    public partial class addedEmailToModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Tran",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Sale",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Config",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Account",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Tran");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Config");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Account");
        }
    }
}
