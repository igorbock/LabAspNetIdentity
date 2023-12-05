namespace TestesProviderJWT.Extensions;

public class IServiceProviderExtensionsTestes
{
    public Mock<IServiceProvider>? C_MockIServiceProvider { get; set; }

    [SetUp]
    public void SetUp()
    {
        var m_MockStore = new Mock<IUserStore<IdentityUser>>();
        var m_MockUserManager = new Mock<UserManager<IdentityUser>>(m_MockStore.Object, null, null, null, null, null, null, null, null);
        m_MockUserManager.Object.UserValidators.Add(new UserValidator<IdentityUser>());
        m_MockUserManager.Object.PasswordValidators.Add(new PasswordValidator<IdentityUser>());

        var m_MockServiceProvider = new Mock<IServiceProvider>();
        m_MockServiceProvider.Setup(a => a.GetService(typeof(UserManager<IdentityUser>))).Returns(m_MockUserManager.Object);

        var m_MockIServiceScope = new Mock<IServiceScope>();
        m_MockIServiceScope.Setup(a => a.ServiceProvider).Returns(m_MockServiceProvider.Object);

        var m_MockIServiceScopeFactory = new Mock<IServiceScopeFactory>();
        m_MockIServiceScopeFactory.Setup(a => a.CreateScope()).Returns(m_MockIServiceScope.Object);

        C_MockIServiceProvider = new();
        C_MockIServiceProvider.Setup(a => a.GetService(typeof(IServiceScopeFactory))).Returns(m_MockIServiceScopeFactory.Object);
    }

    [Test]
    public void CM_ObterIServiceScope()
    {
        using var m_IServiceScope = C_MockIServiceProvider!.Object.CMX_ObterIServiceScope();

        Assert.IsNotNull(m_IServiceScope);
        Assert.Pass();
    }

    [Test]
    public void CM_IServiceScopeEhNull()
    {
        IServiceScope? m_IServiceScope = null;
        try
        {
            C_MockIServiceProvider!.Setup(a => a.GetService(typeof(IServiceScopeFactory)));
            m_IServiceScope = C_MockIServiceProvider!.Object.CMX_ObterIServiceScope();
        }
        catch (NullReferenceException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("IServiceScopeFactory é null"));
            Assert.IsNull(m_IServiceScope);
            Assert.Pass();
        }
        finally
        {
            m_IServiceScope?.Dispose();
        }
    }
}
