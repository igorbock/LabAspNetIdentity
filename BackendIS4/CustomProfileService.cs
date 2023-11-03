namespace BackendIS4;

public class CustomProfileService : IProfileService
{
    public UserManager<IdentityUser>? c_UserManager { get; set; }

    public CustomProfileService(UserManager<IdentityUser>? p_UserManager)
    {
        c_UserManager = p_UserManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var m_Subject = context.Subject.GetSubjectId();
        var m_Usuario = await c_UserManager!.FindByIdAsync(m_Subject);
        if (m_Usuario == null)
            throw new ArgumentNullException(nameof(m_Usuario));

        var m_Claims = new List<Claim>
        {
            new Claim("username", m_Usuario.UserName!),
            new Claim("email", m_Usuario.Email!)
        };

        context.IssuedClaims = m_Claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var m_Subject = context.Subject.GetSubjectId();
        var m_Usuario = await c_UserManager!.FindByIdAsync(m_Subject);

        context.IsActive = m_Usuario != null;
    }
}
