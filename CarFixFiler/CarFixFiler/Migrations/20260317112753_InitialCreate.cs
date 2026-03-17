using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarFixFiler.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaborCostDiscount = table.Column<int>(type: "int", nullable: false),
                    PartCostDiscount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    LicensePlateNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Vin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EngineCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Power = table.Column<int>(type: "int", nullable: false),
                    EngineDisplacement = table.Column<int>(type: "int", nullable: false),
                    ManufatureYear = table.Column<int>(type: "int", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.LicensePlateNumber);
                    table.ForeignKey(
                        name: "FK_Car_Customers_CustomerName",
                        column: x => x.CustomerName,
                        principalTable: "Customers",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    LicensePlateNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => new { x.LicensePlateNumber, x.Date });
                    table.ForeignKey(
                        name: "FK_Services_Car_LicensePlateNumber",
                        column: x => x.LicensePlateNumber,
                        principalTable: "Car",
                        principalColumn: "LicensePlateNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceItems",
                columns: table => new
                {
                    LicensePlateNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchasePrice = table.Column<int>(type: "int", nullable: true),
                    SellingPrice = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceItems", x => new { x.LicensePlateNumber, x.Date, x.ItemName });
                    table.ForeignKey(
                        name: "FK_ServiceItems_Services_LicensePlateNumber_Date",
                        columns: x => new { x.LicensePlateNumber, x.Date },
                        principalTable: "Services",
                        principalColumns: new[] { "LicensePlateNumber", "Date" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Car_CustomerName",
                table: "Car",
                column: "CustomerName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceItems");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
