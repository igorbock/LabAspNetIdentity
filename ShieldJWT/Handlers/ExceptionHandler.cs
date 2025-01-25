namespace ShieldJWT.Handlers;

public class ShieldController : ControllerBase
{
    [NonAction]
    public ObjectResult Handler(Action method)
    {
		try
		{
			var methodReturn = method.DynamicInvoke();
			StatusCode(200, methodReturn);
		}
		catch (Exception ex)
		{

			throw;
		}
        return StatusCode(200, "OK");
    }
}
