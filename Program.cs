using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Sprint4PlusSoft.Services;
using Sprint4PlusSoft.Data;

var builder = WebApplication.CreateBuilder(args);

// Adiciona controllers
builder.Services.AddControllers();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sprint4PlusSoft", Version = "v1" });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Configuração de MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnection") 
                           ?? throw new InvalidOperationException("A string de conexão com o MongoDB está ausente.");
    return new MongoClient(connectionString);
});

// Registro do IMongoDatabase usando o nome do banco de dados incluído na string de conexão
builder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase("Plusoft"); // Nome do banco de dados diretamente
});

// Registro do MongoDbService como Singleton, se necessário
builder.Services.AddSingleton<MongoDbService>();

// Registro de serviços de negócio
builder.Services.AddScoped<ICompanyReportService, CompanyReportService>();
builder.Services.AddScoped<CompanyService>();  
builder.Services.AddHttpClient<ValidationService>();
builder.Services.AddScoped<ValidationService>();
builder.Services.AddScoped<CompanyPredictionService>(); 

// Configuração dos Health Checks
builder.Services.AddHealthChecks()
    .AddMongoDb(
        builder.Configuration.GetConnectionString("DbConnection") ?? throw new InvalidOperationException("A string de conexão com o MongoDB está ausente."),
        name: "MongoDB",
        timeout: TimeSpan.FromSeconds(5),
        tags: new[] { "db" })
    .AddCheck("Server", () =>
    {
        return HealthCheckResult.Healthy("Servidor está operacional.");
    });

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                exception = e.Value.Exception?.Message
            })
        });
        await context.Response.WriteAsync(result);
    }
});

app.UseAuthorization();
app.MapControllers();
app.Run();
