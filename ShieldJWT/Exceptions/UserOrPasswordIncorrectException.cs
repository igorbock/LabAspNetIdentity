namespace ShieldJWT.Exceptions;

public class UserOrPasswordIncorrectException : Exception
{
    private int _code = 401;
    public int Code => _code;

    public UserOrPasswordIncorrectException() : base("Usuário ou senha incorretos") { }
}
