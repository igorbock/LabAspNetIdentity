using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;

namespace BackendIS4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : Controller, IUsuariosController
{
    private readonly UserManager<IdentityUser> _UserManager;

    public UsuariosController(UserManager<IdentityUser> userManager)
    {
        _UserManager = userManager;
    }

    [HttpPost]
    public async Task<string> Create(RegistrarUsuarioDTO user)
    {
        var novoUser = new IdentityUser(user.Nome!)
        {
            Email = user.Email,
            PhoneNumber = user.Telefone
        };

        var result = await _UserManager.CreateAsync(novoUser, user.Senha!);
        if(result.Errors.Any())
            throw new Exception(result.ToString());

        return result.ToString();
    }

    //[Authorize(Policy = "Administrador")]

    [HttpGet("todos")]
    public IEnumerable<IdentityUser> Read() => _UserManager.Users;

    [HttpGet]
    public async Task<IdentityUser> Read(string? name)
    {
        var m_Usuario = new IdentityUser();

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));
        else
            m_Usuario = await _UserManager.FindByNameAsync(name);

        if (m_Usuario == null)
            throw new Exception("Nenhum usuário encontrado!");

        return m_Usuario;
    }

    [HttpPut]
    public async Task<string> Update(IdentityUser user)
    {
        var result = await _UserManager.UpdateAsync(user);
        return result.ToString();
    }

    [HttpDelete]
    public async Task<string> Delete(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Nome está null!");

        var user = await _UserManager.FindByNameAsync(name);
        if (user == null) throw new Exception("Usuário não existe!");
        var result = await _UserManager.DeleteAsync(user);
        return result.ToString();
    }
}
