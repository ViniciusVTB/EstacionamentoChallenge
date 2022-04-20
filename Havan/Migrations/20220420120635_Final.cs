using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Havan.Migrations
{
    public partial class Final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Condutor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desconto = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condutor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorarioLivre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoraInicial = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraFinal = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorarioLivre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Preco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrecoHora = table.Column<int>(type: "int", nullable: false),
                    PrecoHoraAdicional = table.Column<int>(type: "int", nullable: false),
                    DataInicial = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFinal = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preco", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntradaVeiculo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataSaida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Permanencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorHora = table.Column<int>(type: "int", nullable: true),
                    ValorHoraAdicional = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<double>(type: "float", nullable: true),
                    CondutorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradaVeiculo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntradaVeiculo_Condutor_CondutorId",
                        column: x => x.CondutorId,
                        principalTable: "Condutor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntradaVeiculo_CondutorId",
                table: "EntradaVeiculo",
                column: "CondutorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntradaVeiculo");

            migrationBuilder.DropTable(
                name: "HorarioLivre");

            migrationBuilder.DropTable(
                name: "Preco");

            migrationBuilder.DropTable(
                name: "Condutor");
        }
    }
}
