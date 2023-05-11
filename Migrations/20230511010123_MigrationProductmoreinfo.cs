using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bamboo.Migrations
{
    /// <inheritdoc />
    public partial class MigrationProductmoreinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "cost",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "discount",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cost",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "discount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "tax",
                table: "Products");
        }
    }
}
