namespace ProviderJWT.Extensions;

public static class UserManagerExtensions
{
    public static async Task<bool> CMX_UsuarioADMExistenteAsync(this UserManager<IdentityUser> p_UserManager)
    {
        var m_UsuariosADM = await p_UserManager.GetUsersInRoleAsync("ADM");
        if (m_UsuariosADM.Count > 0)
            return true;

        return false;
    }

    public static async Task<string> CMX_CriarUsuario(
        this UserManager<IdentityUser> p_UserManager,
        IdentityUser p_Usuario,
        string p_Senha)
    {
        var m_Resultado = await p_UserManager!.CreateAsync(p_Usuario, p_Senha!);
        if (m_Resultado.Succeeded == false)
            throw new Exception(m_Resultado.ToString());

        return m_Resultado.ToString();
    }
}
