using FluentValidation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UsuariosApp.Domain.Dtos;
using UsuariosApp.Domain.Entities;
using UsuariosApp.Domain.Interfaces.Repositories;
using UsuariosApp.Domain.Interfaces.Security;
using UsuariosApp.Domain.Interfaces.Services;
using UsuariosApp.Domain.Validations;

namespace UsuariosApp.Domain.Services
{
    /// <summary>
    /// Classe de serviço para implementar as operações 
    /// do dominío para a entidade Usuário.
    /// </summary>
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPerfilRepository _perfilRepository;
        private readonly IJwtService _jwtService;

        public UsuarioService(IUsuarioRepository usuarioRepository, IPerfilRepository perfilRepository, IJwtService jwtService)
        {
            _usuarioRepository = usuarioRepository;
            _perfilRepository = perfilRepository;
            _jwtService = jwtService;
        }

        public CriarUsuarioResponse CriarUsuario(CriarUsuarioRequest request)
        {
            #region Capturando e validando os dados do usuário

            var usuario = new Usuario
            {
                Nome = request.nome,
                Email = request.email,
                Senha = request.senha                
            };

            var validation = new UsuarioValidation();
            var result = validation.Validate(usuario);

            if(!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            #endregion

            #region Verificar se o email já está cadastrado para outro usuário

            if(_usuarioRepository.ObterPorEmail(usuario.Email) != null)
            {
                throw new ApplicationException("O email informado já está cadastrado. Tente outro.");
            }

            #endregion

            #region Criptografar a senha

            usuario.Senha = CriptografarSenhaSha256(usuario.Senha);

            #endregion

            #region Associar o usuário ao Perfil 'Operador'

            var perfil = _perfilRepository.ObterPorNome("OPERADOR");
            
            if(perfil == null)
            {
                throw new Exception("Erro ao obter perfil.");
            }

            usuario.PerfilId = perfil.Id;

            #endregion

            #region Salvar o usuário no banco de dados

            _usuarioRepository.Inserir(usuario);

            #endregion

            return new CriarUsuarioResponse(
                    mensagem : "Usuário cadastrado com sucesso!",
                    usuarioId : usuario.Id,
                    nome : usuario.Nome,
                    email : usuario.Email,
                    perfil : perfil.Nome,
                    dataHora : usuario.DataHoraCriacao
                );            
        }

        public AutenticarUsuarioResponse AutenticarUsuario(AutenticarUsuarioRequest request)
        {
            //Buscar o usuário no banco de dados através do email.
            var usuario = _usuarioRepository.ObterPorEmail(request.email);

            //Verificar se o usuário não foi encontrado
            if(usuario == null)
            {
                throw new ApplicationException("Acesso negado. Usuário não encontrado.");
            }

            //Comparando a senha do usuário
            if(!usuario.Senha.Equals(CriptografarSenhaSha256(request.senha)))
            {
                throw new ApplicationException("Acesso negado. Credenciais inválidas.");
            }

            #region Gerar o token do usuário e a data de expiração

            var token = _jwtService.GerarToken(usuario);
            var expiracao = _jwtService.ObterDataExpiracao();

            #endregion

            //Retornar os dados do usuário
            return new AutenticarUsuarioResponse(
                     mensagem : "Usuário autenticado com sucesso.",
                     usuarioId : usuario.Id,
                     nome : usuario.Nome,
                     email : usuario.Email,
                     perfil : usuario.Perfil!.Nome,
                     dataHoraAcesso : DateTime.Now,
                     dataHoraExpiracao : expiracao,
                     accessToken : token
                );
        }

        private string CriptografarSenhaSha256(string senha)
        {
            using SHA256 sha256 = SHA256.Create();

            byte[] bytes = Encoding.UTF8.GetBytes(senha);
            byte[] hashBytes = sha256.ComputeHash(bytes);

            var builder = new StringBuilder();

            foreach (byte b in hashBytes)
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }       
    }
}
