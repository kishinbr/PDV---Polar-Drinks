namespace pim3semestre.Models
{
    public class DashboardViewModel
    {
        // =========================
        // 💰 FINANCEIRO
        // =========================
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

        // =========================
        // 💰 LUCRO
        // =========================
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

        // =========================
        // 💳 PAGAMENTOS (CARDS - HOJE)
        // =========================
        public int QtdPix { get; set; }
        public int QtdCartao { get; set; }
        public int QtdDinheiro { get; set; }

        public decimal TotalPix { get; set; }
        public decimal TotalCartao { get; set; }
        public decimal TotalDinheiro { get; set; }

        // =========================
        // 💳 PAGAMENTOS (GRÁFICO)
        // =========================
        public decimal PixHoje { get; set; }
        public decimal CartaoHoje { get; set; }
        public decimal DinheiroHoje { get; set; }

        public decimal PixSemana { get; set; }
        public decimal CartaoSemana { get; set; }
        public decimal DinheiroSemana { get; set; }

        public decimal PixMes { get; set; }
        public decimal CartaoMes { get; set; }
        public decimal DinheiroMes { get; set; }

        public decimal PixTotal { get; set; }
        public decimal CartaoTotal { get; set; }
        public decimal DinheiroTotal { get; set; }

        // =========================
        // 📈 VENDAS (GRÁFICOS)
        // =========================
        public List<decimal> VendasHojeLista { get; set; } = new();   // por hora
        public List<decimal> VendasSemana { get; set; } = new();      // últimos 7 dias
        public List<decimal> VendasMesGrafico { get; set; } = new();  // últimos 30 dias
        public List<decimal> VendasAno { get; set; } = new();         // por mês

        // =========================
        // 🔮 ML SIMPLES
        // =========================
        public decimal PrevisaoAmanha { get; set; }
    }
}