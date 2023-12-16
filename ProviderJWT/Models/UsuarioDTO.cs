namespace ProviderJWT.Models;

public record UsuarioDTO
{
    public string? Id { get; init; }
    public string? Nome { get; init; }
    public string? Email { get; init; }
    public string? Telefone { get; init; }
    public string? Cargo { get; init; }
    public bool? Ativo { get; init; }
}
