namespace pim3semestre.Models
{
    public class DashboardViewModel
    {
        public decimal TotalHoje { get; set; }
        public decimal TotalMes { get; set; }
        public decimal TotalAno { get; set; }
        public decimal TicketMedio { get; set; }

        // =========================
        // 📊 VENDAS
        // =========================
        public int VendasHoje { get; set; }
        public int VendasMes { get; set; }
        public string? HorarioPico { get; set; }
        public string? DiaMaisVendas { get; set; }

        public decimal LucroHoje { get; set; }
        public decimal LucroMes { get; set; }

        public string? ProdutoMaisLucrativo { get; set; }
        public decimal PrevisaoMes { get; set; }

        // =========================
        // 📦 ESTOQUE
        // =========================
        public int EstoqueBaixo { get; set; }
        public int SemEstoque { get; set; }
        public int TotalProdutos { get; set; }
        public string? ProdutoMaisVendido { get; set; }

        // =========================
        // 🚨 ALERTAS
        // =========================
        public int ProdutosAbaixoMinimo { get; set; }
        public int EstoqueParado { get; set; }
        public decimal QuedaVendasPercentual { get; set; }

        // =========================
        // 📈 EXTRAS
        // =========================
        public int ItensVendidosHoje { get; set; }
        public decimal FaturamentoHoje { get; set; }
        public decimal MetaMesPercentual { get; set; }


        public int QtdPix { get; set; }
        public int QtdCartao { get; set; }
        public int QtdDinheiro { get; set; }

        public decimal TotalPix { get; set; }
        public decimal TotalCartao { get; set; }
        public decimal TotalDinheiro { get; set; }

        // =========================
        // 🔮 ML SIMPLES
        // =========================
        public decimal PrevisaoAmanha { get; set; }
    }
}
