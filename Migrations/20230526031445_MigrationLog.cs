using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bamboo.Migrations
{
    /// <inheritdoc />
    public partial class MigrationLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    logID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    log = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.logID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
