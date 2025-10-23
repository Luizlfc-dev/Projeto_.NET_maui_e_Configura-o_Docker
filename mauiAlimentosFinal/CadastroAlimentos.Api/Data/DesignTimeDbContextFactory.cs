using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CadastroAlimentos.Api.Data;
using DotNetEnv; 
using System; // <<< ADICIONE ESTE USING PARA CONSOLE

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // --- Bloco para encontrar o .env na raiz da solução ---
        // Pega o diretório atual (que pode ser bin/Debug/... dentro da API)
        string currentDirectory = Directory.GetCurrentDirectory(); 
        DirectoryInfo? solutionRoot = new DirectoryInfo(currentDirectory);

        // Sobe na árvore de diretórios até encontrar o arquivo .sln
        // (Ajuste o nome do .sln se for diferente)
        while (solutionRoot != null && !solutionRoot.GetFiles("mauiAlimentosFinal.sln").Any()) 
        {
            solutionRoot = solutionRoot.Parent;
        }

        if (solutionRoot != null)
        {
            string envPath = Path.Combine(solutionRoot.FullName, ".env");
            Console.WriteLine($"Procurando .env em: {envPath}");
            if (File.Exists(envPath))
                {
                    Env.Load(path: envPath); // Carrega usando o caminho absoluto encontrado
                    Console.WriteLine(".env encontrado e carregado.");
                }
            else
                {
                    Console.WriteLine("AVISO: Arquivo .env não encontrado na raiz da solução.");
                }
        }
        else
        {
            Console.WriteLine("AVISO: Raiz da solução (.sln) não encontrada. Não foi possível carregar .env automaticamente.");
        }
        // --- Fim do Bloco ---

        var wsl_ip = Env.GetString("WSL_IP");
        var password = Env.GetString("SA_PASSWORD");
        var database = Env.GetString("DB_DATABASE");

        // --- DEBUG: Imprimir os valores lidos ---
        Console.WriteLine($"WSL_IP lido: '{wsl_ip}'");
        Console.WriteLine($"SA_PASSWORD lido: '{password}'");
        Console.WriteLine($"DB_DATABASE lido: '{database}'");
        // ----------------------------------------

        // Se algum valor for nulo ou vazio, a string ficará inválida
        if (string.IsNullOrEmpty(wsl_ip) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(database))
        {
             Console.WriteLine("ERRO FATAL: Uma ou mais variáveis do .env estão vazias!");
             // Lançamos uma exceção clara para parar aqui
             throw new InvalidOperationException("Não foi possível ler todas as variáveis do .env. Verifique o arquivo .env e sua localização.");
        }

        var connectionString = $"Server={wsl_ip},1433;Database={database};User ID=sa;Password={password};TrustServerCertificate=True;";
        Console.WriteLine($"String de Conexão Gerada: {connectionString}"); // Imprime a string final

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}