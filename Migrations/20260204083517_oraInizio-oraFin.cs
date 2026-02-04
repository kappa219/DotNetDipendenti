using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace corsosharp.Migrations
{
    /// <inheritdoc />
    public partial class oraIniziooraFin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "OraFine",
                table: "giornate_lavorative",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "OraInizio",
                table: "giornate_lavorative",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OraFine",
                table: "giornate_lavorative");

            migrationBuilder.DropColumn(
                name: "OraInizio",
                table: "giornate_lavorative");
        }
    }
}
