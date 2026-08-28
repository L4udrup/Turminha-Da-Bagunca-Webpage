using Microsoft.EntityFrameworkCore;
using TurminhaDaBagunca.API.Data;
using TurminhaDaBagunca.API.Models;
using TurminhaDaBagunca.API.Dtos;

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

            // Criando um novo objeto na classe Usuario com os dados recebidos na requisição
            // Vou utilizar esse objeto para preparar os dados do usuário para serem salvos no banco
            // NovoUsuario é o objeto que pode ser salvo no banco
            // usuario agora é o objeto que veio da requisição, que não pode ser salvo no banco(Dto)
            var NovoUsuario = new Usuario
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                // SenhaHash é o campo que vai receber a senha do usuário, mas de forma criptografada (ainda não está criptografada)
                SenhaHash = usuario.Senha,
            };

            // Adicionando o usuário recebido ao DbSet de usuários
            // Vou utilizar o Entity Framework para preparar esse usuário para ser salvo no banco
            db.Usuarios.Add(NovoUsuario);

            // Salvando as alterações realizadas no banco de dados de forma assíncrona
            // Vou utilizar esse método para realmente efetivar o cadastro do usuário no banco
            await db.SaveChangesAsync();

            // Retornando uma resposta 201 Created informando que o usuário foi criado
            // Vou retornar também o endereço do usuário criado e os seus dados no body da resposta
            return Results.Created($"/Usuarios/{NovoUsuario.ID}", NovoUsuario);
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
    }
}