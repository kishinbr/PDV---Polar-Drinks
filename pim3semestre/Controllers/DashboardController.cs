using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pim3semestre.Data;
using pim3semestre.Filters;
using pim3semestre.Models;

namespace pim3semestre.Controllers
{
    [AuthFilter]
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
                .Where(v => v.VendaData >= inicio7Dias)
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
            model.SemEstoque = produtos.Count(p => (p.ProdutoQtdEstoque ?? 0) == 0);
            model.EstoqueBaixo = produtos.Count(p => (p.ProdutoQtdEstoque ?? 0) <= p.ProdutoEstoqueMinimo);

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
            model.PrevisaoAmanha = vendas7Dias.Any()
                ? vendas7Dias.Average(v => v.VendaValorTotal)
                : 0;

            // GRÁFICOS
            model.VendasHojeLista = vendasHoje
                .GroupBy(v => v.VendaData.Hour)
                .OrderBy(g => g.Key)
                .Select(g => (decimal)g.Count())
                .ToList();

            model.VendasSemana = vendas7Dias
                .GroupBy(v => v.VendaData.Date)
                .OrderBy(g => g.Key)
                .Select(g => (decimal)g.Count())
                .ToList();

            model.VendasMesGrafico = vendas30Dias
                .GroupBy(v => v.VendaData.Date)
                .OrderBy(g => g.Key)
                .Select(g => (decimal)g.Count())
                .ToList();

            model.VendasAno = vendasAno
                .GroupBy(v => v.VendaData.Month)
                .OrderBy(g => g.Key)
                .Select(g => (decimal)g.Count())
                .ToList();

            return View(model);
        }
    }
}