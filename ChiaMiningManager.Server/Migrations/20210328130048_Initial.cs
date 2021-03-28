using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChiaMiningManager.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Miners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Token = table.Column<string>(type: "text", nullable: true),
                    Plots = table.Column<short>(type: "smallint", nullable: false),
                    PlotMinutes = table.Column<long>(type: "bigint", nullable: false),
                    NextIncrement = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Miners", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Miners_Name",
                table: "Miners",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Miners_Token",
                table: "Miners",
                column: "Token",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Miners");
        }
    }
}
