using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bamboo.Migrations
{
    /// <inheritdoc />
    public partial class migrationad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ownerOfBusinessID",
                table: "CustomUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_userID",
                table: "Orders",
                column: "userID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CustomUsers_userID",
                table: "Orders",
                column: "userID",
                principalTable: "CustomUsers",
                principalColumn: "userID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CustomUsers_userID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_userID",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "ownerOfBusinessID",
                table: "CustomUsers",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
