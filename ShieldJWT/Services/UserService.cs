namespace ShieldJWT.Services;

public class UserService : IShieldUser
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IShieldMail _mailService;
    private readonly ShieldDbContext _dbContext;
    private readonly TokenServiceAbstract _tokenService;

    public UserService(
        IPasswordHasher<User> passwordHasher, 
        ShieldDbContext dbContext, 
        IShieldMail mailService, 
        TokenServiceAbstract tokenService)
    {
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
        _mailService = mailService;
        _tokenService = tokenService;
    }

    public ShieldReturnType ChangePassword(string email, string newPassword)
    {
        try
        {
            var user = _dbContext.Users.FirstOrDefault(a => a.Email == email);
            if (user == null)
                throw new Exception("Usuário não encontrado");

            var newHash = _passwordHasher.HashPassword(user, newPassword);

            var changedPassword = new ChangedPassword
            {
                Email = user.Email,
                NewHash = newHash,
                Date = DateTime.UtcNow,
                Confirmed = false
            };

            var byteFill = new byte[4];

            using var random = RandomNumberGenerator.Create();
            random.GetBytes(byteFill);
            var newCode = Math.Abs(BitConverter.ToInt32(byteFill, 0) % 1000000);
            var stringCode = newCode.ToString("D6");

            changedPassword.ConfimationCode = stringCode;

            _dbContext.ChangedPasswords!.Add(changedPassword);
            _dbContext.SaveChanges();
            _mailService.SendConfirmCodeTo(user.Email, user.Username, stringCode, "Confirmação de alteração de senha - Shield JWT");

            return new ShieldReturnType(user.Email);
        }
        catch (Exception ex)
        {
            return new ShieldReturnType(ex.Message, 500);
        }
    }

    public ShieldReturnType ConfirmPassword(string email, string confirmationCode)
    {
        try
        {
            var change = _dbContext.ChangedPasswords.Last(a => a.Email == email);
            if (change.Date.Subtract(DateTime.UtcNow).TotalMinutes > 5)
                throw new Exception("Tempo expirado");

            var confirmed = confirmationCode == change!.ConfimationCode;
            if (confirmed == false)
                throw new Exception("Código não autorizado");

            var user = _dbContext.Users.FirstOrDefault(a => a.Email == email);
            user!.Hash = change.NewHash;
            _dbContext.Users.Update(user);

            change.Confirmed = true;
            change.Date = DateTime.SpecifyKind(change.Date, DateTimeKind.Utc);
            _dbContext.ChangedPasswords!.Update(change);

            _dbContext.SaveChanges();

            return new ShieldReturnType();
        }
        catch (Exception ex)
        {
            return new ShieldReturnType(ex.Message, 500);
        }
    }

    public ShieldReturnType Create(CreateUser newUser)
    {
        try
        {
            if (Regex.Match(newUser.Username, "^(?=[a-zA-Z0-9_.]{6,30}$)(?!.*\\.\\.)[a-zA-Z0-9_.]+$").Success == false)
                throw new Exception("O nome de usuário não pode conter caracteres especiais. No mínimo 6 e no máximo 30 caracteres.");

            if (Regex.Match(newUser.Email, "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?").Success == false)
                throw new Exception("O e-mail informado é incorreto");

            if (_dbContext.Users.Any(a => a.Email == newUser.Email))
                throw new Exception("O e-mail informado não está disponível");

            if (_dbContext.Users.Any(a => a.Username == newUser.Username))
                throw new Exception("O nome de usuário não está disponível");

            var user = new User
            {
                Email = newUser.Email,
                Username = newUser.Username,
                EmailConfirmed = false
            };

            var newHash = _passwordHasher.HashPassword(user, newUser.Password);
            user.Hash = newHash;

            var newAccount = new ChangedPassword
            {
                Email = newUser.Email,
                Date = DateTime.UtcNow,
                NewHash = newHash,
                Confirmed = false
            };

            var byteFill = new byte[4];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(byteFill);
            var newCode = Math.Abs(BitConverter.ToInt32(byteFill, 0) % 1000000);
            var stringCode = newCode.ToString("D6");

            newAccount.ConfimationCode = stringCode;

            _dbContext.Add(newUser);
            _dbContext.ChangedPasswords!.Add(newAccount);
            _dbContext.SaveChanges();

            _mailService.SendConfirmCodeTo(newUser.Email, newUser.Username, "", "Confirmação de nova conta - Shield JWT");

            return new ShieldReturnType($"Usuário criado com sucesso. Confirme o código enviado no e-mail '{newUser.Email}'");
        }
        catch (Exception ex)
        {
            return new ShieldReturnType(ex.Message, 500);
        }
    }

    public ShieldReturnType Login(string username, string password)
    {
        var user = _dbContext.Users.First(a => a.Username == username) ?? throw new Exception("Usuário não encontrado");

        if (user.EmailConfirmed == false)
            throw new Exception("Usuário não encontrado");

        var newHash = _passwordHasher.HashPassword(user, password);
        var hash = user.Hash;

        var result = _passwordHasher.VerifyHashedPassword(user, hash!, password);
        if (result == PasswordVerificationResult.Failed)
            throw new Exception("Usuário ou senha incorretos");

        return new ShieldReturnType(_tokenService.GenerateToken(username, user.Email, "", null));
    }
}
