namespace ShieldJWT.Filters;

public class CompanyActionFilter : IAsyncActionFilter
{
    private readonly IShieldCompany _companyService;

    public CompanyActionFilter(IShieldCompany companyService)
    {
        _companyService = companyService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Guid idCompany = Guid.Empty;
        ActionExecutedContext? nextActionExecutionDelegate = null;
        var log = new Log
        {
            InputTime = DateTime.Now
        };

        try
        {
            var guidCorrect = context.HttpContext.Request.Headers.TryGetValue("X-Company-Header", out var guidString);
            if (guidCorrect == false)
                throw new ShieldException(401, "Empresa não autorizada");

            var guidParseCorrect = Guid.TryParse(guidString, out idCompany);
            if (guidParseCorrect == false)
                throw new ShieldException(401, "Empresa não autorizada");

            _companyService.ValidateCompany(idCompany);

            context.HttpContext.Items.Add("IdCompany", idCompany);

            nextActionExecutionDelegate = await next();
        }
        catch (ShieldException ex)
        {
            var problem = new ProblemDetails
            {
                Status = ex?.Code ?? StatusCodes.Status401Unauthorized,
                Title = ex?.Message ?? "Empresa não autorizada",
                Detail = ex?.StackTrace ?? "A empresa não foi encontrada",
                Type = $"https://httpstatuses.com/{ex?.Code}",
                Instance = context.HttpContext.Request.Path
            };

            context.HttpContext.Response.StatusCode = problem.Status.Value;
            context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;

            var json = System.Text.Json.JsonSerializer.Serialize(problem);
            await context.HttpContext.Response.WriteAsync(json);
        }
        finally
        {
            if (idCompany != Guid.Empty)
            {
                var result = nextActionExecutionDelegate!.Result as ObjectResult;
                var value = result!.Value as ShieldReturnType;

                log.Method = context.HttpContext.Request.Method;
                log.Endpoint = context.HttpContext.Request.Path;
                log.IdCompany = idCompany;
                log.ReturnType = System.Text.Json.JsonSerializer.Serialize(value);
                log.OutputTime = DateTime.Now;

                _companyService.CreateLog(log);
            }
        }
    }
}
