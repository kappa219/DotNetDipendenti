using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace corsosharp.Migrations
{
    /// <inheritdoc />
    public partial class AddGiornateLavorative : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "giornate_lavorative",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    dipendente_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ore_lavorate = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Azienda = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mansione = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_giornate_lavorative", x => x.Id);
                    table.ForeignKey(
                        name: "FK_giornate_lavorative_anagrafica_dipendente_dipendente_id",
                        column: x => x.dipendente_id,
                        principalTable: "anagrafica_dipendente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_giornate_lavorative_dipendente_id",
                table: "giornate_lavorative",
                column: "dipendente_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "giornate_lavorative");
        }
    }
}
