using Microsoft.EntityFrameworkCore.Migrations;

namespace Infinterest.Migrations
{
    public partial class UserProfileMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImgUrl",
                table: "users",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Contact",
                table: "users",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                table: "users",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Vendor_Bio",
                table: "users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor_Company",
                table: "users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor_Contact",
                table: "users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor_ImgUrl",
                table: "users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor_Website",
                table: "users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Vendor_Bio",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Vendor_Company",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Vendor_Contact",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Vendor_ImgUrl",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Vendor_Website",
                table: "users");

            migrationBuilder.AlterColumn<string>(
                name: "ImgUrl",
                table: "users",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Contact",
                table: "users",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                table: "users",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
