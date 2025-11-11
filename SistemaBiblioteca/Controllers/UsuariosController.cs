using Microsoft.AspNetCore.Mvc;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Services;

namespace SistemaBiblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CadastrarUsuario([FromBody] Usuario usuario)
        {
            var resultado = await _usuarioService.CadastrarUsuario(usuario);
            return CreatedAtAction(nameof(ObterUsuario), new { id = resultado.Id }, resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> ObterUsuario(int id)
        {
            var usuario = await _usuarioService.ObterUsuarioPorId(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> ObterTodosUsuarios()
        {
            var usuarios = await _usuarioService.ObterTodosUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("{id}/emprestimos-ativos")]
        public async Task<ActionResult<int>> ObterQuantidadeEmprestimosAtivos(int id)
        {
            var quantidade = await _usuarioService.ObterQuantidadeEmprestimosAtivos(id);
            return Ok(new { UsuarioId = id, QuantidadeEmprestimosAtivos = quantidade });
        }
    }
}
