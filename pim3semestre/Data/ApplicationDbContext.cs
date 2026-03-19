using Microsoft.EntityFrameworkCore;
using pim3semestre.Models;

namespace pim3semestre.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // 📦 PRODUTO / CATEGORIA
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<CategoriaModel> Categorias { get; set; }

        // 🚚 FORNECEDOR / COMPRA
        public DbSet<FornecedorModel> Fornecedores { get; set; }
        public DbSet<CompraEstoqueModel> ComprasEstoque { get; set; }
        public DbSet<ItemCompraModel> ItensCompra { get; set; }

        // 💰 VENDA
        public DbSet<VendaFinalModel> Vendas { get; set; }
        public DbSet<ItemVendaModel> ItensVenda { get; set; }

        // 👤 USUÁRIOS
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<FuncionarioModel> Funcionarios { get; set; }

        // 📊 ESTOQUE
        public DbSet<MovimentacaoEstoqueModel> MovimentacoesEstoque { get; set; }

        // 🔧 CONFIGURAÇÕES AVANÇADAS (relacionamentos)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔗 Produto → Categoria
            modelBuilder.Entity<ProdutoModel>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaID);

            // 🔗 CompraEstoque → Fornecedor
            modelBuilder.Entity<CompraEstoqueModel>()
                .HasOne(c => c.Fornecedor)
                .WithMany(f => f.Compras)
                .HasForeignKey(c => c.FornecedorID);

            // 🔗 ItemCompra → CompraEstoque
            modelBuilder.Entity<ItemCompraModel>()
                .HasOne(ic => ic.Compra)
                .WithMany(c => c.Itens)
                .HasForeignKey(ic => ic.CompraID);

            // 🔗 ItemCompra → Produto
            modelBuilder.Entity<ItemCompraModel>()
                .HasOne(ic => ic.Produto)
                .WithMany()
                .HasForeignKey(ic => ic.ProdutoID);

            // 🔗 ItemVenda → Venda
            modelBuilder.Entity<ItemVendaModel>()
                .HasOne(iv => iv.Venda)
                .WithMany(v => v.Itens)
                .HasForeignKey(iv => iv.VendaID);

            // 🔗 ItemVenda → Produto
            modelBuilder.Entity<ItemVendaModel>()
                .HasOne(iv => iv.Produto)
                .WithMany()
                .HasForeignKey(iv => iv.ProdutoID);

            // 🔗 Venda → Cliente (opcional)
            modelBuilder.Entity<VendaFinalModel>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Vendas)
                .HasForeignKey(v => v.ClienteID)
                .OnDelete(DeleteBehavior.SetNull);

            // 🔗 Venda → Funcionario
            modelBuilder.Entity<VendaFinalModel>()
                .HasOne(v => v.Funcionario)
                .WithMany(f => f.Vendas)
                .HasForeignKey(v => v.FuncionarioID);

            // 🔗 Movimentacao → Produto
            modelBuilder.Entity<MovimentacaoEstoqueModel>()
                .HasOne(m => m.Produto)
                .WithMany()
                .HasForeignKey(m => m.ProdutoID);

            // 🔗 Movimentacao → ItemCompra (opcional)
            modelBuilder.Entity<MovimentacaoEstoqueModel>()
                .HasOne(m => m.ItemCompra)
                .WithMany()
                .HasForeignKey(m => m.ItemCompraID)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔗 Movimentacao → ItemVenda (opcional)
            modelBuilder.Entity<MovimentacaoEstoqueModel>()
                .HasOne(m => m.ItemVenda)
                .WithMany()
                .HasForeignKey(m => m.ItemVendaID)
                .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<ProdutoModel>()
                    .Property(p => p.ProdutoPrecoVenda)
                    .HasPrecision(18, 2);

                modelBuilder.Entity<CompraEstoqueModel>()
                    .Property(c => c.CompraValorTotal)
                    .HasPrecision(18, 2);

                modelBuilder.Entity<ItemCompraModel>()
                    .Property(ic => ic.ItemCompraPreco)
                    .HasPrecision(18, 2);

                modelBuilder.Entity<ItemVendaModel>()
                    .Property(iv => iv.ItemVendaPreco)
                    .HasPrecision(18, 2);

                modelBuilder.Entity<VendaFinalModel>()
                    .Property(v => v.VendaValorTotal)
                    .HasPrecision(18, 2);
        }
    }
}