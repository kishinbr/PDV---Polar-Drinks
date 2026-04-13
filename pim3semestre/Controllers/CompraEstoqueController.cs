using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pim3semestre.Data;
using pim3semestre.Filters;
using pim3semestre.Models;
using pim3semestre.ViewModels;

namespace pim3semestre.Controllers
{
    [AuthFilter]
    public class CompraEstoqueController : Controller
    {
        //faz a injeção de dependência do contexto para acessar o banco de dados
        private readonly ApplicationDbContext _db;
        //construtor para receber o contexto via injeção de dependência
        public CompraEstoqueController(ApplicationDbContext db)
        {
            _db = db;
        }

        //exibe a lista de compras pendentes e concluídas
        public IActionResult Index()
        {

            var vm = new CompraIndexViewModel
            {
                Pendentes = _db.ComprasEstoque
                    .Include(c => c.Fornecedor)
                    .Where(c => c.CompraStatus == "Aguardando")
                    .OrderByDescending(c => c.CompraData)
                    .ToList(),

                Concluidas = _db.ComprasEstoque
                    .Include(c => c.Fornecedor)
                    .Where(c => c.CompraStatus == "Concluído")
                    .OrderByDescending(c => c.CompraData)
                    .ToList()
            };

            return View(vm);
        }
        //exibe o formulário para cadastrar uma nova compra
        public IActionResult Cadastrar()
        {
            //cria um objeto da viewmodel para passar os dados necessários para a view
            var vm = new CompraCreateViewModel
            {
                //faz uma consulta ao banco de dados para obter a lista de fornecedores e produtos, e transforma em SelectListItem para exibir no dropdown
                Fornecedores = _db.Fornecedores
                    .Select(f => new SelectListItem
                    {
                        Value = f.FornecedorID.ToString(),
                        Text = f.FornecedorNome
                    }).ToList(),

                Produtos = _db.Produtos
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
        //recebe uma viewmodel como parâmetro, que contém os dados do formulário preenchido pelo usuário
        public IActionResult Cadastrar(CompraCreateViewModel vm)
        {
            //verifica se o modelo é válido
            if (!ModelState.IsValid)
            {
                //se não for, recarrega as listas de fornecedores e produtos para exibir novamente o formulário com os erros
                vm.Fornecedores = _db.Fornecedores
                    .Select(f => new SelectListItem
                    {
                        Value = f.FornecedorID.ToString(),
                        Text = f.FornecedorNome
                    }).ToList();

                vm.Produtos = _db.Produtos
                    .Select(p => new SelectListItem
                    {
                        Value = p.ProdutoID.ToString(),
                        Text = p.ProdutoNome
                    }).ToList();

                return View(vm);
            }

            //cria um objeto de compra com os dados do formulário e a data atual, e inicializa a lista de itens vazia
            var compra = new CompraEstoqueModel
            {
                FornecedorID = vm.FornecedorID.Value,
                CompraData = DateTime.Now,
                CompraStatus = "Aguardando",
                Itens = new List<ItemCompraModel>()
            };

            //percorre a lista de itens da viewmodel e adiciona à compra apenas os itens que têm produto selecionado, quantidade e preço válidos
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

            //verifica se a compra tem pelo menos um item válido, caso contrário exibe um erro e recarrega o formulário
            if (compra.Itens.Count == 0)
            {
                ModelState.AddModelError("", "Adicione pelo menos um produto");

                vm.Fornecedores = _db.Fornecedores
                    .Select(f => new SelectListItem
                    {
                        Value = f.FornecedorID.ToString(),
                        Text = f.FornecedorNome
                    }).ToList();

                vm.Produtos = _db.Produtos
                    .Select(p => new SelectListItem
                    {
                        Value = p.ProdutoID.ToString(),
                        Text = p.ProdutoNome
                    }).ToList();

                return View(vm);
            }

            //adiciona a compra ao contexto e salva as alterações no banco de dados
            _db.ComprasEstoque.Add(compra);
            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Compra cadastrada com sucesso!";
            return RedirectToAction("Index");
        }

        //exibe os detalhes de uma compra, incluindo os itens e o fornecedor, e um botão para confirmar a entrega se a compra estiver pendente
        public IActionResult Detalhes(int id, bool confirmar = false)
        {
            var compra = _db.ComprasEstoque
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

        //recebe o id da compra a ser confirmada, atualiza o estoque dos produtos e a data de entrega, e redireciona para os detalhes da compra com uma mensagem de sucesso
        public IActionResult ConfirmarEntrega(int id)
        {
            var compra = _db.ComprasEstoque
                .Include(c => c.Itens)
                .FirstOrDefault(c => c.CompraID == id);
            //verifica se a compra existe e se ainda não foi confirmada, caso contrário redireciona para a lista de compras
            if (compra == null)
                return NotFound();
            //se a compra já estiver concluída, não faz nada e redireciona para a lista de compras
            if (compra.CompraStatus == "Concluído")
                return RedirectToAction("Index");

            //percorre os itens da compra, atualiza a quantidade em estoque do produto correspondente, e registra uma movimentação de entrada no estoque
            foreach (var item in compra.Itens)
            {
                var produto = _db.Produtos
                    .FirstOrDefault(p => p.ProdutoID == item.ProdutoID);

                if (produto == null) continue;

                produto.ProdutoQtdEstoque += item.ItemCompraQtd;

                var movimentacao = new MovimentacaoEstoqueModel
                {
                    MovimentacaoTipo = MovimentacaoEstoqueModel.Tipos.Entrada,
                    MovimentacaoQtd = item.ItemCompraQtd,
                    ProdutoID = produto.ProdutoID,
                    ItemCompraID = item.ItemCompraID,
                    MovimentacaoData = DateTime.Now
                };
                //adiciona a movimentação ao contexto para salvar no banco de dados
                _db.MovimentacoesEstoque.Add(movimentacao);
            }
            //atualiza o status da compra para "Concluído" e a data de entrega para a data atual
            compra.CompraStatus = "Concluído";
            compra.CompraDataEntrega = DateTime.Now;

            //salva as alterações no banco de dados
            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Entrega confirmada e estoque atualizado!";
            return RedirectToAction("Index");
        }
        //exibe a confirmação de exclusão de uma compra, mostrando os detalhes da compra e um botão para confirmar a exclusão
        public IActionResult Excluir(int id)
        {
            var compra = _db.ComprasEstoque
                .Include(c => c.Itens)
                .ThenInclude(i => i.Produto)
                .Include(c => c.Fornecedor)
                .FirstOrDefault(c => c.CompraID == id);

            if (compra == null)
                return NotFound();


            var vm = new CompraDetalhesViewModel
            {
                Compra = compra,
                Itens = compra.Itens.ToList(),
                PodeConfirmar = false
            };

            return View(vm);
        }


        //recebe o id da compra a ser excluída, remove a compra do contexto e salva as alterações no banco de dados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarExclusao(int id)
        {
            var compra = _db.ComprasEstoque
                .Include(c => c.Itens)
                .FirstOrDefault(c => c.CompraID == id);

            if (compra == null)
                return NotFound();

            _db.ComprasEstoque.Remove(compra);
            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Compra excluída com sucesso!";
            return RedirectToAction("Index");
        }
    }
}