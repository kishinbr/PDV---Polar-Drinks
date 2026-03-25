using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace pim3semestre.ViewModels
{
    public class CompraCreateViewModel
    {
        [Required(ErrorMessage = "Selecione um fornecedor")]
        public int? FornecedorID { get; set; }

        public List<ItemCompraCreateVM> Itens { get; set; } = new List<ItemCompraCreateVM>();

        public IEnumerable<SelectListItem>? Fornecedores { get; set; }
        public IEnumerable<SelectListItem>? Produtos { get; set; }
    }

    public class ItemCompraCreateVM
    {
        [Required(ErrorMessage = "Selecione um produto")]
        public int? ProdutoID { get; set; } 

        [Required(ErrorMessage = "Informe a quantidade")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade inválida")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "Informe o preço")]
        [Range(0.01, 999999.99, ErrorMessage = "Preço inválido")]
        public decimal Preco { get; set; }
    }
}