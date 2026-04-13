using System.ComponentModel.DataAnnotations;

namespace pim3semestre.Models
{
    public class MovimentacaoEstoqueModel
    {
        public static class Tipos
        {
            public const string Entrada = "Entrada";
            public const string Saida = "Saida";
            public const string Edicao = "Edicao";
        }

        [Key]
        public int MovimentacaoID { get; set; }

        public string? MovimentacaoTipo { get; set; }

        public int MovimentacaoQtd { get; set; }

        public string? MovimentacaoDescricao { get; set; }

        public DateTime MovimentacaoData { get; set; } = DateTime.Now;

        public int ProdutoID { get; set; }
        public ProdutoModel? Produto { get; set; }

        public int? ItemCompraID { get; set; }
        public ItemCompraModel? ItemCompra { get; set; }

        public int? ItemVendaID { get; set; }
        public ItemVendaModel? ItemVenda { get; set; }
    }
}