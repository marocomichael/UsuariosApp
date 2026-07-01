using System;
using System.Collections.Generic;
using System.Text;

namespace UsuariosApp.Infra.Security.Settings
{
    public class JwtSettings
    {
        public string? ChaveAssinatura { get; set; }
        public string? Emissor { get; set; }
        public string? Destinatario { get; set; }
        public int ExpiracaoEmHoras { get; set; }
    }
}
