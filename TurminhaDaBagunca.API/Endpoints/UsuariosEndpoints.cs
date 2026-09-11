using Microsoft.EntityFrameworkCore;
using TurminhaDaBagunca.API.Data;
using TurminhaDaBagunca.API.Models;
using TurminhaDaBagunca.API.Dtos;
//usado para acessar o banco de dados e manipular os dados dos usuários
using System.ComponentModel.DataAnnotations;
// Usado para criar e trabalhar com tokens JWT,
// permitindo gerar o token que será enviado ao usuário após o login
using System.IdentityModel.Tokens.Jwt;
// Usado para criar as Claims
using System.Security.Claims;
// Usado para configurar a segurança do JWT,
// como a chave usada para assinar e validar o token e o algoritmo de assinatura
using Microsoft.IdentityModel.Tokens;
// Usado para converter a chave secreta do JWT de texto para bytes
using System.Text;


namespace TurminhaDaBagunca.API.Endpoints;

public class UsuariosEndpoints
{
    public static void Registrar(WebApplication app)
    {

        /* ========================================
                         Get/Read
        ======================================== */

        // Criando um endpoint GET para buscar todos os usuários cadastrados no banco de dados
        // Vou utilizar esse endpoint para consultar os usuários através da minha API
        app.MapGet("/Usuarios", async (AppDbContext db) =>
        {
            // Buscando todos os usuários da tabela Usuarios utilizando o Entity Framework
            var Usuarios = await db.Usuarios.ToListAsync();


            // Criando uma lista de objetos UsuarioResponseDto para retornar apenas os dados que quero expor na resposta
            var UsuariosResponse = Usuarios.Select(usuario => new UsuarioResponseDto
            {
                ID = usuario.ID,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                DataCadastro = usuario.DataCadastro
            });

            // Retornando os usuários encontrados para quem fizer a requisição
            return Results.Ok(UsuariosResponse);
        });

        // Buscando um usuário com ID igual o número digitado na URL
        app.MapGet("/Usuarios/{id}", async (int id, AppDbContext db) =>
        {

            // Filtrando o primeiro item cujo o id seja igual ao id da URL
            var Usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.ID == id);

            // Se não achar retorna 404
            if (Usuario == null)
            {
                return Results.NotFound();
            }

            // Criando um objeto UsuarioResponseDto para retornar apenas os dados que quero expor na resposta
            var UsuarioResponse = new UsuarioResponseDto
            {
                ID = Usuario.ID,
                Nome = Usuario.Nome,
                Email = Usuario.Email,
                Telefone = Usuario.Telefone,
                DataCadastro = Usuario.DataCadastro
            };

            return Results.Ok(UsuarioResponse);
        });

        /* =========================================================
                                 Post/Create
        ========================================================= */

        // Criando um endpoint POST para cadastrar um novo usuário no banco de dados
        // Vou utilizar esse endpoint para receber os dados de um usuário e realizar o cadastro
        app.MapPost("/Usuarios", async (UsuarioCreateDto usuario, AppDbContext db) =>
        {

            // Validando os dados recebidos na requisição utilizando DataAnnotations
            var erros = new List<ValidationResult>();
            var contexto = new ValidationContext(usuario);
            var valido = Validator.TryValidateObject(usuario, contexto, erros, true);
            if (!valido)
            {
                return Results.BadRequest(erros);
            }

            // Criando um novo objeto na classe Usuario com os dados recebidos na requisição
            // Vou utilizar esse objeto para preparar os dados do usuário para serem salvos no banco
            // NovoUsuario é o objeto que pode ser salvo no banco
            // usuario agora é o objeto que veio da requisição, que não pode ser salvo no banco(Dto)
            var NovoUsuario = new Usuario
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                // Gerando o hash da senha recebida na requisição utilizando a biblioteca BCrypt.Net
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.Senha),
            };

            // Adicionando o usuário recebido ao DbSet de usuários
            // Vou utilizar o Entity Framework para preparar esse usuário para ser salvo no banco
            db.Usuarios.Add(NovoUsuario);

            // Salvando as alterações realizadas no banco de dados de forma assíncrona
            // Vou utilizar esse método para realmente efetivar o cadastro do usuário no banco
            await db.SaveChangesAsync();

