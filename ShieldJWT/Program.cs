var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .Build();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddScoped<TokenServiceAbstract, TokenService>();
builder.Services.AddScoped<IShieldUser, UserService>();
builder.Services.AddScoped<IShieldMail, MailService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordService>();

builder.Services.AddDbContext<ShieldDbContext>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), bld =>
    {
        bld.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    });
});

var app = builder.Build();

app.UseSwaggerUI(options => 
{
    app.MapOpenApi();
    options.RoutePrefix = "swagger";
    options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
