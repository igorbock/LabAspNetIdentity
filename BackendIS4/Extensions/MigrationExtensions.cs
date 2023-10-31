namespace BackendIS4.Extensions;

public static class MigrationExtensions
{
    public static async Task MigrationExtensionAsync(this WebApplication _application)
    {
        //IdentityServer4 Configurations
        var service = _application.Services.GetService<IServiceScopeFactory>();
        if (service == null)
            throw new Exception($"{nameof(IServiceScopeFactory)} é null");

        using var serviceScope = service.CreateScope();
        var persistedGrantDbContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
        persistedGrantDbContext.Database.Migrate();

        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        context.Database.Migrate();

        var naoExisteClients = context.Clients.Any() == false;
        if (naoExisteClients)
        {
            foreach (var client in Config.Clients)
                context.Clients.Add(client.ToEntity());

            context.SaveChanges();
        }

        var naoExisteIdentityResources = context.IdentityResources.Any() == false;
        if (naoExisteIdentityResources)
        {
            foreach (var resource in Config.IdentityResources)
                context.IdentityResources.Add(resource.ToEntity());

            context.SaveChanges();
        }

        var naoExisteApiScopes = context.ApiScopes.Any() == false;
        if (naoExisteApiScopes)
        {
            foreach (var resource in Config.ApiScopes)
                context.ApiScopes.Add(resource.ToEntity());

            context.SaveChanges();
        }

        //Usuários - ASP.NET Identity
        var IS4DbContext = serviceScope.ServiceProvider.GetRequiredService<IS4DbContext>();
        if (IS4DbContext == null)
            throw new Exception($"{nameof(IS4DbContext)} é null.");
        IS4DbContext.Database.Migrate();

        var userMgr = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var jaExisteUsuariosMigrados = userMgr.Users.Count() > 0;
        if (jaExisteUsuariosMigrados)
            return;

        var usuariosIdentity = new List<IdentityUser>();
        var usuarios = Config.TestUsers;

        foreach (var item in usuarios)
        {
            var novoUsuario = new IdentityUser
            {
                UserName = item.Username,
                Email = $"{item.Username.ToLower()}@email.com",
                EmailConfirmed = true
            };

            usuariosIdentity.Add(novoUsuario);

            var resultado = await userMgr.CreateAsync(novoUsuario, item.Password);
            if (resultado.Succeeded == false)
            {
                var erros = new List<string>();
                foreach (var erro in resultado.Errors)
                    erros.Add($"Código: {erro.Code} | Erro: {erro.Description}");

                var errosEmString = string.Join("\n", erros);

                throw new Exception($"Ocorreu um erro com o usuário '{novoUsuario.UserName}': {errosEmString}");
            }
        }
    }
}
