namespace ProviderJWT.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuarioController : Controller
{
    public UserManager<IdentityUser> C_UserManager { get; private set; }
    public RoleManager<IdentityRole> C_RoleManager { get; private set; }
    public IUsuariosService? C_UsuariosService { get; private set; }

    public UsuarioController(
        UserManager<IdentityUser> p_UserManager,
        RoleManager<IdentityRole> p_RoleManager,
        IUsuariosService? p_UsuariosService)
    {
        C_UserManager = p_UserManager;
        C_RoleManager = p_RoleManager;
        C_UsuariosService = p_UsuariosService;
    }

    [HttpPost("professor")]
    [Authorize(Policy = "Administrador")]
    public async Task<string> CM_CriarProfessor([FromBody] RegistrarUsuarioDTO p_UsuarioDTO)
    {
        var m_IdentityUser = new IdentityUser(p_UsuarioDTO.Nome!);
        return await C_UsuariosService!.CM_CriarProfessorAsync(m_IdentityUser, p_UsuarioDTO);
    }


    [HttpPost("aluno")]
    [Authorize(Policy = "Professor")]
    public async Task<string> CM_CriarAluno([FromBody] RegistrarUsuarioDTO p_UsuarioDTO)
    {
        var m_IdentityUser = new IdentityUser(p_UsuarioDTO.Nome!);
        return await C_UsuariosService!.CM_CriarAlunoAsync(m_IdentityUser, p_UsuarioDTO);
    }

    [HttpGet("id")]
    [Authorize(Policy = "Professor")]
    public async Task<IdentityUser> CM_ObterUsuarioPorID(string p_Id) => await C_UserManager.FindByIdAsync(p_Id) ?? throw new KeyNotFoundException();

    [HttpGet]
    [Authorize(Policy = "Administrador")]
    public async Task<IdentityUser> Read(string? name)
    {
        var m_Usuario = new IdentityUser();

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));
        else
            m_Usuario = await C_UserManager.FindByNameAsync(name);

        if (m_Usuario == null)
            throw new Exception("Nenhum usuário encontrado!");

        return m_Usuario;
    }

    [HttpGet("claims")]
    public async Task<IEnumerable<Claim>> CM_ObterClaimsDoUsuario(string p_IdUsuario)
    {
        var m_Usuario = await C_UserManager!.FindByIdAsync(p_IdUsuario) ?? throw new KeyNotFoundException();

        return await C_UserManager!.GetClaimsAsync(m_Usuario);
    }

    [HttpPut]
    [Authorize(Policy = "Administrador")]
    public async Task<string> Update(IdentityUser user)
    {
        var result = await C_UserManager.UpdateAsync(user);
        return result.ToString();
    }

    [HttpDelete]
    [Authorize(Policy = "Administrador")]
    public async Task<string> Delete(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Nome está null!");

        var user = await C_UserManager.FindByNameAsync(name);
        if (user == null) throw new Exception("Usuário não existe!");
        var result = await C_UserManager.DeleteAsync(user);
        return result.ToString();
    }

    [HttpDelete("desativar")]
    [Authorize(Policy = "Administrador")]
    public async Task<string> Desativar(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Nome está null!");

        var user = await C_UserManager.FindByNameAsync(name);
        if (user == null) throw new Exception("Usuário não existe!");
        var result = await C_UserManager.RemovePasswordAsync(user);
        return result.ToString();
    }

    [HttpPost("ativar")]
    [Authorize(Policy = "Administrador")]
    public async Task<string> Ativar(string name, string p_Senha)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Nome está null!");

        if (string.IsNullOrEmpty(p_Senha))
            throw new ArgumentNullException(nameof(p_Senha));

        var user = await C_UserManager.FindByNameAsync(name);
        if (user == null) throw new Exception("Usuário não existe!");
        var result = await C_UserManager.AddPasswordAsync(user, p_Senha);
        return result.ToString();
    }

    [HttpGet("claim")]
    public async Task<IEnumerable<IdentityUser>> CM_ObterUsuariosPorClaim(string p_NomeClaim, string p_ValorClaim)
    {
        if (string.IsNullOrWhiteSpace(p_NomeClaim) || string.IsNullOrWhiteSpace(p_ValorClaim))
            throw new Exception("Nome ou valor da claim estão null!");

        var m_Claim = new Claim(p_NomeClaim, p_ValorClaim);
        return await C_UserManager.GetUsersForClaimAsync(m_Claim);
    }

    [HttpGet("alunos")]
    [Authorize("Professor")]
    public async Task<IEnumerable<AlunoDTO>> CM_ObterAlunos(string? p_Nome) => await C_UsuariosService!.CM_ObterAlunosAsync(p_Nome);

    [HttpGet("professores")]
    [Authorize(Policy = "Administrador")]
    public async Task<IEnumerable<UsuarioDTO>> CM_ObterProfessores(string? p_Nome) => await C_UsuariosService!.CM_ObterProfessoresAsync(p_Nome);
}
