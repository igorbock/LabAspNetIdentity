namespace ShieldJWT.Controllers;

public abstract class ShieldControllerAbstract : ControllerBase
{
    private readonly IShieldCompany _companyService;

    public ShieldControllerAbstract(IShieldCompany companyService)
    {
        _companyService = companyService;
    }

    [NonAction]
    public IActionResult Handler(Delegate method, params object[] parameters)
    {
        ShieldReturnType? genericReturn = null;
        Guid idCompany = (Guid)HttpContext.Items["IdCompany"]!;

        try
        {
            var methodReturn = method.DynamicInvoke(parameters);

            genericReturn = methodReturn as ShieldReturnType;
            if (genericReturn is not null && genericReturn.Code.ToString().StartsWith('2') == false)
                throw new ShieldException(genericReturn.Code, genericReturn.Message);

            return StatusCode(200, genericReturn);
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
