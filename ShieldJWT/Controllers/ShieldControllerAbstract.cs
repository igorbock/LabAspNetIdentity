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
        Guid idCompany = Guid.Empty;

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
        finally
        {
            if (HttpContext.Request.Path.Value!.Contains("user/login") && genericReturn!.Code == 200)
                genericReturn.Message = genericReturn.Message.EncryptString("teste13783413417583473478348714");

            if (idCompany != Guid.Empty)
            {
                var log = new Log
                {
                    Method = HttpContext.Request.Method,
                    Endpoint = HttpContext.Request.Path,
                    IdCompany = idCompany,
                    ReturnType = System.Text.Json.JsonSerializer.Serialize(genericReturn)
                };

                _companyService.CreateLog(log);
            }
        }
    }
}
