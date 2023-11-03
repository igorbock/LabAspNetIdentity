using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibIS4.Interfaces
{
    public interface IUsuariosService
    {
        Task<string> CM_ObterNomeUsuarioAsync();
        Task<IEnumerable<IdentityUser>> CM_ObterUsuariosAsync();
        Task<IdentityUser> CM_ObterDadosUsuarioAsync(string p_Nome);
    }
}
