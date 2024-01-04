namespace ProviderJWT.Models;

public record RegistrarSenhaDTO
{
    public string? C_Matricula { get; init; }
    public string? C_Senha { get; init; }
}
