using CadastroAlimentos9.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroAlimentos9.Data;

public class AppDbContext : DbContext
{
    public DbSet<Alimento> Alimentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // ATENÇÃO: Substitua pelo nome da sua instância SQL Server
        string connectionString = "Server=IP_DO_SEU_UBUNTU,1433;Database=CadastroAlimentosDB;User ID=sa;Password=SENHA_DO_SEU_BD;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectionString);
    }
}