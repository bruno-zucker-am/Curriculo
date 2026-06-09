// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using ProjetosDB.Models;

namespace ProjetosDB.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Informacao> Informacao { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Formacao> Formacao { get; set; }
        public DbSet<Curso> Curso { get; set; }
        public DbSet<Experiencia> Experiencia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definir o esquema padrão para as tabelas
            modelBuilder.HasDefaultSchema("Curriculos");

            // Relacionamento 1 para 1: Informacao -> Endereco
            modelBuilder.Entity<Informacao>()
                .HasOne(i => i.Endereco)
                .WithOne(e => e.Informacao)
                .HasForeignKey<Endereco>(e => e.InformacaoId);

            // Relacionamento 1 para Muitos: Informacao -> Formacoes
            modelBuilder.Entity<Informacao>()
                .HasMany(i => i.Formacoes)
                .WithOne(f => f.Informacao)
                .HasForeignKey(f => f.InformacaoId);

            // Relacionamento 1 para Muitos: Informacao -> Experiencias
            modelBuilder.Entity<Informacao>()
                .HasMany(i => i.Experiencias)
                .WithOne(e => e.Informacao)
                .HasForeignKey(e => e.InformacaoId);

            // Relacionamento 1 para Muitos: Informacao -> Cursos
            modelBuilder.Entity<Informacao>()
                .HasMany(i => i.Cursos)
                .WithOne(c => c.Informacao)
                .HasForeignKey(c => c.InformacaoId);
        }
    }
}