namespace ProviderJWT.Models;

public class RegistrarUsuarioDTO
{
    private string? nome;
    private string? email;
    private string? telefone;
    private string? senha;
    private string? confirmasenha;

    [MinLength(4, ErrorMessage = "O campo 'Nome' deve ter no mínimo 4 caracteres")]
    [MaxLength(30, ErrorMessage = "O campo 'Nome' deve ter no máximo 30 caracteres")]
    [Required(ErrorMessage = "O campo 'Nome' é obrigatório")]
    [RegularExpression("^[\\w-]{4,30}$")]
    public string? Nome
    {
        get => nome;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(Nome), "O campo 'Nome' é obrigatório");
            if (value.Length < 4) throw new ArgumentOutOfRangeException(nameof(Nome), "O campo 'Nome' deve ter no mínimo 4 caracteres");
            if (value.Length > 30) throw new ArgumentOutOfRangeException(nameof(Nome), "O campo 'Nome' deve ter no máximo 30 caracteres");

            nome = value;
        }
    }

    [MinLength(10, ErrorMessage = "O campo 'Email' deve ter no mínimo 10 caracteres")]
    [MaxLength(50, ErrorMessage = "O campo 'Email' deve ter no máximo 50 caracteres")]
    [Required(ErrorMessage = "O campo 'Email' é obrigatório")]
    [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
    public string? Email
    {
        get => email;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(Email), "O campo 'Email' é obrigatório");
            if (value.Length < 10) throw new ArgumentOutOfRangeException(nameof(Email), "O campo 'Email' deve ter no mínimo 10 caracteres");
            if (value.Length > 50) throw new ArgumentOutOfRangeException(nameof(Email), "O campo 'Email' deve ter no máximo 50 caracteres");

            var m_Validacao = Regex.Match(value, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            if (m_Validacao.Success == false)
                throw new ArgumentException("O campo 'Email' não é correspondente", nameof(Email));

            email = value;
        }
    }
    
    [Required(ErrorMessage = "O campo 'Telefone' é obrigatório")]
    [RegularExpression("^[(][0-9]{2}[)][9][0-9]{4}[-][0-9]{4}$")]
    public string? Telefone
    {
        get => telefone;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(Telefone), "O campo 'Telefone' é obrigatório");
            var m_Validacao = Regex.Match(value, "^[(][0-9]{2}[)][9][0-9]{4}[-][0-9]{4}$");
            if (m_Validacao.Success == false)
                throw new ArgumentException("O campo 'Telefone' deve ser como o correspondente: (xx)9xxxx-xxxx", nameof(Telefone));

            telefone = value;
        }
    }

    [MinLength(8, ErrorMessage = "O campo 'Senha' deve ter no mínimo 8 caracteres")]
    [MaxLength(30, ErrorMessage = "O campo 'Senha' deve ter no máximo 30 caracteres")]
    [Required(ErrorMessage = "O campo 'Senha' é obrigatório")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[a-zA-Z0-9]).{8,}$", ErrorMessage = "A senha deve ter 8 caracteres, incluindo pelo menos uma letra minúscula, uma letra maiúscula, um número e um caractere alfanumérico")]
    public string? Senha
    {
        get => senha;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(Senha), "O campo 'Senha' é obrigatório");
            if (value.Length < 8) throw new ArgumentOutOfRangeException(nameof(Senha), "O campo 'Senha' deve ter no mínimo 8 caracteres");
            if (value.Length > 30) throw new ArgumentOutOfRangeException(nameof(Senha), "O campo 'Senha' deve ter no máximo 30 caracteres");
            var m_Validacao = Regex.Match(value, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[a-zA-Z0-9]).{8,}$");
            if (m_Validacao.Success == false)
                throw new ArgumentException("A senha deve ter 8 caracteres, incluindo pelo menos uma letra minúscula, uma letra maiúscula, um número e um caractere alfanumérico");

            senha = value;
        }
    }

    [Compare("Senha", ErrorMessage = "As senhas devem ser iguais")]
    public string? ConfirmaSenha
    {
        get => confirmasenha;
        set
        {
            if (string.Compare(Senha, value) != 0)
                throw new ArgumentException("As senhas devem ser iguais");

            confirmasenha = value;
        }
    }
}
