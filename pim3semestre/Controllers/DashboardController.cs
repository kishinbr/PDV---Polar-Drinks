using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pim3semestre.Data;
using pim3semestre.Models;

namespace pim3semestre.Controllers
{
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

            var vendas = _db.Vendas
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .ToList();

            var produtos = _db.Produtos.ToList();

            var model = new DashboardViewModel();

            // =========================
            // 💰 FINANCEIRO
            // =========================
            model.TotalHoje = vendas
                .Where(v => v.VendaData.Date == hoje)
                .Sum(v => v.VendaValorTotal);

            model.TotalMes = vendas
                .Where(v => v.VendaData >= inicioMes)
                .Sum(v => v.VendaValorTotal);

            model.TotalAno = vendas
                .Where(v => v.VendaData >= inicioAno)
                .Sum(v => v.VendaValorTotal);

            model.TicketMedio = vendas.Any()
                ? vendas.Average(v => v.VendaValorTotal)
                : 0;

            // =========================
            // 📊 BASE
            // =========================
            var vendasHoje = vendas.Where(v => v.VendaData.Date == hoje).ToList();
            var vendasMes = vendas.Where(v => v.VendaData >= inicioMes).ToList();

            // =========================
            // 💰 LUCRO
            // =========================
            model.LucroHoje = vendasHoje.Sum(v =>
                v.Itens.Sum(i =>
                    (i.ItemVendaPreco - (i.Produto.ProdutoPrecoCusto ?? 0)) * i.ItemVendaQtd
                )
            );

            model.LucroMes = vendasMes.Sum(v =>
                v.Itens.Sum(i =>
                    (i.ItemVendaPreco - (i.Produto.ProdutoPrecoCusto ?? 0)) * i.ItemVendaQtd
                )
            );

            // =========================
            // 📊 VENDAS
            // =========================
            model.VendasHoje = vendasHoje.Count;
            model.VendasMes = vendasMes.Count;

            model.DiaMaisVendas = vendas
                .GroupBy(v => v.VendaData.DayOfWeek)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key.ToString())
                .FirstOrDefault();

            // =========================
            // 💳 PAGAMENTOS (CARDS HOJE)
            // =========================
            model.QtdPix = vendasHoje.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "pix");
            model.QtdCartao = vendasHoje.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "cartão");
            model.QtdDinheiro = vendasHoje.Count(v => (v.VendaTipoPagamento ?? "").ToLower() == "dinheiro");

            model.TotalPix = vendasHoje.Where(v => (v.VendaTipoPagamento ?? "").ToLower() == "pix").Sum(v => v.VendaValorTotal);
            model.TotalCartao = vendasHoje.Where(v => (v.VendaTipoPagamento ?? "").ToLower() == "cartão").Sum(v => v.VendaValorTotal);
            model.TotalDinheiro = vendasHoje.Where(v => (v.VendaTipoPagamento ?? "").ToLower() == "dinheiro").Sum(v => v.VendaValorTotal);

            // =========================
            // 💳 PAGAMENTOS (GRÁFICOS)
            // =========================
            Func<string, DateTime?, int> totalQtd = (tipo, dataInicio) =>
                vendas.Count(v =>
                    (v.VendaTipoPagamento ?? "").ToLower() == tipo &&
                    (dataInicio == null || v.VendaData >= dataInicio)
                );

            // HOJE
            model.PixHoje = totalQtd("pix", hoje);
            model.CartaoHoje = totalQtd("cartão", hoje);
            model.DinheiroHoje = totalQtd("dinheiro", hoje);

            // SEMANA
            var inicioSemana = hoje.AddDays(-7);
            model.PixSemana = totalQtd("pix", inicioSemana);
            model.CartaoSemana = totalQtd("cartão", inicioSemana);
            model.DinheiroSemana = totalQtd("dinheiro", inicioSemana);

            // MÊS (30 dias)
            var inicio30 = hoje.AddDays(-30);
            model.PixMes = totalQtd("pix", inicio30);
            model.CartaoMes = totalQtd("cartão", inicio30);
            model.DinheiroMes = totalQtd("dinheiro", inicio30);

            // TOTAL
            model.PixTotal = totalQtd("pix", null);
            model.CartaoTotal = totalQtd("cartão", null);
            model.DinheiroTotal = totalQtd("dinheiro", null);

            // =========================
            // 📦 ESTOQUE
            // =========================
            model.TotalProdutos = produtos.Count();
            model.SemEstoque = produtos.Count(p => (p.ProdutoQtdEstoque ?? 0) == 0);
            model.EstoqueBaixo = produtos.Count(p =>
                (p.ProdutoQtdEstoque ?? 0) <= p.ProdutoEstoqueMinimo);

            model.ProdutoMaisVendido = vendas
                .SelectMany(v => v.Itens)
                .GroupBy(i => i.Produto.ProdutoNome)
                .OrderByDescending(g => g.Sum(x => x.ItemVendaQtd))
                .Select(g => g.Key)
                .FirstOrDefault();

            // =========================
            // 📈 EXTRAS
            // =========================
            model.ItensVendidosHoje = vendasHoje
                .SelectMany(v => v.Itens)
                .Sum(i => i.ItemVendaQtd);

            model.FaturamentoHoje = model.TotalHoje;

            // =========================
            // 🔮 ML
            // =========================
            var ultimos7Dias = vendas.Where(v => v.VendaData >= hoje.AddDays(-7)).ToList();

            model.PrevisaoAmanha = ultimos7Dias.Any()
                ? ultimos7Dias.Average(v => v.VendaValorTotal)
                : 0;

            model.ProdutoMaisLucrativo = vendas
                .SelectMany(v => v.Itens)
                .GroupBy(i => i.Produto.ProdutoNome)
                .OrderByDescending(g => g.Sum(i =>
                    (i.ItemVendaPreco - (i.Produto.ProdutoPrecoCusto ?? 0)) * i.ItemVendaQtd
                ))
                .Select(g => g.Key)
                .FirstOrDefault();

            // =========================
            // 📈 VENDAS (GRÁFICOS)
            // =========================

            // HOJE (por hora)
            model.VendasHojeLista = vendasHoje
                .GroupBy(v => v.VendaData.Hour)
                .OrderBy(g => g.Key)
                .Select(g => g.Count()) // 🔥 quantidade de vendas
                .Select(x => (decimal)x)
                .ToList();

            // SEMANA
            model.VendasSemana = vendas
                .Where(v => v.VendaData >= hoje.AddDays(-7))
                .GroupBy(v => v.VendaData.Date)
                .OrderBy(g => g.Key)
                .Select(g => g.Count()) // 🔥 quantidade de vendas
                .Select(x => (decimal)x)
                .ToList();

            // MÊS (30 dias)
            model.VendasMesGrafico = vendas
                .Where(v => v.VendaData >= hoje.AddDays(-30))
                .GroupBy(v => v.VendaData.Date)
                .OrderBy(g => g.Key)
                .Select(g => g.Count()) // 🔥 quantidade de vendas
                .Select(x => (decimal)x)
                .ToList();

            // ANO (por mês)
            model.VendasAno = vendas
                .Where(v => v.VendaData.Year == hoje.Year)
                .GroupBy(v => v.VendaData.Month)
                .OrderBy(g => g.Key)
                .Select(g => g.Count()) // 🔥 quantidade de vendas
                .Select(x => (decimal)x)
                .ToList();

            return View(model);
        }
    }
}