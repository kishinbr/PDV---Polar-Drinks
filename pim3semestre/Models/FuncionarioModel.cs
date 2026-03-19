using System.ComponentModel.DataAnnotations;

namespace pim3semestre.Models
{
    public class FuncionarioModel
    {
        [Key]
        public int FuncionarioID { get; set; }

        public string? FuncionarioNome { get; set; }

        public string? FuncionarioCPF { get; set; }

        public string? FuncionarioLogin { get; set; }

        public string? FuncionarioSenha { get; set; }

        public string? FuncionarioEmail { get; set; }

        public string? FuncionarioTelefone { get; set; }

        public DateTime FuncionarioDataNas { get; set; }

        public List<VendaFinalModel> Vendas { get; set; } = new List<VendaFinalModel>();
    }
}
