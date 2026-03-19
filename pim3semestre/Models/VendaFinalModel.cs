using System.ComponentModel.DataAnnotations;

namespace pim3semestre.Models
{
   
    public class VendaFinalModel
    {
        [Key]
        public int VendaID { get; set; }

        public decimal VendaValorTotal { get; set; }

        public string? VendaTipo { get; set; } 

        public DateTime VendaData { get; set; } = DateTime.Now;

        public int? ClienteID { get; set; }
        public ClienteModel? Cliente { get; set; }

        public int FuncionarioID { get; set; }

        public FuncionarioModel? Funcionario { get; set; }

        public List<ItemVendaModel> Itens { get; set; } = new List<ItemVendaModel>();
    }
    
}
