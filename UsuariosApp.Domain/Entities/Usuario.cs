using System;
using System.Collections.Generic;
using System.Text;

namespace UsuariosApp.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public DateTime DataHoraCriacao { get; set; } = DateTime.Now;
        public Guid PerfilId { get; set; }

        #region Relacionamentos

        public Perfil? Perfil { get; set; }

        #endregion
    }
}
