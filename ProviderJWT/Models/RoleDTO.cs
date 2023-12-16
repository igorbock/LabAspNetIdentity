namespace ProviderJWT.Models;

public record RoleDTO
{
    public string? RoleId { get; init; }
    public string? UsuarioId { get; init; }
    public string? Nome { get; init; }
}
