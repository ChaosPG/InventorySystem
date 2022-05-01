using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventorySystem.Migrations
{
    public partial class IdentityDBUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Category_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category_Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Category_ID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Customer_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer_FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer_LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer_Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer_Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Customer_ID);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Bill_Number = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Payment_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Other_Details = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Bill_Number);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    Supplier_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Supplier_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier_Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier_Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier_Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Supplier_ID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Order_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date_of_Order = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Order_Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Order_ID);
                    table.ForeignKey(
                        name: "FK_Orders_Customer_Customer_ID",
                        column: x => x.Customer_ID,
                        principalTable: "Customer",
                        principalColumn: "Customer_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Product_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Product_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Product_Unit = table.Column<int>(type: "int", nullable: false),
                    Product_Price = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    Product_Quantity = table.Column<int>(type: "int", nullable: false),
                    Other_Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier_ID = table.Column<int>(type: "int", nullable: false),
                    Category_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Product_ID);
                    table.ForeignKey(
                        name: "FK_Product_Category_Category_ID",
                        column: x => x.Category_ID,
                        principalTable: "Category",
                        principalColumn: "Category_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Supplier_Supplier_ID",
                        column: x => x.Supplier_ID,
                        principalTable: "Supplier",
                        principalColumn: "Supplier_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Detail",
                columns: table => new
                {
                    Order_Detail_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unit_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    Order_Detail_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Product_ID = table.Column<int>(type: "int", nullable: false),
                    Order_ID = table.Column<int>(type: "int", nullable: false),
                    Bill_Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Detail", x => x.Order_Detail_ID);
                    table.ForeignKey(
                        name: "FK_Order_Detail_Orders_Order_ID",
                        column: x => x.Order_ID,
                        principalTable: "Orders",
                        principalColumn: "Order_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Detail_Payment_Bill_Number",
                        column: x => x.Bill_Number,
                        principalTable: "Payment",
                        principalColumn: "Bill_Number",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Detail_Product_Product_ID",
                        column: x => x.Product_ID,
                        principalTable: "Product",
                        principalColumn: "Product_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_Detail_Bill_Number",
                table: "Order_Detail",
                column: "Bill_Number");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Detail_Order_ID",
                table: "Order_Detail",
                column: "Order_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Detail_Product_ID",
                table: "Order_Detail",
                column: "Product_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Customer_ID",
                table: "Orders",
                column: "Customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Category_ID",
                table: "Product",
                column: "Category_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Supplier_ID",
                table: "Product",
                column: "Supplier_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order_Detail");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Supplier");
        }
    }
}
