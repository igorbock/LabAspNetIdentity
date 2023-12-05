namespace TestesProviderJWT.Services;

public class UsuariosServicesTestes
{
    public Mock<UserManager<IdentityUser>>? C_MockUserManager { get; set; }
    public Mock<RoleManager<IdentityRole>>? C_MockRoleManager { get; set; }
    public RegistrarUsuarioDTO? C_UsuarioDTO { get; set; }
    public IdentityUser? C_Usuario { get; set; }
    public IdentityRole? C_IdentityRole { get; set; }

    [SetUp]
    public void Setup()
    {
        var m_MockStore = new Mock<IUserStore<IdentityUser>>();
        C_MockUserManager = new Mock<UserManager<IdentityUser>>(m_MockStore.Object, null, null, null, null, null, null, null, null);
        C_MockUserManager.Object.UserValidators.Add(new UserValidator<IdentityUser>());
        C_MockUserManager.Object.PasswordValidators.Add(new PasswordValidator<IdentityUser>());

        var m_RoleStore = new Mock<IRoleStore<IdentityRole>>();
        C_MockRoleManager = new Mock<RoleManager<IdentityRole>>(m_RoleStore.Object, null, null, null, null);
    }

    [Test]
    public async Task CM_CriarProfessorAsyncTeste()
    {
        C_UsuarioDTO = new RegistrarUsuarioDTO
        {
            Nome = "Teste1",
            Email = "teste@teste.com.br",
            Telefone = "(99)98765-4321",
            Senha = "Teste123@",
            ConfirmaSenha = "Teste123@"
        };

        C_Usuario = new IdentityUser(C_UsuarioDTO.Nome!);
        C_IdentityRole = new IdentityRole("PROFESSOR");
        C_MockUserManager!.Setup(a => a.CreateAsync(C_Usuario!, C_UsuarioDTO.Senha!)).Returns(Task.FromResult(IdentityResult.Success));
        C_MockUserManager.Setup(a => a.AddToRoleAsync(C_Usuario!, "PROFESSOR"));
        C_MockRoleManager!.Setup(a => a.FindByNameAsync("PROFESSOR")).Returns(Task.FromResult(C_IdentityRole)!);

        var m_Service = new UsuariosService(C_MockUserManager!.Object, C_MockRoleManager!.Object, new MatriculaHelper());
        var m_Resultado = await m_Service.CM_CriarProfessorAsync(C_Usuario!, C_UsuarioDTO);

        Assert.That(m_Resultado, Is.EqualTo("Succeeded"));
        Assert.Pass();
    }

    [Test]
    public void CM_CriarProfessorNomeInvalidoTeste()
    {
        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = "tst",
                Email = "teste@email.com",
                Senha = "Teste123@",
                ConfirmaSenha = "Teste123@"
            };

