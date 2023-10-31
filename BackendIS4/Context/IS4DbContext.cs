namespace BackendIS4.Context;

public class IS4DbContext : IdentityDbContext
{
    public IS4DbContext() : base() { }

    public IS4DbContext(DbContextOptions<IS4DbContext> _options) : base(_options) { }
}
