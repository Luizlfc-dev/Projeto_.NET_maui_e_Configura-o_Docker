using CadastroAlimentos.Api.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// --- Início da Configuração ---

// 1. Pegue a senha do banco
// Lembre-se que o "Server" agora é o nome do contêiner,
// pois eles estão na mesma rede Docker (alimentos-net).

var password = builder.Configuration["SA_PASSWORD"];
var database = builder.Configuration["DB_DATABASE"];
var ID_BD = builder.Configuration["ID_BD"];
var server = "mssql-dev"; // O nome do nosso serviço de banco (veja o compose)

var connectionString = $"Server={server},1433;Database={database};User ID={ID_BD};Password={password};TrustServerCertificate=True;";

// 2. Configure o DbContext (como na etapa que corrigiu o log)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. ADICIONE ESTA LINHA (ESTAVA FALTANDO)
// Isso registra os serviços necessários para os Controllers
builder.Services.AddControllers();

// --- Fim da Configuração ---

// Adiciona serviços padrões da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure o pipeline de HTTP (a ordem importa)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// 4. ADICIONE ESTA LINHA (ESTAVA FALTANDO)
// Isso mapeia as rotas para os seus [ApiController] (como o AlimentosController)
app.MapControllers();

app.Run();