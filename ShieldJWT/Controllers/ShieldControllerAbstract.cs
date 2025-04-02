namespace ShieldJWT.Controllers;

public abstract class ShieldControllerAbstract : ControllerBase
{
    [NonAction]
    public IActionResult Handler(Delegate method, params object[] parameters)
    {
        ShieldReturnType genericReturn;

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
            genericReturn = new ShieldReturnType(ex.Message, ex.Code);
            return StatusCode(ex.Code, genericReturn);
        }
        catch (Exception ex)
        {
            genericReturn = new ShieldReturnType(ex.Message, 500);
            return StatusCode(500, genericReturn);
        }
    }
}
