using ShieldJWTLib.Models;

namespace ShieldJWT.Services;

public class UserService : IShieldUser
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IShieldMail _mailService;
    private readonly ShieldDbContext _dbContext;

    public UserService(IPasswordHasher<User> passwordHasher, ShieldDbContext dbContext, IShieldMail mailService)
    {
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
        _mailService = mailService;
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

    public ShieldReturnType Create(string email, string username, string newPassword)
    {
        try
        {
            if (_dbContext.Users.Any(a => a.Email == email))
                throw new Exception("O e-mail informado é incorreto");

            if (_dbContext.Users.Any(a => a.Username == username))
                throw new Exception("O nome de usuário não está disponível");

            var newUser = new User
            {
                Email = email,
                Username = username,
                EmailConfirmed = false
            };

            var newHash = _passwordHasher.HashPassword(newUser, newPassword);
            newUser.Hash = newHash;

            var newAccount = new ChangedPassword
            {
                Email = email,
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

            _mailService.SendConfirmCodeTo(email, username, "", "Confirmação de nova conta - Shield JWT");

            return new ShieldReturnType();
        }
        catch (Exception ex)
        {
            return new ShieldReturnType(ex.Message, 500);
        }
    }
}
