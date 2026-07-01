using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsuariosApp.Domain.Entities;
using UsuariosApp.Domain.Interfaces.Security;
using UsuariosApp.Infra.Security.Settings;

namespace UsuariosApp.Infra.Security.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GerarToken(Usuario usuario)
        {
            //Informações do usuário que serão gravadas no TOKEN JWT
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()), //Id do usuário
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email), //Email do usuário
                new Claim("name", usuario.Nome), //Nome do usuário
                new Claim("perfil", usuario.Perfil!.Nome), //Nome do perfil do usuário
            };

            //Configurar a chave que será usada para criptografar e assinar o token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.ChaveAssinatura!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Gerando o TOKEN
            var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Emissor!,
                    audience: _jwtSettings.Destinatario!,
                    claims: claims,
                    expires: ObterDataExpiracao(),
                    signingCredentials: credentials
                );

            //Retornar o TOKEN
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public DateTime ObterDataExpiracao()
        {
            return DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoEmHoras);
        }
    }
}
