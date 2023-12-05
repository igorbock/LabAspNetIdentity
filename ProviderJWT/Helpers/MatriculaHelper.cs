namespace ProviderJWT.Helpers;

public class MatriculaHelper : IMatriculaHelper
{
    public async Task<Claim> CM_ObterClaimDaMatriculaAsync(UserManager<IdentityUser> p_UserManager)
    {
        var m_Professores = await p_UserManager.GetUsersInRoleAsync("PROFESSOR");
        var m_NumeroProfessores = m_Professores.Count;
        var m_NumeroUsuarios = p_UserManager.Users.Count();
        var m_UsuarioADM = 1;
        var m_TotalAlunos = m_NumeroUsuarios - m_NumeroProfessores - m_UsuarioADM;

        ++m_TotalAlunos;
        var m_NumeroMatricula = m_TotalAlunos.ToString().PadLeft(6, '0');
        return new Claim("matricula", m_NumeroMatricula, typeof(string).Name);
    }
}
