# UsuariosApp

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat-square&logo=csharp)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=flat-square&logo=microsoftsqlserver)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=flat-square&logo=docker)
![Swagger](https://img.shields.io/badge/Swagger-UI-85EA2D?style=flat-square&logo=swagger)
![License](https://img.shields.io/badge/License-MIT-yellow?style=flat-square)

API RESTful para cadastro e autenticação de usuários com arquitetura limpa (Clean Architecture) seguindo princípios de Domain-Driven Design (DDD). Desenvolvida em .NET 10 com Entity Framework Core, SQL Server, autenticação JWT e documentação interativa via Swagger e Scalar.

---

## Funcionalidades

- **Cadastro de usuário** — Criação de conta com nome, e-mail e senha, com perfil "OPERADOR" atribuído automaticamente
- **Autenticação JWT** — Login com e-mail e senha, retornando token JWT com claims (ID, e-mail, nome, perfil)
- **Endpoint protegido** — Rota que exige autenticação para retornar dados do usuário
- **Validação de dados** — Validações com FluentValidation (senha forte, e-mail válido, nome entre 6-100 caracteres)
- **Prevenção de duplicidade** — Impede cadastro de e-mails já existentes (HTTP 409 Conflict)
- **Documentação interativa** — Swagger UI e Scalar para explorar e testar os endpoints

---

## Stack Tecnológica

| Categoria | Tecnologia | Versão |
|---|---|---|
| Linguagem | C# | 12.0 |
| Framework | ASP.NET Core | 10.0 |
| ORM | Entity Framework Core | 10.0.7 |
| Banco de dados | SQL Server (Docker) | 2022 |
| Autenticação | JWT Bearer | 10.0.8 |
| Validação | FluentValidation | 12.1.1 |
| Documentação | Swagger (Swashbuckle) | 10.1.7 |
| Documentação (alternativa) | Scalar | 2.14.10 |
| Testes | xUnit + FluentAssertions + Bogus | — |
| Testes de integração | Microsoft.AspNetCore.Mvc.Testing | 10.0.8 |

---

## Arquitetura

O projeto segue os princípios da **Clean Architecture** (Arquitetura Limpa) com **Domain-Driven Design**, dividido em 4 camadas:

```
UsuariosApp.API          ─►  Apresentação (Controllers, Program.cs, Middleware)
UsuariosApp.Domain       ─►  Domínio (Entidades, DTOs, Interfaces, Services, Validações)
UsuariosApp.Infra.Data   ─►  Infraestrutura (DbContext, Mappings, Repositórios, Migrations)
UsuariosApp.Infra.Security ─► Infraestrutura (JWT Settings, JWT Service)
```

**Fluxo de uma requisição:**

```
Cliente → Controller → Service (Domínio) → Repository (Infra.Data) → SQL Server
                                              ↕
                                        JWT Service (Infra.Security)
```

---

## Pré-requisitos

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Visual Studio 2022+ ou VS Code com extensão C#

---

## Configuração e Execução

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/UsuariosApp.git
cd UsuariosApp
```

### 2. Suba o SQL Server com Docker Compose

```bash
docker-compose up -d
```

O container será iniciado na porta `1434` com a senha `Coti@2026`.

### 3. Aplique as migrations do Entity Framework

```bash
dotnet ef database update --project UsuariosApp.Infra.Data --startup-project UsuariosApp.API
```

Isso criará as tabelas `PERFIL` e `USUARIO` e semeará os perfis **OPERADOR** e **ADMINISTRADOR**.

### 4. Execute a aplicação

```bash
dotnet run --project UsuariosApp.API/UsuariosApp.API.csproj
```

A API estará disponível em `http://localhost:5212`.

### 5. Acesse a documentação interativa

- **Swagger UI:** http://localhost:5212/swagger
- **Scalar:** http://localhost:5212/scalar/v1

---

## Endpoints da API

### `POST /api/v1/usuario/criar`

Cadastra um novo usuário.

**Request:**

```json
{
  "nome": "João Silva",
  "email": "joao.silva@email.com",
  "senha": "Senha@123"
}
```

**Response (201 Created):**

```json
{
  "mensagem": "Usuário cadastrado com sucesso.",
  "usuarioId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "nome": "João Silva",
  "email": "joao.silva@email.com",
  "perfil": "OPERADOR",
  "dataHora": "2026-07-01T12:00:00"
}
```

**Erros:**
| Status | Descrição |
|---|---|
| 400 | Dados inválidos (validação) |
| 409 | E-mail já cadastrado |
| 422 | Erro de validação de negócio |

---

### `POST /api/v1/usuario/autenticar`

Autentica um usuário e retorna um token JWT.

**Request:**

```json
{
  "email": "joao.silva@email.com",
  "senha": "Senha@123"
}
```

**Response (200 OK):**

```json
{
  "mensagem": "Usuário autenticado com sucesso.",
  "usuarioId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "nome": "João Silva",
  "email": "joao.silva@email.com",
  "perfil": "OPERADOR",
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "dataExpiracao": "2026-07-01T13:00:00",
  "dataHora": "2026-07-01T12:00:00"
}
```

**Erros:**
| Status | Descrição |
|---|---|
| 401 | E-mail ou senha inválidos |

---

### `GET /api/v1/usuario/obter-dados`

Endpoint protegido que retorna dados do usuário autenticado.

**Cabeçalho:**

```
Authorization: Bearer {accessToken}
```

**Response (200 OK):**

```json
{
  "mensagem": "Dados do usuário obtidos com sucesso."
}
```

**Erros:**
| Status | Descrição |
|---|---|
| 401 | Token ausente, inválido ou expirado |

---

## Executando os Testes

```bash
dotnet test UsuariosApp.Tests/UsuariosApp.Tests.csproj
```

A suíte de testes cobre:

| Classe de Teste | Cenários |
|---|---|
| `CriarUsuarioTest` | Cadastro válido, validação de campos vazios, e-mail duplicado, senha fraca |
| `AutenticarUsuarioTest` | Autenticação válida, usuário não encontrado, senha incorreta |
| `ObterDadosUsuarioTest` | Acesso autenticado, acesso não autenticado (401) |

> ⚠️ Os testes de integração exigem o container do SQL Server em execução.

---

## Estrutura do Projeto

```
UsuariosApp/
├── docker-compose.yml                 # Configuração do SQL Server
├── UsuariosApp.slnx                   # Solução .NET
│
├── UsuariosApp.API/                   # Camada de Apresentação
│   ├── Program.cs                     # Startup, DI, middleware, JWT, Swagger
│   ├── appsettings.json               # Configurações (string de conexão, JWT)
│   └── Controllers/
│       └── UsuarioController.cs       # Endpoints da API
│
├── UsuariosApp.Domain/                # Camada de Domínio
│   ├── Entities/                      # Entidades (Usuario, Perfil)
│   ├── Dtos/                          # Objetos de transferência (Request/Response)
│   ├── Interfaces/                    # Contratos (Services, Repositories, Security)
│   ├── Services/                      # Lógica de negócio (UsuarioService)
│   └── Validations/                   # Regras de validação (FluentValidation)
│
├── UsuariosApp.Infra.Data/            # Camada de Infraestrutura - Dados
│   ├── Contexts/DataContext.cs        # DbContext do EF Core
│   ├── Mappings/                      # Fluent API (mapeamento das tabelas)
│   ├── Repositories/                  # Implementação dos repositórios
│   └── Migrations/                    # Migrations do EF Core
│
├── UsuariosApp.Infra.Security/        # Camada de Infraestrutura - Segurança
│   ├── Settings/JwtSettings.cs        # POCO de configuração do JWT
│   └── Services/JwtService.cs         # Geração de tokens JWT
│
└── UsuariosApp.Tests/                 # Testes de Integração
    ├── CriarUsuarioTest.cs
    ├── AutenticarUsuarioTest.cs
    └── ObterDadosUsuarioTest.cs
```

---

## Configuração

### appsettings.json

```json
{
  "ConnectionStrings": {
    "UsuariosApp": "Data Source=localhost,1434;Initial Catalog=master;Persist Security Info=True;User ID=sa;Password=Coti@2026;Encrypt=False"
  },
  "Jwt": {
    "ChaveAssinatura": "A4176791-19E4-48EE-8602-0A568D3E6FCD",
    "Emissor": "UsuariosApp",
    "Destinatario": "AngularWeb",
    "ExpiracaoEmHoras": 1
  }
}
```

---

## Tecnologias em Detalhe

### .NET 10 e ASP.NET Core
- API Minimal e Controller-based
- Injeção de dependência nativa
- OpenAPI/Swagger integrado

### Entity Framework Core 10.0
- Database First via migrations
- Fluent API para mapeamento das entidades
- Provider SQL Server

### Autenticação JWT
- Tokens com claims customizadas (ID, e-mail, nome, perfil)
- Configuração via `appsettings.json`
- Validação automática do token em endpoints protegidos

### FluentValidation
- Validação declarativa com regras encadeadas
- Integração com o pipeline do ASP.NET Core

### xUnit + FluentAssertions
- Testes de integração com `WebApplicationFactory`
- Dados mockados com Bogus
- Assertions legíveis com FluentAssertions

---

## Licença

Este projeto está licenciado sob a licença MIT. Consulte o arquivo [LICENSE](LICENSE) para obter mais informações.

---

Desenvolvido com ❤️ usando .NET 10 e Clean Architecture.
