using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCPractice.Migrations
{
    /// <inheritdoc />
    public partial class Fewthingsaddindatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MobileNumber",
                table: "OrderHeader",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "OrderHeader");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "OrderHeader",
                newName: "MobileNumber");
        }
    }
}
