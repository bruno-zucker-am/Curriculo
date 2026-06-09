using Microsoft.EntityFrameworkCore;
using ProjetosDB.Data;
using ProjetosDB.Services;
using System.Text.Json;
using QuestPDF.Infrastructure;

// Configura a licença do QuestPDF para a versão Community
QuestPDF.Settings.License = LicenseType.Community;

// Cria o builder para configurar os serviços e o pipeline do aplicativo
var builder = WebApplication.CreateBuilder(args);

// Configura a string de conexão para o banco de dados e adiciona o contexto do Entity Framework
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Adiciona o serviço CurriculoService para injeção de dependência
builder.Services.AddScoped<ProjetosDB.Services.CurriculoService>();

// Configura o CORS para permitir solicitações de qualquer origem, método e cabeçalho, e expõe o cabeçalho "Content-Disposition" para permitir o download de arquivos
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("Content-Disposition"); // <-- ISSO AQUI É VITAL
    });
});

// Configura os controladores para usar opções de serialização JSON personalizadas, permitindo que o C# aceite qualquer case do Angular e envie tudo em camelCase
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Isso faz o C# ACEITAR qualquer case no que vem do Angular
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

        // ISSO AQUI garante que o C# ENVIE tudo em minúsculo (tipo, nome, id)
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Cria o aplicativo e configura o pipeline de middleware
var app = builder.Build();

// Configura o middleware para usar CORS, roteamento, autenticação, autorização e mapeamento de controladores
app.UseCors("AllowAll");
app.UseRouting();

// Mova os Controllers para cima
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); 

// Arquivos estáticos e Fallback por último
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

// Aplica as migrações pendentes ao banco de dados quando o aplicativo é iniciado, garantindo que o esquema do banco de dados esteja atualizado
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Inicia o aplicativo
app.Run();