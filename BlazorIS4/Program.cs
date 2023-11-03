using Blazored.LocalStorage;
using BlazorIS4;
using BlazorIS4.Services;
using LibIS4.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

builder.Services.AddBlazoredLocalStorage();
#if DEBUG
{
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5071/") });
}
#else
{
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://backendis420231031091854.azurewebsites.net/") });
}
#endif
builder.Services.AddScoped<IUsuariosService, UsuariosService>();

builder.Services.AddScoped<IS4AuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<IS4AuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
