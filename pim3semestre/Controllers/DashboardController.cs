using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pim3semestre.Data;
using pim3semestre.Filters;
using pim3semestre.Models;

namespace pim3semestre.Controllers
{
    [AuthFilter]
    [AdminFilter]
    public class DashboardController : Controller
    {
        readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
            var inicioAno = new DateTime(hoje.Year, 1, 1);
            var inicio7Dias = hoje.AddDays(-7);
            var inicio30Dias = hoje.AddDays(-30);

            var vendasHoje = _db.Vendas
                .Include(v => v.Itens).ThenInclude(i => i.Produto)
                .Where(v => v.VendaData.Date == hoje)
                .ToList();

            var vendasMes = _db.Vendas
                .Include(v => v.Itens).ThenInclude(i => i.Produto)
                .Where(v => v.VendaData >= inicioMes)
                .ToList();

            var vendasAno = _db.Vendas
                .Include(v => v.Itens).ThenInclude(i => i.Produto)
                .Where(v => v.VendaData >= inicioAno)
                .ToList();

            var vendas7Dias = _db.Vendas
                .Include(v => v.Itens).ThenInclude(i => i.Produto)
                .Where(v => v.VendaData.Date >= hoje.AddDays(-6))
                .ToList();

            var vendas30Dias = _db.Vendas
                .Include(v => v.Itens).ThenInclude(i => i.Produto)
                .Where(v => v.VendaData >= inicio30Dias)
                .ToList();

            var todasVendas = _db.Vendas
                .Include(v => v.Itens).ThenInclude(i => i.Produto)
                .ToList();

            var produtos = _db.Produtos.ToList();

            var model = new DashboardViewModel();

            // FINANCEIRO
            model.TotalHoje = vendasHoje.Sum(v => v.VendaValorTotal);
            model.TotalMes = vendasMes.Sum(v => v.VendaValorTotal);

            model.LucroHoje = vendasHoje.Sum(v =>
                v.Itens.Sum(i =>
                    (i.ItemVendaPreco - (i.Produto.ProdutoPrecoCusto ?? 0)) * i.ItemVendaQtd));

            model.LucroMes = vendasMes.Sum(v =>
                v.Itens.Sum(i =>
                    (i.ItemVendaPreco - (i.Produto.ProdutoPrecoCusto ?? 0)) * i.ItemVendaQtd));

