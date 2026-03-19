using System.ComponentModel.DataAnnotations;

namespace pim3semestre.Models
{
    public class FornecedorModel
    {
        [Key]
        public int FornecedorID { get; set; }

        public string? FornecedorNome { get; set; }

        public string? FornecedorCNPJ { get; set; }

        public string? FornecedorTelefone { get; set; }

        public string? FornecedorEmail { get; set; }

        public string? FornecedorCEP { get; set; }

        public string? FornecedorEstado { get; set; }

        public string? FornecedorCidade { get; set; }

        public string? FornecedorBairro { get; set; }

        public string? FornecedorLogradouro { get; set; }

        public string? FornecedorNum { get; set; }

     
        public List<CompraEstoqueModel> Compras { get; set; } = new List<CompraEstoqueModel>();
    }
}
