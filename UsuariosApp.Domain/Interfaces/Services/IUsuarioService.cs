using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Dtos;

namespace UsuariosApp.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface para contratos dos métodos da camada de serviço de domínio de usuários.
    /// </summary>
    public interface IUsuarioService
    {
        CriarUsuarioResponse CriarUsuario(CriarUsuarioRequest request);
        AutenticarUsuarioResponse AutenticarUsuario(AutenticarUsuarioRequest request);
    }
}
