namespace BackendIS4.Interfaces;

public interface IUsuariosController
{
    public Task<string> Create(RegistrarUsuarioDTO user);
    public IEnumerable<IdentityUser> Read();
    public Task<IdentityUser> Read(string? name);
    public Task<string> Update(IdentityUser user);
    public Task<string> Delete(string name);
}
