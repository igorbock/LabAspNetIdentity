namespace BackendIS4;

public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly UserManager<IdentityUser>? c_UserManager;

    public CustomResourceOwnerPasswordValidator(UserManager<IdentityUser>? p_UserManager)
    {
        c_UserManager = p_UserManager;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var m_User = await c_UserManager!.FindByNameAsync(context.UserName);
        if (m_User == null)
            throw new ArgumentNullException(nameof(m_User));

        if (await c_UserManager!.CheckPasswordAsync(m_User!, context.Password))
            context.Result = new GrantValidationResult(m_User!.Id, OidcConstants.AuthenticationMethods.Password);
    }
}
