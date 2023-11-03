using LibIS4.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

namespace BlazorIS4.Pages;

public partial class PainelControle : ComponentBase
{
    [Inject]
    public IUsuariosService? C_UsuarioService { get; set; }

    public IEnumerable<IdentityUser>? C_Usuarios { get; set; } = new Collection<IdentityUser>();
    public IdentityUser? C_Usuario { get; set; } = new IdentityUser();
    public string? C_MensagemErro { get; set; }

    protected override async Task OnInitializedAsync()
    {
		try
		{
            var m_Nome = await C_UsuarioService!.CM_ObterNomeUsuarioAsync();

			C_Usuarios = await C_UsuarioService!.CM_ObterUsuariosAsync();
            C_Usuario = await C_UsuarioService!.CM_ObterDadosUsuarioAsync(m_Nome);
		}
		catch (Exception ex)
		{
            C_MensagemErro = ex.Message;
		}
    }
}
