using System.ComponentModel.DataAnnotations;

namespace TurminhaDaBagunca.API.Dtos;

public class UsuarioLoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MinLength(8)]
    public string Senha { get; set; } = string.Empty;
}