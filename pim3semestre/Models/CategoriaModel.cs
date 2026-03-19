using System.ComponentModel.DataAnnotations;

namespace pim3semestre.Models
{
    public class CategoriaModel
    {
        [Key]
        public int CategoriaID { get; set; }

        public string? CategoriaNome { get; set; }

        public string? CategoriaDescricao { get; set; }

        public List<ProdutoModel> Produtos { get; set; } = new List<ProdutoModel>();
    }
}
