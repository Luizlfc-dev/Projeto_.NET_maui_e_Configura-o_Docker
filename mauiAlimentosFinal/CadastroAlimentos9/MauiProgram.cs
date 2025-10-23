// ADICIONE OS USINGS QUE FALTAM AQUI
using Microsoft.Extensions.Logging;
using CadastroAlimentos9.ViewModels;
using CadastroAlimentos9.Views;
using System.Net.Http; // <<< ADICIONE ESTE PARA O HTTPCLIENT
using DotNetEnv;
using System.IO;

namespace CadastroAlimentos9
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // --- Bloco para encontrar o .env na raiz da solução ---
            string currentDirectory = Directory.GetCurrentDirectory(); 
            DirectoryInfo? solutionRoot = new DirectoryInfo(currentDirectory);
            while (solutionRoot != null && !solutionRoot.GetFiles("mauiAlimentosFinal.sln").Any()) 
            {
                solutionRoot = solutionRoot.Parent;
            }
            if (solutionRoot != null)
            {
                string envPath = Path.Combine(solutionRoot.FullName, ".env");
                System.Diagnostics.Debug.WriteLine($"MAUI Procurando .env em: {envPath}"); // Use Debug aqui
                if (File.Exists(envPath))
                {
                    Env.Load(path: envPath); 
                    System.Diagnostics.Debug.WriteLine("MAUI .env encontrado e carregado.");
                } else { System.Diagnostics.Debug.WriteLine("MAUI AVISO: .env não encontrado."); }
            } else { System.Diagnostics.Debug.WriteLine("MAUI AVISO: Raiz da solução não encontrada."); }
            // --- Fim do Bloco ---
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // --- ADICIONE ESTAS LINHAS ---
            // Tenta carregar o arquivo .env localizado um diretório acima
            // Substitua pelo seu caminho absoluto real
            

            // 1. Registra o HttpClient para ser usado em todo o app (melhor prática)
            // Usamos o IP do WSL para evitar o bloqueio de rede do "localhost"
            var wslIp = Env.GetString("WSL_IP");
            System.Diagnostics.Debug.WriteLine($"---> Valor lido para wslIp: '{wslIp}' <---"); // ADICIONE ISSO
            builder.Services.AddSingleton<HttpClient>(s => new HttpClient
            {
                BaseAddress = new System.Uri($"http://{wslIp}:8080") // Use o SEU IP ATUAL AQUI
            }); // <<< FALTAVA O ); AQUI

            // 2. Registra o ViewModel e a Page
            builder.Services.AddSingleton<AlimentosViewModel>();
            builder.Services.AddSingleton<AlimentosPage>();

            // --- FIM DAS LINHAS ---

            return builder.Build(); // SÓ PRECISA DE UM RETURN
        }
    }
}