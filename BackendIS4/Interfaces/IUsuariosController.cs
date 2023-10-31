namespace BackendIS4.Interfaces;

public interface IUsuariosController
{
    public Task<string> Create(UsuarioDTO user);
    public Task<IEnumerable<IdentityUser>> Read(string? name);
    public Task<string> Update(IdentityUser user);
    public Task<string> Delete(string name);
}
