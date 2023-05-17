using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bamboo.Migrations
{
    /// <inheritdoc />
    public partial class migrationasdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ownerOfBusinessID",
                table: "CustomUsers",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ownerOfBusinessID",
                table: "CustomUsers");
        }
    }
}