            Assert.Fail();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("O campo 'Nome' deve ter no mínimo 4 caracteres (Parameter 'Nome')"));
        }

        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = "tstlkjdgldjglkadjgiodfhdfkjnkdvnkjdflskdhiouhsjafddsbvzhgcjksbfdsklhdfgaoieurrrrrrrrrrrsvd",
                Email = "teste@email.com",
                Senha = "Teste123@",
                ConfirmaSenha = "Teste123@"
            };

            Assert.Fail();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("O campo 'Nome' deve ter no máximo 30 caracteres (Parameter 'Nome')"));
        }

        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = string.Empty,
                Email = "teste@email.com",
                Senha = "Teste123@",
                ConfirmaSenha = "Teste123@"
            };

            Assert.Fail();
        }
        catch (ArgumentNullException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("O campo 'Nome' é obrigatório (Parameter 'Nome')"));
        }

        Assert.Pass();
    }

    [Test]
    public void CM_CriarProfessorEmailInvalidoTeste()
    {
        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = "Teste1",
                Email = "t@ema.br",
                Telefone = "(91)95555-5555",
                Senha = "Teste123@",
                ConfirmaSenha = "Teste123@"
            };

            Assert.Fail();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("O campo 'Email' deve ter no mínimo 10 caracteres (Parameter 'Email')"));
        }

        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = "Teste1",
                Email = "testetstlkjdgldjglkadjgiodfhdfkjnkdvnkjdfldsfjkAOSFHDJGAKDJSKDFSDDKJFskdhiouhsjafddsbvzhgcjksbfdsklhdfgaoieurrrrrrrrrrrsvd@email.com",
                Telefone = "(91)95555-5555",
                Senha = "Teste123@",
                ConfirmaSenha = "Teste123@"
            };

            Assert.Fail();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("O campo 'Email' deve ter no máximo 50 caracteres (Parameter 'Email')"));
        }

        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = "Teste1",
                Email = string.Empty,
                Telefone = "(91)95555-5555",
                Senha = "Teste123@",
                ConfirmaSenha = "Teste123@"
            };

            Assert.Fail();
        }
        catch (ArgumentNullException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("O campo 'Email' é obrigatório (Parameter 'Email')"));
        }

        Assert.Pass();
    }

    [Test]
    public void CM_CriarProfessorTelefoneInvalidoTeste()
    {
        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = "Teste1",
                Email = "teste@email.com.br",
                Telefone = "9195555-5555",
                Senha = "Teste123@",
                ConfirmaSenha = "Teste123@"
            };

            Assert.Fail();
        }
        catch (ArgumentException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("O campo 'Telefone' deve ser como o correspondente: (xx)9xxxx-xxxx (Parameter 'Telefone')"));
        }

        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = "Teste1",
                Email = "teste@email.com.br",
                Telefone = "91955555555",
                Senha = "Teste123@",
                ConfirmaSenha = "Teste123@"
            };

            Assert.Fail();
        }
        catch (ArgumentException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("O campo 'Telefone' deve ser como o correspondente: (xx)9xxxx-xxxx (Parameter 'Telefone')"));
        }

        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = "Teste1",
                Email = "teste@email.com.br",
                Telefone = string.Empty,
                Senha = "Teste123@",
                ConfirmaSenha = "Teste123@"
            };

            Assert.Fail();
        }
        catch (ArgumentNullException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("O campo 'Telefone' é obrigatório (Parameter 'Telefone')"));
        }

        Assert.Pass();
    }

    [Test]
    public void CM_CriarProfessorSenhaInvalidaTeste()
    {
        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = "Teste1",
                Email = "teste@email.com.br",
                Telefone = "(91)95555-5555",
                Senha = "teste1",
                ConfirmaSenha = "teste1"
            };

            Assert.Fail();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("O campo 'Senha' deve ter no mínimo 8 caracteres (Parameter 'Senha')"));
        }

        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = "Teste1",
                Email = "teste@email.com.br",
                Telefone = "(91)95555-5555",
                Senha = "dfkjlfadkfjdjfasjhieaogjdvndklfjakojfnvkdlfjaksdjgiehrdfadjfajffjadg",
                ConfirmaSenha = "dfkjlfadkfjdjfasjhieaogjdvndklfjakojfnvkdlfjaksdjgiehrdfadjfajffjadg"
            };

            Assert.Fail();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("O campo 'Senha' deve ter no máximo 30 caracteres (Parameter 'Senha')"));
        }

        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = "Teste1",
                Email = "teste@email.com.br",
                Telefone = "(91)95555-5555",
                Senha = string.Empty,
                ConfirmaSenha = string.Empty
            };

            Assert.Fail();
        }
        catch (ArgumentNullException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("O campo 'Senha' é obrigatório (Parameter 'Senha')"));
        }

        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = "Teste1",
                Email = "teste@email.com.br",
                Telefone = "(91)95555-5555",
                Senha = "senhateste",
                ConfirmaSenha = "senhateste"
            };

            Assert.Fail();
        }
        catch (ArgumentException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("A senha deve ter 8 caracteres, incluindo pelo menos uma letra minúscula, uma letra maiúscula, um número e um caractere alfanumérico"));
        }

        Assert.Pass();
    }

    [Test]
    public void CM_CriarProfessorConfirmaSenhaInvalidaTeste()
    {
        try
        {
            C_UsuarioDTO = new RegistrarUsuarioDTO
            {
                Nome = "Teste1",
                Email = "teste@email.com.br",
                Telefone = "(91)95555-5555",
                Senha = "Teste123@",
                ConfirmaSenha = "Teste123"
            };

            Assert.Fail();
        }
        catch (ArgumentException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("As senhas devem ser iguais"));
        }

        Assert.Pass();
    }
}
