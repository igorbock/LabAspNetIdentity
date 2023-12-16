namespace ProviderJWT.Interfaces;

public interface IClaimsService
{
    Task<IEnumerable<IdentityUserClaim<string>>> CM_ObterTodasClaimsDosUsuarios();
}
