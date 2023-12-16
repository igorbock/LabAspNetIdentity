namespace ProviderJWT.Interfaces;

public interface IUsuarioService<TipoUsuario> where TipoUsuario : class
{
    Task<IEnumerable<TipoUsuario>> CM_ObterUsuariosAsync(string p_Nome);
    Task<Claim> CM_CriarUsuarioAsync(TipoUsuario p_Usuario);
    Task<byte[]> CM_CriarQRCodeAsync(string p_Matricula);
    Task CM_DesativarUsuarioAsync(TipoUsuario p_Usuario);
    Task<Tuple<bool, string>> CM_UsuarioPossuiSenhaAsync(string p_Nome);
}
