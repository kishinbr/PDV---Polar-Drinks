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

        public IActionResult Cadastrar()
        {
            var produtos = _db.Produtos
                .Where(p => p.ProdutoAtivo)
                .ToList();

            ViewBag.Produtos = produtos;

            return View();
        }

        [HttpPost]
        public IActionResult FinalizarVenda([FromBody] VendaFinalModel venda)
        {
            if (venda == null || venda.Itens.Count == 0)
            {
                return BadRequest("Venda inválida");
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
                        return BadRequest("Produto não encontrado.");
                    }

                    if (produto.ProdutoQtdEstoque < item.ItemVendaQtd)
                    {
                        return BadRequest($"Estoque insuficiente para: {produto.ProdutoNome}");
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

                // 🔥 CONFIRMA TUDO
                transaction.Commit();

                return Ok(new { mensagem = "Venda realizada com sucesso!" });
            }
            catch (Exception ex)
            {
                // 🔥 DESFAZ TUDO
                transaction.Rollback();

                return BadRequest($"Erro ao salvar venda: {ex.Message}");
            }
        }
    }
}