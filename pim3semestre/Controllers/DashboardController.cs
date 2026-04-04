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
            // 💰 LUCRO
            // =========================
            var vendasHoje = vendas.Where(v => v.VendaData.Date == hoje).ToList();
            var vendasMes = vendas.Where(v => v.VendaData >= inicioMes).ToList();

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
            // 💳 PAGAMENTOS (HOJE)
            // =========================
            model.QtdPix = vendasHoje.Count(v =>
                (v.VendaTipoPagamento ?? "").ToLower() == "pix");

            model.QtdCartao = vendasHoje.Count(v =>
                (v.VendaTipoPagamento ?? "").ToLower() == "cartão");

            model.QtdDinheiro = vendasHoje.Count(v =>
                (v.VendaTipoPagamento ?? "").ToLower() == "dinheiro");


            model.TotalPix = vendasHoje
                .Where(v => (v.VendaTipoPagamento ?? "").ToLower() == "pix")
                .Sum(v => v.VendaValorTotal);

            model.TotalCartao = vendasHoje
                .Where(v => (v.VendaTipoPagamento ?? "").ToLower() == "cartão")
                .Sum(v => v.VendaValorTotal);

            model.TotalDinheiro = vendasHoje
                .Where(v => (v.VendaTipoPagamento ?? "").ToLower() == "dinheiro")
                .Sum(v => v.VendaValorTotal);
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
            // 🔮 ML 1 - PREVISÃO
            // =========================
            //var ultimos7Dias = vendas
            //    .Where(v => v.VendaData >= hoje.AddDays(-7))
            //    .ToList();

            //model.PrevisaoAmanha = ultimos7Dias.Any()
            //    ? ultimos7Dias.Average(v => v.VendaValorTotal)
            //    : 0;

            //var ultimos14Dias = vendas
            //    .Where(v => v.VendaData >= hoje.AddDays(-14))
            //    .ToList();

            //var semanaAtual = ultimos14Dias
            //    .Where(v => v.VendaData >= hoje.AddDays(-7))
            //    .Sum(v => v.VendaValorTotal);

            //var semanaAnterior = ultimos14Dias
            //    .Where(v => v.VendaData < hoje.AddDays(-7))
            //    .Sum(v => v.VendaValorTotal);

            model.ProdutoMaisLucrativo = vendas
             .SelectMany(v => v.Itens)
             .GroupBy(i => i.Produto.ProdutoNome)
             .OrderByDescending(g => g.Sum(i =>
                 (i.ItemVendaPreco - (i.Produto.ProdutoPrecoCusto ?? 0)) * i.ItemVendaQtd
             ))
             .Select(g => g.Key)
             .FirstOrDefault();

            var diasPassados = DateTime.DaysInMonth(hoje.Year, hoje.Month);
            var diasDecorridos = hoje.Day;

            var mediaDiaria = diasDecorridos > 0
                ? model.TotalMes / diasDecorridos
                : 0;

            model.PrevisaoMes = mediaDiaria * diasPassados;

            return View(model);
        }
    }
}