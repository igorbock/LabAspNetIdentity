using Blazored.LocalStorage;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorIS4;

public class IS4AuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient? c_HttpClient;
    private readonly ILocalStorageService? c_LocalStorage;
    private readonly string? c_TOKEN = "AuthToken";

    public IS4AuthenticationStateProvider(HttpClient p_HttpClient, ILocalStorageService p_LocalStorage)
    {
        c_HttpClient = p_HttpClient;
        c_LocalStorage = p_LocalStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var m_TokenHandler = new JwtSecurityTokenHandler();

        var m_ClaimsIdentity = new ClaimsIdentity();
        var m_ClaimsPrincipal = new ClaimsPrincipal(m_ClaimsIdentity);

        if (m_TokenHandler.CanReadToken(await c_LocalStorage!.GetItemAsStringAsync(c_TOKEN)) == false)
            return new AuthenticationState(m_ClaimsPrincipal);

        var m_TokenJWT = m_TokenHandler.ReadJwtToken(await c_LocalStorage!.GetItemAsStringAsync(c_TOKEN));
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(m_TokenJWT.Claims, "IS4Authentication")));
    }

    public async Task LoginAsync(string p_Usuario, string p_Senha)
    {
        var m_Resultado = await c_HttpClient!.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = "connect/token",
            ClientId = "ClientLab",
            ClientSecret = "lab_segredo",
            Scope = "API-LAB",
            UserName = p_Usuario,
            Password = p_Senha
        });

        if (m_Resultado.IsError)
            return;

        await c_LocalStorage!.SetItemAsStringAsync(c_TOKEN, m_Resultado.AccessToken);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task Logout()
    {
        await c_LocalStorage!.RemoveItemAsync(c_TOKEN);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
