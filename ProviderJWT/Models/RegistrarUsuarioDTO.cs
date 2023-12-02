namespace ProviderJWT.Models
{
    public class RegistrarUsuarioDTO
    {
        [MinLength(4, ErrorMessage = "O campo 'Nome' deve ter no mínimo 4 caracteres")]
        [MaxLength(30, ErrorMessage = "O campo 'Nome' deve ter no máximo 30 caracteres")]
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório")]
        public string? Nome { get; set; }

        [MinLength(10, ErrorMessage = "O campo 'E-mail' deve ter no mínimo 10 caracteres")]
        [MaxLength(50, ErrorMessage = "O campo 'E-mail' deve ter no máximo 50 caracteres")]
        [Required(ErrorMessage = "O campo 'E-mail' é obrigatório")]
        public string? Email { get; set; }

        [MinLength(8, ErrorMessage = "O campo 'Telefone' deve ter no mínimo 8 caracteres")]
        [MaxLength(16, ErrorMessage = "O campo 'Telefone' deve ter no máximo 16 caracteres")]
        [Required(ErrorMessage = "O campo 'Telefone' é obrigatório")]
        public string? Telefone { get; set; }

        [MinLength(8, ErrorMessage = "O campo 'Senha' deve ter no mínimo 8 caracteres")]
        [MaxLength(30, ErrorMessage = "O campo 'Senha' deve ter no máximo 30 caracteres")]
        [Required(ErrorMessage = "O campo 'Senha' é obrigatório")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[a-zA-Z0-9]).{8,}$", ErrorMessage = "A senha deve ter 8 caracteres, incluindo pelo menos uma letra minúscula, uma letra maiúscula, um número e um caractere alfanumérico")]
        public string? Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senhas devem ser iguais")]
        public string? ConfirmaSenha { get; set; }
    }
}
