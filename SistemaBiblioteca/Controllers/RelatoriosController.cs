using Microsoft.AspNetCore.Mvc;
using SistemaBiblioteca.Services;

namespace SistemaBiblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly IRelatorioService _relatorioService;

        public RelatoriosController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        [HttpGet("livros-mais-emprestados")]
        public async Task<ActionResult<IEnumerable<object>>> ObterLivrosMaisEmprestados()
        {
            var resultado = await _relatorioService.ObterLivrosMaisEmprestados();
            return Ok(resultado);
        }

        [HttpGet("usuarios-mais-emprestimos")]
        public async Task<ActionResult<IEnumerable<object>>> ObterUsuariosComMaisEmprestimos()
        {
            var resultado = await _relatorioService.ObterUsuariosComMaisEmprestimos();
            return Ok(resultado);
        }

        [HttpGet("emprestimos-atrasados")]
        public async Task<ActionResult<IEnumerable<object>>> ObterEmprestimosEmAtraso()
        {
            var resultado = await _relatorioService.ObterEmprestimosEmAtraso();
            return Ok(resultado);
        }
    }
}
