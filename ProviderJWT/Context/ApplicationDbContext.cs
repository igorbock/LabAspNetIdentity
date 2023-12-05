namespace ProviderJWT.Context;

public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> p_Options) : base(p_Options) { }
}
