using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miles.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposTransacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComprovanteUrl",
                table: "Transacoes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Transacoes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Transacoes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FaturaId",
                table: "Transacoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParcelaAtual",
                table: "Transacoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Parcelas",
                table: "Transacoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusFatura",
                table: "Transacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComprovanteUrl",
                table: "Transacoes");

            migrationBuilder.DropColumn(
                name: "DataAtualizacao",
                table: "Transacoes");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Transacoes");

            migrationBuilder.DropColumn(
                name: "FaturaId",
                table: "Transacoes");

            migrationBuilder.DropColumn(
                name: "ParcelaAtual",
                table: "Transacoes");

            migrationBuilder.DropColumn(
                name: "Parcelas",
                table: "Transacoes");

            migrationBuilder.DropColumn(
                name: "StatusFatura",
                table: "Transacoes");
        }
    }
}
