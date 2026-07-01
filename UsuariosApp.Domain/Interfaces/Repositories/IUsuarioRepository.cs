using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface de repositório para operações de usuário.
    /// </summary>
    public interface IUsuarioRepository
    {
        void Inserir(Usuario usuario);
        Usuario? ObterPorEmail(string email);
    }
}
