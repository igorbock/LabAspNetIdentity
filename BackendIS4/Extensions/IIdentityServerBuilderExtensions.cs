namespace BackendIS4.Extensions;

public static class IIdentityServerBuilderExtensions
{
    public static void CMX_ConfigurarDEBUG(this IIdentityServerBuilder builder)
    {
        builder
            .AddDeveloperSigningCredential()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddAspNetIdentity<IdentityUser>()
            .AddProfileService<CustomProfileService>()
            .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();
    }

    public static void CMX_ConfigurarRELEASE(this IIdentityServerBuilder builder, string p_ConnectionString, string p_MigrationsAssembly)
    {
        builder
            .AddDeveloperSigningCredential()
            .AddConfigurationStore(opt => { opt.ConfigureDbContext = db => db.UseNpgsql(p_ConnectionString, sql => sql.MigrationsAssembly(p_MigrationsAssembly)); })
            .AddOperationalStore(opt => { opt.ConfigureDbContext = db => db.UseNpgsql(p_ConnectionString, sql => sql.MigrationsAssembly(p_MigrationsAssembly)); })
            .AddAspNetIdentity<IdentityUser>()
            .AddProfileService<CustomProfileService>()
            .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();
    }
}
