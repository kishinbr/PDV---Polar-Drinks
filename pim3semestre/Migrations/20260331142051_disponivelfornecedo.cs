using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pim3semestre.Migrations
{
    /// <inheritdoc />
    public partial class disponivelfornecedo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FornecedorAtivo",
                table: "Fornecedores",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FornecedorAtivo",
                table: "Fornecedores");
        }
    }
}
