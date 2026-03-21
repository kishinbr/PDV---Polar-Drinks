using Microsoft.AspNetCore.Mvc;
using pim3semestre.Models;
using System.Diagnostics;

namespace pim3semestre.Controllers
{
    public class EstoqueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
    
}
