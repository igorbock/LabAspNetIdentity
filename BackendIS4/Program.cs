using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var MigrationsAssembly = typeof(Program).Assembly.GetName().Name;
var ConnectionString = "Host=localhost;Port=5433;Database=identity;User Id=postgres;Password=teste";

builder.Services.AddDbContext<IS4DbContext>(opt => opt.UseNpgsql(ConnectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IS4DbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddConfigurationStore(opt => { opt.ConfigureDbContext = db => db.UseNpgsql(ConnectionString, sql => sql.MigrationsAssembly(MigrationsAssembly)); })
    .AddOperationalStore(opt => { opt.ConfigureDbContext = db => db.UseNpgsql(ConnectionString, sql => sql.MigrationsAssembly(MigrationsAssembly)); })
    .AddAspNetIdentity<IdentityUser>();
    //.AddInMemoryApiScopes(Config.ApiScopes)
    //.AddInMemoryClients(Config.Clients)
    //.AddInMemoryApiResources(Config.ApiResources)
    //.AddTestUsers(Config.TestUsers);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}

app.UseSwagger();
app.UseSwaggerUI();

await app.MigrationExtensionAsync();

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseIdentityServer();

app.Run();
