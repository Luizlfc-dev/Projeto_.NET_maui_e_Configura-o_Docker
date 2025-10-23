using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CadastroAlimentos.Api.Data;
using DotNetEnv;

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
        
        var builder = WebApplication.CreateBuilder(args);
        var password = builder.Configuration["SA_PASSWORD"];
        var database = builder.Configuration["DB_DATABASE"];
        var wslip = builder.Configuration["WSL_IP"];
        var id = builder.Configuration["ID_BD"]; 
        
        var connectionString = $"Server={wslip},1433;Database={database};User ID={id};Password={password};TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}