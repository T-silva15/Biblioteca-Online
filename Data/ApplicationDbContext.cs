using Biblioteca_LAB.Migrations;
using Biblioteca_LAB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_LAB.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
		}

		public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autor { get; set; }
        public DbSet<LivroAutor> LivroAutores { get; set; }
        public DbSet<GereAdm> AlteracoesAdm { get; set; }
		public DbSet<Requisita> Requisita { get; set; } = default!;
		public DbSet<Armazena> Armazenas { get; set; } = default!;
		public DbSet<Biblioteca> Biblioteca { get; set; } = default!;


		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LivroAutor>()
                .HasKey(la => new { la.LivroIsbn, la.AutorId });

            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.LivroAutores)
                .HasForeignKey(la => la.LivroIsbn);

            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Autor)
                .WithMany(a => a.LivroAutores)
                .HasForeignKey(la => la.AutorId);

            modelBuilder.Entity<Requisita>(entity =>
            {
                entity.Property(e => e.Estado)
                      .HasColumnType("bit") // Ou "boolean" dependendo do banco de dados
                      .IsRequired();
            });




            base.OnModelCreating(modelBuilder);
        }

    }
}
