using Microsoft.AspNetCore.Mvc;
using pim3semestre.Filters;

namespace pim3semestre.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuario, string senha)
        {
            var usuarioConfig = _config["Auth:Usuario"];
            var senhaConfig = _config["Auth:Senha"];

            if (usuario == usuarioConfig && senha == senhaConfig)
            {
                HttpContext.Session.SetString("Logado", "true");
                HttpContext.Session.SetString("Usuario", usuario);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Erro = "Usuário ou senha inválidos";
            return View();
        }

        [HttpGet]
        public IActionResult Deslogar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}