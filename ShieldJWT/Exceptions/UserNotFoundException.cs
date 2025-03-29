namespace ShieldJWT.Exceptions;

public class UserNotFoundException : Exception
{
	private int _code = 401;
	public int Code => _code;

	public UserNotFoundException() : base("Usuário não encontrado") { }
}
