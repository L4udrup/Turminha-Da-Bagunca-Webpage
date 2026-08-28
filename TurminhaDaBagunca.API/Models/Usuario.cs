namespace TurminhaDaBagunca.API.Models;

public class Usuario
{
    public int ID {get; set; }
    public string Nome {get; set; } = string.Empty;
    public string Email {get; set; } = string.Empty;
    public string Telefone {get; set; } = string.Empty;
    public string SenhaHash {get; set; } = string.Empty;
    public DateTime DataCadastro {get; set; } = DateTime.UtcNow;
}