using Microsoft.EntityFrameworkCore;
using TurminhaDaBagunca.API.Data;
using TurminhaDaBagunca.API.Endpoints;

// Criando o builder da aplicação para configurar os serviços e recursos que a API vai utilizar
var builder = WebApplication.CreateBuilder(args);

// Adicionando o OpenAPI aos serviços da aplicação
// Vou utilizar o OpenAPI para documentar e testar os endpoints da minha API
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
});

// Adicionando o AppDbContext aos serviços da aplicação para podermos utilizar o Entity Framework
// e conectá-lo ao banco de dados SQL Server LocalDB
builder.Services.AddDbContext<AppDbContext>(Options => Options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

// Construindo a aplicação depois de terminar suas configurações
var app = builder.Build();

// Chamando o método Registrar da classe UsuariosEndpoints para registrar os endpoints de usuários na aplicação
UsuariosEndpoints.Registrar(app);

// Criando o endpoint da documentação OpenAPI
// Vou utilizar esse endpoint para visualizar a documentação da minha API
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Configurando o Swagger UI para exibir a documentação da API de forma interativa
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Turminha da Bagunça API";
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}











// Iniciando a aplicação
app.Run();