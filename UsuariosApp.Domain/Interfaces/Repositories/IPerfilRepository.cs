using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Domain.Interfaces.Repositories
{
    public interface IPerfilRepository
    {
        Perfil? ObterPorNome(string nome);
    }
}
