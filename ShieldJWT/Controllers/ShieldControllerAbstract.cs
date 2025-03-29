namespace ShieldJWT.Controllers;

public abstract class ShieldControllerAbstract : ControllerBase
{
    [NonAction]
    public IActionResult Handler(Delegate method, params object[] parameters)
    {
        try
        {
            var methodReturn = method.DynamicInvoke(parameters);

            var shieldReturnType = methodReturn as ShieldReturnType;
            if (shieldReturnType is not null && shieldReturnType.Code.ToString().StartsWith('2') == false)
                throw new ShieldException(shieldReturnType.Code, shieldReturnType.Message);

            return StatusCode(200, methodReturn);
        }
        catch (ShieldException ex)
        {
            return StatusCode(ex.Code, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
