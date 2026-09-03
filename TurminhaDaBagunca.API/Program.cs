using Microsoft.EntityFrameworkCore;
using TurminhaDaBagunca.API.Data;
using TurminhaDaBagunca.API.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

// Adicionando o serviço de autenticação à aplicação
builder.Services.AddAuthentication()
    // Configurando a autenticação JWT Bearer para a aplicação
    .AddJwtBearer(options =>
    {
        // Configurando a validação do token JWT
        var jwtkey = builder.Configuration["Jwt:Key"];

        // Regras que a API vai usar para decidir se um token JWT é válido ou não
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Definindo a chave que vou usar para validar a assinatura do token JWT 
            // (transformando a key em bytes para que o algoritmo de criptografia possa utilizá-la)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtkey!)),

            // Eu quero que você realmente verifique essa chave?
            ValidateIssuerSigningKey = true,

            // Quero verificar quem emitiu o token JWT?
            ValidateIssuer = true,

            // O emissor que eu considero válido é o que está configurado no appsettings.json, na seção "Jwt:Issuer"
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            
            ValidateAudience = true,
            // Verificando se o token JWT é destinado para a audiência correta
            ValidAudience = builder.Configuration["Jwt:Audience"],

            // Verificando se o token JWT ainda é válido (não expirou)
            ValidateLifetime = true,
        };
    });

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