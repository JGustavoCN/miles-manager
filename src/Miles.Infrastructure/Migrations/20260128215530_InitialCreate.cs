using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miles.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false),
                    SenhaHash = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramasFidelidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    Banco = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramasFidelidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramasFidelidade_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cartoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    Bandeira = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    Limite = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    DiaVencimento = table.Column<int>(type: "INT", nullable: false),
                    FatorConversao = table.Column<decimal>(type: "DECIMAL(10,4)", nullable: false, defaultValue: 1.0m),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ProgramaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cartoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cartoes_ProgramasFidelidade_ProgramaId",
                        column: x => x.ProgramaId,
                        principalTable: "ProgramasFidelidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cartoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    Valor = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false),
                    Categoria = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    CotacaoDolar = table.Column<decimal>(type: "DECIMAL(10,4)", nullable: false),
                    PontosEstimados = table.Column<int>(type: "INT", nullable: false),
                    CartaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacoes_Cartoes_CartaoId",
                        column: x => x.CartaoId,
                        principalTable: "Cartoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cartoes_ProgramaId",
                table: "Cartoes",
                column: "ProgramaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cartoes_UsuarioId",
                table: "Cartoes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramasFidelidade_UsuarioId",
                table: "ProgramasFidelidade",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_CartaoId",
                table: "Transacoes",
                column: "CartaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_Data",
                table: "Transacoes",
                column: "Data");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transacoes");

            migrationBuilder.DropTable(
                name: "Cartoes");

            migrationBuilder.DropTable(
                name: "ProgramasFidelidade");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
