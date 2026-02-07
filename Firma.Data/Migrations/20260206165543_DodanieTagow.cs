using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firma.Data.Migrations
{
    /// <inheritdoc />
    public partial class DodanieTagow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    IdTagu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.IdTagu);
                });

            migrationBuilder.CreateTable(
                name: "TowarTag",
                columns: table => new
                {
                    IdTowaru = table.Column<int>(type: "int", nullable: false),
                    IdTagu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TowarTag", x => new { x.IdTowaru, x.IdTagu });
                    table.ForeignKey(
                        name: "FK_TowarTag_Tag_IdTagu",
                        column: x => x.IdTagu,
                        principalTable: "Tag",
                        principalColumn: "IdTagu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TowarTag_Towar_IdTowaru",
                        column: x => x.IdTowaru,
                        principalTable: "Towar",
                        principalColumn: "IdTowaru",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TowarTag_IdTagu",
                table: "TowarTag",
                column: "IdTagu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TowarTag");

            migrationBuilder.DropTable(
                name: "Tag");
        }
    }
}
