namespace ShieldJWT.Exceptions;

public class ShieldException : Exception
{
    public int Code { get; set; }

    public ShieldException(int code, string message) : base(message)
    {
        Code = code;
    }
}
