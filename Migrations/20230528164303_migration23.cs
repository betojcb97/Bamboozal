using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bamboo.Migrations
{
    /// <inheritdoc />
    public partial class migration23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "productsIdsAndQuantitiesSerialized",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "productsIdsAndQuantitiesSerialized",
                table: "Carts");
        }
    }
}
