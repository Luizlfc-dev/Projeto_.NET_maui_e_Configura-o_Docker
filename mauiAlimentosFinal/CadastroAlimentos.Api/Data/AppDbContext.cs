using CadastroAlimentos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroAlimentos.Api.Data
{
    public class AppDbContext : DbContext
    {
        // 1. ADICIONAMOS ESTE CONSTRUTOR
        // Este construtor permite que o Program.cs "injete" as opções
        // (incluindo a connection string) para dentro deste DbContext.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 2. O DbSet (como já estava)
        public DbSet<Alimento> Alimentos { get; set; }

        // 3. REMOVEMOS O MÉTODO "OnConfiguring"
        // Não precisamos mais dele, pois a connection string
        // agora é configurada 100% no Program.cs.
    }
}