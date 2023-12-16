namespace ProviderJWT.Interfaces;

public interface IRolesService
{
    Task<IEnumerable<RoleDTO>> CM_ObterUserRolesAsync();
}
