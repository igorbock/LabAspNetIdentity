namespace ShieldJWT.Controllers;

public abstract class ShieldController : ControllerBase
{
    [NonAction]
    public IActionResult Handler<SendType1, SendType2, ReturnType>(Func<SendType1, SendType2, ReturnType> method, SendType1 param1, SendType2 param2)
    {
        try
        {
            var methodReturn = method.Invoke(param1, param2);
            return StatusCode(200, methodReturn);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }
}
