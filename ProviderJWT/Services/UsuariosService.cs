namespace ProviderJWT.Services;

public class UsuariosService : IUsuariosService
{
    public UserManager<IdentityUser>? C_UserManager { get; private set; }
    public RoleManager<IdentityRole>? C_RoleManager { get; private set; }
    public IMatriculaHelper? C_MatriculaHelper { get; private set; }
    public ApplicationDbContext? C_Contexto { get; private set; }

    public UsuariosService(
        UserManager<IdentityUser>? p_UserManager, 
        RoleManager<IdentityRole>? p_RoleManager,
        IMatriculaHelper? p_MatriculaHelper,
        ApplicationDbContext? p_Contexto)
    {
        C_UserManager = p_UserManager;
        C_RoleManager = p_RoleManager;
        C_MatriculaHelper = p_MatriculaHelper;
        C_Contexto = p_Contexto;
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
        p_Usuario.Email = p_UsuarioDTO.Email;
        p_Usuario.PhoneNumber = p_UsuarioDTO.Telefone;

        var m_Matricula = await C_MatriculaHelper!.CM_ObterClaimDaMatriculaAsync(C_UserManager!);
        var m_Resultado = await C_UserManager!.CMX_CriarUsuario(p_Usuario, p_UsuarioDTO.Senha!);

        await C_UserManager!.AddClaimAsync(p_Usuario, m_Matricula);

        var m_RoleAluno = await C_RoleManager!.FindByNameAsync("ALUNO");
        if (m_RoleAluno == null)
            await C_RoleManager!.CreateAsync(new IdentityRole("ALUNO"));

        await C_UserManager!.AddToRoleAsync(p_Usuario, "ALUNO");

        return m_Resultado.ToString();
    }

    public async Task<IEnumerable<AlunoDTO>> CM_ObterAlunosAsync(string? p_Nome)
    {
        var m_Usuarios = await C_Contexto!.Users.ToListAsync();
        if (string.IsNullOrWhiteSpace(p_Nome) == false)
            m_Usuarios = m_Usuarios.Where(a => a.NormalizedUserName!.Contains(p_Nome.ToUpper())).ToList();

        var m_Retorno = new List<AlunoDTO>();
        var m_Roles = await C_Contexto!.Roles.Where(a => a.Name!.Equals("ALUNO")).ToListAsync();
        var m_UserRoles = await C_Contexto!.UserRoles.ToListAsync();
        var m_UserClaims = await C_Contexto!.UserClaims.ToListAsync();

        foreach (var item in m_UserRoles)
        {
            var m_Role = m_Roles.Find(a => a.Id.Equals(item.RoleId));
            if (m_Role == null) continue;
            var m_Usuario = m_Usuarios.Find(a => a.Id.Equals(item.UserId));
            var m_Claim = m_UserClaims.Find(a => a.UserId.Equals(item.UserId));
            m_Retorno.Add(new AlunoDTO
            {
                Id = m_Usuario!.Id,
                Nome = m_Usuario.UserName,
                Email = m_Usuario.Email,
                Telefone = m_Usuario.PhoneNumber,
                Cargo = m_Role!.Name,
                Matricula = int.Parse(m_Claim!.ClaimValue!)
            });
        }

        return m_Retorno;
    }

    public async Task<IEnumerable<UsuarioDTO>> CM_ObterProfessoresAsync(string? p_Nome)
    {
        var m_Usuarios = await C_Contexto!.Users.ToListAsync();
        if (string.IsNullOrWhiteSpace(p_Nome) == false)
            m_Usuarios = m_Usuarios.Where(a => a.NormalizedUserName!.Contains(p_Nome.ToUpper())).ToList();

        var m_Retorno = new List<UsuarioDTO>();
        var m_Roles = await C_Contexto!.Roles.Where(a => a.Name! != "ALUNO" && a.Name! != "ADM").ToListAsync();
        var m_UserRoles = await C_Contexto!.UserRoles.ToListAsync();

        foreach (var item in m_UserRoles)
        {
            var m_Role = m_Roles.Find(a => a.Id.Equals(item.RoleId));
            if (m_Role == null) continue;
            var m_Usuario = m_Usuarios.Find(a => a.Id.Equals(item.UserId));
            m_Retorno.Add(new UsuarioDTO
            {
                Id = m_Usuario!.Id,
                Nome = m_Usuario.UserName,
                Email = m_Usuario.Email,
                Telefone = m_Usuario.PhoneNumber,
                Cargo = m_Role!.Name,
                Ativo = string.IsNullOrEmpty(m_Usuario.PasswordHash) == false ? true : false
            });
        }

        return m_Retorno;
    }
}
