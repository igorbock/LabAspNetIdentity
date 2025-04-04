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
        ShieldReturnType genericReturn;

        try
        {
            HttpContext.Request.Headers.TryGetValue("X-Company-Header", out var idCompany);
            var guidCorrect = Guid.TryParse(idCompany, out Guid result);
            if (guidCorrect == false)
                throw new ShieldException(401, "Empresa não autorizada");

            _companyService.ValidateCompany(result);
            
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
