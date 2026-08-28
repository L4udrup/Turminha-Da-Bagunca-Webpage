using Microsoft.EntityFrameworkCore;
using TurminhaDaBagunca.API.Models;

namespace TurminhaDaBagunca.API.Data;

// Criando a classe AppDbContext que herda as funcionalidades de DbContext(EF) para trabalhar com o banco de dados
public class AppDbContext : DbContext
{
    // Criando o construtor do AppDbContext para receber as configurações do banco
    // e passar essas configurações para o DbContext
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    // Criando um DbSet de Usuario para representar a tabela de usuários no banco
    // e permitir consultar, adicionar, alterar e remover usuários através do Entity Framework
    public DbSet<Usuario> Usuarios { get; set; }
}