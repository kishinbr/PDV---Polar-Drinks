using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pim3semestre.Migrations
{
    /// <inheritdoc />
    public partial class AjusteVenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Clientes_ClienteID",
                table: "Vendas");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Funcionarios_FuncionarioID",
                table: "Vendas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Vendas_ClienteID",
                table: "Vendas");

            migrationBuilder.DropIndex(
                name: "IX_Vendas_FuncionarioID",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "ClienteID",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "FuncionarioID",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "VendaTipo",
                table: "Vendas");

            migrationBuilder.AddColumn<decimal>(
                name: "ItemVendaTotal",
                table: "ItensVenda",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemVendaTotal",
                table: "ItensVenda");

            migrationBuilder.AddColumn<int>(
                name: "ClienteID",
                table: "Vendas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FuncionarioID",
                table: "Vendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VendaTipo",
                table: "Vendas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteCPF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClienteDataNas = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClienteEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClienteNome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClienteSenha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClienteTelefone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteID);
                });

            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    FuncionarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuncionarioCPF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionarioDataNas = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FuncionarioEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionarioLogin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionarioNome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionarioSenha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionarioTelefone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.FuncionarioID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_ClienteID",
                table: "Vendas",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_FuncionarioID",
                table: "Vendas",
                column: "FuncionarioID");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Clientes_ClienteID",
                table: "Vendas",
                column: "ClienteID",
                principalTable: "Clientes",
                principalColumn: "ClienteID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Funcionarios_FuncionarioID",
                table: "Vendas",
                column: "FuncionarioID",
                principalTable: "Funcionarios",
                principalColumn: "FuncionarioID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
