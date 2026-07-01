using Azure;
using Azure.Core;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using UsuariosApp.Domain.Dtos;

namespace UsuariosApp.Tests
{
    public class AutenticarUsuarioTest
    {
        private readonly Faker faker = new Faker("pt_BR");
        private readonly HttpClient client = new WebApplicationFactory<Program>().CreateClient();

        [Fact(
            DisplayName = "Autenticar usuário - Deve autenticar um usuário válido com sucesso."
        )]
        public void DeveAutenticarUsuarioValidoComSucesso()
        {
            #region Criando um novo usuário

            //ARRANGE
            var requestCriarUsuario = new CriarUsuarioRequest(
                    nome: faker.Person.FullName,
                    email: faker.Internet.Email(),
                    senha: "@Teste2026"
                );

            //ACT            
            var responseCriarUsuario = client.PostAsJsonAsync("api/v1/usuario/criar", requestCriarUsuario).Result;

            //ASSERTS
            responseCriarUsuario.StatusCode.Should().Be(HttpStatusCode.Created);

            #endregion

            #region Autenticar o usuário

            //ARRANGE
            var requestAutenticar = new AutenticarUsuarioRequest(
                    email : requestCriarUsuario.email,
                    senha : requestCriarUsuario.senha
                );

            //ACT            
            var responseAutenticar = client.PostAsJsonAsync("api/v1/usuario/autenticar", requestAutenticar).Result;

            //ASSERTS
            responseAutenticar.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = responseAutenticar.Content.ReadFromJsonAsync<AutenticarUsuarioResponse>().Result;

            data!.mensagem.Should().Contain("Usuário autenticado com sucesso");
            data!.usuarioId.Should().NotBeEmpty();
            data!.nome.Should().Be(requestCriarUsuario.nome);
            data!.email.Should().Be(requestCriarUsuario.email);
            data!.perfil.Should().Be("OPERADOR");
            data!.accessToken.Should().NotBeEmpty();

            #endregion
        }

        [Fact(
            DisplayName = "Autenticar usuário - Deve retornar acesso negado para usuário não encontrado."
        )]
        public void DeveRetornarAcessoNegadoParaUsuarioInvalido()
        {
            //ARRANGE
            var request = new AutenticarUsuarioRequest(
                    email: faker.Internet.Email(),
                    senha: "@Teste123"
                );

            //ACT            
            var responseAutenticar = client.PostAsJsonAsync("api/v1/usuario/autenticar", request).Result;

            //ASSERTS
            responseAutenticar.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var data = responseAutenticar.Content.ReadAsStringAsync().Result;

            data!.Should().Contain("Acesso negado. Usuário não encontrado");
        }

        [Fact(
            DisplayName = "Autenticar usuário - Deve retornar acesso negado para usuário com senha inválida."
        )]
        public void DeveRetornarAcessoNegadoParaUsuarioComSenhaInvalida()
        {
            #region Criando um novo usuário

            //ARRANGE
            var requestCriarUsuario = new CriarUsuarioRequest(
                    nome: faker.Person.FullName,
                    email: faker.Internet.Email(),
                    senha: "@Teste2026"
                );

            //ACT            
            var responseCriarUsuario = client.PostAsJsonAsync("api/v1/usuario/criar", requestCriarUsuario).Result;

            //ASSERTS
            responseCriarUsuario.StatusCode.Should().Be(HttpStatusCode.Created);

            #endregion

            #region Autenticar o usuário usando uma senha inválida

            //ARRANGE
            var requestAutenticar = new AutenticarUsuarioRequest(
                    email: requestCriarUsuario.email,
                    senha: "@SenhaInvalida2026"
                );

            //ACT            
            var responseAutenticar = client.PostAsJsonAsync("api/v1/usuario/autenticar", requestAutenticar).Result;

            //ASSERTS
            responseAutenticar.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var data = responseAutenticar.Content.ReadAsStringAsync().Result;

            data!.Should().Contain("Acesso negado. Credenciais inválidas");

            #endregion
        }
    }
}
