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
        public IActionResult Index(DateTime? dataInicio, DateTime? dataFim)
        {
            var vendasQuery = _db.Vendas
                                 .Include(v => v.Itens)
                                 .AsQueryable();

            if (dataInicio.HasValue)
                vendasQuery = vendasQuery.Where(v => v.VendaData.Date >= dataInicio.Value.Date);

            if (dataFim.HasValue)
                vendasQuery = vendasQuery.Where(v => v.VendaData.Date <= dataFim.Value.Date);

            var vendas = vendasQuery.OrderByDescending(v => v.VendaData).ToList();
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
                decimal totalVenda = 0;

                // ================= VALIDAR + CALCULAR COM DESCONTO =================
                foreach (var item in venda.Itens)
                {
                    var produto = _db.Produtos.FirstOrDefault(p => p.ProdutoID == item.ProdutoID);

                    if (produto == null)
                    {
                        TempData["MensagemErro"] = $"Produto não encontrado: ID {item.ProdutoID}";
                        transaction.Rollback();
                        ViewBag.Produtos = _db.Produtos.Where(p => p.ProdutoAtivo).ToList();
                        return View("Cadastrar", venda);
                    }

                    if ((produto.ProdutoQtdEstoque ?? 0) < item.ItemVendaQtd)
                    {
                        TempData["MensagemErro"] = $"Estoque insuficiente para: {produto.ProdutoNome}";
                        transaction.Rollback();
                        ViewBag.Produtos = _db.Produtos.Where(p => p.ProdutoAtivo).ToList();
                        return View("Cadastrar", venda);
                    }

                    decimal precoBase = produto.ProdutoPrecoVenda ?? 0;
                    decimal desconto = produto.ProdutoPromocao;

                    decimal precoFinal = precoBase;

                    if (desconto > 0)
                    {
                        precoFinal = precoBase - (precoBase * (desconto / 100));
                    }

                    // Atualiza item com valor correto (IGNORA o front)
                    item.ItemVendaPreco = precoFinal;
                    item.ItemVendaTotal = precoFinal * item.ItemVendaQtd;

                    totalVenda += item.ItemVendaTotal;
                }

                // ================= TOTAL FINAL =================
                venda.VendaValorTotal = totalVenda;

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
                ViewBag.Produtos = _db.Produtos.Where(p => p.ProdutoAtivo).ToList();
                return View("Cadastrar", venda);
            }
        }
    }
}