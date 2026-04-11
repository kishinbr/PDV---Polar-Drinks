using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pim3semestre.Migrations
{
    /// <inheritdoc />
    public partial class remocaocategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Categorias_CategoriaID",
                table: "Produtos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_CategoriaID",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "CategoriaID",
                table: "Produtos");

            migrationBuilder.AddColumn<string>(
                name: "MovimentacaoDescricao",
                table: "MovimentacoesEstoque",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovimentacaoDescricao",
                table: "MovimentacoesEstoque");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaID",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaAtiva = table.Column<bool>(type: "bit", nullable: false),
                    CategoriaDescricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoriaNome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CategoriaID",
                table: "Produtos",
                column: "CategoriaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Categorias_CategoriaID",
                table: "Produtos",
                column: "CategoriaID",
                principalTable: "Categorias",
                principalColumn: "CategoriaID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
