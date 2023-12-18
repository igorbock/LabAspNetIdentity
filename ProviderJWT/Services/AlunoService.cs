namespace ProviderJWT.Services;

public class AlunoService : IUsuarioService<UsuarioDTO>
{
    public UserManager<IdentityUser>? C_UserManager { get; private set; }
    public RoleManager<IdentityRole>? C_RoleManager { get; private set; }
    public IMatriculaHelper? C_MatriculaHelper { get; private set; }

    public AlunoService(
        UserManager<IdentityUser> p_UserManager,
        RoleManager<IdentityRole> p_RoleManager,
        IMatriculaHelper? c_MatriculaHelper)
    {
        C_UserManager = p_UserManager;
        C_RoleManager = p_RoleManager;
        C_MatriculaHelper = c_MatriculaHelper;
    }

    public async Task<Claim> CM_CriarUsuarioAsync(UsuarioDTO p_Usuario)
    {
        var m_NovoUsuario = new IdentityUser
        {
            UserName = p_Usuario.Nome,
            Email = p_Usuario.Email,
            PhoneNumber = p_Usuario.Telefone
        };

        var m_ResultadoUsuario = await C_UserManager!.CreateAsync(m_NovoUsuario);
        if (m_ResultadoUsuario.Succeeded == false)
            throw new Exception(m_ResultadoUsuario.Errors.ToString());

        var m_Matricula = await C_MatriculaHelper!.CM_ObterClaimDaMatriculaAsync(C_UserManager!);
        await C_UserManager!.AddClaimAsync(m_NovoUsuario, m_Matricula);

        var m_RoleAluno = await C_RoleManager!.FindByNameAsync(Constantes.ALUNO);
        if (m_RoleAluno == null)
            await C_RoleManager!.CreateAsync(new IdentityRole(Constantes.ALUNO));

        await C_UserManager!.AddToRoleAsync(m_NovoUsuario, Constantes.ALUNO);

        return m_Matricula;
    }

    public async Task<IEnumerable<UsuarioDTO>> CM_ObterUsuariosAsync(string p_Nome)
    {
        var m_ArgumentoEstaPreenchido = string.IsNullOrWhiteSpace(p_Nome) == false;
        var m_Alunos = await C_UserManager!.GetUsersInRoleAsync(Constantes.ALUNO);
        if (m_ArgumentoEstaPreenchido)
            m_Alunos = m_Alunos.Where(a => a.UserName!.Contains(p_Nome)).ToList();

        var m_Retorno = new List<UsuarioDTO>();
        foreach (var item in m_Alunos)
        {
            m_Retorno.Add(new UsuarioDTO
            {
                Id = item.Id,
                Cargo = Constantes.ALUNO,
                Email = item.Email,
                Telefone = item.PhoneNumber,
                Nome = item.UserName,
                Ativo = !string.IsNullOrEmpty(item.PasswordHash)
            });
        }

        return m_Retorno;
    }

    public Task<byte[]> CM_CriarQRCodeAsync(string p_Matricula)
    {
        var m_QRCodeOptions = new QrCodeEncodingOptions
        {
            DisableECI = true,
            CharacterSet = "ISO-8859-1",
            Width = 300,
            Height = 300
        };

        var m_QRCodeWriter = new BarcodeWriterPixelData()
        {
            Format = BarcodeFormat.QR_CODE,
            Options = m_QRCodeOptions
        };

        var m_PixelData = m_QRCodeWriter.Write(p_Matricula);

#pragma warning disable CA1416 // Validar a compatibilidade da plataforma
        using var m_Bitmap = new Bitmap(m_PixelData.Width, m_PixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

        using var m_MemoryStream = new MemoryStream();

        var m_BitmapData = m_Bitmap.LockBits(new System.Drawing.Rectangle(0, 0, m_PixelData.Width, m_PixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        try
        {
            System.Runtime.InteropServices.Marshal.Copy(m_PixelData.Pixels, 0, m_BitmapData.Scan0, m_PixelData.Pixels.Length);
        }
        finally
        {
            m_Bitmap.UnlockBits(m_BitmapData);
        }

        // save to stream as PNG
        m_Bitmap.Save(m_MemoryStream, System.Drawing.Imaging.ImageFormat.Png);
        return Task.FromResult(m_MemoryStream.ToArray());
#pragma warning restore CA1416 // Validar a compatibilidade da plataforma
    }

    public async Task CM_AtivarOuDesativarUsuarioAsync(UsuarioDTO p_Usuario)
    {
        var m_Usuario = await C_UserManager!.FindByIdAsync(p_Usuario.Id!) ?? throw new KeyNotFoundException(nameof(p_Usuario.Id));
        var m_UsuarioAtivo = await C_UserManager.HasPasswordAsync(m_Usuario);
        if (m_UsuarioAtivo == false)
            return;

        var m_Logins = await C_UserManager.GetLoginsAsync(m_Usuario);
        foreach (var item in m_Logins)
            await C_UserManager.RemoveLoginAsync(m_Usuario, item.LoginProvider, item.ProviderKey);

        var m_Resultado = await C_UserManager!.RemovePasswordAsync(m_Usuario);
        if (m_Resultado.Succeeded == false)
            throw new Exception(m_Resultado.Errors.ToString());
    }

    public async Task<Tuple<bool, string>> CM_UsuarioPossuiSenhaAsync(string p_Nome)
    {
        var m_Usuario = await C_UserManager!.FindByNameAsync(p_Nome) ?? throw new KeyNotFoundException(nameof(p_Nome));
        var m_PossuiSenha = await C_UserManager!.HasPasswordAsync(m_Usuario);
        var m_Claims = await C_UserManager!.GetClaimsAsync(m_Usuario);
        var m_ExisteMatricula = m_Claims.Where(a => a.Type.Equals("matricula")).Any();
        if (m_ExisteMatricula == false)
            return new Tuple<bool, string>(m_PossuiSenha, "EhProfessor");

        return new Tuple<bool, string>(m_PossuiSenha, m_Claims.First(a => a.Type.Equals("matricula")).Value);
    }

    public async Task CM_AlterarTelefoneUsuarioAsync(UsuarioDTO p_Usuario)
    {
        var m_Usuario = await C_UserManager!.FindByNameAsync(p_Usuario.Nome!) ?? throw new KeyNotFoundException(nameof(p_Usuario.Nome));
        var m_Token = await C_UserManager.GenerateChangePhoneNumberTokenAsync(m_Usuario, p_Usuario.Telefone!);
        var m_Resultado = await C_UserManager!.ChangePhoneNumberAsync(m_Usuario, p_Usuario.Telefone!, m_Token);
        if (m_Resultado.Succeeded == false)
            throw new Exception(m_Resultado.Errors.ToString());
    }
}
