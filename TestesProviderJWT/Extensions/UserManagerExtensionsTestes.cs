namespace TestesProviderJWT.Extensions;

public class UserManagerExtensionsTestes
{
    public Mock<UserManager<IdentityUser>>? C_MockUserManager { get; set; }
    public IList<IdentityUser>? C_Usuarios { get; set; }

    [SetUp]
    public void Setup()
    {
        C_Usuarios = new List<IdentityUser>
        {
            new IdentityUser("Teste1")
        };

        var m_MockStore = new Mock<IUserStore<IdentityUser>>();
        C_MockUserManager = new Mock<UserManager<IdentityUser>>(m_MockStore.Object, null, null, null, null, null, null, null, null);
        C_MockUserManager.Object.UserValidators.Add(new UserValidator<IdentityUser>());
        C_MockUserManager.Object.PasswordValidators.Add(new PasswordValidator<IdentityUser>());

        C_MockUserManager.Setup(a => a.GetUsersInRoleAsync("ADM")).Returns(Task.FromResult(C_Usuarios));
    }

    [Test]
    public async Task CM_UsuarioADMExistente()
    {
        var m_Existente = await C_MockUserManager!.Object.CMX_UsuarioADMExistenteAsync();
        if (m_Existente)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public async Task CM_UsuarioADMInexistente()
    {
        C_Usuarios!.Clear();
        var m_Existente = await C_MockUserManager!.Object.CMX_UsuarioADMExistenteAsync();
        if (m_Existente)
            Assert.Fail();
        else
            Assert.Pass();
    }
}
