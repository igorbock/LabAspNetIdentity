namespace TestesProviderJWT.Extensions;

public class IServiceScopeExtensionsTestes
{
    public Mock<IServiceScope>? C_ServiceScope { get; set; }
    public Mock<IServiceScope>? C_ServiceScopeNull { get; set; }
    public IList<IdentityUser>? C_UsuariosRetornoADM { get; set; }

    [SetUp]
    public void Setup()
    {
        var m_MockIConfiguration = new Mock<IConfiguration>();
        m_MockIConfiguration.Setup(a => a["Usuario:Nome"]).Returns("Usuario");
        m_MockIConfiguration.Setup(a => a["Usuario:Email"]).Returns("email@teste.com");
        m_MockIConfiguration.Setup(a => a["Usuario:Telefone"]).Returns("(99)99999-9999");
        m_MockIConfiguration.Setup(a => a["Usuario:Senha"]).Returns("Usuario123@");
        m_MockIConfiguration.Setup(a => a["Usuario:ConfirmaSenha"]).Returns("Usuario123@");

        C_UsuariosRetornoADM = new List<IdentityUser>
        {
            new IdentityUser("Teste1")
        };

        var m_MockStore = new Mock<IUserStore<IdentityUser>>();
        var m_MockUserManager = new Mock<UserManager<IdentityUser>>(m_MockStore.Object, null, null, null, null, null, null, null, null);
        m_MockUserManager.Object.UserValidators.Add(new UserValidator<IdentityUser>());
        m_MockUserManager.Object.PasswordValidators.Add(new PasswordValidator<IdentityUser>());
        m_MockUserManager.Setup(a => a.GetUsersInRoleAsync("ADM")).Returns(Task.FromResult(C_UsuariosRetornoADM!));
        m_MockUserManager.Setup(a => a.CreateAsync(It.IsAny<IdentityUser>()));
        m_MockUserManager.Setup(a => a.AddToRoleAsync(It.IsAny<IdentityUser>(), "ADM"));

        var m_RoleStore = new Mock<IRoleStore<IdentityRole>>();
        var m_MockRoleManager = new Mock<RoleManager<IdentityRole>>(m_RoleStore.Object, null, null, null, null);
        m_MockRoleManager.Setup(a => a.CreateAsync(It.IsAny<IdentityRole>()));

        var m_MockServiceProvider = new Mock<IServiceProvider>();
        m_MockServiceProvider.Setup(a => a.GetService(typeof(IConfiguration))).Returns(m_MockIConfiguration.Object);
        m_MockServiceProvider.Setup(a => a.GetService(typeof(UserManager<IdentityUser>))).Returns(m_MockUserManager.Object);
        m_MockServiceProvider.Setup(a => a.GetService(typeof(RoleManager<IdentityRole>))).Returns(m_MockRoleManager.Object);

        C_ServiceScope = new();
        C_ServiceScope.Setup(a => a.ServiceProvider).Returns(m_MockServiceProvider.Object);

        var m_ServiceProviderNull = new Mock<IServiceProvider>();
        m_ServiceProviderNull.Setup(a => a.GetService(typeof(IConfiguration)));

        C_ServiceScopeNull = new();
        C_ServiceScopeNull.Setup(a => a.ServiceProvider).Returns(m_ServiceProviderNull.Object);
    }

    [Test]
    public void CM_ObterUsuarioDoJSONTeste()
    {
        var m_Usuario = C_ServiceScope!.Object.CMX_ObterUsuarioADM();

        Assert.That(m_Usuario.Nome, Is.EqualTo("Usuario"));
        Assert.That(m_Usuario.Email, Is.EqualTo("email@teste.com"));
        Assert.That(m_Usuario.Telefone, Is.EqualTo("(99)99999-9999"));
        Assert.That(m_Usuario.Senha, Is.EqualTo("Usuario123@"));
        Assert.That(m_Usuario.ConfirmaSenha, Is.EqualTo("Usuario123@"));
        Assert.Pass();

        try
        {
            C_ServiceScopeNull!.Object.CMX_ObterUsuarioADM();
        }
        catch (NullReferenceException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("m_Configuracao"));
            Assert.Pass();
        }
    }

    [Test]
    public async Task CM_UsuarioADMExistenteTeste()
    {
        await C_ServiceScope!.Object.CMX_MigrarUsuarioADMAsync();

        Assert.Pass();
    }

    [Test]
    public async Task CM_UsuarioADMInexistente()
    {
        C_UsuariosRetornoADM!.Clear();

        await C_ServiceScope!.Object.CMX_MigrarUsuarioADMAsync();
    }
}
