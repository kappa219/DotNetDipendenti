using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace corsosharp.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyTipologiaLavoro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_anagrafica_dipendente_tipologia_lavoro_id",
                table: "anagrafica_dipendente",
                column: "tipologia_lavoro_id");

            migrationBuilder.AddForeignKey(
                name: "FK_anagrafica_dipendente_tipologia_lavoro_tipologia_lavoro_id",
                table: "anagrafica_dipendente",
                column: "tipologia_lavoro_id",
                principalTable: "tipologia_lavoro",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_anagrafica_dipendente_tipologia_lavoro_tipologia_lavoro_id",
                table: "anagrafica_dipendente");

            migrationBuilder.DropIndex(
                name: "IX_anagrafica_dipendente_tipologia_lavoro_id",
                table: "anagrafica_dipendente");
        }
    }
}
