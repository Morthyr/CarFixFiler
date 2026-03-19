using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarFixFiler.Migrations
{
    /// <inheritdoc />
    public partial class AddProductNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductNumber",
                table: "ServiceItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductNumber",
                table: "ServiceItems");
        }
    }
}
