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
        //atibuto para acessar o banco de dados , por leitura somente
        readonly ApplicationDbContext _db;

        //construtor para injetar o banco de dados
        public EstoqueController(ApplicationDbContext db)
        {
            _db = db;
        }
        //acao default quando acessar a rota /Estoque , listando os produtos disponiveis
        public IActionResult Index()
        {
            // usando o método Include para carregar os dados relacionados da categoria junto com os produtos
            var produtos = _db.Produtos
                .Include(p => p.Categoria)
                .ToList();

            return View(produtos);
        }
        public IActionResult Cadastrar()
        {
            ViewBag.Categorias = new SelectList(_db.Categorias, "CategoriaID", "CategoriaNome");

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

        
            ViewBag.Categorias = new SelectList(_db.Categorias, "CategoriaID", "CategoriaNome");

            return View();
        }
    }
    
}
