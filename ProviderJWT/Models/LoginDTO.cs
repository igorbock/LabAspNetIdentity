namespace ProviderJWT.Models;

public class LoginDTO
{
    [MinLength(4, ErrorMessage = "O campo 'Usuario' deve ter no mínimo 4 caracteres")]
    [MaxLength(30, ErrorMessage = "O campo 'Usuario' deve ter no máximo 30 caracteres")]
    [Required(ErrorMessage = "O campo 'Usuario' é obrigatório")]
    public string? Usuario { get; set; }

    [Required(ErrorMessage = "O campo 'Senha' é obrigatório")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[a-zA-Z0-9]).{8,}$", ErrorMessage = "A senha deve ter 8 caracteres, incluindo pelo menos uma letra minúscula, uma letra maiúscula, um número e um caractere alfanumérico")]
    public string? Senha { get; set; }
}
