namespace ProviderJWT.Interfaces;

public interface IMatriculaHelper
{
    Task<Claim> CM_ObterClaimDaMatriculaAsync(UserManager<IdentityUser> p_UserManager);
}
