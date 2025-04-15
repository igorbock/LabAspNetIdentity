namespace ShieldJWT.Extensions;

public static class StringExtensions
{
    public static string EncryptString(this string plainText)
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        
        using var ms = new MemoryStream();
        ms.Write(aes.IV, 0, aes.IV.Length);

        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);

        using var sw = new StreamWriter(cs);
        sw.Write(plainText);

        return Convert.ToBase64String(ms.ToArray());
    }
}
