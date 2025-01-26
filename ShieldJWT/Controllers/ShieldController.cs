namespace ShieldJWT.Controllers;

public class ShieldController : ControllerBase
{
    [NonAction]
    public ObjectResult Handler(Action method)
    {
        try
        {
            var methodReturn = method.DynamicInvoke();
            return StatusCode(200, methodReturn);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }
}
