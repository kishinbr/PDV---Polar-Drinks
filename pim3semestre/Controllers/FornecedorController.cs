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
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var fornecedor = _db.Fornecedores.FirstOrDefault(x => x.FornecedorID == id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(FornecedorModel fornecedor)
        {
            if (!ModelState.IsValid)
            {
                TempData["MensagemErro"] = "Erro ao editar fornecedor.";
                return View(fornecedor);
            }

            var fornecedorDb = _db.Fornecedores.FirstOrDefault(x => x.FornecedorID == fornecedor.FornecedorID);

            if (fornecedorDb == null)
            {
                return NotFound();
            }

            // Atualizando campos
            fornecedorDb.FornecedorNome = fornecedor.FornecedorNome;
            fornecedorDb.FornecedorCNPJ = fornecedor.FornecedorCNPJ;
            fornecedorDb.FornecedorTelefone = fornecedor.FornecedorTelefone;
            fornecedorDb.FornecedorEmail = fornecedor.FornecedorEmail;
            fornecedorDb.FornecedorCEP = fornecedor.FornecedorCEP;
            fornecedorDb.FornecedorCidade = fornecedor.FornecedorCidade;
            fornecedorDb.FornecedorEstado = fornecedor.FornecedorEstado;
            fornecedorDb.FornecedorBairro = fornecedor.FornecedorBairro;
            fornecedorDb.FornecedorLogradouro = fornecedor.FornecedorLogradouro;
            fornecedorDb.FornecedorNum = fornecedor.FornecedorNum;
            fornecedorDb.FornecedorAtivo = fornecedor.FornecedorAtivo;

            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Fornecedor editado com sucesso!";
            return RedirectToAction("Index");
        }


        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(FornecedorModel fornecedor)
        {
            if (!ModelState.IsValid)
            {
                TempData["MensagemErro"] = "Preencha todos os campos corretamente.";
                return View(fornecedor);
            }

            bool cnpjExiste = _db.Fornecedores
                .Any(x => x.FornecedorCNPJ == fornecedor.FornecedorCNPJ);

            if (cnpjExiste)
            {
                ModelState.AddModelError("FornecedorCNPJ", "Este CNPJ já está cadastrado.");
                return View(fornecedor);
            }

            _db.Fornecedores.Add(fornecedor);
            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Fornecedor cadastrado com sucesso!";
            return RedirectToAction("Index");
        }
    }
}
