namespace ProviderJWT.Services;

public class UsuariosService : IUsuariosService
{
    public UserManager<IdentityUser>? C_UserManager { get; set; }
    public RoleManager<IdentityRole>? C_RoleManager { get; set; }
    public IMatriculaHelper? C_MatriculaHelper { get; private set; }

    public UsuariosService(
        UserManager<IdentityUser>? p_UserManager, 
        RoleManager<IdentityRole>? p_RoleManager,
        IMatriculaHelper? p_MatriculaHelper)
    {
        C_UserManager = p_UserManager;
        C_RoleManager = p_RoleManager;
        C_MatriculaHelper = p_MatriculaHelper;

    }

    public async Task<string> CM_CriarProfessorAsync(
        IdentityUser p_Usuario,
        RegistrarUsuarioDTO p_UsuarioDTO)
    {
        p_Usuario.Email = p_UsuarioDTO.Email;
        p_Usuario.PhoneNumber = p_UsuarioDTO.Telefone;

        var m_Resultado = await C_UserManager!.CMX_CriarUsuario(p_Usuario, p_UsuarioDTO.Senha!);

        var m_RoleProfessor = await C_RoleManager!.FindByNameAsync("PROFESSOR");
        if (m_RoleProfessor == null)
            await C_RoleManager!.CreateAsync(new IdentityRole("PROFESSOR"));

        await C_UserManager!.AddToRoleAsync(p_Usuario, "PROFESSOR");

        return m_Resultado.ToString();
    }

    public async Task<string> CM_CriarAlunoAsync(IdentityUser p_Usuario, RegistrarUsuarioDTO p_UsuarioDTO)
    {
        var m_IdentityUser = new IdentityUser(p_UsuarioDTO.Nome!);
        var m_Matricula = await C_MatriculaHelper!.CM_ObterClaimDaMatriculaAsync(C_UserManager!);
        var m_Resultado = await C_UserManager!.CMX_CriarUsuario(m_IdentityUser, p_UsuarioDTO.Senha!);

        await C_UserManager!.AddClaimAsync(m_IdentityUser, m_Matricula);

        return m_Resultado.ToString();
    }
}
