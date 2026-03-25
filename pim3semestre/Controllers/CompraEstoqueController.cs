using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pim3semestre.Data;
using pim3semestre.Models;
using pim3semestre.ViewModels;

namespace pim3semestre.Controllers
{
    public class CompraEstoqueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompraEstoqueController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var vm = new CompraIndexViewModel
            {
                Pendentes = _context.ComprasEstoque
                    .Include(c => c.Fornecedor)
                    .Where(c => c.CompraStatus == "Aguardando")
                    .OrderByDescending(c => c.CompraData)
                    .ToList(),

                Concluidas = _context.ComprasEstoque
                    .Include(c => c.Fornecedor)
                    .Where(c => c.CompraStatus == "Concluído")
                    .OrderByDescending(c => c.CompraData)
                    .ToList()
            };

            return View(vm);
        }

        public IActionResult Cadastrar()
        {
            var vm = new CompraCreateViewModel
            {
                Fornecedores = _context.Fornecedores
                    .Select(f => new SelectListItem
                    {
                        Value = f.FornecedorID.ToString(),
                        Text = f.FornecedorNome
                    }).ToList(),

                Produtos = _context.Produtos
                    .Select(p => new SelectListItem
                    {
                        Value = p.ProdutoID.ToString(),
                        Text = p.ProdutoNome
                    }).ToList(),

                Itens = new List<ItemCompraCreateVM> { new ItemCompraCreateVM() }
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Cadastrar(CompraCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Fornecedores = _context.Fornecedores
                    .Select(f => new SelectListItem
                    {
                        Value = f.FornecedorID.ToString(),
                        Text = f.FornecedorNome
                    }).ToList();

                vm.Produtos = _context.Produtos
                    .Select(p => new SelectListItem
                    {
                        Value = p.ProdutoID.ToString(),
                        Text = p.ProdutoNome
                    }).ToList();

                return View(vm);
            }

            var compra = new CompraEstoqueModel
            {
                FornecedorID = vm.FornecedorID.Value,
                CompraData = DateTime.Now,
                CompraStatus = "Aguardando",
                Itens = new List<ItemCompraModel>()
            };

            foreach (var item in vm.Itens)
            {
                if (item.ProdutoID.HasValue && item.Quantidade > 0 && item.Preco > 0)
                {
                    compra.Itens.Add(new ItemCompraModel
                    {
                        ProdutoID = item.ProdutoID.Value,
                        ItemCompraQtd = item.Quantidade,
                        ItemCompraPreco = item.Preco
                    });
                }
            }

            if (compra.Itens.Count == 0)
            {
                ModelState.AddModelError("", "Adicione pelo menos um produto");

                vm.Fornecedores = _context.Fornecedores
                    .Select(f => new SelectListItem
                    {
                        Value = f.FornecedorID.ToString(),
                        Text = f.FornecedorNome
                    }).ToList();

                vm.Produtos = _context.Produtos
                    .Select(p => new SelectListItem
                    {
                        Value = p.ProdutoID.ToString(),
                        Text = p.ProdutoNome
                    }).ToList();

                return View(vm);
            }

            _context.ComprasEstoque.Add(compra);
            _context.SaveChanges();

            TempData["MensagemSucesso"] = "Compra cadastrada com sucesso!";
            return RedirectToAction("Index");
        }

        public IActionResult Detalhes(int id, bool confirmar = false)
        {
            var compra = _context.ComprasEstoque
                .Include(c => c.Fornecedor)
                .Include(c => c.Itens)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefault(c => c.CompraID == id);

            if (compra == null)
                return NotFound();

            var vm = new CompraDetalhesViewModel
            {
                Compra = compra,
                Itens = compra.Itens.ToList(),
                PodeConfirmar = confirmar 
            };

            return View(vm);
        }

        public IActionResult ConfirmarEntrega(int id)
        {
            var compra = _context.ComprasEstoque
                .Include(c => c.Itens)
                .FirstOrDefault(c => c.CompraID == id);

            if (compra == null)
                return NotFound();

            if (compra.CompraStatus == "Concluído")
                return RedirectToAction("Index");

            foreach (var item in compra.Itens)
            {
                var produto = _context.Produtos
                    .FirstOrDefault(p => p.ProdutoID == item.ProdutoID);

                if (produto == null) continue;

                produto.ProdutoQtdEstoque += item.ItemCompraQtd;

                var movimentacao = new MovimentacaoEstoqueModel
                {
                    MovimentacaoTipo = "Entrada",
                    MovimentacaoQtd = item.ItemCompraQtd,
                    ProdutoID = produto.ProdutoID,
                    ItemCompraID = item.ItemCompraID,
                    MovimentacaoData = DateTime.Now
                };

                _context.MovimentacoesEstoque.Add(movimentacao);
            }

            compra.CompraStatus = "Concluído";
            compra.CompraDataEntrega = DateTime.Now;

            _context.SaveChanges();

            TempData["MensagemSucesso"] = "Entrega confirmada e estoque atualizado!";
            return RedirectToAction("Index");
        }
    }
}