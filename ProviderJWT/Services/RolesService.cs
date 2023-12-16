namespace ProviderJWT.Services;

public class RolesService : IRolesService
{
    public ApplicationDbContext? C_Contexto { get; set; }

    public RolesService(ApplicationDbContext p_Contexto)
    {
        C_Contexto = p_Contexto;
    }

    public async Task<IEnumerable<RoleDTO>> CM_ObterUserRolesAsync()
    {
        var m_Retorno = new List<RoleDTO>();
        var m_UserRoles = await C_Contexto!.UserRoles.ToListAsync();
        foreach(var item in m_UserRoles)
        {
            var m_Role = C_Contexto!.Roles.First(a => a.Id.Equals(item.RoleId));
            m_Retorno.Add(new RoleDTO { Nome = m_Role.Name, RoleId = item.RoleId, UsuarioId = item.UserId });
        }

        return m_Retorno;
    }
}