            // PAGAMENTOS (CARDS - HOJE)
            model.QtdPix = vendasHoje.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "pix");
            model.QtdCartao = vendasHoje.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "cartão");
            model.QtdDinheiro = vendasHoje.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "dinheiro");

            model.TotalPix = vendasHoje.Where(v => (v.VendaTipoPagamento ?? "").ToLower() == "pix").Sum(v => v.VendaValorTotal);
            model.TotalCartao = vendasHoje.Where(v => (v.VendaTipoPagamento ?? "").ToLower() == "cartão").Sum(v => v.VendaValorTotal);
            model.TotalDinheiro = vendasHoje.Where(v => (v.VendaTipoPagamento ?? "").ToLower() == "dinheiro").Sum(v => v.VendaValorTotal);

            // PAGAMENTOS (GRÁFICOS)
            model.PixHoje = vendasHoje.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "pix");
            model.CartaoHoje = vendasHoje.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "cartão");
            model.DinheiroHoje = vendasHoje.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "dinheiro");

            model.PixSemana = vendas7Dias.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "pix");
            model.CartaoSemana = vendas7Dias.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "cartão");
            model.DinheiroSemana = vendas7Dias.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "dinheiro");

            model.PixMes = vendas30Dias.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "pix");
            model.CartaoMes = vendas30Dias.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "cartão");
            model.DinheiroMes = vendas30Dias.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "dinheiro");

            model.PixTotal = todasVendas.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "pix");
            model.CartaoTotal = todasVendas.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "cartão");
            model.DinheiroTotal = todasVendas.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "dinheiro");

            // ESTOQUE
            model.SemEstoque = produtos.Count(p => p.ProdutoAtivo && (p.ProdutoQtdEstoque ?? 0) == 0);

            model.EstoqueBaixo = produtos.Count(p =>p.ProdutoAtivo && (p.ProdutoQtdEstoque ?? 0) <= p.ProdutoEstoqueMinimo);

            // PRODUTOS
            model.ProdutoMaisVendido = todasVendas
                .SelectMany(v => v.Itens)
                .GroupBy(i => i.Produto.ProdutoNome)
                .OrderByDescending(g => g.Sum(x => x.ItemVendaQtd))
                .Select(g => g.Key)
                .FirstOrDefault();

            model.ProdutoMaisLucrativo = todasVendas
                .SelectMany(v => v.Itens)
                .GroupBy(i => i.Produto.ProdutoNome)
                .OrderByDescending(g => g.Sum(i =>
                    (i.ItemVendaPreco - (i.Produto.ProdutoPrecoCusto ?? 0)) * i.ItemVendaQtd))
                .Select(g => g.Key)
                .FirstOrDefault();

            // PREVISÃO
                var vendasPorDia = vendas7Dias
                .GroupBy(v => v.VendaData.Date)
                .OrderBy(g => g.Key)
                .Select(g => g.Sum(v => v.VendaValorTotal))
                .ToList();

                //n = número de dias com vendas , max 7
                int n = vendasPorDia.Count;
                
                //n deve ser maior que 1 , pois para fazer uma reta de regressão linear , precisamos de pelo menos 2 pontos
                if (n > 1)
                {
                    //criando as variáveis para calcular a reta de regressão linear (y = ax + b)
                    double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

                    //para cada dia de venda , calculamos os valores de x e y para a reta de regressão linear
                    for (int i = 0; i < n; i++)
                    {
                        //x é o número do dia (1, 2, 3, ..., n) e y é o valor total das vendas desse dia
                        double x = i + 1; 
                        double y = (double)vendasPorDia[i];

                        //somatorias de x, y, xy e x^2 para calcular os coeficientes da reta de regressão linear
                        sumX += x;
                        sumY += y;
                        sumXY += x * y;
                        sumX2 += x * x;
                    }

                    //calculo dos coeficientes a e b da reta de regressão linear (y = ax + b)
                    double a = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
                    double b = (sumY - a * sumX) / n;

                    //previsão para o próximo dia (n + 1) usando a reta de regressão linear
                    double previsao = a * (n + 1) + b;

                    //garantindo que a previsão não seja negativa
                    model.PrevisaoAmanha = (decimal)Math.Max(previsao, 0);

                }
                //se não houver vendas suficientes para calcular a previsão, definimos como 0
                else
                {
                    model.PrevisaoAmanha = 0;

                }

                var vendasPorProduto = _db.Vendas
                .Include(v => v.Itens)
                    .ThenInclude(i => i.Produto)
                .ToList()
                .SelectMany(v => v.Itens)
                .Where(i => i.Produto != null)
                .GroupBy(i => i.ProdutoID)
                .Select(g => new
                {
                    ProdutoNome = g.First().Produto.ProdutoNome,
                    Dados = g.Select(x => new
                    {
                        Data = x.Venda.VendaData.Date,
                        Quantidade = x.ItemVendaQtd
                    })
                })
                .ToList();

                var tendencias = new List<(string Nome, double Slope)>();

                foreach (var p in vendasPorProduto)
                {
                    var dadosPorDia = p.Dados
                        .GroupBy(x => x.Data)
                        .OrderBy(x => x.Key)
                        .Select(g => g.Sum(x => x.Quantidade))
                        .ToList();

                    int quantidadeDias = dadosPorDia.Count;

                    if (quantidadeDias < 2)
                        continue;

                    double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

                    for (int i = 0; i < quantidadeDias; i++)
                    {
                        double x = i + 1;
                        double y = (double)dadosPorDia[i];

                        sumX += x;
                        sumY += y;
                        sumXY += x * y;
                        sumX2 += x * x;
                    }

                    double slope = (quantidadeDias * sumXY - sumX * sumY) /
                                   (quantidadeDias * sumX2 - sumX * sumX);

                    tendencias.Add((p.ProdutoNome, slope));
                }

                model.Top3ProdutosAlta = tendencias
                    .OrderByDescending(t => t.Slope)
                    .Take(3)
                    .Select(t => t.Nome)
                    .ToList();

            // GRÁFICOS
            model.VendasHojeLista = vendasHoje
                .GroupBy(v => v.VendaData.Hour)
                .OrderBy(g => g.Key)
                .Select(g => (decimal)g.Count())
                .ToList();

            var ultimos7Dias = Enumerable.Range(0, 7)
            .Select(i => hoje.AddDays(-i))
            .OrderBy(d => d)
            .ToList();

            var vendasAgrupadas = vendas7Dias
                .GroupBy(v => v.VendaData.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            model.VendasSemana = ultimos7Dias
                .Select(dia => (decimal)(vendasAgrupadas.ContainsKey(dia) ? vendasAgrupadas[dia] : 0))
                .ToList();

            var ultimos30Dias = Enumerable.Range(0, 30)
                .Select(i => hoje.AddDays(-i))
                .OrderBy(d => d)
                .ToList();

            var vendasAgrupadasMes = vendas30Dias
                .GroupBy(v => v.VendaData.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            model.VendasMesGrafico = ultimos30Dias
                .Select(dia => (decimal)(vendasAgrupadasMes.ContainsKey(dia) ? vendasAgrupadasMes[dia] : 0))
                .ToList();

            var mesesAno = Enumerable.Range(1, 12).ToList();

            var vendasAgrupadasAno = vendasAno
                .GroupBy(v => v.VendaData.Month)
                .ToDictionary(g => g.Key, g => g.Count());

            model.VendasAno = mesesAno
                .Select(mes => (decimal)(vendasAgrupadasAno.ContainsKey(mes) ? vendasAgrupadasAno[mes] : 0))
                .ToList();

            return View(model);
        }
    }
}