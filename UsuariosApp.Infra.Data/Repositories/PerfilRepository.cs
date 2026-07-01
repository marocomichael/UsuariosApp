using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;
using UsuariosApp.Domain.Interfaces.Repositories;
using UsuariosApp.Infra.Data.Contexts;

namespace UsuariosApp.Infra.Data.Repositories
{
    /// <summary>
    /// Implementação dos métodos da interface IPerfilRepository
    /// </summary>
    public class PerfilRepository : IPerfilRepository
    {
        private readonly DataContext _dataContext;

        public PerfilRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Perfil? ObterPorNome(string nome)
        {
            return _dataContext
                    .Set<Perfil>()
                    .Where(p => p.Nome.Equals(nome))
                    .FirstOrDefault();
        }
    }
}
