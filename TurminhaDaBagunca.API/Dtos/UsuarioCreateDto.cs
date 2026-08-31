namespace TurminhaDaBagunca.API.Dtos;
using System.ComponentModel.DataAnnotations;


// Criando a classe UsuarioCreateDto para representar os dados que serão recebidos na requisição de criação de um novo usuário
// Vou utilizar essa classe para receber os dados do usuário que serão enviados no corpo da requisição
public class UsuarioCreateDto
{
    [Required]
    public string Nome { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [Phone]
    public string Telefone { get; set; } = string.Empty;
    [Required]
    [MinLength(8)]
    public string Senha { get; set; } = string.Empty;
}