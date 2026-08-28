namespace TurminhaDaBagunca.API.Dtos;


// Criando a classe UsuarioCreateDto para representar os dados que serão recebidos na requisição de criação de um novo usuário
// Vou utilizar essa classe para receber os dados do usuário que serão enviados no corpo da requisição
public class UsuarioCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}