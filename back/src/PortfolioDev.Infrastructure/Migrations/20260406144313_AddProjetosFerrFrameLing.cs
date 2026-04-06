using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioDev.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjetosFerrFrameLing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FerramentaProjeto",
                columns: table => new
                {
                    FerramentasId = table.Column<int>(type: "integer", nullable: false),
                    ProjetosId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FerramentaProjeto", x => new { x.FerramentasId, x.ProjetosId });
                    table.ForeignKey(
                        name: "FK_FerramentaProjeto_Ferramentas_FerramentasId",
                        column: x => x.FerramentasId,
                        principalTable: "Ferramentas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FerramentaProjeto_Projetos_ProjetosId",
                        column: x => x.ProjetosId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkProjeto",
                columns: table => new
                {
                    FrameworksId = table.Column<int>(type: "integer", nullable: false),
                    ProjetosId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkProjeto", x => new { x.FrameworksId, x.ProjetosId });
                    table.ForeignKey(
                        name: "FK_FrameworkProjeto_Frameworks_FrameworksId",
                        column: x => x.FrameworksId,
                        principalTable: "Frameworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FrameworkProjeto_Projetos_ProjetosId",
                        column: x => x.ProjetosId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinguagemProjeto",
                columns: table => new
                {
                    LinguagensId = table.Column<int>(type: "integer", nullable: false),
                    ProjetosId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinguagemProjeto", x => new { x.LinguagensId, x.ProjetosId });
                    table.ForeignKey(
                        name: "FK_LinguagemProjeto_Linguagens_LinguagensId",
                        column: x => x.LinguagensId,
                        principalTable: "Linguagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinguagemProjeto_Projetos_ProjetosId",
                        column: x => x.ProjetosId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FerramentaProjeto_ProjetosId",
                table: "FerramentaProjeto",
                column: "ProjetosId");

            migrationBuilder.CreateIndex(
                name: "IX_FrameworkProjeto_ProjetosId",
                table: "FrameworkProjeto",
                column: "ProjetosId");

            migrationBuilder.CreateIndex(
                name: "IX_LinguagemProjeto_ProjetosId",
                table: "LinguagemProjeto",
                column: "ProjetosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FerramentaProjeto");

            migrationBuilder.DropTable(
                name: "FrameworkProjeto");

            migrationBuilder.DropTable(
                name: "LinguagemProjeto");
        }
    }
}
