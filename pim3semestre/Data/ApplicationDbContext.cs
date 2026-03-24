using Microsoft.EntityFrameworkCore;
using pim3semestre.Models;

namespace pim3semestre.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<CategoriaModel> Categorias { get; set; }

        public DbSet<FornecedorModel> Fornecedores { get; set; }
        public DbSet<CompraEstoqueModel> ComprasEstoque { get; set; }
        public DbSet<ItemCompraModel> ItensCompra { get; set; }

        public DbSet<VendaFinalModel> Vendas { get; set; }
        public DbSet<ItemVendaModel> ItensVenda { get; set; }


        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<FuncionarioModel> Funcionarios { get; set; }

        public DbSet<MovimentacaoEstoqueModel> MovimentacoesEstoque { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProdutoModel>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaID);

            modelBuilder.Entity<CompraEstoqueModel>()
                .HasOne(c => c.Fornecedor)
                .WithMany(f => f.Compras)
                .HasForeignKey(c => c.FornecedorID);

            modelBuilder.Entity<CompraEstoqueModel>()
                .Ignore(c => c.CompraValorTotal);

            modelBuilder.Entity<ItemCompraModel>()
                .HasOne(ic => ic.Compra)
                .WithMany(c => c.Itens)
                .HasForeignKey(ic => ic.CompraID);

            modelBuilder.Entity<ItemCompraModel>()
                .HasOne(ic => ic.Produto)
                .WithMany()
                .HasForeignKey(ic => ic.ProdutoID);

            modelBuilder.Entity<ItemVendaModel>()
                .HasOne(iv => iv.Venda)
                .WithMany(v => v.Itens)
                .HasForeignKey(iv => iv.VendaID);

            modelBuilder.Entity<ItemVendaModel>()
                .HasOne(iv => iv.Produto)
                .WithMany()
                .HasForeignKey(iv => iv.ProdutoID);

            modelBuilder.Entity<VendaFinalModel>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Vendas)
                .HasForeignKey(v => v.ClienteID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<VendaFinalModel>()
                .HasOne(v => v.Funcionario)
                .WithMany(f => f.Vendas)
                .HasForeignKey(v => v.FuncionarioID);

            modelBuilder.Entity<MovimentacaoEstoqueModel>()
                .HasOne(m => m.Produto)
                .WithMany()
                .HasForeignKey(m => m.ProdutoID);

            modelBuilder.Entity<MovimentacaoEstoqueModel>()
                .HasOne(m => m.ItemCompra)
                .WithMany()
                .HasForeignKey(m => m.ItemCompraID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimentacaoEstoqueModel>()
                .HasOne(m => m.ItemVenda)
                .WithMany()
                .HasForeignKey(m => m.ItemVendaID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProdutoModel>()
                .Property(p => p.ProdutoPrecoVenda)
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