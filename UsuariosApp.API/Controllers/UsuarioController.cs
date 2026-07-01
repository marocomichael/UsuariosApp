using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsuariosApp.Domain.Dtos;
using UsuariosApp.Domain.Interfaces.Services;

namespace UsuariosApp.API.Controllers
{
    [Route("api/v1/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("criar")]
        [ProducesResponseType(typeof(CriarUsuarioResponse), 201)] //CREATED
        [ProducesResponseType(typeof(string), 400)] //BAD REQUEST
        [ProducesResponseType(typeof(string), 409)] //CONFLICT
        [ProducesResponseType(typeof(string), 500)] //INTERNAL SERVER ERROR
        public IActionResult Criar([FromBody] CriarUsuarioRequest request)
        {
            try
            {
                var response = _usuarioService.CriarUsuario(request);
                return StatusCode(201, response);
            }
            catch (ValidationException e)
            {
                return StatusCode(400, e.Errors.Select(e => e.ErrorMessage));
            }
            catch (ApplicationException e)
            {
                return StatusCode(409, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("autenticar")]
        [ProducesResponseType(typeof(AutenticarUsuarioResponse), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult Autenticar([FromBody] AutenticarUsuarioRequest request)
        {
            try
            { 
                var response = _usuarioService.AutenticarUsuario(request);
                return StatusCode(200, response);
            }
            catch(ApplicationException e)
            {
                return StatusCode(401, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize]
        [HttpGet("obter-dados")]
        public IActionResult ObterDados()
        {
            return Ok("Sucesso!"); //teste!
        }
    }
}
