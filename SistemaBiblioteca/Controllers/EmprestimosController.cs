using Microsoft.AspNetCore.Mvc;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Services;

namespace SistemaBiblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmprestimosController : ControllerBase
    {
        private readonly IEmprestimoService _emprestimoService;

        public EmprestimosController(IEmprestimoService emprestimoService)
        {
            _emprestimoService = emprestimoService;
        }

        [HttpPost]
        public async Task<ActionResult<Emprestimo>> RegistrarEmprestimo([FromBody] EmprestimoRequest request)
        {
            try
            {
                var emprestimo = await _emprestimoService.RegistrarEmprestimo(request.ISBN, request.UsuarioId);
                return CreatedAtAction(nameof(ObterEmprestimo), new { id = emprestimo.Id }, emprestimo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/devolucao")]
        public async Task<ActionResult<Emprestimo>> RegistrarDevolucao(int id)
        {
            try
            {
                var emprestimo = await _emprestimoService.RegistrarDevolucao(id);
                return Ok(emprestimo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Emprestimo>> ObterEmprestimo(int id)
        {
            var emprestimo = await _emprestimoService.ObterEmprestimoPorId(id);
            if (emprestimo == null)
                return NotFound();

            return Ok(emprestimo);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Emprestimo>>> ObterEmprestimosPorUsuario(int usuarioId)
        {
            var emprestimos = await _emprestimoService.ObterEmprestimosPorUsuario(usuarioId);
            return Ok(emprestimos);
        }
    }

    public class EmprestimoRequest
    {
        public string ISBN { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
    }
}
