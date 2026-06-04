using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace corsosharp.Migrations
{
    /// <inheritdoc />
    public partial class AddFotoPercorso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoPercorso",
                table: "anagrafica_dipendente",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoPercorso",
                table: "anagrafica_dipendente");
        }
    }
}
