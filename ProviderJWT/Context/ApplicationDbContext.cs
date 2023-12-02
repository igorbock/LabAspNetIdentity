namespace ProviderJWT.Context;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> p_Options) : base(p_Options) { }
}
