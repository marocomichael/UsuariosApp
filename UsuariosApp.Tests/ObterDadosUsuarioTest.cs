using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using UsuariosApp.Domain.Dtos;

namespace UsuariosApp.Tests
{
    public class ObterDadosUsuarioTest
    {
        private readonly Faker faker = new Faker("pt_BR");
        private readonly HttpClient client = new WebApplicationFactory<Program>().CreateClient();

        [Fact(
            DisplayName = "Obter dados do usuário - Deve retornar os dados do usuário autenticado com sucesso."
        )]
        public void DeveRetornarOsDadosDoUsuarioAutenticadoComSucesso()
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
                    email: requestCriarUsuario.email,
                    senha: requestCriarUsuario.senha
                );

            //ACT            
            var responseAutenticar = client.PostAsJsonAsync("api/v1/usuario/autenticar", requestAutenticar).Result;

            //ASSERTS
            responseAutenticar.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = responseAutenticar.Content.ReadFromJsonAsync<AutenticarUsuarioResponse>().Result;
            var token = data!.accessToken;

            #endregion

            #region Obter os dados do usuário autenticado usando o TOKEN JWT

            //ARRANGE
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //ACT            
            var responseObterDados = client.GetAsync("api/v1/usuario/obter-dados").Result;

            //ASSERTS
            responseObterDados.StatusCode.Should().Be(HttpStatusCode.OK);

            #endregion
        }

        [Fact(
            DisplayName = "Obter dados do usuário - Não deve retornar os dados do usuário sem autenticação."
        )]
        public void NaoDeveRetornarOsDadosDoUsuarioSemAutenticacao()
        {
            //ACT            
            var responseObterDados = client.GetAsync("api/v1/usuario/obter-dados").Result;

            //ASSERTS
            responseObterDados.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
