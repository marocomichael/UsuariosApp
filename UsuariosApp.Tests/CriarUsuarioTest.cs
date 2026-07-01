using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using UsuariosApp.Domain.Dtos;

namespace UsuariosApp.Tests
{
    public class CriarUsuarioTest
    {
        private readonly Faker faker = new Faker("pt_BR");
        private readonly HttpClient client = new WebApplicationFactory<Program>().CreateClient();

        [Fact(
            DisplayName = "Criar usuário - Deve cadastrar um usuário válido com sucesso."
        )]
        public void DeveCadastrarUmUsuarioValidoComSucesso()
        {
            //ARRANGE
            var request = new CriarUsuarioRequest(
                    nome: faker.Person.FullName,
                    email: faker.Internet.Email(),
                    senha: "@Teste2026"
                );

            //ACT            
            var response = client.PostAsJsonAsync("api/v1/usuario/criar", request).Result;

            //ASSERTS
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var data = response.Content.ReadFromJsonAsync<CriarUsuarioResponse>().Result;

            data!.mensagem.Should().Be("Usuário cadastrado com sucesso!");
            data!.usuarioId.Should().NotBeEmpty();
            data!.nome.Should().Be(request.nome);
            data!.email.Should().Be(request.email);
            data!.perfil.Should().Be("OPERADOR");
        }

        [Fact(
            DisplayName = "Criar usuário - Deve retornar mensagem de erro para campos obrigatórios."
        )]
        public void DeveRetornarMensagemDeErroParaCamposObrigatorios()
        {
            //ARRANGE
            var request = new CriarUsuarioRequest(
                    nome: string.Empty,
                    email: string.Empty,
                    senha: string.Empty
                );

            //ACT            
            var response = client.PostAsJsonAsync("api/v1/usuario/criar", request).Result;

            //ASSERTS
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var data = response.Content.ReadAsStringAsync().Result;

            data.Should().Contain("O nome do usuário é obrigatório");
            data.Should().Contain("O email do usuário é obrigatório");
            data.Should().Contain("A senha do usuário é obrigatória");
        }

        [Fact(
            DisplayName = "Criar usuário - Não deve permitir cadastrar usuários com o mesmo e-mail."
        )]
        public void NaoDevePermitirCadasdtrarUsuariosComMesmoEmail()
        {
            //ARRANGE
            var email = faker.Internet.Email();

            var primeiroUsuario = new CriarUsuarioRequest(
                    nome: faker.Person.FullName,
                    email: email,
                    senha: "@Teste2026"
                );

            var segundoUsuario = new CriarUsuarioRequest(
                    nome: faker.Person.FullName,
                    email: email,
                    senha: "@Teste2026"
                );

            //ACT
            var primeiraResposta = client.PostAsJsonAsync("api/v1/usuario/criar", primeiroUsuario).Result;
            var segundaResposta = client.PostAsJsonAsync("api/v1/usuario/criar", segundoUsuario).Result;

            //ASSERTS
            primeiraResposta.StatusCode.Should().Be(HttpStatusCode.Created);

            segundaResposta.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var data = segundaResposta.Content.ReadAsStringAsync().Result;

            data.Should().Contain("O email informado já está cadastrado. Tente outro");
        }

        [Fact(
            DisplayName = "Criar usuário - Deve validar senha forte do usuário."
        )]
        public void DeveValidarSenhaForteDoUsuario()
        {
            //ARRANGE
            var request = new CriarUsuarioRequest(
                    nome: faker.Person.FullName,
                    email: faker.Internet.Email(),
                    senha: "123456"
                );

            //ACT
            var response = client.PostAsJsonAsync("api/v1/usuario/criar", request).Result;

            //ASSERTS
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var data = response.Content.ReadAsStringAsync().Result;

            data.Should().Contain("A senha deve ter pelo menos 1 letra maiúscula, 1 letra minúscula, 1 número, 1 caractere especial e no mínimo 8 caracteres");
        }
    }
}