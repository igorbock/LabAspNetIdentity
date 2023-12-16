namespace ProviderJWT.Interfaces;

public interface IUsuariosService
{
    Task<string> CM_CriarProfessorAsync(IdentityUser p_Usuario, RegistrarUsuarioDTO p_UsuarioDTO);
    Task<string> CM_CriarAlunoAsync(IdentityUser p_Usuario, RegistrarUsuarioDTO p_UsuarioDTO);
    Task<IEnumerable<AlunoDTO>> CM_ObterAlunosAsync(string? p_Nome);
    Task<IEnumerable<UsuarioDTO>> CM_ObterProfessoresAsync(string? p_Nome);
}
