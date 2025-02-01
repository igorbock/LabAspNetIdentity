namespace ShieldJWT.Services;

public class PasswordService : IPasswordHasher<User>
{
    public string HashPassword(User user, string password)
    {
        using var rfc2898 = new Rfc2898DeriveBytes(password, 16, 10000, HashAlgorithmName.SHA256);
        var salt = rfc2898.Salt;
        var hash = rfc2898.GetBytes(32);

        var hashBytes = new byte[salt.Length + hash.Length];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, salt.Length);
        Buffer.BlockCopy(hash, 0, hashBytes, salt.Length, hash.Length);

        return Convert.ToBase64String(hashBytes);
    }

    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        var hashBytes = Convert.FromBase64String(hashedPassword);
        var salt = new byte[16];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, 16);

        using var rfc2898 = new Rfc2898DeriveBytes(providedPassword, salt, 10000, HashAlgorithmName.SHA256);
        var computedHash = rfc2898.GetBytes(32);

        for (int i = 0; i < computedHash.Length; i++)
            if (hashBytes[i + 16] != computedHash[i])
                return PasswordVerificationResult.Failed;

        return PasswordVerificationResult.Success;
    }
}
