using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CadastroAlimentos.Api.Data;

// Este arquivo SÓ é usado pelas ferramentas de linha de comando (dotnet ef)
// Ele permite que o 'dotnet ef database update' funcione no seu PowerShell
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // ATENÇÃO: COLOQUE SEU IP ATUAL DO WSL AQUI
        // 1. Abra o terminal Ubuntu
        // 2. Rode 'ip addr show eth0'
        // 3. Pegue o IP (ex: 172.31.120.50) e coloque abaixo

        var wsl_ip = "172.31.115.162"; // <<< COLOQUE SEU IP DO WSL AQUI
        var password = "TesteForte!123"; // A senha que definimos

        var connectionString = $"Server={wsl_ip},1433;Database=CadastroAlimentosDB;User ID=sa;Password={password};TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}