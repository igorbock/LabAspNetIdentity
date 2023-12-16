namespace ProviderJWT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenController : Controller
{
    public IConfiguration C_Configuration { get; set; }
    public UserManager<IdentityUser>? C_UserManager { get; set; }
    public SignInManager<IdentityUser>? C_SignInManager { get; set; }

    public TokenController(
        IConfiguration p_Configuration,
        SignInManager<IdentityUser>? p_SignInManager,
        UserManager<IdentityUser>? p_UserManager)
    {
        C_Configuration = p_Configuration;
        C_SignInManager = p_SignInManager;
        C_UserManager = p_UserManager;
    }

    [HttpPost]
    public async Task<IActionResult> CM_Login([FromBody] LoginDTO p_LoginDTO)
    {
        var m_Usuario = await C_UserManager!.FindByNameAsync(p_LoginDTO.Usuario!);
        if (m_Usuario == null)
            return Unauthorized();

        var m_Resultado = await C_SignInManager!.CheckPasswordSignInAsync(m_Usuario, p_LoginDTO.Senha!, lockoutOnFailure: false);
        if (m_Resultado.Succeeded == false)
            return Unauthorized();

        var m_Claims = await C_UserManager.GetClaimsAsync(m_Usuario);
        var m_Roles = await C_UserManager.GetRolesAsync(m_Usuario);
        foreach (var item in m_Roles)
            m_Claims.Add(new Claim(ClaimTypes.Role, item));

        m_Claims.Add(new Claim(ClaimTypes.Name, m_Usuario.UserName!));

        var m_Retorno = cm_GerarJWT(m_Claims);

        return Ok(m_Retorno);
    }

    [NonAction]
    private string cm_GerarJWT(IEnumerable<Claim>? p_Claims = null)
    {
        var m_Issuer = C_Configuration["Jwt:Issuer"];
        var m_Audience = C_Configuration["Jwt:Audience"];
        var m_Expiracao = DateTime.UtcNow.AddMinutes(5);
        var m_SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(C_Configuration["Jwt:Key"]!));
        var m_Credentials = new SigningCredentials(m_SecurityKey, SecurityAlgorithms.HmacSha256);

        var m_NovoToken = new JwtSecurityToken(
            issuer: m_Issuer,
            audience: m_Audience,
            expires: m_Expiracao,
            claims: p_Claims,
            signingCredentials: m_Credentials);

        var m_TokenHandler = new JwtSecurityTokenHandler();
        var m_Retorno = m_TokenHandler.WriteToken(m_NovoToken);

        return m_Retorno;
    }
}
