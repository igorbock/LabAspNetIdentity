using LibIS4.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BlazorIS4.Pages;

public partial class Registrar : ComponentBase
{
    [Inject]
    public HttpClient? C_HttpClient { get; set; }
    [Inject]
    public NavigationManager? C_NavigationManager { get; set; }

    public RegistrarUsuarioDTO? C_RegistrarDTO { get; set; } = new RegistrarUsuarioDTO();
    public string? C_MensagemErro { get; set; }

    public async Task CM_Registrar()
    {
        var m_JSON = JsonSerializer.Serialize(C_RegistrarDTO);
        var m_StringContent = new StringContent(m_JSON, Encoding.UTF8, "application/json");

        try
        {
            var m_Resultado = await C_HttpClient!.PostAsync("api/usuarios", m_StringContent);
            m_Resultado.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            C_MensagemErro = ex.Message;
        }

        C_NavigationManager!.NavigateTo("/login");
    }
}
