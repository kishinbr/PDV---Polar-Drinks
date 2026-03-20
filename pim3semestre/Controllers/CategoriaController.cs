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


        // ao clicar em editar, o id referente a categoria é passado para a action, onde é feita uma consulta no banco de dados para encontrar a categoria com o id correspondente,
        // se a categoria for encontrada, ela é passada para a view para ser editada, caso contrário, é retornado um erro 404 (Not Found)
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

        // ao clicar em salvar, a categoria editada é enviada para a action, onde é verificado se os dados são válidos,
        // se forem, a categoria é atualizada no banco de dados e o usuário é redirecionado para a página de listagem de categorias, caso contrário,
        // a mesma view é retornada com os dados preenchidos para que o usuário possa corrigir os erros

        [HttpPost]
        public IActionResult Editar(CategoriaModel categoria)
        {
            if (ModelState.IsValid)
            {
                _db.Categorias.Update(categoria);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Categoria Editada com sucesso!";

                return RedirectToAction("Index");
            }
            TempData["MensagemErro"] = "Ocorreu algum erro ao Editar";
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

            TempData["MensagemSucesso"] = "Categoria Excluida com sucesso!";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Cadastrar(CategoriaModel categoria)
        {
            if(ModelState.IsValid)
            {
                _db.Categorias.Add(categoria);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Categoria Cadastrada com sucesso!";

                return RedirectToAction("Index");
            }
            TempData["MensagemErro"] = "Ocorreu algum erro ao Cadastrar";

            return View();
        }
    }
}
