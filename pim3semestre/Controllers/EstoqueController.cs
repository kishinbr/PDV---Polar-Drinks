using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pim3semestre.Data;
using pim3semestre.Models;
using System.Diagnostics;

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
            var produtos = _db.Produtos
                .Include(p => p.Categoria)
                .ToList();

            return View(produtos);
        }


        public IActionResult Cadastrar()
        {
            ViewBag.Categorias = new SelectList(
                _db.Categorias.Where(c => c.CategoriaAtiva).ToList(),
                "CategoriaID",
                "CategoriaNome"
            );

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

            ViewBag.Categorias = new SelectList(
                _db.Categorias.Where(c => c.CategoriaAtiva).ToList(),
                "CategoriaID",
                "CategoriaNome"
            );

            return View();
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

            // Dropdown apenas com categorias ativas
            ViewBag.Categorias = new SelectList(
                _db.Categorias.Where(c => c.CategoriaAtiva).ToList(),
                "CategoriaID",
                "CategoriaNome"
            );

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(ProdutoModel produto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(
                    _db.Categorias.Where(c => c.CategoriaAtiva).ToList(),
                    "CategoriaID",
                    "CategoriaNome"
                );
                return View(produto);
            }

            var produtoDb = _db.Produtos.FirstOrDefault(p => p.ProdutoID == produto.ProdutoID);
            if (produtoDb == null) return NotFound();

            int quantidadeAntiga = produtoDb.ProdutoQtdEstoque ?? 0;
            int quantidadeNova = produto.ProdutoQtdEstoque ?? 0;
            int diferenca = quantidadeNova - quantidadeAntiga;

            if (diferenca != 0)
            {
                var movimentacao = new MovimentacaoEstoqueModel
                {
                    ProdutoID = produtoDb.ProdutoID,
                    MovimentacaoQtd = Math.Abs(diferenca),
                    MovimentacaoData = DateTime.Now,
                    MovimentacaoTipo = "Edicao"
                };
                _db.MovimentacoesEstoque.Add(movimentacao);
            }

            produtoDb.ProdutoNome = produto.ProdutoNome;
            produtoDb.ProdutoDescricao = produto.ProdutoDescricao;
            produtoDb.ProdutoCodBarra = produto.ProdutoCodBarra;
            produtoDb.ProdutoPrecoVenda = produto.ProdutoPrecoVenda;
            produtoDb.ProdutoQtdEstoque = produto.ProdutoQtdEstoque;
            produtoDb.CategoriaID = produto.CategoriaID;
            produtoDb.ProdutoAtivo = produto.ProdutoAtivo;
            produtoDb.ProdutoEstoqueMinimo= produto.ProdutoEstoqueMinimo;
            produtoDb.ProdutoPrecoCusto = produto.ProdutoPrecoCusto;

            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Produto atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EdicaoRapida(int ProdutoID, decimal ProdutoPrecoVenda, decimal ProdutoPromocao)
        {
            var produto = _db.Produtos.FirstOrDefault(p => p.ProdutoID == ProdutoID);
            if (produto != null)
            {
                produto.ProdutoPrecoVenda = ProdutoPrecoVenda;
                produto.ProdutoPromocao = ProdutoPromocao;

                _db.SaveChanges();
                TempData["MensagemSucesso"] = "Produto atualizado com sucesso!";
            }
            return RedirectToAction("Index");
        }
    }
}