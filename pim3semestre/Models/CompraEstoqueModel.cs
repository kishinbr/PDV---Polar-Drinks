using System.ComponentModel.DataAnnotations;

namespace pim3semestre.Models
{
    public class CompraEstoqueModel
    {
        [Key]
        public int CompraID { get; set; }

        public DateTime CompraData { get; set; } = DateTime.Now;

        public decimal CompraValorTotal { get; set; }

        public int FornecedorID { get; set; }

    
        public FornecedorModel? Fornecedor { get; set; }

        public List<ItemCompraModel> Itens { get; set; } = new List<ItemCompraModel>();
    }
}
