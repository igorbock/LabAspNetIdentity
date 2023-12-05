namespace ProviderJWT.Requirements;

public class RoleRequirementAuthorizationHandler : AuthorizationHandler<RoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        foreach(var item in requirement.C_Role!)
            if (context.User.Claims.Where(a => a.Value.Equals(item)).Any())
                context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
