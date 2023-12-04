namespace ProviderJWT.Extensions;

public static class MigrationExtensions
{
    public static async Task MigrationExtensionAsync(this WebApplication p_Application)
    {
        try
        {
            //ServiceScope
            var m_Service = p_Application.Services.GetService<IServiceScopeFactory>();
            if (m_Service == null)
                throw new Exception($"{nameof(IServiceScopeFactory)} é null");

            using var m_ServiceScope = m_Service.CreateScope();

            //LabAcademiaContext + Migração
            var m_ApplicationDbContext = m_ServiceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (m_ApplicationDbContext == null)
                throw new Exception($"{nameof(m_ApplicationDbContext)} é null.");
            m_ApplicationDbContext.Database.Migrate();

            //Usuário ADM
            var m_UsuarioADM = m_ServiceScope.CMX_ObterUsuarioADM();

            var m_UserManager = m_ServiceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var m_ADM = await m_UserManager.GetUsersInRoleAsync("ADM");
            if (m_ADM.Count > 0)
                return;

            var m_Usuario = new IdentityUser
            {
                UserName = m_UsuarioADM.Nome,
                NormalizedUserName = m_UsuarioADM.Nome!.ToUpper(),
                PhoneNumber = m_UsuarioADM.Telefone,
                PhoneNumberConfirmed = true,
                Email = m_UsuarioADM.Email,
                EmailConfirmed = true
            };

            await m_UserManager.CreateAsync(m_Usuario, m_UsuarioADM.Senha!);

            var m_RoleManager = m_ServiceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var m_RoleADM = new IdentityRole("ADM");
            await m_RoleManager.CreateAsync(m_RoleADM);

            await m_UserManager.AddToRoleAsync(m_Usuario, "ADM");
        }
        catch (JsonException) { }
        catch (Exception) { }
    }
}