namespace ProviderJWT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlunoController : Controller
{
    public IUsuarioService<UsuarioDTO> C_AlunoService { get; set; }

    public AlunoController(IUsuarioService<UsuarioDTO> p_AlunoService)
    {
        C_AlunoService = p_AlunoService;
    }

    [HttpPost]
    public async Task<string> CM_RegistrarAluno(UsuarioDTO p_Aluno)
    {
        var m_Claim = await C_AlunoService.CM_CriarUsuarioAsync(p_Aluno);
        return m_Claim.Value;
    }

    [HttpGet("qrcode")]
    public async Task<string> CM_CriarQRCode(string p_Matricula)
    {
        var m_QRCode = await C_AlunoService.CM_CriarQRCodeAsync(p_Matricula);
        var m_ArrayBytesEmString = Convert.ToBase64String(m_QRCode);
        return string.Format("data:image/png;base64,{0}", m_ArrayBytesEmString);
    }

    [HttpGet]
    public async Task<IEnumerable<UsuarioDTO>> CM_ObterAlunos(string? p_Nome) => await C_AlunoService!.CM_ObterUsuariosAsync(p_Nome!);

    [HttpGet("temsenha")]
    public async Task<Tuple<bool, string>> CM_AlunoPossuiSenha(string p_Nome) => await C_AlunoService!.CM_UsuarioPossuiSenhaAsync(p_Nome);

    [HttpDelete]
    public async Task CM_DesativarAluno(string p_Nome)
    {
        var m_Alunos = await C_AlunoService!.CM_ObterUsuariosAsync(p_Nome);
        var m_Aluno = m_Alunos.SingleOrDefault();
        await C_AlunoService!.CM_DesativarUsuarioAsync(m_Aluno!);
    }

    [HttpPut]
    public async Task CM_AlterarTelefone(UsuarioDTO p_Usuario) => await C_AlunoService!.CM_AlterarTelefoneUsuarioAsync(p_Usuario);

    [HttpPost("senha")]
    public async Task CM_AtribuirSenha(RegistrarSenhaDTO p_DTO) => await C_AlunoService!.CM_AtribuirSenhaAoUsuario(p_DTO);
}
