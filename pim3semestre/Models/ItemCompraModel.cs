using System.ComponentModel.DataAnnotations;

namespace pim3semestre.Models
{
    public class ItemCompraModel
    {
        [Key]
        public int ItemCompraID { get; set; }

        public int ItemCompraQtd { get; set; }

        public decimal ItemCompraPreco { get; set; }

        public int ProdutoID { get; set; }
        public ProdutoModel? Produto { get; set; }

        public int CompraID { get; set; }
        public CompraEstoqueModel? Compra { get; set; }
    }
}
