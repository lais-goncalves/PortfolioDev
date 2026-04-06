using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioDev.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FrameworkLinguagemRelN1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FrameworkLinguagem");

            migrationBuilder.AddColumn<int>(
                name: "LinguagemId",
                table: "Frameworks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Frameworks_LinguagemId",
                table: "Frameworks",
                column: "LinguagemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Frameworks_Linguagens_LinguagemId",
                table: "Frameworks",
                column: "LinguagemId",
                principalTable: "Linguagens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Frameworks_Linguagens_LinguagemId",
                table: "Frameworks");

            migrationBuilder.DropIndex(
                name: "IX_Frameworks_LinguagemId",
                table: "Frameworks");

            migrationBuilder.DropColumn(
                name: "LinguagemId",
                table: "Frameworks");

            migrationBuilder.CreateTable(
                name: "FrameworkLinguagem",
                columns: table => new
                {
                    FrameworksId = table.Column<int>(type: "integer", nullable: false),
                    LinguagensId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkLinguagem", x => new { x.FrameworksId, x.LinguagensId });
                    table.ForeignKey(
                        name: "FK_FrameworkLinguagem_Frameworks_FrameworksId",
                        column: x => x.FrameworksId,
                        principalTable: "Frameworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FrameworkLinguagem_Linguagens_LinguagensId",
                        column: x => x.LinguagensId,
                        principalTable: "Linguagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FrameworkLinguagem_LinguagensId",
                table: "FrameworkLinguagem",
                column: "LinguagensId");
        }
    }
}
