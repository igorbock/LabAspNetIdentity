namespace TestesProviderJWT.Helpers;

public class MatriculaHelperTestes
{
    public Mock<UserManager<IdentityUser>>? C_UserManager { get; private set; }
    public IMatriculaHelper? C_Helper { get; set; }

    [SetUp]
    public void Setup()
    {
        var m_Usuarios = new List<IdentityUser>
        {
            new IdentityUser("Administrador"),
            new IdentityUser("Aluno1"),
            new IdentityUser("Aluno2"),
            new IdentityUser("Aluno3"),
            new IdentityUser("Professor1"),
            new IdentityUser("Professor2")
        };

        IList<IdentityUser> m_Professores = new List<IdentityUser>
        {
            new IdentityUser("Professor1"),
            new IdentityUser("Professor2")
        };

        var m_MockStore = new Mock<IUserStore<IdentityUser>>();
        var m_MockUserManager = new Mock<UserManager<IdentityUser>>(m_MockStore.Object, null, null, null, null, null, null, null, null);
        m_MockUserManager.Object.UserValidators.Add(new UserValidator<IdentityUser>());
        m_MockUserManager.Object.PasswordValidators.Add(new PasswordValidator<IdentityUser>());

        C_UserManager = m_MockUserManager;
        C_UserManager.Setup(a => a.Users).Returns(m_Usuarios.AsQueryable());
        C_UserManager.Setup(a => a.GetUsersInRoleAsync("PROFESSOR")).Returns(Task.FromResult(m_Professores));

        C_Helper = new MatriculaHelper();
    }

    [Test]
    public async Task CM_ObterClaimMatricula()
    {
        var m_Claim = await C_Helper!.CM_ObterClaimDaMatriculaAsync(C_UserManager!.Object);

        Assert.That(m_Claim.Value, Is.EqualTo("000004"));
        Assert.That(m_Claim.Type, Is.EqualTo("matricula"));
        Assert.Pass();
    }
}