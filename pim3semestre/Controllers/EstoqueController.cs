using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pim3semestre.Data;
using pim3semestre.Models;

namespace pim3semestre.Controllers
{
    public class EstoqueController : Controller
    {
        readonly ApplicationDbContext _db;

        public EstoqueController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var produtos = _db.Produtos.ToList();
            return View(produtos);
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(ProdutoModel produto)
        {
            if (ModelState.IsValid)
            {
                _db.Produtos.Add(produto);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Produto cadastrado com sucesso!";
                return RedirectToAction("Index");
            }

            return View(produto);
        }

        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var produto = _db.Produtos.FirstOrDefault(p => p.ProdutoID == id);
            if (produto == null) return NotFound();

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(ProdutoModel produto)
        {
            if (!ModelState.IsValid)
            {
                var produtoOriginal = _db.Produtos
                    .FirstOrDefault(p => p.ProdutoID == produto.ProdutoID);

                return View(produtoOriginal);
            }

            var produtoDb = _db.Produtos.FirstOrDefault(p => p.ProdutoID == produto.ProdutoID);
            if (produtoDb == null) return NotFound();


            produtoDb.ProdutoNome = produto.ProdutoNome;
            produtoDb.ProdutoDescricao = produto.ProdutoDescricao;
            produtoDb.ProdutoCodBarra = produto.ProdutoCodBarra;
            produtoDb.ProdutoPrecoVenda = produto.ProdutoPrecoVenda;
            produtoDb.ProdutoAtivo = produto.ProdutoAtivo;
            produtoDb.ProdutoEstoqueMinimo = produto.ProdutoEstoqueMinimo;
            produtoDb.ProdutoPrecoCusto = produto.ProdutoPrecoCusto;
            produtoDb.ProdutoPromocao = produto.ProdutoPromocao;
            produtoDb.ProdutoQtdEstoque = produto.ProdutoQtdEstoque;


            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Produto atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EdicaoRapida(int ProdutoID, decimal? ProdutoPrecoVenda, decimal? ProdutoPromocao)
        {
            if (!ModelState.IsValid)
            {
                TempData["MensagemErro"] = "Valores inválidos!";
                return RedirectToAction("Index");
            }

            var produto = _db.Produtos.FirstOrDefault(p => p.ProdutoID == ProdutoID);

            if (produto == null)
            {
                TempData["MensagemErro"] = "Produto não encontrado!";
                return RedirectToAction("Index");
            }

            if (ProdutoPrecoVenda == null || ProdutoPrecoVenda < 0)
            {
                TempData["MensagemErro"] = "Preço inválido!";
                return RedirectToAction("Index");
            }

            if (ProdutoPromocao == null || ProdutoPromocao < 0 || ProdutoPromocao > 100)
            {
                TempData["MensagemErro"] = "Promoção inválida!";
                return RedirectToAction("Index");
            }

            produto.ProdutoPrecoVenda = ProdutoPrecoVenda.Value;
            produto.ProdutoPromocao = ProdutoPromocao ?? 0;

            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Produto atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AjustarEstoque(int ProdutoID, int NovaQuantidade, string Descricao)
        {
            var produto = _db.Produtos.FirstOrDefault(p => p.ProdutoID == ProdutoID);
            if (produto == null) return NotFound();

            int quantidadeAntiga = produto.ProdutoQtdEstoque ?? 0;
            int diferenca = NovaQuantidade - quantidadeAntiga;

            if (diferenca != 0)
            {
                var movimentacao = new MovimentacaoEstoqueModel
                {
                    ProdutoID = produto.ProdutoID,

                    MovimentacaoQtd = diferenca,

                    MovimentacaoData = DateTime.Now,
                    MovimentacaoTipo = "Edicao",
                    MovimentacaoDescricao = Descricao
                };

                _db.MovimentacoesEstoque.Add(movimentacao);
            }

            produto.ProdutoQtdEstoque = NovaQuantidade;

            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Estoque ajustado com sucesso!";
            return RedirectToAction("Editar", new { id = ProdutoID });
        }

        public IActionResult Movimentacoes(int? produtoId)
        {
            var produtos = _db.Produtos
                .Where(p => p.ProdutoAtivo)
                .ToList();

            ViewBag.Produtos = produtos;

            if (produtoId == null)
            {
                return View(new List<MovimentacaoEstoqueModel>());
            }
            var produto = _db.Produtos.FirstOrDefault(p => p.ProdutoID == produtoId);

            if (produto == null)
            {
                return View(new List<MovimentacaoEstoqueModel>());
            }
            var movimentacoes = _db.MovimentacoesEstoque
                .Where(m => m.ProdutoID == produtoId)
                .OrderByDescending(m => m.MovimentacaoData)
                .ToList();

            ViewBag.ProdutoSelecionado = produto;

            return View(movimentacoes);
        }
    }
}