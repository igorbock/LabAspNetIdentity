namespace ProviderJWT.Services;

public class ClaimsService : IClaimsService
{
    public ApplicationDbContext? C_Contexto { get; set; }
    public ClaimsService(ApplicationDbContext? p_Contexto)
    {
        C_Contexto = p_Contexto;
    }

    public async Task<IEnumerable<IdentityUserClaim<string>>> CM_ObterTodasClaimsDosUsuarios()
        => await C_Contexto!.UserClaims.ToListAsync();
}
