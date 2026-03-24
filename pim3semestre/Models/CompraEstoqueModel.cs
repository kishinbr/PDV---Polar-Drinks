using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pim3semestre.Models
{
    public class CompraEstoqueModel
    {
        [Key]
        public int CompraID { get; set; }

        [Required]
        public DateTime CompraData { get; set; } = DateTime.Now;

        public DateTime? CompraDataEntrega { get; set; }

        [Required(ErrorMessage = "O status da compra é obrigatório")]
        [StringLength(20)]
        public string CompraStatus { get; set; } = "Aguardando";

        [NotMapped]
        public decimal CompraValorTotal
        {
            get
            {
                return Itens.Sum(i => i.ItemCompraQtd * i.ItemCompraPreco);
            }
        }

        [Required(ErrorMessage = "Selecione um fornecedor")]
        public int FornecedorID { get; set; }

        public FornecedorModel? Fornecedor { get; set; }

        public ICollection<ItemCompraModel> Itens { get; set; } = new List<ItemCompraModel>();
    }
}