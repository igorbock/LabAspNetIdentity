namespace ProviderJWT.Interfaces;

public interface IUsuariosService
{
    Task<string> CM_CriarProfessorAsync(IdentityUser p_Usuario, RegistrarUsuarioDTO p_UsuarioDTO);
    Task<string> CM_CriarAlunoAsync(IdentityUser p_Usuario, RegistrarUsuarioDTO p_UsuarioDTO);
}
