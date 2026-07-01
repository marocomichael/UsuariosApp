using System;
using System.Collections.Generic;
using System.Text;

namespace UsuariosApp.Domain.Dtos
{
    /// <summary>
    /// DTO para representar os dados da resposta que serão
    /// retornados após a autenticação do usuário
    /// </summary>
    public record AutenticarUsuarioResponse(
            string mensagem,
            Guid usuarioId,
            string nome,
            string email,
            string perfil,
            DateTime dataHoraAcesso,
            DateTime dataHoraExpiracao,
            string accessToken
        );
}
