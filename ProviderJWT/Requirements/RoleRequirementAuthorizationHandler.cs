namespace ProviderJWT.Requirements;

public class RoleRequirementAuthorizationHandler : AuthorizationHandler<RoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        if (context.User.Claims.Where(a => a.Value.Equals(requirement.C_Role)).Any())
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
