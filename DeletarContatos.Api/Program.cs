using DeletarContatos.Api.Swagger;
using DeletarContatos.Infrastructure.Exceptions;
using DeletarContatos.Service.Contato;
using Prometheus;
using DeletarContatos.Infrastructure.MassTransit;
using DeletarContatos.Service.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o servi�o de health check
builder.Services.AddHealthChecks();

// Adiciona a configura��o do appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables(); // Permite sobrescrever com vari�veis de ambiente

// Configurar MassTransit
builder.Services.ConfigureMassTransit(builder.Configuration);

// Monitoramento com Application Insights
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("NamedClient", client =>
{
    client.BaseAddress = new Uri("https://fiap-api-gateway.azure-api.net/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddScoped<IContatoService, ContatoService>();

builder.Services.AddScoped<IRabbitMqPublisherService, RabbitMqPublisherService>();

var app = builder.Build();

// Mapeia o endpoint de health check
app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adicionar middleware do Prometheus
app.UseMetricServer();
app.UseHttpMetrics();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run();
