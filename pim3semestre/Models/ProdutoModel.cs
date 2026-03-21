using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pim3semestre.Models
{
    public class ProdutoModel
    {
        [Key]
        public int ProdutoID { get; set; }

        public string? ProdutoNome { get; set; }

        public string? ProdutoDescricao { get; set; }

        public decimal ProdutoPrecoVenda { get; set; }

        public string? ProdutoCodBarra { get; set; }

        public int ProdutoQtdEstoque { get; set; }

        public bool ProdutoAtivo { get; set; }

        [ForeignKey("CategoriaID")]
        public int CategoriaID { get; set; }

        public CategoriaModel? Categoria { get; set; }
    }
}
