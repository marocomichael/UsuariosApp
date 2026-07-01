using System;
using System.Collections.Generic;
using System.Text;

namespace UsuariosApp.Domain.Dtos
{
    /// <summary>
    /// DTO para representar os dados da resposta que serão
    /// retornados após a criação do usuário
    /// </summary>
    public record CriarUsuarioResponse(
            string mensagem,    //Mensagem de sucesso
            Guid usuarioId,     //Id do usuário gravado no banco de dados
            string nome,        //Nome do usuário
            string email,       //Email do usuário
            string perfil,      //Perfil do usuário
            DateTime dataHora   //Data e hora do cadastro
        );
}
