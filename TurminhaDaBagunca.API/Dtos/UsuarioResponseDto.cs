namespace TurminhaDaBagunca.API.Dtos;


public class UsuarioResponseDto
{
    public int ID { get; set;}
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
}