            // Criando um objeto UsuarioResponseDto para retornar apenas os dados que quero expor na resposta
            var UsuarioResponse = new UsuarioResponseDto
            {
                ID = NovoUsuario.ID,
                Nome = NovoUsuario.Nome,
                Email = NovoUsuario.Email,
                Telefone = NovoUsuario.Telefone,
                DataCadastro = NovoUsuario.DataCadastro
            };

            // Retornando uma resposta 201 Created informando que o usuário foi criado
            // Vou retornar também o endereço do usuário criado e os seus dados no body da resposta
            return Results.Created($"/Usuarios/{NovoUsuario.ID}", UsuarioResponse);
        });

        /* =========================================================
                                 Put/Update
        ========================================================= */

        // Rota para alterar os dados que forem enviados para o UsuarioUpdateDto
        app.MapPut("/Usuarios/{id}", async (int id, UsuarioUpdateDto NovoUsuario, AppDbContext db) =>
        {
            var Usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.ID == id);

            if (Usuario == null)
            {
                return Results.NotFound();
            }

            // Atualizando os dados do usuário com os dados recebidos na requisição
            Usuario.Nome = NovoUsuario.Nome;
            Usuario.Email = NovoUsuario.Email;
            Usuario.Telefone = NovoUsuario.Telefone;
            Usuario.SenhaHash = NovoUsuario.Senha;

            await db.SaveChangesAsync();

            // Criando um objeto UsuarioResponseDto para retornar apenas os dados que quero expor na resposta
            var UsuarioResponse = new UsuarioResponseDto
            {
                ID = Usuario.ID,
                Nome = Usuario.Nome,
                Email = Usuario.Email,
                Telefone = Usuario.Telefone,
                DataCadastro = Usuario.DataCadastro
            };

            return Results.Ok(UsuarioResponse);
        });

        /* =========================================================
                                 Delete
        ========================================================= */

        app.MapDelete("/Usuarios/{id}", async (int id, AppDbContext db) =>
        {
            var Usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.ID == id);

            if (Usuario == null)
            {
                return Results.NotFound();
            }

            db.Usuarios.Remove(Usuario);

            await db.SaveChangesAsync();

            return Results.NoContent();
        });
        
        /* =========================================================
                                 Login
        ========================================================= */

        // IConfiguration me permite acessar as configurações do appsettings.json dentro do endpoint (Nativo do .NET)
        app.MapPost("/Usuarios/Login", async (UsuarioLoginDto usuario, AppDbContext db, IConfiguration config) =>
        {
            var UsuarioDb = await db.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);

            if (UsuarioDb == null)
            {
                return Results.Unauthorized();
            }

            var senhaCorreta = BCrypt.Net.BCrypt.Verify(usuario.Senha, UsuarioDb.SenhaHash);

            if (!senhaCorreta)
            {
                return Results.Unauthorized();
            }

            // Criando as claims do token JWT (informações do usuario)
            // Vou utilizar essas claims para gerar o token JWT que será retornado para o usuário
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, UsuarioDb.ID.ToString()),
                new Claim(ClaimTypes.Name, UsuarioDb.Nome),
                new Claim(ClaimTypes.Email, UsuarioDb.Email),
            };

            // Recebendo a chave secreta do JWT do appsettings.json
            var jwtkey = config["Jwt:Key"];
            // Criando as credenciais de assinatura do token JWT (chave secreta e algoritmo de assinatura)
            var credentials = new SigningCredentials(
                // Chave secreta do JWT convertida para bytes e utilizada para assinar o token JWT
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtkey!)),
                // Algoritmo de assinatura do token JWT
                SecurityAlgorithms.HmacSha256
            );

            // Criando o token JWT com as claims do usuário
            // Vou utilizar esse token para autenticar o usuário na minha API
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                // signingCredentials contém a chave e o algoritmo que vou usar para assinar o token JWT
                signingCredentials: credentials,

                expires: DateTime.UtcNow.AddHours(5)
            );


            // Gerando o token JWT em formato de string para enviar para o usuário
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(tokenString);
        });
    }
}