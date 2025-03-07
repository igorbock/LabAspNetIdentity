var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
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
#if DEBUG
    opt.UseNpgsql(configuration.GetConnectionString("DEVaultConnection"));
#else
    opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
#endif
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwaggerUI(options => 
{
    options.RoutePrefix = "swagger";
    options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
