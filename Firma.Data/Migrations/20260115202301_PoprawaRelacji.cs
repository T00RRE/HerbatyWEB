using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firma.Data.Migrations
{
    /// <inheritdoc />
    public partial class PoprawaRelacji : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Towar_Rodzaj_RodzajIdRodzaju",
                table: "Towar");

            migrationBuilder.DropIndex(
                name: "IX_Towar_RodzajIdRodzaju",
                table: "Towar");

            migrationBuilder.DropColumn(
                name: "RodzajIdRodzaju",
                table: "Towar");

            migrationBuilder.CreateIndex(
                name: "IX_Towar_IdRodzaju",
                table: "Towar",
                column: "IdRodzaju");

            migrationBuilder.AddForeignKey(
                name: "FK_Towar_Rodzaj_IdRodzaju",
                table: "Towar",
                column: "IdRodzaju",
                principalTable: "Rodzaj",
                principalColumn: "IdRodzaju",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Towar_Rodzaj_IdRodzaju",
                table: "Towar");

            migrationBuilder.DropIndex(
                name: "IX_Towar_IdRodzaju",
                table: "Towar");

            migrationBuilder.AddColumn<int>(
                name: "RodzajIdRodzaju",
                table: "Towar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Towar_RodzajIdRodzaju",
                table: "Towar",
                column: "RodzajIdRodzaju");

            migrationBuilder.AddForeignKey(
                name: "FK_Towar_Rodzaj_RodzajIdRodzaju",
                table: "Towar",
                column: "RodzajIdRodzaju",
                principalTable: "Rodzaj",
                principalColumn: "IdRodzaju",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
