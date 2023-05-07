using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bamboo.Migrations
{
    /// <inheritdoc />
    public partial class Migrationfixx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_businessID",
                table: "Products",
                column: "businessID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Businesses_businessID",
                table: "Products",
                column: "businessID",
                principalTable: "Businesses",
                principalColumn: "businessID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Businesses_businessID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_businessID",
                table: "Products");
        }
    }
}
