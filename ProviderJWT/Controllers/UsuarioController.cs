namespace ProviderJWT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : Controller
{
    public UserManager<IdentityUser> C_UserManager { get; set; }

    public UsuarioController(UserManager<IdentityUser> p_UserManager)
    {
        C_UserManager = p_UserManager;
    }

    [HttpPost]
    public async Task<string> Create(RegistrarUsuarioDTO user)
    {
        var novoUser = new IdentityUser(user.Nome!)
        {
            Email = user.Email,
            PhoneNumber = user.Telefone
        };        

        var m_NumeroUsuarios = C_UserManager.Users.Count();
        ++m_NumeroUsuarios;
        var m_NumeroMatricula = m_NumeroUsuarios.ToString().PadLeft(6, '0');
        var m_Matricula = new Claim("matricula", m_NumeroMatricula, typeof(string).Name);

        var result = await C_UserManager.CreateAsync(novoUser, user.Senha!);
        if (result.Errors.Any())
            throw new Exception(result.ToString());

        await C_UserManager.AddClaimAsync(novoUser, m_Matricula);

        return result.ToString();
    }

    [HttpGet("todos")]
    public IEnumerable<IdentityUser> Read() => C_UserManager.Users;

    [HttpGet("id")]
    public async Task<IdentityUser> CM_ObterUsuarioPorID(string p_Id) => await C_UserManager.FindByIdAsync(p_Id) ?? throw new KeyNotFoundException();

    [HttpGet]
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
    public async Task<string> Update(IdentityUser user)
    {
        var result = await C_UserManager.UpdateAsync(user);
        return result.ToString();
    }

    [HttpDelete]
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
}
