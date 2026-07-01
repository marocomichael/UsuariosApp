using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using UsuariosApp.Domain.Interfaces.Repositories;
using UsuariosApp.Domain.Interfaces.Security;
using UsuariosApp.Domain.Interfaces.Services;
using UsuariosApp.Domain.Services;
using UsuariosApp.Infra.Data.Contexts;
using UsuariosApp.Infra.Data.Repositories;
using UsuariosApp.Infra.Security.Services;
using UsuariosApp.Infra.Security.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

//Configurações do Swagger
builder.Services.AddEndpointsApiExplorer(); //Adiciona suporte para explorar os endpoints da API
builder.Services.AddSwaggerGen(); //gerar a documentação da API usando o Swagger

//Capturando a string de conexão do banco de dados no appsettings.json
var connectionString = builder.Configuration.GetConnectionString("UsuariosApp");

//Injeção de dependência da classe de contexto do EF
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

//Injeção de dependência das interfaces (contratos) e classes (implementações) do sistema
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPerfilRepository, PerfilRepository>();

//Injeção de dependência para o JWT (autenticação)
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<IJwtService, JwtService>();

//Configuração para validar os TOKENS JWT
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings!.Emissor,
            ValidateAudience = true,
            ValidAudience = jwtSettings!.Destinatario,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(jwtSettings.ChaveAssinatura!))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//Configurações do Swagger
app.UseSwagger(); //Habilita o middleware do Swagger para gerar a documentação da API
app.UseSwaggerUI(); //Habilita a interface de usuário do Swagger para visualizar a documentação da API

app.MapScalarApiReference(s => s.WithTheme(ScalarTheme.BluePlanet));

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

//Definindo a classe Program.cs como pública
public partial class Program { }