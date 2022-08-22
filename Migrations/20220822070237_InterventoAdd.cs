using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebSite.Migrations
{
    public partial class InterventoAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Intervento",
                columns: table => new
                {
                    InterventoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoIntervento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataIntervento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Completato = table.Column<bool>(type: "bit", nullable: true),
                    AziendaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intervento", x => x.InterventoId);
                    table.ForeignKey(
                        name: "FK_Intervento_Azienda_AziendaID",
                        column: x => x.AziendaID,
                        principalTable: "Azienda",
                        principalColumn: "AziendaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Intervento_AziendaID",
                table: "Intervento",
                column: "AziendaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Intervento");
        }
    }
}
