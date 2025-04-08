namespace ShieldJWT.Middlewares;

public class CompanyMiddleware
{
    private readonly RequestDelegate _next;

    public CompanyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IShieldCompany companyService)
    {
        try
        {
            var guidCorrect = context.Request.Headers.TryGetValue("X-Company-Header", out var guidString);
            if (guidCorrect == false)
                throw new ShieldException(401, "Empresa não autorizada");

            var guidParseCorrect = Guid.TryParse(guidString, out Guid idCompany);
            if (guidParseCorrect == false)
                throw new ShieldException(401, "Empresa não autorizada");

            companyService.ValidateCompany(idCompany);

            context.Items.Add("IdCompany", idCompany);

            await _next(context);
            //TODO #23
            //var log = new Log
            //{
            //    Endpoint = context.Request.Path,
            //    Method = context.Request.Method,
            //    IdCompany = idCompany
            //};
        }
        catch (ShieldException ex)
        {
            var problem = new ProblemDetails
            {
                Status = ex?.Code ?? StatusCodes.Status401Unauthorized,
                Title = ex?.Message ?? "Empresa não autorizada",
                Detail = ex?.StackTrace ?? "A empresa não foi encontrada",
                Type = $"https://httpstatuses.com/{ex?.Code}",
                Instance = context.Request.Path
            };

            context.Response.StatusCode = problem.Status.Value;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            var json = System.Text.Json.JsonSerializer.Serialize(problem);
            await context.Response.WriteAsync(json);

            return;
        }
    }
}
