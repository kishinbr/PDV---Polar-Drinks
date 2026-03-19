using Microsoft.AspNetCore.Mvc;
using pim3semestre.Data;
using pim3semestre.Models;

namespace pim3semestre.Controllers
{
    public class CategoriaController : Controller
    {
        //atibuto para acessar o banco de dados , por leitura somente
        readonly ApplicationDbContext _db;

        //construtor para injetar o banco de dados
        public CategoriaController(ApplicationDbContext db)
        {
            _db = db;
        }


        //acao default quando acessar a rota /Categoria , listando as categorias disponiveis
        public IActionResult Index()
        {
            IEnumerable<CategoriaModel> categorias = _db.Categorias;

            return View(categorias);
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CategoriaModel? categoria = _db.Categorias.FirstOrDefault(x => x.CategoriaID == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost]

        public IActionResult Editar(CategoriaModel categoria)
        {
            if (ModelState.IsValid)
            {
                _db.Categorias.Update(categoria);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        [HttpGet]
        public IActionResult Excluir(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            CategoriaModel? categoria = _db.Categorias.FirstOrDefault(x => x.CategoriaID == id);

            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        public IActionResult Excluir(CategoriaModel categoria)
        {
            if(categoria == null)
            {
                return NotFound();
            }

            _db.Categorias.Remove(categoria);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Cadastrar(CategoriaModel categoria)
        {
            if(ModelState.IsValid)
            {
                _db.Categorias.Add(categoria);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
