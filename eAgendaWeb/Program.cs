using eAgendaWeb.Compartilhado.Aplicacao;
using eAgendaWeb.Compartilhado.Apresentacao;
using eAgendaWeb.Compartilhado.Infra;

var builder = WebApplication.CreateBuilder(args);

// Configuração de Dependências (Dependency Injection)
builder.Services.AddInfraRepositories();

builder.Services.AddApplicationServices(builder.Configuration, builder.Logging);

builder.Services.AddPresentationConfig(builder.Configuration);

var app = builder.Build();

// Configuração de Middlewares
app.UseStaticFiles();

app.UseRouting();
app.MapDefaultControllerRoute();

// Execução do Servidor
app.Run();
