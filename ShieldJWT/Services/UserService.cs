namespace ShieldJWT.Services;

public class UserService : IShieldUser
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IShieldMail _mailService;
    private readonly ShieldDbContext _dbContext;

    public UserService(IPasswordHasher<User> passwordHasher, ShieldDbContext dbContext)
    {
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
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
            //_mailService.SendConfirmCodeTo(user.Pessoa!.Email!, user.Pessoa!.Nome!, stringCode);

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

    public ShieldReturnType Create(string email, string username, string newPassword)
    {
        try
        {
            var newUser = new User
            {
                Email = email,
                Username = username
            };
            newUser.Hash = _passwordHasher.HashPassword(newUser, newPassword);

            _dbContext.Add(newUser);
            _dbContext.SaveChanges();

            //_mailService.SendEmail

            return new ShieldReturnType();
        }
        catch (Exception ex)
        {
            return new ShieldReturnType(ex.Message, 500);
        }
    }
}
