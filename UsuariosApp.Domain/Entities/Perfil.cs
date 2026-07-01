using System;
using System.Collections.Generic;
using System.Text;

namespace UsuariosApp.Domain.Entities
{
    public class Perfil
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;

        #region Relacionamentos

        public List<Usuario>? Usuarios { get; set; }

        #endregion
    }
}
