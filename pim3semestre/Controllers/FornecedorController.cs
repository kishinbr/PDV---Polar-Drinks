using Microsoft.AspNetCore.Mvc;
using pim3semestre.Data;
using pim3semestre.Models;

namespace pim3semestre.Controllers
{
    public class FornecedorController : Controller
    {
        //atibuto para acessar o banco de dados , por leitura somente
        readonly ApplicationDbContext _db;

        //construtor para injetar o banco de dados
        public FornecedorController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            IEnumerable<FornecedorModel> fornecedores = _db.Fornecedores;

            return View(fornecedores);
        }

        


        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FornecedorModel? fornecedor = _db.Fornecedores.FirstOrDefault(x => x.FornecedorID == id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        [HttpPost]
        public IActionResult Editar(FornecedorModel fornecedor)
        {
            if (ModelState.IsValid)
            {
                _db.Fornecedores.Update(fornecedor);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Fornecedor Editada com sucesso!";

                return RedirectToAction("Index");
            }
            TempData["MensagemErro"] = "Ocorreu algum erro ao Editar";
            return View(fornecedor);
        }

        [HttpGet]
        public IActionResult Excluir(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            FornecedorModel? fornecedor = _db.Fornecedores.FirstOrDefault(x => x.FornecedorID == id);

            if (fornecedor == null)
            {
                return NotFound();
            }
            return View(fornecedor);
        }

        [HttpPost]
        public IActionResult Excluir(FornecedorModel fornecedor)
        {
            if (fornecedor == null)
            {
                return NotFound();
            }

            _db.Fornecedores.Remove(fornecedor);
            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Fornecedor Excluido com sucesso!";
            return RedirectToAction("Index");
        }



        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(FornecedorModel fornecedor)
        {
            if (ModelState.IsValid)
            {
                _db.Fornecedores.Add(fornecedor);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Fornecedor Cadastrado com sucesso!";

                return RedirectToAction("Index");
            }
            TempData["MensagemErro"] = "Ocorreu algum erro ao Cadastrar";

            return View();
        }
    }
}
