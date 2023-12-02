namespace ProviderJWT.Extensions;

public static class IServiceScopeExtension
{
    public static RegistrarUsuarioDTO CMX_ObterUsuarioADM(this IServiceScope p_ServiceScope)
    {
        var m_Configuracao = p_ServiceScope.ServiceProvider.GetRequiredService<IConfiguration>();
        var m_UsuarioADM = new RegistrarUsuarioDTO
        {
            Nome = m_Configuracao["Usuario:Nome"],
            Email = m_Configuracao["Usuario:Email"],
            Telefone = m_Configuracao["Usuario:Telefone"],
            Senha = m_Configuracao["Usuario:Senha"],
            ConfirmaSenha = m_Configuracao["Usuario:ConfirmaSenha"]
        };

        if (m_UsuarioADM.Nome != "Administrador")
            throw new JsonException();

        return m_UsuarioADM;
    }
}
