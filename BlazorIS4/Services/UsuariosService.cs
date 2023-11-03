using Blazored.LocalStorage;
using LibIS4.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace BlazorIS4.Services;

public class UsuariosService : IUsuariosService
{
    private readonly HttpClient? c_HttpClient;
    private readonly ILocalStorageService? c_LocalStorage;
    private readonly string? c_TOKEN = "AuthToken";

    public UsuariosService(HttpClient? p_HttpClient, ILocalStorageService? p_LocalStorage)
    {
        c_HttpClient = p_HttpClient;
        c_LocalStorage = p_LocalStorage;
    }

    public async Task<IdentityUser> CM_ObterDadosUsuarioAsync(string p_Nome)
    {
        var m_Endereco = $"api/usuarios?name={p_Nome}";
        var m_Consulta = await c_HttpClient!.GetAsync(m_Endereco);
        m_Consulta.EnsureSuccessStatusCode();

        var m_JSONConsulta = await m_Consulta.Content.ReadAsStringAsync();
        var m_Usuarios = JsonSerializer.Deserialize<IdentityUser>(m_JSONConsulta)!;
        return m_Usuarios;
    }

    public async Task<IEnumerable<IdentityUser>> CM_ObterUsuariosAsync()
    {
        var m_Consulta = await c_HttpClient!.GetAsync("api/usuarios/todos");
        m_Consulta.EnsureSuccessStatusCode();

        var m_JSONConsulta = await m_Consulta.Content.ReadAsStringAsync();
        var m_Usuarios = JsonSerializer.Deserialize<IEnumerable<IdentityUser>>(m_JSONConsulta);

        return m_Usuarios!;
    }

    public async Task<string> CM_ObterNomeUsuarioAsync()
    {
        var m_TokenHandler = new JwtSecurityTokenHandler();
        if (m_TokenHandler.CanReadToken(await c_LocalStorage!.GetItemAsStringAsync(c_TOKEN)) == false)
            throw new Exception("Usuário não está autenticado!");

        var m_TokenJWT = m_TokenHandler.ReadJwtToken(await c_LocalStorage!.GetItemAsStringAsync(c_TOKEN));
        var m_Subject = m_TokenJWT.Subject;
        var m_Usuarios = await CM_ObterUsuariosAsync();
        var m_Usuario = m_Usuarios.FirstOrDefault(a => a.Id == m_Subject);

        if (m_Usuario == null)
            throw new Exception("Usuário é null!");

        return m_Usuario.UserName!;
    }
}
