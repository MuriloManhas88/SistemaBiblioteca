using Microsoft.AspNetCore.Mvc;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Services;
using SistemaBiblioteca.Enums;
using SistemaBiblioteca.Exceptions;

namespace SistemaBiblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly ILivroService _livroService;

        public LivrosController(ILivroService livroService)
        {
            _livroService = livroService;
        }

        [HttpPost]
        public async Task<ActionResult<Livro>> CadastrarLivro([FromBody] Livro livro)
        {
            var resultado = await _livroService.CadastrarLivro(livro);
            return CreatedAtAction(nameof(ObterLivro), new { isbn = resultado.ISBN }, resultado);
        }

        [HttpGet("{isbn}")]
        public async Task<ActionResult<Livro>> ObterLivro(string isbn)
        {
            var livro = await _livroService.ObterLivroPorISBN(isbn);
            if (livro == null)
                return NotFound();

            return Ok(livro);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Livro>>> ObterTodosLivros()
        {
            var livros = await _livroService.ObterTodosLivros();
            return Ok(livros);
        }

        [HttpPut("{isbn}/status")]
        public async Task<ActionResult<Livro>> AtualizarStatus(string isbn, [FromBody] StatusLivro novoStatus)
        {
            try
            {
                var livro = await _livroService.AtualizarStatusLivro(isbn, novoStatus);
                return Ok(livro);
            }
            catch (LivroNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BibliotecaException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
