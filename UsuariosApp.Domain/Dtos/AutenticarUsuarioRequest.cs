using System;
using System.Collections.Generic;
using System.Text;

namespace UsuariosApp.Domain.Dtos
{
    /// <summary>
    /// DTO para representar os dados de requisição (entrada)
    /// para a operação de autenticação de usuário.
    /// </summary>
    public record AutenticarUsuarioRequest(
            string email,
            string senha
        );
}
