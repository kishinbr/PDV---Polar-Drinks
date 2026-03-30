using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using pim3semestre.Data;
using pim3semestre.Models;

namespace pim3semestre.Controllers
{
    public class VendaController : Controller
    {
        readonly ApplicationDbContext _db;

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
    }
}