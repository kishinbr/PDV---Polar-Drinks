using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pim3semestre.Migrations
{
    /// <inheritdoc />
    public partial class CriandoBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoriaDescricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaID);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteNome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClienteTelefone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClienteEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClienteSenha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClienteDataNas = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClienteCPF = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteID);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    FornecedorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FornecedorNome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FornecedorCNPJ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FornecedorTelefone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FornecedorEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FornecedorCEP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FornecedorEstado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FornecedorCidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FornecedorBairro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FornecedorLogradouro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FornecedorNum = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.FornecedorID);
                });

            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    FuncionarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuncionarioNome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionarioCPF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionarioLogin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionarioSenha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionarioEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionarioTelefone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionarioDataNas = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.FuncionarioID);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    ProdutoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdutoNome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProdutoDescricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProdutoPrecoVenda = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ProdutoCodBarra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProdutoQtdEstoque = table.Column<int>(type: "int", nullable: false),
                    ProdutoAtivo = table.Column<bool>(type: "bit", nullable: false),
                    CategoriaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.ProdutoID);
                    table.ForeignKey(
                        name: "FK_Produtos_Categorias_CategoriaID",
                        column: x => x.CategoriaID,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComprasEstoque",
                columns: table => new
                {
                    CompraID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompraData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompraValorTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FornecedorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprasEstoque", x => x.CompraID);
                    table.ForeignKey(
                        name: "FK_ComprasEstoque_Fornecedores_FornecedorID",
                        column: x => x.FornecedorID,
                        principalTable: "Fornecedores",
                        principalColumn: "FornecedorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vendas",
                columns: table => new
                {
                    VendaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendaValorTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    VendaTipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendaData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClienteID = table.Column<int>(type: "int", nullable: true),
                    FuncionarioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendas", x => x.VendaID);
                    table.ForeignKey(
                        name: "FK_Vendas_Clientes_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Clientes",
                        principalColumn: "ClienteID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Vendas_Funcionarios_FuncionarioID",
                        column: x => x.FuncionarioID,
                        principalTable: "Funcionarios",
                        principalColumn: "FuncionarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItensCompra",
                columns: table => new
                {
                    ItemCompraID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCompraQtd = table.Column<int>(type: "int", nullable: false),
                    ItemCompraPreco = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ProdutoID = table.Column<int>(type: "int", nullable: false),
                    CompraID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensCompra", x => x.ItemCompraID);
                    table.ForeignKey(
                        name: "FK_ItensCompra_ComprasEstoque_CompraID",
                        column: x => x.CompraID,
                        principalTable: "ComprasEstoque",
                        principalColumn: "CompraID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensCompra_Produtos_ProdutoID",
                        column: x => x.ProdutoID,
                        principalTable: "Produtos",
                        principalColumn: "ProdutoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItensVenda",
                columns: table => new
                {
                    ItemVendaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemVendaQtd = table.Column<int>(type: "int", nullable: false),
                    ItemVendaPreco = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ProdutoID = table.Column<int>(type: "int", nullable: false),
                    VendaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensVenda", x => x.ItemVendaID);
                    table.ForeignKey(
                        name: "FK_ItensVenda_Produtos_ProdutoID",
                        column: x => x.ProdutoID,
                        principalTable: "Produtos",
                        principalColumn: "ProdutoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensVenda_Vendas_VendaID",
                        column: x => x.VendaID,
                        principalTable: "Vendas",
                        principalColumn: "VendaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovimentacoesEstoque",
                columns: table => new
                {
                    MovimentacaoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovimentacaoTipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovimentacaoQtd = table.Column<int>(type: "int", nullable: false),
                    MovimentacaoData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProdutoID = table.Column<int>(type: "int", nullable: false),
                    ItemCompraID = table.Column<int>(type: "int", nullable: true),
                    ItemVendaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentacoesEstoque", x => x.MovimentacaoID);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_ItensCompra_ItemCompraID",
                        column: x => x.ItemCompraID,
                        principalTable: "ItensCompra",
                        principalColumn: "ItemCompraID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_ItensVenda_ItemVendaID",
                        column: x => x.ItemVendaID,
                        principalTable: "ItensVenda",
                        principalColumn: "ItemVendaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_Produtos_ProdutoID",
                        column: x => x.ProdutoID,
                        principalTable: "Produtos",
                        principalColumn: "ProdutoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComprasEstoque_FornecedorID",
                table: "ComprasEstoque",
                column: "FornecedorID");

            migrationBuilder.CreateIndex(
                name: "IX_ItensCompra_CompraID",
                table: "ItensCompra",
                column: "CompraID");

            migrationBuilder.CreateIndex(
                name: "IX_ItensCompra_ProdutoID",
                table: "ItensCompra",
                column: "ProdutoID");

            migrationBuilder.CreateIndex(
                name: "IX_ItensVenda_ProdutoID",
                table: "ItensVenda",
                column: "ProdutoID");

            migrationBuilder.CreateIndex(
                name: "IX_ItensVenda_VendaID",
                table: "ItensVenda",
                column: "VendaID");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_ItemCompraID",
                table: "MovimentacoesEstoque",
                column: "ItemCompraID");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_ItemVendaID",
                table: "MovimentacoesEstoque",
                column: "ItemVendaID");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_ProdutoID",
                table: "MovimentacoesEstoque",
                column: "ProdutoID");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CategoriaID",
                table: "Produtos",
                column: "CategoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_ClienteID",
                table: "Vendas",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_FuncionarioID",
                table: "Vendas",
                column: "FuncionarioID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimentacoesEstoque");

            migrationBuilder.DropTable(
                name: "ItensCompra");

            migrationBuilder.DropTable(
                name: "ItensVenda");

            migrationBuilder.DropTable(
                name: "ComprasEstoque");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropTable(
                name: "Fornecedores");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Funcionarios");
        }
    }
}
