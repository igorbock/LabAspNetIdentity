namespace ProviderJWT.Extensions;

public static class IServiceScopeExtensions
{
    public static async Task CMX_MigrarUsuarioADMAsync(this IServiceScope p_IServiceScope)
    {
        var m_UsuarioADM = p_IServiceScope.CMX_ObterUsuarioADM();

        UserManager<IdentityUser> m_UserManager;
        m_UserManager = p_IServiceScope.ServiceProvider.GetService<UserManager<IdentityUser>>() ?? throw new NullReferenceException(nameof(m_UserManager));

        var m_UsuarioADMExistente = await m_UserManager.CMX_UsuarioADMExistenteAsync();
        if (m_UsuarioADMExistente)
            return;

        var m_Usuario = new IdentityUser
        {
            UserName = m_UsuarioADM.Nome,
            NormalizedUserName = m_UsuarioADM.Nome!.ToUpper(),
            PhoneNumber = m_UsuarioADM.Telefone,
            PhoneNumberConfirmed = true,
            Email = m_UsuarioADM.Email,
            EmailConfirmed = true
        };

        await m_UserManager.CreateAsync(m_Usuario, m_UsuarioADM.Senha!);

        RoleManager<IdentityRole> m_RoleManager;
        m_RoleManager = p_IServiceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>() ?? throw new NullReferenceException(nameof(m_RoleManager));
        var m_RoleADM = new IdentityRole("ADM");
        await m_RoleManager.CreateAsync(m_RoleADM);

        await m_UserManager.AddToRoleAsync(m_Usuario, "ADM");
    }

    public static RegistrarUsuarioDTO CMX_ObterUsuarioADM(this IServiceScope p_ServiceScope)
    {
        IConfiguration m_Configuracao;
        m_Configuracao = p_ServiceScope.ServiceProvider.GetService<IConfiguration>() ?? throw new NullReferenceException(nameof(m_Configuracao));

        var m_UsuarioADM = new RegistrarUsuarioDTO
        {
            Nome = m_Configuracao!["Usuario:Nome"],
            Email = m_Configuracao["Usuario:Email"],
            Telefone = m_Configuracao["Usuario:Telefone"],
            Senha = m_Configuracao["Usuario:Senha"],
            ConfirmaSenha = m_Configuracao["Usuario:ConfirmaSenha"]
        };

        return m_UsuarioADM;
    }

    public static void CMX_MigrarBancoDeDados(this IServiceScope p_ServiceScope)
    {
        var m_ApplicationDbContext = p_ServiceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (m_ApplicationDbContext == null)
            throw new Exception($"{nameof(m_ApplicationDbContext)} é null.");

        m_ApplicationDbContext.Database.Migrate();
    }
}
