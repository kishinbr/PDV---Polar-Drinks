using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pim3semestre.Migrations
{
    /// <inheritdoc />
    public partial class tipopagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VendaTipoPagamento",
                table: "Vendas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendaTipoPagamento",
                table: "Vendas");
        }
    }
}
