using System.ComponentModel.DataAnnotations;

namespace pim3semestre.Models
{
    public class CategoriaModel
    {
        [Key]
        public int CategoriaID { get; set; }


        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        public string? CategoriaNome { get; set; }

        [Required(ErrorMessage = "A descrição da categoria é obrigatória.")]


        public string? CategoriaDescricao { get; set; }

        public List<ProdutoModel> Produtos { get; set; } = new List<ProdutoModel>();
    }
}
