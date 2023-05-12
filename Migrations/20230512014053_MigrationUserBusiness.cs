using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bamboo.Migrations
{
    /// <inheritdoc />
    public partial class MigrationUserBusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "businessID",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_businessID",
                table: "AspNetUsers",
                column: "businessID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Businesses_businessID",
                table: "AspNetUsers",
                column: "businessID",
                principalTable: "Businesses",
                principalColumn: "businessID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Businesses_businessID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_businessID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "businessID",
                table: "AspNetUsers");
        }
    }
}
