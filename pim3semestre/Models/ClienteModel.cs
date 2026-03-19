using System.ComponentModel.DataAnnotations;

namespace pim3semestre.Models
{
    public class ClienteModel
    {
        [Key]
        public int ClienteID { get; set; }

        public string? ClienteNome { get; set; }

        public string? ClienteTelefone { get; set; }

        public string? ClienteEmail { get; set; }

        public string? ClienteSenha { get; set; }

        public DateTime ClienteDataNas { get; set; }

        public string? ClienteCPF { get; set; }

        public List<VendaFinalModel> Vendas { get; set; } = new List<VendaFinalModel>();
    }
}
