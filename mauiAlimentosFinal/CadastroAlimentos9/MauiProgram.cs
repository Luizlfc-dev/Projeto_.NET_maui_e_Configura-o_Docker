// ADICIONE OS USINGS QUE FALTAM AQUI
using Microsoft.Extensions.Logging;
using CadastroAlimentos9.ViewModels;
using CadastroAlimentos9.Views;
using System.Net.Http; // <<< ADICIONE ESTE PARA O HTTPCLIENT
using DotNetEnv;

namespace CadastroAlimentos9
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
           
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

            // 1. Registra o HttpClient para ser usado em todo o app (melhor prática)
            // Usamos o IP do WSL para evitar o bloqueio de rede do "localhost"
            var wslIp = Env.GetString("WSL_IP");
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