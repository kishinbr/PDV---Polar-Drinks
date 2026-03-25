using pim3semestre.Models;

namespace pim3semestre.ViewModels
{
    public class CompraDetalhesViewModel
    {
        public CompraEstoqueModel Compra { get; set; }

        public bool PodeConfirmar { get; set; }
        public List<ItemCompraModel> Itens { get; set; }
    }
}