namespace ProviderJWT.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "Administrador")]
public class RoleController : Controller
{
    public IRolesService? C_RoleService { get; set; }

    public RoleController(IRolesService? p_RoleService)
    {
        C_RoleService = p_RoleService;
    }

    [HttpGet]
    public async Task<IEnumerable<RoleDTO>> CM_ObterUsuariosRoles()
        => await C_RoleService!.CM_ObterUserRolesAsync();
}
