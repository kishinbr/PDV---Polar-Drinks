using System.ComponentModel.DataAnnotations;

namespace pim3semestre.Models
{
   
    public class VendaModel
    {
        [Key]
        public int VendaID { get; set; }

        public decimal VendaValorTotal { get; set; }

        public string? VendaTipoPagamento { get; set; } 

        public DateTime VendaData { get; set; } = DateTime.Now;

        public List<ItemVendaModel> Itens { get; set; } = new List<ItemVendaModel>();
    }
    
}
