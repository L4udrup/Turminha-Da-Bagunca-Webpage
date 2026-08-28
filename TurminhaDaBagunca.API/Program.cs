using Microsoft.EntityFrameworkCore;
using TurminhaDaBagunca.API.Data;
using TurminhaDaBagunca.API.Endpoints;

// Criando o builder da aplicação para configurar os serviços e recursos que a API vai utilizar
var builder = WebApplication.CreateBuilder(args);

// Adicionando o OpenAPI aos serviços da aplicação
// Vou utilizar o OpenAPI para documentar e testar os endpoints da minha API
builder.Services.AddOpenApi();

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
}











// Iniciando a aplicação
app.Run();