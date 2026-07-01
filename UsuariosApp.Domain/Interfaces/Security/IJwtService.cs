using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Domain.Interfaces.Security
{
    public interface IJwtService
    {
        string GerarToken(Usuario usuario);

        DateTime ObterDataExpiracao();
    }
}
