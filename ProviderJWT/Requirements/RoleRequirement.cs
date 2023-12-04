namespace ProviderJWT.Requirements;

public class RoleRequirement : IAuthorizationRequirement
{
    public string? C_Role { get; private set; }

    public RoleRequirement(string? p_Role)
    {
        C_Role = p_Role;
    }
}
