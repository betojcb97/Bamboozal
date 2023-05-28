using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bamboo.Migrations
{
    /// <inheritdoc />
    public partial class productsquantities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Businesses_businessID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Carts_cartID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Orders_orderID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_cartID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_orderID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Orders_businessID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "cartID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "orderID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "businessID",
                table: "Orders");

            migrationBuilder.AddColumn<decimal>(
                name: "cost",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "discount",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "subtotal",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cost",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "discount",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "subtotal",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "tax",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "total",
                table: "Carts");

            migrationBuilder.AddColumn<Guid>(
                name: "cartID",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "orderID",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "businessID",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Products_cartID",
                table: "Products",
                column: "cartID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_orderID",
                table: "Products",
                column: "orderID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_businessID",
                table: "Orders",
                column: "businessID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Businesses_businessID",
                table: "Orders",
                column: "businessID",
                principalTable: "Businesses",
                principalColumn: "businessID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Carts_cartID",
                table: "Products",
                column: "cartID",
                principalTable: "Carts",
                principalColumn: "cartID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Orders_orderID",
                table: "Products",
                column: "orderID",
                principalTable: "Orders",
                principalColumn: "orderID");
        }
    }
}
