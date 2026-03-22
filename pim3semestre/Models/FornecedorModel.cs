using System.ComponentModel.DataAnnotations;

namespace pim3semestre.Models
{
    public class FornecedorModel
    {
        [Key]
        public int FornecedorID { get; set; }

        [Required(ErrorMessage = "O nome do fornecedor é obrigatório")]
        [StringLength(150)]
        public string? FornecedorNome { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [StringLength(18, ErrorMessage = "O CNPJ deve ter no máximo 18 caracteres")]
        public string? FornecedorCNPJ { get; set; }


        [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres")]
        [Required(ErrorMessage = "O Telefone é obrigatório")]
        public string? FornecedorTelefone { get; set; }

        [Required(ErrorMessage = "O emaiol é obrigatório")]
        [StringLength(150)]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? FornecedorEmail { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório")]
        [StringLength(10, ErrorMessage = "CEP deve ter no máximo 10 caracteres")]
        public string? FornecedorCEP { get; set; }

        [StringLength(50)]
        public string? FornecedorEstado { get; set; }

        [StringLength(100)]
        public string? FornecedorCidade { get; set; }

        [StringLength(100)]
        public string? FornecedorBairro { get; set; }

        [StringLength(150)]
        public string? FornecedorLogradouro { get; set; }

        [StringLength(10)]
        public string? FornecedorNum { get; set; }

        public List<CompraEstoqueModel> Compras { get; set; } = new List<CompraEstoqueModel>();
    }
}