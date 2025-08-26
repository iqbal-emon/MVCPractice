using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCPractice.Migrations
{
    /// <inheritdoc />
    public partial class checkoutpageworking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "OrderHeader");
        }
    }
}
