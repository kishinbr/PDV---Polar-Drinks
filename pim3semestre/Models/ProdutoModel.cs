using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pim3semestre.Models
{
    public class ProdutoModel
    {
        [Key]
        public int ProdutoID { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório")]
        [StringLength(100)]
        public string? ProdutoNome { get; set; }

        [StringLength(300)]
        public string? ProdutoDescricao { get; set; }

        [Required(ErrorMessage = "O custo é obrigatório")]
        [Range(0.00, 999999.99)]
        public decimal? ProdutoPrecoCusto { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.00, 999999.99, ErrorMessage = "O preço inválido")]
        public decimal? ProdutoPrecoVenda { get; set; }

        [Range(0, 100, ErrorMessage = "Promoção deve ser entre 0 e 100%")]
        public decimal ProdutoPromocao { get; set; } = 0; 

        [Required(ErrorMessage = "O código de barras é obrigatório")]
        [StringLength(20)]
        public string? ProdutoCodBarra { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantidade inválida")]
        public int? ProdutoQtdEstoque { get; set; }

        [Required(ErrorMessage = "O estoque mínimo é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "Estoque mínimo inválido")]
        public int? ProdutoEstoqueMinimo { get; set; } 

        public bool ProdutoAtivo { get; set; }

        [Required(ErrorMessage = "Selecione uma categoria")]
        public int? CategoriaID { get; set; }

        public CategoriaModel? Categoria { get; set; }

        // Propriedade calculada para indicar estoque baixo
        [NotMapped]
        public bool EstoqueBaixo => ProdutoQtdEstoque.HasValue && ProdutoQtdEstoque.Value <= ProdutoEstoqueMinimo;
    }
}