using System;
using System.Collections.Generic;
using System.Text;

namespace UsuariosApp.Domain.Dtos
{
    /// <summary>
    /// DTO para representar os dados de requisição (entrada)
    /// para a operação de cadastro de usuário.
    /// </summary>
    public record CriarUsuarioRequest(
            string nome,    //Nome do usuário
            string email,   //Email do usuário
            string senha    //Senha do usuário
        );
}
