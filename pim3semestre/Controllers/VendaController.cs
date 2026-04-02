using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pim3semestre.Data;
using pim3semestre.Models;

namespace pim3semestre.Controllers
{
    public class VendaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VendaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Historico(DateTime? dataInicio, DateTime? dataFim)
        {
            var query = _db.Vendas
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .AsQueryable();

            if (dataInicio.HasValue)
                query = query.Where(v => v.VendaData >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(v => v.VendaData <= dataFim.Value);

            var vendas = query
                .OrderByDescending(v => v.VendaData)
                .ToList();

            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;

            return View(vendas);
        }
        public IActionResult Detalhes(int id)
        {
            var venda = _db.Vendas
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefault(v => v.VendaID == id);

            if (venda == null)
                return NotFound();

            return View(venda);
        }
        public IActionResult Cadastrar()
        {
            var produtos = _db.Produtos
                .Where(p => p.ProdutoAtivo)
                .ToList();

            ViewBag.Produtos = produtos;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FinalizarVenda(VendaFinalModel venda)
        {
            // ================= VALIDAÇÕES =================
            if (venda == null || venda.Itens.Count == 0)
            {
                TempData["MensagemErro"] = "Adicione pelo menos um item à venda.";
                var produtos = _db.Produtos.Where(p => p.ProdutoAtivo).ToList();
                ViewBag.Produtos = produtos;
                return View("Cadastrar", venda);
            }

            if (string.IsNullOrEmpty(venda.VendaTipoPagamento))
            {
                TempData["MensagemErro"] = "Selecione um tipo de pagamento.";
                var produtos = _db.Produtos.Where(p => p.ProdutoAtivo).ToList();
                ViewBag.Produtos = produtos;
                return View("Cadastrar", venda);
            }

            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // ================= VALIDAR ESTOQUE =================
                foreach (var item in venda.Itens)
                {
                    var produto = _db.Produtos.FirstOrDefault(p => p.ProdutoID == item.ProdutoID);
                    if (produto == null)
                    {
                        TempData["MensagemErro"] = $"Produto não encontrado: ID {item.ProdutoID}";
                        transaction.Rollback();
                        var produtos = _db.Produtos.Where(p => p.ProdutoAtivo).ToList();
                        ViewBag.Produtos = produtos;
                        return View("Cadastrar", venda);
                    }

                    if (produto.ProdutoQtdEstoque < item.ItemVendaQtd)
                    {
                        TempData["MensagemErro"] = $"Estoque insuficiente para: {produto.ProdutoNome}";
                        transaction.Rollback();
                        var produtos = _db.Produtos.Where(p => p.ProdutoAtivo).ToList();
                        ViewBag.Produtos = produtos;

                        return View("Cadastrar", venda);
                    }
                }

                // ================= CALCULAR TOTAL =================
                venda.VendaValorTotal = venda.Itens.Sum(i => i.ItemVendaTotal);

                // ================= SALVAR VENDA =================
                _db.Vendas.Add(venda);
                _db.SaveChanges();

                // ================= BAIXAR ESTOQUE =================
                foreach (var item in venda.Itens)
                {
                    var produto = _db.Produtos.First(p => p.ProdutoID == item.ProdutoID);
                    produto.ProdutoQtdEstoque -= item.ItemVendaQtd;

                    var movimentacao = new MovimentacaoEstoqueModel
                    {
                        ProdutoID = produto.ProdutoID,
                        MovimentacaoQtd = item.ItemVendaQtd,
                        MovimentacaoTipo = "Saida",
                        MovimentacaoData = DateTime.Now,
                        ItemVendaID = item.ItemVendaID
                    };

                    _db.MovimentacoesEstoque.Add(movimentacao);
                }

                _db.SaveChanges();
                transaction.Commit();

                TempData["MensagemSucesso"] = "Venda realizada com sucesso!";

                return RedirectToAction("Cadastrar");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                TempData["MensagemErro"] = $"Erro ao salvar venda: {ex.Message}";
                var produtos = _db.Produtos.Where(p => p.ProdutoAtivo).ToList();
                ViewBag.Produtos = produtos;
                return View("Cadastrar", venda);
            }
        }
    }
}