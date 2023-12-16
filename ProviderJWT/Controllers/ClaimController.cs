namespace ProviderJWT.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "Administrador")]
public class ClaimController : Controller
{
    public IClaimsService? C_ClaimsService { get; set; }

    public ClaimController(IClaimsService? p_ClaimsService)
    {
        C_ClaimsService = p_ClaimsService;
    }

    [HttpGet]
    public async Task<IEnumerable<IdentityUserClaim<string>>> CM_ObterClaimsDosUsuarios()
        => await C_ClaimsService!.CM_ObterTodasClaimsDosUsuarios();
}
