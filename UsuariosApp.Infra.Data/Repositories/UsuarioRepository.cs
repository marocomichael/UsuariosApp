using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;
using UsuariosApp.Domain.Interfaces.Repositories;
using UsuariosApp.Infra.Data.Contexts;

namespace UsuariosApp.Infra.Data.Repositories
{
    /// <summary>
    /// Implementação dos métodos da interface IUsuarioRepository
    /// </summary>
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DataContext _dataContext;

        public UsuarioRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Inserir(Usuario usuario)
        {
            _dataContext.Add(usuario);
            _dataContext.SaveChanges();
        }

        public Usuario? ObterPorEmail(string email)
        {
            return _dataContext
                    .Set<Usuario>()
                    .Include(u => u.Perfil) //JOIN
                    .Where(u => u.Email.Equals(email))
                    .FirstOrDefault();
        }
    }
}
