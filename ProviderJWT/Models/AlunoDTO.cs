namespace ProviderJWT.Models;

public record AlunoDTO
{
    public string? Id { get; init; }
    public string? Nome { get; init; }
    public string? Email { get; init; }
    public string? Telefone { get; init; }
    public string? Cargo { get; init; }
    public int? Matricula { get; init; }
}
