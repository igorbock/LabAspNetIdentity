namespace ProviderJWT.Extensions;

public static class SystemStringExtensions
{
    public static RegistrarUsuarioDTO CMX_ObterUsuarioAdministrador(this string p_JSON)
    {
        if (string.IsNullOrWhiteSpace(p_JSON))
            throw new ArgumentNullException(nameof(p_JSON));

        var m_Usuario = JsonSerializer.Deserialize<RegistrarUsuarioDTO>(p_JSON);
        if (m_Usuario == null)
            throw new JsonException();

        return m_Usuario;
    }
}
